using ArcaneOdyssey.Items.Debug;
using ArcaneOdysseyMusic;
using System.IO;
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
		internal bool givenEliusSpawner = false;

		public override void LoadWorldData(TagCompound tag)
		{
			givenEliusSpawner = tag.GetBool("giveneliusitem");
			if (tag.ContainsKey("eliusarena"))
			{
				eliusArena = tag.GetIntArray("eliusarena").FromIntArray();
				if (eliusArena == default && ExternalModSupport.NotInSubworld)
					ArcaneOdysseyMod.NoticeQueue.Add("This world was created before Lord Elius was added, his arena has not generated.");
			}
			else
			{
				if (ExternalModSupport.NotInSubworld)
					ArcaneOdysseyMod.NoticeQueue.Add("This world was created before Lord Elius was added, his arena has not generated.");
			}
		}

		public override void SaveWorldData(TagCompound tag)
		{
			tag.Add("eliusarena", eliusArena.ToIntArray());
			if (givenEliusSpawner)
				tag.Add("giveneliusitem", givenEliusSpawner);
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(eliusArena);
			writer.Write(givenEliusSpawner);
		}

		public override void NetReceive(BinaryReader reader)
		{
			eliusArena = reader.ReadRectangle();
			givenEliusSpawner = reader.ReadBoolean();
		}

		public override void Load()
		{
			eliusArena = default;
			givenEliusSpawner = false;
		}

		public override void Unload()
		{
			eliusArena = default;
			givenEliusSpawner = false;
		}

		public override void OnWorldLoad()
		{
			eliusArena = default;
			givenEliusSpawner = false;
		}

		public override void OnWorldUnload()
		{
			eliusArena = default;
			givenEliusSpawner = false;
		}

		public override void PreUpdateWorld()
		{
			if (!givenEliusSpawner)
			{
				foreach (var player in Main.ActivePlayers)
				{
					if (Main.netMode == NetmodeID.SinglePlayer || NetMessage.DoesPlayerSlotCountAsAHost(player.whoAmI))
					{
						if (eliusArena == default && ExternalModSupport.NotInSubworld)
						{
							player.QuickSpawnItem(Main.LocalPlayer.GetSource_FromThis(), ModContent.ItemType<EliusArenaSpawner>());
							givenEliusSpawner = true;
						}
					}
				}
			}
		}
	}
}