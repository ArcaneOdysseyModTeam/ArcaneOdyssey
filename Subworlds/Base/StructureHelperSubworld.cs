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
		public const int WorldBorderOutboundsTotal = WorldBorderOutbounds * 2;

		public override int Width => Structure.width + WorldBorderOutboundsTotal - 1;

		public override int Height => Structure.height + WorldBorderOutboundsTotal - 1;

		public override List<GenPass> Tasks => [new LoadStructurePass(this)];

		public virtual Point SpawnLocation => default;

		public StructureData Structure => Generator.GetStructureData($"Structures/{StructureName}", Mod);

		public abstract string StructureName { get; }

		public class LoadStructurePass(StructureHelperSubworld structure) : GenPass($"Generating Structure {structure.Name}", 0)
		{
			private StructureHelperSubworld subworld = structure;
			protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
			{
				Main.worldSurface = Main.maxTilesY - 25;
				Main.rockLayer = Main.maxTilesY - 30;
				Generator.GenerateFromData(subworld.Structure, new(WorldBorderOutbounds));
				if (subworld.SpawnLocation != default)
				{
					AOUtils.SetWorldSpawn(subworld.SpawnLocation.Point16() + new Point16(WorldBorderOutbounds));
				}
			}
		}
	}
}
