// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 01ebbca688067013efb2c99a342576bc3816cfb16757353f85e5c306783baf65
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.CloudStorage.Interfaces;
using LagoVista.CloudStorage.Storage;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using LagoVista.IoT.Verifiers.Repos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LagoVista.IoT.Verifiers.CloudRepos.Repos
{
    public class VerifierResultRepo : LagoVista.CloudStorage.DocumentDB.DocumentDBRepoBase<VerificationResults>, IVerifierResultRepo
    {
        public VerifierResultRepo(IDocumentCloudCachedServices services) : base(services)
        {
        }
        public Task AddResultAsync(VerificationResults result)
        {
            return CreateDocumentAsync(result);
        }

        public async Task<IEnumerable<VerificationResults>> GetResultsForComponentAsync(string componentId)
        {
            var items = await base.QueryAsync(qry => qry.Component.Id == componentId);
            return items;
        }
    }
}
