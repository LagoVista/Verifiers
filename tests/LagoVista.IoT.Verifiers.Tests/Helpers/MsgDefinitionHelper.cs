// --- BEGIN CODE INDEX META (do not edit) ---
// ContentHash: 3392294c6517368573bb190e68948ff2133e5ac25bb2ac590e0fbcc9ecab323c
// IndexVersion: 2
// --- END CODE INDEX META ---
using LagoVista.Core.Models;
using LagoVista.IoT.DeviceMessaging.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagoVista.IoT.Verifiers.Tests.Helpers
{
    public static class MsgDefinitionHelper
    {
        public static void AddByte(this DeviceMessageDefinition msgDefinition, string key)
        {
            msgDefinition.Fields.Add(new DeviceMessageDefinitionField() { Key = "key1",
                SearchLocation = EntityHeader<SearchLocations>.Create(SearchLocations.Body), BinaryOffset = 2,
                ParsedBinaryFieldType = EntityHeader<ParseBinaryValueType>.Create(ParseBinaryValueType.Byte) });

        }
    }
}
