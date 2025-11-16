// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: b41bc1614473ac1e2c8fe1c376415c2644f3c7f65b10354000b38219c9970252
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using LagoVista.IoT.Runtime.Core.Module;
using LagoVista.IoT.Verifiers.Managers;
using LagoVista.IoT.Verifiers.Runtime;
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
            services.AddTransient<IMessageAttributeParserVerifierRuntime, MessageAttributeParserVerifierRuntime>();
            services.AddTransient<IMessageParserVerifierRuntime, MessageParserVerifierRuntime>();
        }
    }
}
