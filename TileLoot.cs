using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Scrolls.Equipment.Common;
using ArcaneOdyssey.Content.Items.Scrolls.Equipment.Rare;
using ArcaneOdyssey.Content.Items.Scrolls.Usable.Common;
using ArcaneOdyssey.Content.Items.Scrolls.Usable.Lost;
using ArcaneOdyssey.Content.Items.Scrolls.Usable.Rare;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{

	public class TileLoot : GlobalTile
	{
		private static int commonpity = 0;
		private static int rarepity = 0;
		private static int lostpity = 0;
		public override void Drop(int i, int j, int type)
		{
			if (type == TileID.Pots || (ExternalModSupport.HasCalamity && ExternalModSupport.Calamity.TryFind<ModTile>("AbyssalPots", out var tile) && type == tile.Type))
			{
				if (Player.GetClosestRollLuck(i, j, 50 - (commonpity++ / 2)) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.Next(GetAllCommonScrollDrops()));
					commonpity = 0;
				}
				if (AOUtils.BossesKilled > 0 && Player.GetClosestRollLuck(i, j, 150 - (rarepity++ / 2)) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.Next(GetAllRareScrollDrops()));
					rarepity = 0;
				}
				if (Main.hardMode && Player.GetClosestRollLuck(i, j, 300 - (lostpity++ / 2)) == 0)
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

			void AddOption<T>() where T : Scroll
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
				AddOption<BreathtakerTechnique>();
			}

			if (NPC.downedBoss2)
			{
				AddOption<BeamScroll>();
				AddOption<HoverScroll>();
			}

			if (NPC.downedBoss3)
			{
				AddOption<AuraScroll>();
				AddOption<JavelinSpell>();
				AddOption<SelinoTechnique>();
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

			void AddOption<T>() where T : Scroll
			{
				options.Add(ModContent.ItemType<T>());
			}

			AddOption<HoundRite>();
			AddOption<WalkRite>();
			AddOption<AxeTechnique>();
			AddOption<BarrageSpell>();

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
			}

			if (Main.hardMode)
			{
				AddOption<RaySpell>();
				AddOption<MeteorScroll>();
				AddOption<FlightScroll>();
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

			void AddOption<T>() where T : Scroll
			{
				options.Add(ModContent.ItemType<T>());
			}

			AddOption<AnnihilationScroll>();
			AddOption<AcumenTechnique>();
			AddOption<CrescendoTechnique>();
			AddOption<ElementalSpell>();
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				AddOption<EnchantmentSpell>();
			}

			return [.. options];
		}
	}
}
