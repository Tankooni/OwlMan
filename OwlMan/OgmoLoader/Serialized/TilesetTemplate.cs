using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo.OgmoLoader.Serialized
{
	public partial class TilesetTemplate
	{
		/**
		* Name of the Tileset.
		*/
		public string label;
		/**
		* Path3D to the Tileset image, relative to the Project's path.
		*/
		public string path;
		/**
		* Base64 version of the Tileset image.
		*/
		public string image;
		/**
		* Width of a Tile in this Tileset.
		*/
		public int tileWidth;
		/**
		* Height of a Tile in this Tileset.
		*/
		public int tileHeight;
		/**
		* Empty pixels that separate each Tile on the X axis in this Tileset image.
		*/
		public int tileSeparationX;
		/**
		* Empty pixels that separate each Tile on the Y axis in this Tileset image.
		*/
		public int tileSeparationY;
		}
}
