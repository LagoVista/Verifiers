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
