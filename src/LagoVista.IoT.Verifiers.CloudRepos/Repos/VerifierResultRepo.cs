using LagoVista.IoT.Verifiers.Repos;
using LagoVista.IoT.Verifiers.Models;
using System.Threading.Tasks;
using LagoVista.Core.PlatformSupport;
using LagoVista.CloudStorage.Storage;
using System.Collections.Generic;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;

namespace LagoVista.IoT.Verifiers.CloudRepos.Repos
{
    public class VerifierResultRepo : LagoVista.CloudStorage.Storage.TableStorageBase<VerificationResults>, IVerifierResultRepo
    {
        public VerifierResultRepo(IVerifierSettings verifierSettings, ILogger logger) : base(verifierSettings.VerifiersTableStorage.AccountId, verifierSettings.VerifiersTableStorage.AccessKey, logger)
        {

        }

        public Task AddResultAsync(VerificationResults result)
        {
            return InsertAsync(result);
        }

        public Task<IEnumerable<VerificationResults>> GetResultsForComponentAsync(string componentId)
        {
            return base.GetByFilterAsync(FilterOptions.Create("ComponentId", FilterOptions.Operators.Equals, componentId));
        }
    }
}
