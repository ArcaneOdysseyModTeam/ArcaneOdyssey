using ArcaneOdyssey.Guidebook;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.AOPlayers
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		public override void PreUpdateBuffs()
		{
			if (Main.myPlayer == Player.whoAmI)
			{
				foreach (var page in AvailablePages())
				{
					if (!unlockedPages.Contains(page.Name))
					{
						Main.NewText(Mod.CustomLocalization("NewGuide", page.DisplayName.Value).Value);
					}
				}

				foreach (string str in AvailablePages().Select(e => e.Name))
				{
					if (!unlockedPages.Contains(str))
						unlockedPages.Add(str);
				}
			}
		}

		private static int SortPages(GuidebookPage x, GuidebookPage y)
		{
			if (x.PageNum > y.PageNum)
			{
				return 1;
			}
			if (x.PageNum < y.PageNum)
			{
				return -1;
			}
			return 0;
		}

		internal List<string> unlockedPages = [];

		public List<GuidebookPage> AvailablePages()
		{
			List<GuidebookPage> pages = [.. GuidebookSystem.AllPages];
			pages.Sort(new Comparison<GuidebookPage>(SortPages));
			pages.RemoveAll(e => !e.MetConditions(Player));
			return pages;
		}
	}
}
