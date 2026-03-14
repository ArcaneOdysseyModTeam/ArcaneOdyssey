using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.EmptyScrolls;
using ArcaneOdyssey.Items.Scrolls.Equipment.Common;
using ArcaneOdyssey.Items.Scrolls.Equipment.Rare;
using ArcaneOdyssey.Items.Scrolls.Usable.Common;
using ArcaneOdyssey.Items.Scrolls.Usable.Lost;
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
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.Next(GetAllCommonScrollDrops()));
					commonpity = 0;
				}
				else if (Player.GetClosestRollLuck(i, j, 150 - (rarepity++ / 2)) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.Next(GetAllRareScrollDrops()));
					rarepity = 0;
				}
				else if (Player.GetClosestRollLuck(i, j, 300 - (lostpity++ / 2)) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.Next(GetAllLostScrollDrops()));
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

			if (AOUtils.BossesKilled > 0)
			{
				AddOption<SmashScroll>();
			}

			if (NPC.downedBoss2)
			{
				AddOption<BeamScroll>();
				AddOption<HoverScroll>();
			}

			if (NPC.downedBoss3)
			{
				AddOption<AuraScroll>();
			}

			if (Main.hardMode)
			{
				AddOption<ShotScroll>();
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
			if (AOUtils.BossesKilled > 0)
			{
				void AddOption<T>() where T : RareScroll
				{
					options.Add(ModContent.ItemType<T>());
				}

				AddOption<HoundRite>();
				AddOption<WalkRite>();
				AddOption<AxeTechnique>();
				AddOption<BarrageSpell>();
				AddOption<BreathtakerTechnique>();

				if ((NPC.downedBoss1 && Main.expertMode) || NPC.downedBoss3)
				{
					AddOption<ReflexScroll>();
				}

				if (NPC.downedBoss2)
				{
					AddOption<SelinoTechnique>();
				}

				if (NPC.downedBoss3)
				{
					AddOption<ArrayScroll>();
					AddOption<PulsarScroll>();
					AddOption<JavelinSpell>();
					AddOption<SelinoTechnique>();
				}

				if (Main.hardMode)
				{
					AddOption<RaySpell>();
					AddOption<MeteorScroll>();
					AddOption<FlightScroll>();
					AddOption<GreatjumpTechnique>();
				}
			}
			else
			{
				options.Add(ModContent.ItemType<CommonEmptyScroll>());
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
			if (Main.hardMode)
			{
				void AddOption<T>() where T : LostScroll
				{
					options.Add(ModContent.ItemType<T>());
				}

				AddOption<AnnihilationScroll>();
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
				AddOption<CrescendoTechnique>();
				AddOption<ElementalSpell>();
			}
			else
			{
				options.Add(ModContent.ItemType<RareEmptyScroll>());
			}

			return [.. options];
		}
	}
}
