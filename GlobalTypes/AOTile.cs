using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Scrolls.Attacks.Common;
using ArcaneOdyssey.Items.Scrolls.Attacks.Rare;
using System.Collections.Generic;
using System.IO;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.GlobalTypes
{
	public class AOTile : GlobalTile
	{
		internal static IEnumerable<Scroll> allScrolls;
		internal static IEnumerable<CommonScroll> commonScrolls;
		internal static IEnumerable<RareScroll> rareScrolls;
		internal static IEnumerable<LostScroll> lostScrolls;
		public override void SetStaticDefaults()
		{
			allScrolls = ModContent.GetContent<Scroll>();
			commonScrolls = ModContent.GetContent<CommonScroll>();
			rareScrolls = ModContent.GetContent<RareScroll>();
			lostScrolls = ModContent.GetContent<LostScroll>();
		}

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

			foreach (var scroll in commonScrolls)
			{
				if (scroll.MetConditions())
				{
					options.Add(scroll.Type);
				}
			}

			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				options.RemoveAll(Main.LocalPlayer.HasItemInAnyInventory);
			}

			if (options.Count == 0)
			{
				options.Add(ModContent.ItemType<BlastScroll>());
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
				options.Add(ModContent.ItemType<AcumenTechnique>());
				foreach (var scroll in rareScrolls)
				{
					if (scroll.MetConditions())
					{
						options.Add(scroll.Type);
					}
				}
			}

			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				options.RemoveAll(Main.LocalPlayer.HasItemInAnyInventory);
				if (Main.LocalPlayer.ArcaneOdyssey().acumen)
				{
					options.RemoveAll(e => e == ModContent.ItemType<AcumenTechnique>());
				}
			}

			if (options.Count == 0)
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
			if (NPC.downedMoonlord)
			{
				foreach (var scroll in lostScrolls)
				{
					if (scroll.MetConditions())
					{
						options.Add(scroll.Type);
					}
				}
			}

			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				options.RemoveAll(Main.LocalPlayer.HasItemInAnyInventory);
			}

			if (options.Count == 0)
			{
				options.AddRange(GetAllRareScrollDrops());
			}

			return [.. options];
		}

		public static int[] GetAllScrollDrops()
		{
			List<int> options = [];

			foreach (var scroll in commonScrolls)
			{
				if (scroll.MetConditions())
				{
					options.Add(scroll.Type);
				}
			}

			if (Main.hardMode)
			{
				options.Add(ModContent.ItemType<AcumenTechnique>());
				foreach (var scroll in rareScrolls)
				{
					if (scroll.MetConditions())
					{
						options.Add(scroll.Type);
					}
				}
			}

			if (NPC.downedMoonlord)
			{
				foreach (var scroll in lostScrolls)
				{
					if (scroll.MetConditions())
					{
						options.Add(scroll.Type);
					}
				}
			}

			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				options.RemoveAll(Main.LocalPlayer.HasItemInAnyInventory);
			}

			if (options.Count == 0)
			{
				options.Add(ModContent.ItemType<BlastScroll>());
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
