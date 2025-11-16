// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 0cc4d37eab5738086acf7a8646705815ed2f7bafe8187ee6b79d37bb46dde9b9
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using LagoVista.IoT.Verifiers.CloudRepos.Repos;
using LagoVista.IoT.Verifiers.Managers;
using LagoVista.IoT.Verifiers.Repos;

namespace LagoVista.IoT.Verifiers.CloudRepos
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IVerifierManager, VerifierManager>();
            services.AddTransient<IVerifierRepo, VerifierRepo>();
            services.AddTransient<IVerifierResultRepo, VerifierResultRepo>();
        }
    }
}
