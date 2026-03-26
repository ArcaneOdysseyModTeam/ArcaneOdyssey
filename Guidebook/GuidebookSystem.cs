using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Guidebook
{
	public class GuidebookSystem : ModSystem
	{
		public override void Unload()
		{
			PageCount = 0;
		}

		public static int PageCount = 0;


		public static SetFactory Factory = null;


		public static GuidebookPage[] AllPages = [];

		public override void PostSetupContent()
		{
			foreach (var page in GuidebookPage.PagesOrdered.Keys)
			{
				AllPages[GuidebookPage.Get(page).PageNum] = GuidebookPage.Get(page);
			}
		}

		public override void ResizeArrays()
		{
			Factory = new(PageCount, nameof(GuidebookSystem), i => GuidebookPage.Get(i).Name);
			AllPages = Factory.CreateCustomSet<GuidebookPage>(null);
		}
	}
}
