using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using LagoVista.IoT.Verifiers.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Verifiers.Repos
{
    public interface IVerifierResultRepo
    {
        Task AddResultAsync(VerificationResult result);

        Task<IEnumerable<VerificationResult>> GetResultsForComponentAsync(string componentId);
    }
}
