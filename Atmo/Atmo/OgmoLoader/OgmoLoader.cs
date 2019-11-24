using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Atmo.OgmoLoader.Serialized;
using static Godot.File;

namespace Atmo.OgmoLoader
{
	public class OgmoLoader
	{
		public OgmoProject project;
		public Dictionary<string, OgmoLevel> Levels;

		private Dictionary<string, Type> _types;
		//private Dictionary<string, GridDefinition> _gridTypes;
		//private Dictionary<string, TilemapDefinition> _tilemapTypes;

		public OgmoLoader()
		{
			_types = new Dictionary<string, Type>();
			//_gridTypes = new Dictionary<string, GridDefinition>();
			//_tilemapTypes = new Dictionary<string, TilemapDefinition>();
		}

		/// <summary>
		/// Register an Entity class under a different name than it appears in code.
		/// This is only needed if you're using a single class to represent multiple Ogmo types,
		/// or in the case of a naming mismatch.
		/// </summary>
		/// <param name="name">The name to register the alias with.</param>
		public void RegisterClassAlias<T>(string name) where T : Node2D
		{
			RegisterClassAlias(typeof(T), name);
		}

		/// <summary>
		/// Register an Entity class under a different name than it appears in code.
		/// This is only needed if you're using a single class to represent multiple Ogmo types,
		/// or in the case of a naming mismatch.
		/// </summary>
		/// <param name="type">The Type to register.</param>
		/// <param name="name">The name to register the alias with.</param>
		public void RegisterClassAlias(Type type, string name)
		{
			_types[name] = type;
		}

		/// <summary>
		/// Loads all levels for the game and project data.
		/// </summary>
		/// <returns>The generated starting scene.</returns>
		public Node2D Load()
		{
			string pathToProjectFile = "res://ogmo/project.ogmo";
			string pathToLevelsDir = "res://ogmo/levels";

			project = JsonConvert.DeserializeObject<OgmoProject>(ObtainFileString(pathToProjectFile));
			Levels = new Dictionary<string, OgmoLevel>();

			var levelDir = new Directory();
			levelDir.Open(pathToLevelsDir);
			levelDir.ListDirBegin(true, true);

			string levelPath;
			OgmoLevel startLevel = null;
			while ((levelPath = levelDir.GetNext()) != string.Empty)
			{
				if (!levelPath.EndsWith(".json"))
					continue;
				Levels.Add(levelPath.Replace(".json", string.Empty), JsonConvert.DeserializeObject<OgmoLevel>(ObtainFileString(pathToLevelsDir + "/" + levelPath)));

				if (levelPath.StartsWith("Start", StringComparison.InvariantCultureIgnoreCase))
					startLevel = Levels.Last().Value;
			}
			

			return GenerateScene(project, startLevel);
		}

		private string ObtainFileString(string path)
		{
			var file = new File();
			file.Open(path, 1); //Readonly
			string result = file.GetAsText();
			file.Close();
			return result;
		}

		private Node2D GenerateScene(OgmoProject project, OgmoLevel level)
		{
			var tileMap = (TileMap)((PackedScene)ResourceLoader.Load("res://prefab/TileMap.tscn")).Instance();
			Node2D ultimateParent = new Node2D();
			ultimateParent.AddChild(tileMap);

			//foreach(int id in tileMap.TileSet.GetTilesIds())
			//{
			//	GD.Print(id.GetType());
			//	GD.Print(id.ToString());
			//}

			//Load set tiles in
			var tileData = level.layers.First(x => x.name == "Tiles").data2D;
			for (int y = 0; y < tileData.Count; y++)
			{
				for (int x = 0; x < tileData[y].Count; x++)
				{
					tileMap.SetCell(x, y, tileData[y][x]);
					//GD.Print("x: ", x, ", y: ", y, ", data: ", tileData[y][x]);
				}
			}

			var playerScene = ((PackedScene)ResourceLoader.Load("res://prefab/PlayerOwl.tscn"));
			var bugScene = ((PackedScene)ResourceLoader.Load("res://prefab/Bug.tscn"));

			foreach (var entity in level.layers.First(x => x.name == "Entity").entities)
			{
				Node2D childInstance = null;
				switch (entity.name)
				{
					case "LevelSpawn":
						childInstance = (Node2D)playerScene.Instance();
						break;
					case "Walker":
						childInstance = (Node2D)bugScene.Instance();
						break;
				}
				if (childInstance != null)
				{
					//childInstance.SetOwner(ultimateParent);
					//ultimateParent.AddChild(childInstance);
					//childInstance.SetPosition(new Vector2(entity.x, entity.y));
				}
			}

			return ultimateParent;
		}
	}
}
