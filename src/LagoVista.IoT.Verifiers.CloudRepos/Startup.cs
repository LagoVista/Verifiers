using LagoVista.IoT.Verifiers.CloudRepos.Repos;
using LagoVista.IoT.Verifiers.Managers;
using LagoVista.IoT.Verifiers.Repos;
using Microsoft.Extensions.DependencyInjection;

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
