using System.Globalization;
using System.Reflection;

//Resources:VerifierResources:Common_Description
namespace LagoVista.IoT.Verifiers.Resources
{
	public class VerifierResources
	{
        private static global::System.Resources.ResourceManager _resourceManager;
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private static global::System.Resources.ResourceManager ResourceManager 
		{
            get 
			{
                if (object.ReferenceEquals(_resourceManager, null)) 
				{
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LagoVista.IoT.Verifiers.Resources.VerifierResources", typeof(VerifierResources).GetTypeInfo().Assembly);
                    _resourceManager = temp;
                }
                return _resourceManager;
            }
        }
        
        /// <summary>
        ///   Returns the formatted resource string.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private static string GetResourceString(string key, params string[] tokens)
		{
			var culture = CultureInfo.CurrentCulture;;
            var str = ResourceManager.GetString(key, culture);

			for(int i = 0; i < tokens.Length; i += 2)
				str = str.Replace(tokens[i], tokens[i+1]);
										
            return str;
        }
        
        /// <summary>
        ///   Returns the formatted resource string.
        /// </summary>
		/*
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private static HtmlString GetResourceHtmlString(string key, params string[] tokens)
		{
			var str = GetResourceString(key, tokens);
							
			if(str.StartsWith("HTML:"))
				str = str.Substring(5);

			return new HtmlString(str);
        }*/
		
		public static string Common_Description { get { return GetResourceString("Common_Description"); } }
//Resources:VerifierResources:Common_IsPublic

		public static string Common_IsPublic { get { return GetResourceString("Common_IsPublic"); } }
//Resources:VerifierResources:Common_Key

		public static string Common_Key { get { return GetResourceString("Common_Key"); } }
//Resources:VerifierResources:Common_Key_Help

		public static string Common_Key_Help { get { return GetResourceString("Common_Key_Help"); } }
//Resources:VerifierResources:Common_Key_Validation

		public static string Common_Key_Validation { get { return GetResourceString("Common_Key_Validation"); } }
//Resources:VerifierResources:Common_Name

		public static string Common_Name { get { return GetResourceString("Common_Name"); } }
//Resources:VerifierResources:Verifier_Description

		public static string Verifier_Description { get { return GetResourceString("Verifier_Description"); } }
//Resources:VerifierResources:Verifier_ExpectedOutput

		public static string Verifier_ExpectedOutput { get { return GetResourceString("Verifier_ExpectedOutput"); } }
//Resources:VerifierResources:Verifier_Header

		public static string Verifier_Header { get { return GetResourceString("Verifier_Header"); } }
//Resources:VerifierResources:Verifier_Header_Help

		public static string Verifier_Header_Help { get { return GetResourceString("Verifier_Header_Help"); } }
//Resources:VerifierResources:Verifier_Help

		public static string Verifier_Help { get { return GetResourceString("Verifier_Help"); } }
//Resources:VerifierResources:Verifier_Input

		public static string Verifier_Input { get { return GetResourceString("Verifier_Input"); } }
//Resources:VerifierResources:Verifier_InputType

		public static string Verifier_InputType { get { return GetResourceString("Verifier_InputType"); } }
//Resources:VerifierResources:Verifier_InputType_Binary

		public static string Verifier_InputType_Binary { get { return GetResourceString("Verifier_InputType_Binary"); } }
//Resources:VerifierResources:Verifier_InputType_Help

		public static string Verifier_InputType_Help { get { return GetResourceString("Verifier_InputType_Help"); } }
//Resources:VerifierResources:Verifier_InputType_Select

		public static string Verifier_InputType_Select { get { return GetResourceString("Verifier_InputType_Select"); } }
//Resources:VerifierResources:Verifier_InputType_Text

		public static string Verifier_InputType_Text { get { return GetResourceString("Verifier_InputType_Text"); } }
//Resources:VerifierResources:Verifier_PathAndQueryString

		public static string Verifier_PathAndQueryString { get { return GetResourceString("Verifier_PathAndQueryString"); } }
//Resources:VerifierResources:Verifier_PathAndQueryString_Help

		public static string Verifier_PathAndQueryString_Help { get { return GetResourceString("Verifier_PathAndQueryString_Help"); } }
//Resources:VerifierResources:Verifier_ShouldSucceed

		public static string Verifier_ShouldSucceed { get { return GetResourceString("Verifier_ShouldSucceed"); } }
//Resources:VerifierResources:Verifier_ShouldSucceed_Help

		public static string Verifier_ShouldSucceed_Help { get { return GetResourceString("Verifier_ShouldSucceed_Help"); } }
//Resources:VerifierResources:Verifier_Title

		public static string Verifier_Title { get { return GetResourceString("Verifier_Title"); } }
//Resources:VerifierResources:Verifier_VerifierType

		public static string Verifier_VerifierType { get { return GetResourceString("Verifier_VerifierType"); } }
//Resources:VerifierResources:Verifier_VerifierType_MessageFieldParser

		public static string Verifier_VerifierType_MessageFieldParser { get { return GetResourceString("Verifier_VerifierType_MessageFieldParser"); } }
//Resources:VerifierResources:Verifier_VerifierType_Planner

		public static string Verifier_VerifierType_Planner { get { return GetResourceString("Verifier_VerifierType_Planner"); } }

		public static class Names
		{
			public const string Common_Description = "Common_Description";
			public const string Common_IsPublic = "Common_IsPublic";
			public const string Common_Key = "Common_Key";
			public const string Common_Key_Help = "Common_Key_Help";
			public const string Common_Key_Validation = "Common_Key_Validation";
			public const string Common_Name = "Common_Name";
			public const string Verifier_Description = "Verifier_Description";
			public const string Verifier_ExpectedOutput = "Verifier_ExpectedOutput";
			public const string Verifier_Header = "Verifier_Header";
			public const string Verifier_Header_Help = "Verifier_Header_Help";
			public const string Verifier_Help = "Verifier_Help";
			public const string Verifier_Input = "Verifier_Input";
			public const string Verifier_InputType = "Verifier_InputType";
			public const string Verifier_InputType_Binary = "Verifier_InputType_Binary";
			public const string Verifier_InputType_Help = "Verifier_InputType_Help";
			public const string Verifier_InputType_Select = "Verifier_InputType_Select";
			public const string Verifier_InputType_Text = "Verifier_InputType_Text";
			public const string Verifier_PathAndQueryString = "Verifier_PathAndQueryString";
			public const string Verifier_PathAndQueryString_Help = "Verifier_PathAndQueryString_Help";
			public const string Verifier_ShouldSucceed = "Verifier_ShouldSucceed";
			public const string Verifier_ShouldSucceed_Help = "Verifier_ShouldSucceed_Help";
			public const string Verifier_Title = "Verifier_Title";
			public const string Verifier_VerifierType = "Verifier_VerifierType";
			public const string Verifier_VerifierType_MessageFieldParser = "Verifier_VerifierType_MessageFieldParser";
			public const string Verifier_VerifierType_Planner = "Verifier_VerifierType_Planner";
		}
	}
}
