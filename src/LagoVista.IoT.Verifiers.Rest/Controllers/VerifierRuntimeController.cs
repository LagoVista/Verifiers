using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Pipeline.Admin.Models;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using LagoVista.IoT.Runtime.Core.Module;
using LagoVista.IoT.Web.Common.Controllers;
using LagoVista.UserAdmin.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Verifiers.Rest.Controllers
{

    [Authorize]
    [Route("api")]
    public class VerifierRuntimeController : LagoVistaBaseController
    {
        IFieldParserVerifierRuntime _runtime;        

        public VerifierRuntimeController(UserManager<AppUser> userManager, IFieldParserVerifierRuntime runtime, ILogger logger) : base(userManager, logger)
        {
            _runtime = runtime;
        }

        [HttpPost("verifierruntime/execute")]
        public Task<VerificationResult> VerifyFieldParser([FromBody] VerificationRequest<DeviceMessageDefinitionField> verificationRequest)
        {            
            return _runtime.VerifyAsync(verificationRequest);
        }
    }
}
