namespace ArcaneOdyssey.Guidebook
{
	public class GuidebookSystem : ModSystem
	{

		public static int PageCount = 0;

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
			AllPages = Sets.Factory.CreateCustomSet<GuidebookPage>(null);
		}

		[ReinitializeDuringResizeArrays]
		public static class Sets
		{
			public static SetFactory Factory = new(PageCount, nameof(GuidebookSystem), i => GuidebookPage.Get(i).Name);
			public static bool[] AthenaPage = Factory.CreateBoolSet();
		}
	}
}
