using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Verifiers.Managers
{
    public interface IVerifierManager
    {
        Task<InvokeResult> AddVerifierAsync(Verifier verifier, EntityHeader org, EntityHeader user);
        Task<Verifier> GetVerifierAsync(string id, EntityHeader org, EntityHeader user);
        Task<IEnumerable<VerifierSummary>> GetVerifierForOrgsAsync(string orgId, EntityHeader user);
        Task<IEnumerable<Verifier>> GetVerifierForComponentAsync(string componentId, EntityHeader org, EntityHeader user);
        Task<InvokeResult> UpdateVerifierAsync(Verifier verifier, EntityHeader org, EntityHeader user);
        Task<InvokeResult> DeleteVerifierAsync(string id, EntityHeader org, EntityHeader user);
        Task<bool> QueryVerifierKeyInUseAsync(string key, string orgId);
    }
}
