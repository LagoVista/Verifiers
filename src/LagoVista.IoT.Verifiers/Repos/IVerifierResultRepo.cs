using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Verifiers.Repos
{
    public interface IVerifierResultRepo
    {
        Task AddResultAsync(VerificationResults result);

        Task<IEnumerable<VerificationResults>> GetResultsForComponentAsync(string componentId);
    }
}
