using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Verifiers.Repos
{
    public interface IVerifierRepo
    {
        Task AddVerifierAsync(Verifier verifier);
        Task<Verifier> GetVerifierAsync(string id);
        Task<Verifier> GetVerifierByKeyAsync(string key, string orgId);
        Task<IEnumerable<VerifierSummary>> GetVerifiersForOrgAsync(string orgId);
        Task<IEnumerable<Verifier>> GetVerifiersForComponentAsync(string componentId);
        Task UpdateVerifierAsync(Verifier verifier);
        Task DeleteVerifierAsync(string id);
        Task<bool> QueryKeyInUseAsync(string key, string orgId);
    }
}
