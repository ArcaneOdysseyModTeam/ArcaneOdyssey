using StructureHelper.API;
using StructureHelper.Models;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.WorldBuilding;

namespace ArcaneOdyssey.Subworlds.Base
{
	public abstract class StructureHelperSubworld : AOSubworld
	{
		public const int WorldBorderOutbounds = 40;
		public const int WorldBorderOutboundsTotal = 80;
		public override int Width => structure.width + WorldBorderOutboundsTotal;

		public override int Height => structure.height + WorldBorderOutboundsTotal;

		public override List<GenPass> Tasks => [new LoadStructurePass(this)];

		public virtual Point SpawnLocation => default;

		public StructureData structure;

		public override void Load()
		{
			base.Load();
			structure = Generator.GetStructureData($"Structures/{StructureName}", Mod);
		}

		public abstract string StructureName { get; }

		public class LoadStructurePass(StructureHelperSubworld structure) : GenPass($"Generating Structure {structure.Name}", 0)
		{
			private StructureHelperSubworld subworld = structure;
			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				Generator.GenerateFromData(subworld.structure, new(WorldBorderOutbounds));
				if (subworld.SpawnLocation != default)
				{
					AOUtils.SetWorldSpawn(subworld.SpawnLocation.Point16() + new Point16(WorldBorderOutbounds));
				}
			}
		}
	}
}
