using LagoVista.IoT.Verifiers.Repos;
using System.Threading.Tasks;
using LagoVista.Core.PlatformSupport;
using LagoVista.CloudStorage.Storage;
using System.Collections.Generic;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using LagoVista.IoT.Logging.Loggers;

namespace LagoVista.IoT.Verifiers.CloudRepos.Repos
{
    public class VerifierResultRepo : LagoVista.CloudStorage.DocumentDB.DocumentDBRepoBase<VerificationResults>, IVerifierResultRepo
    {
        bool _shouldConsolidateCollections;

        public VerifierResultRepo(IVerifierSettings repoSettings, IAdminLogger logger) : base(repoSettings.VerifiersDocDbStorage.Uri, repoSettings.VerifiersDocDbStorage.AccessKey, repoSettings.VerifiersDocDbStorage.ResourceName, logger)
        {
            _shouldConsolidateCollections = repoSettings.ShouldConsolidateCollections;
        }

        protected override bool ShouldConsolidateCollections => _shouldConsolidateCollections;

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
