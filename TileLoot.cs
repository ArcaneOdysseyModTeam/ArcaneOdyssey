using ArcaneOdyssey.Content.Items.Scrolls.Equipment.Common;
using ArcaneOdyssey.Content.Items.Scrolls.Equipment.Rare;
using ArcaneOdyssey.Content.Items.Scrolls.Weapons.Common;
using ArcaneOdyssey.Content.Items.Scrolls.Weapons.Lost;
using ArcaneOdyssey.Content.Items.Scrolls.Weapons.Rare;
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
		public override void Drop(int i, int j, int type)
		{
			if (type == TileID.Pots)
			{
				if (Player.GetClosestRollLuck(i, j, 100) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.Next(GetAllCommonScrollDrops()));
				}
				if (AOUtils.BossesKilled > 0 && Player.GetClosestRollLuck(i, j, 300) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.Next(GetAllRareScrollDrops()));
				}
				if (Main.hardMode && Player.GetClosestRollLuck(i, j, 600) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.Next(GetAllLostScrollDrops()));
				}
			}

			if (ExternalModSupport.HasCalamity && ExternalModSupport.Calamity.TryFind<ModTile>("AbyssalPots", out var tile) && type == tile.Type)
			{
				if (Player.GetClosestRollLuck(i, j, 50) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.Next(GetAllCommonScrollDrops()));
				}
				if (AOUtils.BossesKilled > 0 && Player.GetClosestRollLuck(i, j, 150) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.Next(GetAllRareScrollDrops()));
				}
				if (Main.hardMode && Player.GetClosestRollLuck(i, j, 300) == 0)
				{
					Item.NewItem(new EntitySource_TileBreak(i, j), new Vector2(i, j) * 16, Main.rand.Next(GetAllLostScrollDrops()));
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

			void AddOption<T>() where T : ModItem
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

			void AddOption<T>() where T : ModItem
			{
				options.Add(ModContent.ItemType<T>());
			}

			AddOption<HoundRite>();
			AddOption<WalkRite>();

			if ((NPC.downedBoss1 && Main.expertMode) || NPC.downedBoss3)
			{
				AddOption<ReflexScroll>();
			}

			if (NPC.downedBoss3)
			{
				AddOption<ArrayScroll>();
				AddOption<PulsarScroll>();
			}

			if (Main.hardMode)
			{
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

			void AddOption<T>() where T : ModItem
			{
				options.Add(ModContent.ItemType<T>());
			}

			AddOption<AnnihilationScroll>();

			return [.. options];
		}
	}
}
