using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo.OgmoLoader.Serialized
{
	public class LayerTemplate
	{
		/**
		 * Name of the Layer Template.
		 */
		public string name;
		/**
		* Definition of the Layer Template.
		*/
		public LayerValueDefinition definition;
		/**
		 * Size of each cell in the Layer's Grid.
		 */
		public Point gridSize;
		/**
			* Unique Export ID of the Layer.
			*/
		public string exportID;
		/**
		* Enum to determine whether a Tile Layer exports it's Tile Data with IDs or Coords. Only available for Tile Layers.
		*/
		public ExportMode? exportMode;
		/**
		* Enum to determine whether a Tile or Grid Layer exports it's Data as a 1D Array or a 2D Array. Only available for Tile and Grid Layers.
		*/
		public ArrayMode? arrayMode;
		/**
		* Name of this Layer's default Tilemap. Only available for Tile Layers. Can be null.
		*/
		public string defaultTileset;
		/**
		* String Map describing a Grid Layers available Grid Cells. Only available for Grid Layers.
		*/
		public Dictionary<string, string> legend;
		/**
		* Array of Entity Tags that filters out any Entities that DO NOT have any of the Tags described. Only available for Entity Layers.
		*/
		public List<string> requiredTags;
		/**
		* Array of Entity Tags that filters out any Entities that DO have any of the Tags described. Only available for Entity Layers.
		*/
		public List<string> excludedTags;
		/**
		* Directory to search for Decal images. Only available for Decal Layers.
		*/
		public string folder;
		/**
		* Flag to set whether image sequences are included as available Decals. Only available for Decal Layers.
		*/
		public bool? includeImageSequence;
		/**
		* Flag to set whether Decals on this layer are scaleable. Only available for Decal Layers.
		*/
		public bool? scaleable;
		/**
		* Flag to set whether Decals on this layer are rotatable. Only available for Decal Layers.
		*/
		public bool? rotatable;
		/**
		* Array of Value Templates for a Decal Layer. Only available for Decal Layers.
		*/
		public List<ValueTemplate> values;
		}
}
