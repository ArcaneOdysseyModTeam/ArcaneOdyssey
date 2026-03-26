using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Guidebook
{
	public class GuidebookSystem : ModSystem
	{
		public override void PostSetupContent()
		{
			foreach (var page in AllPages)
			{
				_ = page.DisplayName;
				page.GetText();
			}
		}

		public override void Unload()
		{
			PageCount = 0;
		}

		public static int PageCount = 0;


		public static SetFactory Factory = new(PageCount, nameof(GuidebookPage), i => GuidebookPage.Get(i).Name);


		public static GuidebookPage[] AllPages = Factory.CreateCustomSet<GuidebookPage>(null);

		public override void ResizeArrays()
		{
			Factory = new(PageCount, nameof(GuidebookSystem), i => GuidebookPage.Get(i).Name);
			AllPages = Factory.CreateCustomSet<GuidebookPage>(null);
		}
	}
}
