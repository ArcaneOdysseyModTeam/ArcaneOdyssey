using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Biomes
{
	/// <summary>
	/// Djin Ruins
	/// </summary>
	public class EliusArena : ModBiome
	{
		public override int BiomeTorchItemType => ItemID.Torch;

		public override int BiomeCampfireItemType => ItemID.Campfire;


		public override bool IsBiomeActive(Player player)
		{
			return ArenaLoader.eliusArena.Intersects(player.Hitbox);
		}

		public override void OnInBiome(Player player)
		{
			player.noBuilding = true;
		}

		public override int Music => -1;

		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
	}

	public class ArenaLoader : ModSystem
	{
		/// <summary>
		/// Area of the elius arena, in tile coordinates
		/// </summary>
		public static Rectangle eliusArena;

		public override void LoadWorldData(TagCompound tag)
		{
			if (tag.ContainsKey("eliusarena"))
			{
				eliusArena = tag.GetIntArray("eliusarena").FromIntArray();
			}
			else
			{
				ArcaneOdysseyMod.NoticeQueue.Add("This world was created before Lord Elius was added. His arena has not generated. You cannot fight him.");
			}
		}

		public override void SaveWorldData(TagCompound tag)
		{
			if (eliusArena != default)
				tag.Add("eliusarena", eliusArena.ToIntArray());
		}
	}
}
