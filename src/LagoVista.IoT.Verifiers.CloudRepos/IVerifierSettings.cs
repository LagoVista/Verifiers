// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 2403d7d0745503904c4674515434e613fa68adf08983d809353a2b96f2284ca8
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LagoVista.IoT.Verifiers.CloudRepos
{
    public interface IVerifierSettings
    {
        IConnectionSettings VerifiersDocDbStorage { get; set; }
        IConnectionSettings VerifiersTableStorage { get; set; }

        bool ShouldConsolidateCollections { get; }
    }
}
