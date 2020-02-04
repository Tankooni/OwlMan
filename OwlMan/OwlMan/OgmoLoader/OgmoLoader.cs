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

		public static List<Node2D> nodes = new List<Node2D>();

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
		public Node2D Load(out Node2D player, out Vector2 levelBoundsX, out Vector2 levelBoundsY)
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

				if (levelPath.StartsWith("start", StringComparison.InvariantCultureIgnoreCase))
					startLevel = Levels.Last().Value;
			}

			levelBoundsX = new Vector2(0, startLevel.width);
			levelBoundsY = new Vector2(0, startLevel.height);

			return GenerateScene(project, startLevel, out player);
		}

		private string ObtainFileString(string path)
		{
			var file = new File();
			file.Open(path, ModeFlags.Read); //Readonly
			string result = file.GetAsText();
			file.Close();
			return result;
		}

		private Node2D GenerateScene(OgmoProject project, OgmoLevel level, out Node2D player)
		{
			nodes = new List<Node2D>();
			player = null;
			var tileMap = (TileMap)((PackedScene)ResourceLoader.Load("res://prefab/TileMap.tscn")).Instance();
			tileMap.Name = "TileMap";
			Node2D ultimateParent = new Node2D();
			ultimateParent.Name = "Level";
			ultimateParent.AddChild(tileMap);

			//Load set tiles in
			var tileData = level.layers.First(x => x.name == "Tiles").data2D;
			for (int y = 0; y < tileData.Count; y++)
			{
				for (int x = 0; x < tileData[y].Count; x++)
				{
					tileMap.SetCell(x, y, tileData[y][x]);
					if (y == 0)
						tileMap.SetCell(x, -1, 0);
					if (x == 0)
						tileMap.SetCell(-1, y, 0);
					if (x == tileData.Count - 1)
						tileMap.SetCell(tileData.Count, y, 0);
					if (y == tileData[y].Count - 1)
						tileMap.SetCell(x, tileData[y].Count, 0);
				}
			}

			var playerScene = (PackedScene)ResourceLoader.Load("res://prefab/PlayerOwl.tscn");
			var bugScene = ((PackedScene)ResourceLoader.Load("res://prefab/Enemies/Bug.tscn"));
			var beeScene = ((PackedScene)ResourceLoader.Load("res://prefab/Enemies/Beee.tscn"));
			var carnosaurScene = ((PackedScene)ResourceLoader.Load("res://prefab/Enemies/Carnosaur.tscn"));
			var carnosaurusRexScene = ((PackedScene)ResourceLoader.Load("res://prefab/Enemies/CarnosaurusRex.tscn"));

			var playerEntity = level.layers.First(x => x.name == "Entity").entities.First(x => x.name == "LevelSpawn");

			player = (Node2D)playerScene.Instance();
			player.Name = "Player_" + playerEntity._eid;
			player.Position = new Vector2(playerEntity.x, playerEntity.y);

			foreach (var entity in level.layers.First(x => x.name == "Entity").entities)
			{
				Node2D childInstance = null;
				switch (entity.name)
				{
					case "Walker":
						childInstance = (Node2D)bugScene.Instance();
						childInstance.Name = "Walker_" + entity._eid;
						break;
					case "TargetFlyer":
						childInstance = (Node2D)carnosaurScene.Instance();
						childInstance.Name = "Flyer_" + entity._eid;
						break;
					case "Bee":
						childInstance = (Node2D)beeScene.Instance();
						childInstance.Name = "Bee_" + entity._eid;
						break;
					case "Boss":
						childInstance = (Node2D)carnosaurusRexScene.Instance();
						childInstance.Name = "Boss_" + entity._eid;
						break;

				}
				if (childInstance != null)
				{
					childInstance.AddToGroup("enemy", true);
					nodes.Add(childInstance);
					ultimateParent.AddChild(childInstance);
					childInstance.Position = new Vector2(entity.x, entity.y);
				}
			}

			return ultimateParent;
		}
	}
}
