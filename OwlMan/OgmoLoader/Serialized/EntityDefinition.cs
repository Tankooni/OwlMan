using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atmo.OgmoLoader.Serialized
{
	public partial class EntityDefinition
	{
		public string name;
		public int id;
		public string _eid;
		public float x;
		public float y;
		public float? width;
		public float? height;
		public float? originX;
		public float? originY;
		public float? rotation;
		public bool? flippedX;
		public bool? flippedY;
		public List<PointFloat> nodes;
		public Dictionary<string, string> values;

  /**
   * Creates a new Object containing this Entity's custom values that have been parsed to their expected type, based on the Project that is passed in.
   * 
   * If the Entity isnt matched with a Template from the Project, the values will all remain as Strings.
   * If the Entity IS matched, but a value isnt found in the Template, that value remain a String.
   * @param project Project that holds this Entity's Template.
   * @return Object with parsed values
   */
  
	//public function parseValues(OgmoProject project):Dynamic
	//{
	//	var obj = { };
	//	var entityTemplate = project.getEntityTemplate(exportID);

	//	for (key => value in values)
	//	{
	//	var found = false;
	//	if (entityTemplate != null) for (template in entityTemplate.values)
	//	{
	//	if (found) continue;
	//	if (key == template.name) 
	//	{
	//		found = true;
	//		Reflect.setField(obj, key, switch (template.definition)
	//		{
	//		case BOOL:
	//			value == "true" ? true : false;
	//		case INT:
	//			Std.parseInt(value);
	//		case FLOAT:
	//			Std.parseFloat(value);
	//		default:
	//			value;
	//		});
	//	}
	//	}
	//	if (!found) Reflect.setField(obj, key, value);
	//}
	//return obj;
	//}
	}
}
