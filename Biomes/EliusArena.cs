using ArcaneOdysseyMusic;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Biomes
{
	/// <summary>
	/// Djin Ruins
	/// </summary>
	public class EliusArena : ModBiome
	{
		public override bool IsBiomeActive(Player player)
		{
			var playercoords = player.Hitbox.ToTileRect();
			if (EliusArenaLoader.eliusArena.Intersects(playercoords))
			{
				return ExternalModSupport.NotInSubworld;
			}
			return false;
		}

		public override int Music => MusicTrack.Djin.MusicSlot;

		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;

		public override string MapBackground => BackgroundPath;
	}

	public class EliusArenaLoader : ModSystem
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
				if (eliusArena == default && ExternalModSupport.NotInSubworld)
					ArcaneOdysseyMod.NoticeQueue.Add("This world was created before Lord Elius was added. His arena has not generated. You cannot fight him.");
			}
			else
			{
				if (ExternalModSupport.NotInSubworld)
					ArcaneOdysseyMod.NoticeQueue.Add("This world was created before Lord Elius was added. His arena has not generated. You cannot fight him.");
			}
		}

		public override void SaveWorldData(TagCompound tag)
		{
			tag.Add("eliusarena", eliusArena.ToIntArray());
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(eliusArena);
		}

		public override void NetReceive(BinaryReader reader)
		{
			eliusArena = reader.ReadRectangle();
		}

		public override void Load()
		{
			eliusArena = default;
		}

		public override void Unload()
		{
			eliusArena = default;
		}

		public override void OnWorldLoad()
		{
			eliusArena = default;
		}

		public override void OnWorldUnload()
		{
			eliusArena = default;
		}
	}
}
