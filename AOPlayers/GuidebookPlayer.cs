using ArcaneOdyssey.Guidebook;
using System;
using System.Collections.Generic;
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
					if (!unlockedPages.Contains(page.FullName))
					{
						Main.NewText(Mod.CustomLocalization("Guidebook.NewGuide", page.DisplayName.Value).Value);
						unlockedPages.Add(page.FullName);
					}
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

		public List<string> unlockedPages = [];

		public List<GuidebookPage> AvailablePages()
		{
			List<GuidebookPage> pages = [.. GuidebookSystem.AllPages];
			pages.Sort(new Comparison<GuidebookPage>(SortPages));
			pages.RemoveAll(e => !(unlockedPages.Contains(e.FullName) || e.MetConditions(Player)));
			return pages;
		}

		public void AddAthenaPage()
		{
			List<GuidebookPage> pages = [.. GuidebookSystem.AllPages];
			pages.Sort(new Comparison<GuidebookPage>(SortPages));

			foreach (var page in pages)
			{
				if (page.AthenaPage && !unlockedPages.Contains(page.FullName))
				{
					Main.NewText(Mod.CustomLocalization("Guidebook.AthenaPage", page.DisplayName.Value).Value);
					unlockedPages.Add(page.FullName);
					break;
				}
			}
		}
	}
}
