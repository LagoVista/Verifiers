using LagoVista.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LagoVista.IoT.Verifiers.CloudRepos
{
    public class VerifierSettings : IVerifierSettings
    {
        public IConnectionSettings VerifiersDocDbStorage { get; }
        public IConnectionSettings VerifiersTableStorage { get; }

        public VerifierSettings(IConfiguration configuration)
        {
            VerifiersDocDbStorage = configuration.CreateDefaultDBStorageSettings();
            VerifiersTableStorage = configuration.CreateDefaultTableStorageSettings();
        }
    }
}
