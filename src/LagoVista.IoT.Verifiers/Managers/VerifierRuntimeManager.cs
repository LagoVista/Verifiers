// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 5de277ae59fbc8dc48d0e2bf7affc26335ba648619d8e65594c9ab8200f2cfee
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using LagoVista.Core.Managers;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Verifiers.Repos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Verifiers.Managers
{
    public class VerifierRuntimeManager : ManagerBase, IVerifierRuntimeManager
    {
        IVerifierResultRepo _verifierResultRepo;

        public VerifierRuntimeManager(IVerifierResultRepo verifierResultRepo, IAdminLogger logger, IAppConfig appConfig, IDependencyManager depmanager, ISecurity security) : base(logger, appConfig, depmanager, security)
        {
            _verifierResultRepo = verifierResultRepo;
        }
    }
}
