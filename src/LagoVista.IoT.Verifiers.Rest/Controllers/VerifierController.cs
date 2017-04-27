using LagoVista.Core.Interfaces;
using LagoVista.Core.PlatformSupport;
using LagoVista.IoT.Verifiers.Managers;
using LagoVista.IoT.Web.Common.Controllers;
using LagoVista.UserAdmin.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using LagoVista.Core;
using LagoVista.Core.Validation;
using LagoVista.IoT.Verifiers.Models;
using System.Threading.Tasks;
using LagoVista.Core.Models.UIMetaData;

namespace LagoVista.IoT.Verifiers.Rest.Controllers
{
    [Authorize]
    [Route("api")]
    public class VerifierController : LagoVistaBaseController
    {
        IVerifierManager _verifierManager;
        public VerifierController(UserManager<AppUser> userManager, IVerifierManager verifierManager, ILogger logger) : base(userManager, logger)
        {
            _verifierManager = verifierManager;
        }

        /// <summary>
        /// Verifier - Add a Verifier
        /// </summary>
        /// <param name="verifier"></param>
        /// <returns></returns>
        [HttpPost("verifier")]
        public Task<InvokeResult> AddVerifierAsync([FromBody] Verifier verifier)
        {
            return _verifierManager.AddVerifierAsync(verifier, UserEntityHeader, OrgEntityHeader);
        }

        /// <summary>
        /// Verifier - Update Verifier
        /// </summary>
        /// <param name="verifier"></param>
        /// <returns></returns>
        [HttpPut("verifier")]
        public Task<InvokeResult> UpdateVerifierAsync([FromBody] Verifier verifier)
        {
            SetUpdatedProperties(verifier);
            return _verifierManager.UpdateVerifierAsync(verifier, OrgEntityHeader, UserEntityHeader);
        }

        /// <summary>
        /// Verifier - Get Verifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("verifier/{id}")]
        public async Task<DetailResponse<Verifier>> GetVerifierAsync(string id)
        {
            var verifier = await _verifierManager.GetVerifierAsync(id, OrgEntityHeader, UserEntityHeader);

            return DetailResponse<Verifier>.Create(verifier);

        }

        /// <summary>
        /// Verifier - Create New
        /// </summary>
        /// <returns></returns>
        [HttpGet("verifier/factory")]
        public DetailResponse<Verifier> CreateVerifierAsync(string id)
        {
            var verifier = new Verifier();
            verifier.Id = Guid.NewGuid().ToId();
            SetOwnedProperties(verifier);
            SetAuditProperties(verifier);

            return DetailResponse<Verifier>.Create(verifier);
        }

        /// <summary>
        /// Verifier - Get Verifiers for Org
        /// </summary>
        /// <param name="orgid"></param>
        /// <returns></returns>
        [HttpGet("verifiers/org/{orgid}")]
        public async Task<ListResponse<VerifierSummary>> GetVerifiersForOrgAsync(string orgid)
        {
            var verifiers = await _verifierManager.GetVerifierForOrgsAsync(orgid, UserEntityHeader);

            return ListResponse<VerifierSummary>.Create(verifiers);
        }

        /// <summary>
        /// Verifier - Get Verifiers for Component
        /// </summary>
        /// <param name="componentid"></param>
        /// <returns></returns>
        [HttpGet("verifiers/component/{componentid}")]
        public async Task<ListResponse<VerifierSummary>> GetVerifiersForComponentAsync(string componentid)
        {
            var verifiers = await _verifierManager.GetVerifierForComponentAsync(componentid, OrgEntityHeader, UserEntityHeader);
            
            return ListResponse<VerifierSummary>.Create(verifiers);
        }

        /// <summary>
        /// Deployment Config - Key In Use
        /// </summary>
        /// <returns></returns>
        [HttpGet("verifiers/keyinuse/{key}")]
        public Task<bool> VerifierKeyInUse(String key)
        {
            return _verifierManager.QueryVerifierKeyInUseAsync(key, CurrentOrgId);
        }

        /// <summary>
        /// Verifier - Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("verifier/{id}")]
        public Task<InvokeResult> DeleteVerifierAsync(string id)
        {
            return _verifierManager.DeleteVerifierAsync(id, OrgEntityHeader, UserEntityHeader);
        }
    }
}
