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
		public abstract ushort PageNum { get; }

		/// <summary>
		/// The optional image that comes with this page
		/// </summary>
		public Asset<Texture2D> Image;

		/// <summary>
		/// Whether this page is obtainable from finding Athena pages, rather than any specific condition
		/// </summary>
		public virtual bool AthenaPage => false;

		/// <inheritdoc/>
		protected sealed override void Register()
		{
			ModTypeLookup<GuidebookPage>.Register(this);
			GuidebookSystem.PageCount++;
			ModContent.RequestIfExists(Texture, out Image, AssetRequestMode.ImmediateLoad);
		}

		/// <inheritdoc/>
		public sealed override void SetupContent()
		{
			GuidebookSystem.AllPages[PageNum] = this;
			_ = DisplayName; // forces these to generate if they don't exist
			_ = Description;
			SetStaticDefaults();
		}

		internal static Dictionary<string, ushort> PagesOrdered = [];

		public LocalizedText Description => Mod.CoolCustomLocalization(LocalizationCategory + "." + Name + ".Text", () => PrettyPrintName() + " content goes here.");

		public LocalizedText DisplayName => Mod.CoolCustomLocalization(LocalizationCategory + "." + Name + ".DisplayName", PrettyPrintName);

		/// <inheritdoc/>
		public virtual string LocalizationCategory => "Guidebook";

		/// <summary>
		/// Allows you to set conditions for unlocking pages
		/// <para/>Called every frame, so don't do too much here
		/// </summary>
		/// <param name="player">The local player</param>
		/// <returns>Whether the page should be unlocked</returns>
		public virtual bool MetConditions(Player player) => false;

		/// <summary>
		/// Gets a guidebook page by page number
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public static GuidebookPage Get(int page)
		{
			var pages = ModContent.GetContent<GuidebookPage>();
			return pages.ToList().Find(e => e.PageNum == page);
		}

		/// <summary>
		/// Gets a guidebook page by internal name
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public static GuidebookPage Get(string page)
		{
			var pages = ModContent.GetContent<GuidebookPage>();
			return pages.ToList().Find(e => e.Name == page);
		}

		/// <summary>
		/// Gets the page number before the provided page
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public ushort Before<T>() where T : GuidebookPage
		{
			var inst = ModContent.GetInstance<T>();
			if (!PagesOrdered.ContainsKey(Name))
			{
				if (PagesOrdered.TryGetValue(inst.Name, out var value))
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

		/// <summary>
		/// Gets the page number after the provided page
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public ushort After<T>() where T : GuidebookPage
		{
			if (!PagesOrdered.ContainsKey(Name))
			{
				var inst = ModContent.GetInstance<T>();
				if (PagesOrdered.TryGetValue(inst.Name, out var value))
				{
					PagesOrdered[Name] = (ushort)(value + 1);
				}
				else
				{
					PagesOrdered[Name] = (ushort)(inst.PageNum + 1);
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
