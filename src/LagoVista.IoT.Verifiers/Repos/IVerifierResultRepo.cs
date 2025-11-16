// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: ad17bbd4461ee858c7262f38ebb8ce91516daad9df5b86d36966e39453ac5e28
// IndexVersion: 2
// --- END CODE INDEX META ---
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
