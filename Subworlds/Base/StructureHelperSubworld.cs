using StructureHelper.API;
using StructureHelper.Models;
using System.Collections.Generic;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace ArcaneOdyssey.Subworlds.Base
{
	public abstract class StructureHelperSubworld : AOSubworld
	{
		public override int Width => structure.width + 80;

		public override int Height => structure.height + 80;

		public override List<GenPass> Tasks => [new LoadStructurePass(this)];

		public virtual Point SpawnLocation => default;

		public StructureData structure;

		public override void Load()
		{
			structure = Generator.GetStructureData($"Structures/{StructureName}", Mod);
		}

		public abstract string StructureName { get; }
	}

	public class LoadStructurePass(StructureHelperSubworld structure) : GenPass($"Generating Structure {structure.Name}", 0)
	{
		private StructureHelperSubworld subworld = structure;
		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			Generator.GenerateFromData(subworld.structure, new(40, 40));
			if (subworld.SpawnLocation != default)
			{
				Main.spawnTileX = subworld.SpawnLocation.X;
				Main.spawnTileY = subworld.SpawnLocation.Y;
			}
		}
	}
}
