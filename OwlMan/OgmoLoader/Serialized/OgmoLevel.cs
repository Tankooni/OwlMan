using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo.OgmoLoader.Serialized
{
	public partial class OgmoLevel
	{
		/**
		* Width of the Level.
		*/
		public float width;
		/**
		* Height of the Level.
		*/
		public float height;
		/**
		* Offset of the Level on the X axis. Useful for loading multiple chunked Levels.
		*/
		public float offsetX;
		/**
		* Offset of the Level on the Y axis. Useful for loading multiple chunked Levels.
		*/
		public float offsetY;
		/**
		* Array containing all of the Level's Layer Definitions.
		*/
		public List<LayerDefinition> layers;
	}
}
