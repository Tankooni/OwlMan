using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo.OgmoLoader.Serialized
{
	public partial class LayerDefinition
	{
		public string name;
		public string _eid;
		public float offsetX;
		public float offsetY;
		public int gridCellWidth;
		public int gridCellHeight;
		public int gridCellsX;
		public int gridCellsY;
		public List<int> data;
		public List<List<int>> data2D;
		public List<List<int>> dataCoords;
		public List<List<List<int>>> dataCoords2D;
		public List<string> grid;
		public List<List<string>> grid2D;
		public ExportMode? exportMode;
		public ArrayMode? arrayMode;
		public string tileset;
		public List<EntityDefinition> entities;
		public List<DecalDefinition> decals;
	}
}
