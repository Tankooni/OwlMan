using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo.OgmoLoader.Serialized
{
	public partial class ValueTemplate
	{
		/**
		* Name of the Value Template.
		*/
		public string name;
		/**
		* Definition of the Value Template.
		*/
		public ValueDefiniton definition;
		/**
		* Default value(s) for the Value Template.
		*/
		public string defaults;
		/**
		* Flag to set if the value is bounded with a min/max. Only available for Int and Float Value Templates.
		*/
		public bool? bounded;
		/**
		* Minimum value of a Float or Int. Only available for Int and Float Value Templates.
		*/
		public float? min;
		/**
		* Maximum value of a Float or Int. Only available for Int and Float Value Templates.
		*/
		public float? max;
		/**
		* Maximum length of a String. Only available for String Value Template.
		*/
		public int? maxLength;
		/**
		* Flag to set whether to remove whitespace from a String. Only available for String Value Template.
		*/
		public bool? trimWhitespace;
		/**
		* Available options of an Enum. Only available for Enum Value Template.
		*/
		public List<string> choices;
		/**
		* Flag to get whether to include the Alpha component on a Color. Only available for Color Value Template.
		*/
		public bool? includeAlpha;
	}
}
