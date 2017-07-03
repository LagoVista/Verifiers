using LagoVista.Core.Interfaces;
using LagoVista.Core.Managers;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Verifiers.Repos;
using System;
using System.Collections.Generic;
using System.Text;
using LagoVista.Core.Models;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using LagoVista.Core.Validation;
using System.Threading.Tasks;
using static LagoVista.Core.Models.AuthorizeResult;
using LagoVista.IoT.Logging.Loggers;

namespace LagoVista.IoT.Verifiers.Managers
{
    public class VerifierManager : ManagerBase, IVerifierManager
    {
        IVerifierRepo _verifierRepo;
        public VerifierManager(IVerifierRepo verifierRepo, IAdminLogger logger, IAppConfig appConfig, IDependencyManager depmanager, ISecurity security) : base(logger, appConfig, depmanager, security)
        {
            _verifierRepo = verifierRepo;
        }

        public async Task<InvokeResult> AddVerifierAsync(Verifier verifier, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(verifier, AuthorizeActions.Create, user, org);
            ValidationCheck(verifier, Actions.Create);
            await _verifierRepo.AddVerifierAsync(verifier);
            return InvokeResult.Success;
        }

        public async Task<InvokeResult> DeleteVerifierAsync(string id, EntityHeader org, EntityHeader user)
        {
            var verifier = await _verifierRepo.GetVerifierAsync(id);
            await AuthorizeAsync(verifier, AuthorizeActions.Update, user, org);
            await ConfirmNoDepenenciesAsync(verifier);
            return InvokeResult.Success;
        }

        public async Task<Verifier> GetVerifierAsync(string id, EntityHeader org, EntityHeader user)
        {
            var verifier = await _verifierRepo.GetVerifierAsync(id);
            await AuthorizeAsync(verifier, AuthorizeActions.Delete, user, org);
            return verifier;
        }

        public async Task<IEnumerable<Verifier>> GetVerifierForComponentAsync(string componentId, EntityHeader org, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, org.Id, typeof(Verifier));
            return await _verifierRepo.GetVerifiersForComponentAsync(componentId);
        }

        public async Task<IEnumerable<VerifierSummary>> GetVerifierForOrgsAsync(string orgId, EntityHeader user)
        {
            await AuthorizeOrgAccessAsync(user, orgId, typeof(Verifier));
            return await _verifierRepo.GetVerifiersForOrgAsync(orgId);
        }

        public Task<bool> QueryVerifierKeyInUseAsync(string key, string orgId)
        {
            return _verifierRepo.QueryKeyInUseAsync(key, orgId);
        }

        public async Task<InvokeResult> UpdateVerifierAsync(Verifier verifier, EntityHeader org, EntityHeader user)
        {
            await AuthorizeAsync(verifier, AuthorizeActions.Update, user, org);
            ValidationCheck(verifier, Actions.Update);
            await _verifierRepo.UpdateVerifierAsync(verifier);
            return InvokeResult.Success;
        }
    }
}
