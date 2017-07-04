using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using LagoVista.IoT.Logging.Loggers;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using LagoVista.IoT.Runtime.Core.Module;
using LagoVista.IoT.Web.Common.Controllers;
using LagoVista.UserAdmin.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LagoVista.IoT.Verifiers.Rest.Controllers
{

    [Authorize]
    public class VerifierRuntimeController : LagoVistaBaseController
    {
        IMessageParserVerifierRuntime _messageParserRuntime;
        IFieldParserVerifierRuntime _fieldParserRuntime;

        public VerifierRuntimeController(UserManager<AppUser> userManager, IMessageParserVerifierRuntime messageParserRuntime, IFieldParserVerifierRuntime fieldParserRuntime, IAdminLogger logger) : base(userManager, logger)
        {
            _messageParserRuntime = messageParserRuntime;
            _fieldParserRuntime = fieldParserRuntime;
        }

        [HttpPost("/api/verifierruntime/fieldparser/execute")]
        public Task<VerificationResults> VerifyFieldParser([FromBody] VerificationRequest<DeviceMessageDefinitionField> verificationRequest)
        {            
            return _fieldParserRuntime.VerifyAsync(verificationRequest, UserEntityHeader);
        }

        [HttpPost("/api/verifierruntime/messageparser/execute")]
        public Task<VerificationResults> VerifyMessageparser([FromBody] VerificationRequest<DeviceMessageDefinition> messageDefinition)
        {
            return _messageParserRuntime.VerifyAsync(messageDefinition, UserEntityHeader);
        }
    }
}
