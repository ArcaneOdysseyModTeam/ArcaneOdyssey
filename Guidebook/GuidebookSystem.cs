using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Guidebook
{
	public class GuidebookSystem : ModSystem
	{
		public override void Unload()
		{
			PageCount = 0;
			AllPages = [];
			ModGuidebookPage.PagesOrdered = [];
		}

		public static int PageCount = 0;


		public static SetFactory Factory = null;


		public static ModGuidebookPage[] AllPages = [];

		public override void PostSetupContent()
		{
			foreach (var page in ModGuidebookPage.PagesOrdered.Keys)
			{
				AllPages[ModGuidebookPage.Get(page).PageNum] = ModGuidebookPage.Get(page);
			}
		}

		public override void ResizeArrays()
		{
			Factory = new(PageCount, nameof(GuidebookSystem), i => ModGuidebookPage.Get(i).Name);
			AllPages = Factory.CreateCustomSet<ModGuidebookPage>(null);
		}
	}
}
