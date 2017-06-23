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
