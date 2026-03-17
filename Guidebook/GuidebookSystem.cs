using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

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
