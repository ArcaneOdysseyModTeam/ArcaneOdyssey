using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Guidebook
{
	public abstract class GuidebookPage : ModType
	{
		public abstract int PageNum { get; }

		public Asset<Texture2D> Image;

		protected sealed override void Register()
		{
			ModTypeLookup<GuidebookPage>.Register(this);
		}

		public sealed override void SetupContent() => SetStaticDefaults();

		public override void SetStaticDefaults()
		{
			GuidebookSystem.AllPages[PageNum] = this;
			_ = DisplayName;
			_ = Description;
		}

		public static int Count = 0;

		internal static Dictionary<string, int> PagesOrdered = [];

		public override void Load()
		{
			GuidebookSystem.PageCount++;
			ModContent.RequestIfExists(GetType().FullName.Replace('.', '/'), out Image, AssetRequestMode.ImmediateLoad);
		}

		public LocalizedText Description => Mod.CoolCustomLocalization("Guidebook." + Name + ".Text", Name + " Content goes here.");

		public LocalizedText DisplayName => Mod.CoolCustomLocalization("Guidebook." + Name + ".DisplayName", PrettyPrintName);

		public abstract bool MetConditions(Player player);

		public override void Unload()
		{
			Image = null;
		}

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
