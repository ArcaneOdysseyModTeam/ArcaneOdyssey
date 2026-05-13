using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Guidebook
{
	public abstract class GuidebookPage : ModTexturedType, ILocalizedModType
	{
		/// <summary>
		/// use <seealso cref="Before{T}"/> or <seealso cref="After{T}"/>
		/// </summary>
		public abstract int PageNum { get; }

		public Asset<Texture2D> Image;

		public virtual bool AthenaPage => false;

		protected sealed override void Register()
		{
			ModTypeLookup<GuidebookPage>.Register(this);
			GuidebookSystem.PageCount++;
			ModContent.RequestIfExists(Texture, out Image, AssetRequestMode.ImmediateLoad);
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

		public virtual bool MetConditions(Player player) => false;

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
			if (!PagesOrdered.ContainsKey(Name))
			{
				var inst = ModContent.GetInstance<T>();
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
