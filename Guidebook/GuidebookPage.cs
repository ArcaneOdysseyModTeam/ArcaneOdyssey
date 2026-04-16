using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Guidebook
{
	public abstract class GuidebookPage : ModType, ILocalizedModType
	{
		public abstract int PageNum { get; }

		public virtual PageOrdering Position { get; }

		public class PageOrdering
		{
			public int PageNum { get; set; }
		}

		public Asset<Texture2D> Image;

		protected sealed override void Register()
		{
			ModTypeLookup<GuidebookPage>.Register(this);
			GuidebookSystem.PageCount++;
			ModContent.RequestIfExists(GetType().FullName.Replace('.', '/'), out Image, AssetRequestMode.ImmediateLoad);
		}

		public sealed override void SetupContent()
		{
			GuidebookSystem.AllPages[PageNum] = this;
			_ = DisplayName;
			_ = Description;
			SetStaticDefaults();
		}

		public static int Count = 0;

		internal static Dictionary<string, int> PagesOrdered = [];

		public LocalizedText Description => Mod.CoolCustomLocalization(LocalizationCategory + "." + Name + ".Text", () => PrettyPrintName() + " content goes here.");

		public LocalizedText DisplayName => Mod.CoolCustomLocalization(LocalizationCategory + "." + Name + ".DisplayName", PrettyPrintName);

		public virtual string LocalizationCategory => "Guidebook";

		public abstract bool MetConditions(Player player);

		public static GuidebookPage Get(int page)
		{
			var pages = ModContent.GetContent<GuidebookPage>();
			return pages.ToList().Find(e => e.PageNum == page);
		}

		public static GuidebookPage Get(string page)
		{
			var pages = ModContent.GetContent<GuidebookPage>();
			return pages.ToList().Find(e => e.Name == page);
		}

		public int Before<T>() where T : GuidebookPage
		{
			var inst = ModContent.GetInstance<T>();
			if (!PagesOrdered.ContainsKey(Name))
			{
				if (PagesOrdered.TryGetValue(inst.Name, out int value))
				{
					PagesOrdered[Name] = value;
				}
				else
				{
					PagesOrdered[Name] = inst.PageNum;
				}
				var keys = PagesOrdered.Keys;
				foreach (var val in keys)
				{
					if (PagesOrdered[val] >= PagesOrdered[Name] && val != Name)
					{
						PagesOrdered[val]++;
					}
				}
			}
			return PagesOrdered[Name];
		}

		public int After<T>() where T : GuidebookPage
		{
			var inst = ModContent.GetInstance<T>();
			if (!PagesOrdered.ContainsKey(Name))
			{
				if (PagesOrdered.TryGetValue(inst.Name, out int value))
				{
					PagesOrdered[Name] = value + 1;
				}
				else
				{
					PagesOrdered[Name] = inst.PageNum + 1;
				}
				var keys = PagesOrdered.Keys;
				foreach (var val in keys)
				{
					if (PagesOrdered[val] >= PagesOrdered[Name] && val != Name)
					{
						PagesOrdered[val]++;
					}
				}
			}
			return PagesOrdered[Name];
		}
	}
}
