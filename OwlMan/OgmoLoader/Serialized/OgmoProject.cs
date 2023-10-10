using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo.OgmoLoader.Serialized
{
	public partial class OgmoProject
	{
		/**
			* The Name of the Ogmo Project.
			*/
		public string name;
		/**
		* Array of paths that hold the Project's levels.
		*/
		public List<string> levelPaths;
		/**
		* The Project's background color.
		*/
		public string backgroundColor;
		/**
		* The color of the Grid displayed in the Project's Editor
		*/
		public string gridColor;
		/**
		* Flag to set whether the Project describes rotations in Radians or Degrees.
		* If set to `true`; its in Radians. Otherwise it is in Degrees.
		*/
		public bool anglesRadians;
		/**
		* Sets the default exported file type of a Level.
		*/
		public string defaultExportMode;
		/**
		* Maximum Depth that the Editor will search for files for its File Tree.
		*/
		public int directoryDepth;
		/**
		* Default size of newly created levels in the Editor.
		*/
		public Point levelDefaultSize;
		/**
			* Minimum size a level can be.
			*/
		public Point levelMinSize;
		/**
		* Maximum size a level can be.
		*/
		public Point levelMaxSize;
		/**
		* Array of Value Templates for the Project's Levels.
		*/
		public List<ValueTemplate> levelValues;
		/**
		* Array containing all of the Project's available Entity Tags.
		*/
		public List<string> entityTags;
		/**
		* Array containing all of the Project's available Layer Templates.
		*/
		public List<LayerTemplate> layers;
		/**
		* Array containing all of the Project's available Entity Templates.
		*/
		public List<EntityTemplate> entities;
		/**
		* Array containing all of the Project's available Tilesets.
		*/
		public List<TilesetTemplate> tilesets;
	}
}
