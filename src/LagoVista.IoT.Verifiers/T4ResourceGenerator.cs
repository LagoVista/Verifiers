﻿/*12/6/2023 4:30:57 PM*/
using System.Globalization;
using System.Reflection;

//Resources:VerifierResources:Empty
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
		
		public static string Empty { get { return GetResourceString("Empty"); } }
//Resources:VerifierResources:NA

		public static string NA { get { return GetResourceString("NA"); } }
//Resources:VerifierResources:NotApplicable

		public static string NotApplicable { get { return GetResourceString("NotApplicable"); } }
//Resources:VerifierResources:Verifier_Aborted

		public static string Verifier_Aborted { get { return GetResourceString("Verifier_Aborted"); } }
//Resources:VerifierResources:Verifier_Actual

		public static string Verifier_Actual { get { return GetResourceString("Verifier_Actual"); } }
//Resources:VerifierResources:Verifier_Expected

		public static string Verifier_Expected { get { return GetResourceString("Verifier_Expected"); } }
//Resources:VerifierResources:Verifier_Expected_NotMatch_Actual

		public static string Verifier_Expected_NotMatch_Actual { get { return GetResourceString("Verifier_Expected_NotMatch_Actual"); } }
//Resources:VerifierResources:Verifier_Field

		public static string Verifier_Field { get { return GetResourceString("Verifier_Field"); } }
//Resources:VerifierResources:Verifier_IterationCountZero

		public static string Verifier_IterationCountZero { get { return GetResourceString("Verifier_IterationCountZero"); } }
//Resources:VerifierResources:Verifier_IterationsCompleted

		public static string Verifier_IterationsCompleted { get { return GetResourceString("Verifier_IterationsCompleted"); } }
//Resources:VerifierResources:Verifier_MissingExpectedOutput

		public static string Verifier_MissingExpectedOutput { get { return GetResourceString("Verifier_MissingExpectedOutput"); } }
//Resources:VerifierResources:Verifier_MissingInput

		public static string Verifier_MissingInput { get { return GetResourceString("Verifier_MissingInput"); } }
//Resources:VerifierResources:Verifier_MissingInputType

		public static string Verifier_MissingInputType { get { return GetResourceString("Verifier_MissingInputType"); } }
//Resources:VerifierResources:Verifier_ParserFailed

		public static string Verifier_ParserFailed { get { return GetResourceString("Verifier_ParserFailed"); } }
//Resources:VerifierResources:Verifier_ResultDoesNotContainKey

		public static string Verifier_ResultDoesNotContainKey { get { return GetResourceString("Verifier_ResultDoesNotContainKey"); } }

		public static class Names
		{
			public const string Empty = "Empty";
			public const string NA = "NA";
			public const string NotApplicable = "NotApplicable";
			public const string Verifier_Aborted = "Verifier_Aborted";
			public const string Verifier_Actual = "Verifier_Actual";
			public const string Verifier_Expected = "Verifier_Expected";
			public const string Verifier_Expected_NotMatch_Actual = "Verifier_Expected_NotMatch_Actual";
			public const string Verifier_Field = "Verifier_Field";
			public const string Verifier_IterationCountZero = "Verifier_IterationCountZero";
			public const string Verifier_IterationsCompleted = "Verifier_IterationsCompleted";
			public const string Verifier_MissingExpectedOutput = "Verifier_MissingExpectedOutput";
			public const string Verifier_MissingInput = "Verifier_MissingInput";
			public const string Verifier_MissingInputType = "Verifier_MissingInputType";
			public const string Verifier_ParserFailed = "Verifier_ParserFailed";
			public const string Verifier_ResultDoesNotContainKey = "Verifier_ResultDoesNotContainKey";
		}
	}
}

