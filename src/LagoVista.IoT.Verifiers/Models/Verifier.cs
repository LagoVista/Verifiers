using LagoVista.Core.Attributes;
using LagoVista.Core.Interfaces;
using LagoVista.Core.Models;
using LagoVista.Core.Validation;
using LagoVista.IoT.DeviceAdmin.Models;
using LagoVista.IoT.Logging.Exceptions;
using LagoVista.IoT.Runtime.Core.Models.Verifiers;
using LagoVista.IoT.Verifiers.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LagoVista.IoT.Verifiers.Models
{
    public enum VerifierTypes
    {
        [EnumLabel(Verifier.VerifierType_MessageFieldParser, VerifierResources.Names.Verifier_VerifierType_MessageFieldParser, typeof(VerifierResources))]
        MessageFieldParser,
        [EnumLabel(Verifier.VerifierType_Planner, VerifierResources.Names.Verifier_VerifierType_Planner, typeof(VerifierResources))]
        Planner
    }

    public enum InputTypes
    {
        [EnumLabel(Verifier.InputType_Binary, VerifierResources.Names.Verifier_InputType_Binary, typeof(VerifierResources))]
        Binary,
        [EnumLabel(Verifier.InputType_Text, VerifierResources.Names.Verifier_InputType_Text, typeof(VerifierResources))]
        Text
    }

    [EntityDescription(VerifierDomain.Verifiers, VerifierResources.Names.Verifier_Title, VerifierResources.Names.Verifier_Help, VerifierResources.Names.Verifier_Description, EntityDescriptionAttribute.EntityTypes.SimpleModel, typeof(VerifierResources))]
    public class Verifier : IoTModelBase, IVerifier
    {
        public const string InputType_Binary = "binary";
        public const string InputType_Text = "text";

        public const string VerifierType_MessageFieldParser = "messagefieldparser";
        public const string VerifierType_Planner = "planner";

        public string DatabaseName { get; set; }
        public string EntityType { get; set; }

        public Verifier()
        {
            Headers = new ObservableCollection<Header>();
            ExpectedOutputs = new ObservableCollection<ExpectedValue>();
        }

        [FormField(LabelResource: VerifierResources.Names.Common_Key, HelpResource: VerifierResources.Names.Common_Key_Help, FieldType: FieldTypes.Key, RegExValidationMessageResource: VerifierResources.Names.Common_Key_Validation, ResourceType: typeof(VerifierResources), IsRequired: true)]
        public String Key { get; set; }

        [FormField(LabelResource: VerifierResources.Names.Verifier_InputType, EnumType: (typeof(InputTypes)), FieldType: FieldTypes.Picker, ResourceType: typeof(VerifierResources), WaterMark: VerifierResources.Names.Verifier_InputType_Select, HelpResource: VerifierResources.Names.Verifier_InputType_Help, IsRequired: true, IsUserEditable: true)]
        public EntityHeader<InputTypes> InputType { get; set; }

        public EntityHeader<VerifierTypes> VerifierType { get; set; }

        [FormField(LabelResource: VerifierResources.Names.Verifier_ShouldSucceed, HelpResource: VerifierResources.Names.Verifier_ShouldSucceed_Help, ResourceType: typeof(VerifierResources), FieldType: FieldTypes.CheckBox)]
        public bool ShouldSucceed { get; set; }

        [FormField(LabelResource: VerifierResources.Names.Verifier_Header, HelpResource: VerifierResources.Names.Verifier_Header_Help, ResourceType: typeof(VerifierResources),FieldType: FieldTypes.ChildList)]
        public ObservableCollection<Header> Headers { get; set; }

        [FormField(LabelResource: VerifierResources.Names.Verifier_PathAndQueryString, FieldType: FieldTypes.Text, HelpResource: VerifierResources.Names.Verifier_PathAndQueryString_Help, ResourceType: typeof(VerifierResources))]
        public String PathAndQueryString { get; set; }

        [FormField(LabelResource: VerifierResources.Names.Verifier_Component, FieldType: FieldTypes.EntityHeaderPicker, IsRequired: true, ResourceType: typeof(VerifierResources))]
        public EntityHeader Component { get; set; }

        [FormField(LabelResource: VerifierResources.Names.Verifier_Input, FieldType: FieldTypes.MultiLineText, IsRequired: true, ResourceType: typeof(VerifierResources))]
        public string Input { get; set; }

        [FormField(LabelResource: VerifierResources.Names.Verifier_ExpectedOutput, FieldType: FieldTypes.MultiLineText, IsRequired:true, ResourceType: typeof(VerifierResources))]
        public string ExpectedOutput { get; set; }

        [FormField(LabelResource: VerifierResources.Names.Verifier_ExpectedOutput, FieldType: FieldTypes.MultiLineText, IsRequired: true, ResourceType: typeof(VerifierResources))]
        public ObservableCollection<ExpectedValue> ExpectedOutputs { get; set; }


        public byte[] GetBinaryPayload()
        {
            if(String.IsNullOrEmpty(Input))
            {
                throw InvalidConfigurationException.FromErrorCode(ErrorCodes.MissingBinaryInput);
            }

            try
            {
                var bytes = new List<Byte>();

                var bytesList = Input.Split(' ');
                foreach (var byteStr in bytesList)
                {
                    bytes.Add(Byte.Parse(byteStr, System.Globalization.NumberStyles.HexNumber));
                }

                return bytes.ToArray();
            }
            catch(Exception ex)
            {
                throw InvalidConfigurationException.FromErrorCode(ErrorCodes.CouldNotConvertInputToBytes, ex.Message);
            }
        }


        [FormField(LabelResource: VerifierResources.Names.Common_IsPublic, FieldType: FieldTypes.Bool, ResourceType: typeof(VerifierResources))]
        public bool IsPublic { get; set; }
        public EntityHeader OwnerOrganization { get; set; }
        public EntityHeader OwnerUser { get; set; }

        public VerifierSummary CreateSummary()
        {
            return new VerifierSummary()
            {
                Id = Id,
                Name = Name,
                IsPublic = IsPublic,
                Description = Description,
                Key = Key
            };
        }
    }

    public class VerifierSummary : LagoVista.Core.Models.SummaryData
    {

    }
}
