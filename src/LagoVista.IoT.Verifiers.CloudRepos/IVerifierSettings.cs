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
