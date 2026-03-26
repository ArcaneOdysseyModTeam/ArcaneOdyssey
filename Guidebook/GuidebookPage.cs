using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
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
		}

		public static int Count = 0;

		public override void Load()
		{
			GuidebookSystem.PageCount++;
			ModContent.RequestIfExists(GetType().FullName.Replace('.', '/'), out Image, AssetRequestMode.ImmediateLoad);
		}

		public LocalizedText GetText() => Mod.CoolCustomLocalization("Guidebook." + Name + ".Text", Name + " Content goes here.");

		public LocalizedText DisplayName => Mod.CoolCustomLocalization("Guidebook." + Name + ".DisplayName", PrettyPrintName);

		public abstract bool MetConditions(Player player);

		public override void Unload()
		{
			Image = null;
		}

		public static GuidebookPage Get(int page)
		{
			return GuidebookSystem.AllPages.Find(e => e.PageNum == page);
		}
	}

	//GettingStarted
	//MagicTypes
	//FightingStyles
	//Relics
	//Imbuing
	//ObtainingScrolls
	//UsingScrolls
	//Weapons
}
