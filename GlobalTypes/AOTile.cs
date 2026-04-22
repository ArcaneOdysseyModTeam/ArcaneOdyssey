using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.EmptyScrolls;
using ArcaneOdyssey.Items.Scrolls.Equipment.Common;
using ArcaneOdyssey.Items.Scrolls.Equipment.Rare;
using ArcaneOdyssey.Items.Scrolls.Usable.Common;
using ArcaneOdyssey.Items.Scrolls.Usable.Rare;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.GlobalTypes
{
	public class AOTile : GlobalTile
	{
		public static int commonpity = 0;
		public static int rarepity = 0;
		public static int lostpity = 0;
		public override void Drop(int i, int j, int type)
		{
			if (type == TileID.Pots || (ExternalModSupport.HasCalamity && ExternalModSupport.Calamity.TryFind<ModTile>("AbyssalPots", out var tile) && type == tile.Type))
			{
				if (Player.GetClosestRollLuck(i, j, 50 - (commonpity++ / 2)) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j).ToWorldCoordinates(), Main.rand.Next(GetAllCommonScrollDrops()));
					commonpity = 0;
				}
				else if (Player.GetClosestRollLuck(i, j, 150 - (rarepity++ / 2)) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j).ToWorldCoordinates(), Main.rand.Next(GetAllRareScrollDrops()));
					rarepity = 0;
				}
				else if (Player.GetClosestRollLuck(i, j, 300 - (lostpity++ / 2)) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j).ToWorldCoordinates(), Main.rand.Next(GetAllLostScrollDrops()));
					lostpity = 0;
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
		/// Drops after at least one boss defeated
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
					AddOption<MeteorScroll>();
					AddOption<CrescendoTechnique>();
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
				options.Add(ModContent.ItemType<EmptyScroll>());
			}

			return [.. options];
		}

		/// <summary>
		/// Drops in hardmode
		/// </summary>
		/// <returns></returns>
		public static int[] GetAllLostScrollDrops()
		{
			List<int> options = [];
			//if (false)
			//{
			//	void AddOption<T>() where T : LostScroll
			//	{
			//		options.Add(ModContent.ItemType<T>());
			//	}
			//}
			//else
			//{
				options.Add(ModContent.ItemType<RareEmptyScroll>());
			//}

			return [.. options];
		}
	}
}
