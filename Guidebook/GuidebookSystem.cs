using System.Collections.Generic;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Guidebook
{
	public class GuidebookSystem : ModSystem
	{
		public override void PostSetupContent()
		{
			foreach (var page in AllPages)
			{
				page.GetText();
			}
		}

		public static List<GuidebookPage> AllPages = [];
	}
}
