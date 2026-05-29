using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Scrolls.Equipment.Common;
using ArcaneOdyssey.Items.Scrolls.Equipment.Rare;
using ArcaneOdyssey.Items.Scrolls.Usable.Common;
using ArcaneOdyssey.Items.Scrolls.Usable.Rare;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.GlobalTypes
{
	public class AOTile : GlobalTile
	{
		public override void Drop(int i, int j, int type)
		{
			if (type == TileID.Pots || (ExternalModSupport.HasCalamity && ExternalModSupport.Calamity.TryFind<ModTile>("AbyssalPots", out var tile) && type == tile.Type))
			{
				if (Player.GetClosestRollLuck(i, j, 50 - (ScrollPitySystem.pity++ / 2)) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j).ToWorldCoordinates(), Main.rand.Next(GetAllLostScrollDrops())); // will output the highest tier you can obtain
					ScrollPitySystem.pity = 0;
				}
			}
		}

		/// <summary>
		/// Drops with no conditions
		/// </summary>
		/// <returns></returns>
		public static int[] GetAllCommonScrollDrops()
		{
			List<int> options = [];

			void AddOption<T>() where T : CommonScroll
			{
				options.Add(ModContent.ItemType<T>());
			}

			AddOption<BlastScroll>();
			AddOption<ExplosionScroll>();
			AddOption<RainRite>();
			AddOption<CrashScroll>();
			AddOption<LeapScroll>();
			AddOption<HoverScroll>();

			if (AOUtils.BossesKilled > 0)
			{
				AddOption<EffervescenceRite>();
				AddOption<SmashScroll>();
				AddOption<HoundRite>();
				AddOption<CannonScroll>();
			}

			if ((NPC.downedBoss1 && Main.expertMode) || NPC.downedBoss3)
			{
				AddOption<ReflexScroll>();
			}

			if (NPC.downedBoss2)
			{
				AddOption<BeamScroll>();
				AddOption<BarrageSpell>();
				AddOption<BreathtakerTechnique>();
			}

			if (NPC.downedBoss3)
			{
				AddOption<AuraScroll>();
			}

			return [.. options];
		}

		/// <summary>
		/// Drops hardmode
		/// </summary>
		/// <returns></returns>
		public static int[] GetAllRareScrollDrops()
		{
			List<int> options = [];
			if (Main.hardMode)
			{
				void AddOption<T>() where T : RareScroll
				{
					options.Add(ModContent.ItemType<T>());
				}

				AddOption<ShotScroll>();
				AddOption<WalkRite>();
				AddOption<AxeTechnique>();
				AddOption<SelinoTechnique>();
				AddOption<ArrayScroll>();
				AddOption<PulsarScroll>();
				AddOption<JavelinSpell>();
				AddOption<FlightScroll>();
				AddOption<GreatjumpTechnique>();
				AddOption<ElementalSpell>();
				AddOption<SurgeSpell>();

				if (NPC.downedMechBossAny)
				{
					AddOption<RaySpell>();
					AddOption<AnnihilationScroll>();
					AddOption<CrescendoTechnique>();
				}

				if (NPC.downedPlantBoss)
				{
					AddOption<MeteorScroll>();
				}

				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					if (!Main.LocalPlayer.ArcaneOdyssey().acumen)
					{
						AddOption<AcumenTechnique>();
					}
				}
				else
				{
					AddOption<EnchantmentSpell>();
					AddOption<AcumenTechnique>();
				}

			}
			else
			{
				options.AddRange(GetAllCommonScrollDrops());
			}

			return [.. options];
		}

		/// <summary>
		/// Drops post ml
		/// </summary>
		/// <returns></returns>
		public static int[] GetAllLostScrollDrops()
		{
			List<int> options = [];
			if (false)
			{
				void AddOption<T>() where T : LostScroll
				{
					options.Add(ModContent.ItemType<T>());
				}
			}
			else
			{
				options.AddRange(GetAllRareScrollDrops());
			}

			return [.. options];
		}
	}

	public class ScrollPitySystem : ModSystem
	{
		internal static byte pity = 0;

		public override void SaveWorldData(TagCompound tag)
		{
			if (pity > 0)
			{
				tag.Add("pity", pity);
			}
		}

		public override void LoadWorldData(TagCompound tag)
		{
			pity = tag.GetByte("pity");
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(pity);
		}

		public override void NetReceive(BinaryReader reader)
		{
			pity = reader.ReadByte();
		}
	}
}
