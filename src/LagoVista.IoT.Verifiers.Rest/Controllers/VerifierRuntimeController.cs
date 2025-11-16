// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 62642e2913c841c531edba236008e3abd79e4c49c2302f699cecdcb925256102
// IndexVersion: 2
// --- END CODE INDEX META ---
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
        IMessageAttributeParserVerifierRuntime _fieldParserRuntime;

        public VerifierRuntimeController(UserManager<AppUser> userManager, IMessageParserVerifierRuntime messageParserRuntime, IMessageAttributeParserVerifierRuntime fieldParserRuntime, IAdminLogger logger) : base(userManager, logger)
        {
            _messageParserRuntime = messageParserRuntime;
            _fieldParserRuntime = fieldParserRuntime;
        }

        [HttpPost("/api/verifierruntime/messageattributeparser/execute")]
        public Task<VerificationResults> VerifyFieldParser([FromBody] VerificationRequest<MessageAttributeParser> verificationRequest)
        {
            return _fieldParserRuntime.VerifyAsync(verificationRequest, OrgEntityHeader, UserEntityHeader);
        }

        [HttpPost("/api/verifierruntime/messageparser/execute")]
        public Task<VerificationResults> VerifyMessageparser([FromBody] VerificationRequest<DeviceMessageDefinition> messageDefinition)
        {
            return _messageParserRuntime.VerifyAsync(messageDefinition, OrgEntityHeader, UserEntityHeader);
        }
    }
}
