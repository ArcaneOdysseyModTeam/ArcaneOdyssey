using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Guidebook
{
	public abstract class GuidebookPage : ILoadable
	{
		public abstract int PageNum { get; }

		public Asset<Texture2D> Image;

		public string Name => GetType().Name;

		public Mod Mod { get; set; }

		public virtual void Draw()
		{

		}

		public void Load(Mod mod)
		{
			Mod = mod;
			GuidebookSystem.AllPages.Add(this);
			ModContent.RequestIfExists(GetType().FullName.Replace('.', '/'), out Image, AssetRequestMode.ImmediateLoad);
		}

		public LocalizedText GetText() => Mod.CoolCustomLocalization("Guidebook." + Name + ".Text", Name + " Content goes here.");

		public LocalizedText DisplayName => Mod.CoolCustomLocalization("Guidebook." + Name + ".DisplayName", () => Regex.Replace(Name, "([A-Z])", " $1").Trim());

		public abstract bool MetConditions(Player player);

		public void Unload()
		{
			Mod = null;
			GuidebookSystem.AllPages.Clear();
			Image = null;
		}

		public static GuidebookPage Get(string name)
		{
			return GuidebookSystem.AllPages.Find(e => e.Name == name);
		}

		public static GuidebookPage GetInstance<T>() => Get(typeof(T).Name);
		
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
