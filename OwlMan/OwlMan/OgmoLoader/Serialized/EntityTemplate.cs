using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo.OgmoLoader.Serialized
{
	public class EntityTemplate
	{
		public string name;
		public string exportID;
		public int limit;
		public Point size;
		public Point origin;
		public bool originAnchored;
		public EntityShape shape;
		public string color;
		public bool tileX;
		public bool tileY;
		public Point tileSize;
		public bool resizeableX;
		public bool resizeableY;
		public bool rotatable;
		public float rotationDegrees;
		public bool canFlipX;
		public bool canFlipY;
		public bool canSetColor;
		public bool hasNodes;
		public int nodeLimit;
		public int nodeDisplay;
		public bool nodeGhost;
		public List<string> tags;
		public List<ValueTemplate> values;
		}
}
