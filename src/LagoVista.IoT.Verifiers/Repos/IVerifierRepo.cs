// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 1e71eaccbcef0c83b200ae348b432fe1c4516beb9821cdb5761373db3b1d3fec
// IndexVersion: 2
// --- END CODE INDEX META ---
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
