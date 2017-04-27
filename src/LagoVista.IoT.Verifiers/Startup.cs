using LagoVista.IoT.Runtime.Core.Module;
using LagoVista.IoT.Verifiers.Managers;
using LagoVista.IoT.Verifiers.Runtime;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Verifiers
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IVerifierManager, VerifierManager>();
            services.AddTransient<IFieldParserVerifierRuntime, FieldParserVerifierRuntime>();
        }
    }
}
