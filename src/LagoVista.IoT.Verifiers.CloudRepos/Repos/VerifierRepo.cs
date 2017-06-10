using LagoVista.Core.PlatformSupport;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LagoVista.IoT.Verifiers.Repos;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using LagoVista.IoT.Logging.Loggers;

namespace LagoVista.IoT.Verifiers.CloudRepos.Repos
{
    public class VerifierRepo : LagoVista.CloudStorage.DocumentDB.DocumentDBRepoBase<Verifier>, IVerifierRepo
    {
        private bool _shouldConsolidateCollections;

        public VerifierRepo(IVerifierSettings repoSettings, IAdminLogger logger) : base(repoSettings.VerifiersDocDbStorage.Uri, repoSettings.VerifiersDocDbStorage.AccessKey, repoSettings.VerifiersDocDbStorage.ResourceName, logger)
        {
            _shouldConsolidateCollections = repoSettings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections
        {
            get
            {
                return _shouldConsolidateCollections;
            }
        }

        public Task AddVerifierAsync(Verifier verifier)
        {
            return this.CreateDocumentAsync(verifier);
        }

        public Task DeleteVerifierAsync(string id)
        {
            return this.DeleteDocumentAsync(id);
        }

        public Task<Verifier> GetVerifierAsync(string id)
        {
            return this.GetDocumentAsync(id);
        }

        public async Task<Verifier> GetVerifierByKeyAsync(string key, string orgId)
        {
            return (await base.QueryAsync(qry => qry.Key == key && qry.OwnerOrganization.Id == orgId)).FirstOrDefault();
        }

        public async Task<IEnumerable<VerifierSummary>> GetVerifiersForComponentAsync(string componentId)
        {
            var items = await base.QueryAsync(qry => qry.Component.Id == componentId);

            return from item in items
                   select item.CreateSummary();
        }

        public async  Task<IEnumerable<VerifierSummary>> GetVerifiersForOrgAsync(string orgId)
        {
            var items = await base.QueryAsync(qry => qry.IsPublic == true || qry.OwnerOrganization.Id == orgId);

            return from item in items
                   select item.CreateSummary();
        }

        public async Task<bool> QueryKeyInUseAsync(string key, string orgId)
        {
            var items = await base.QueryAsync(attr => (attr.OwnerOrganization.Id == orgId || attr.IsPublic == true) && attr.Key == key);
            return items.Any();
        }

        public Task UpdateVerifierAsync(Verifier verifier)
        {
            return base.UpsertDocumentAsync(verifier);
        }
    }
}
