using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
	public class EmptyScroll : ModItem
	{
		public virtual int AOValue => 500;
		public virtual AORarities AORarity => AORarities.Uncommon;
		public virtual bool SpellScroll => true;

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.noMelee = true;
			Item.knockBack = 4.5f;
			Item.noUseGraphic = true;
			Item.rare = (int)AORarity;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.value = GalleonToCopper(AOValue);
			SetDefaultsScroll();
		}
		public virtual void SetDefaultsScroll() { }

		public virtual void ScrollRecipe() { }

		public override void AddRecipes()
		{
			if (Name == "EmptyScroll")
			{
				CreateRecipe().AddIngredient<Paper>(10).AddTile(TileID.Bookcases).Register();
				Recipe.Create(ItemID.PaperAirplaneA, 5).AddIngredient<Paper>().Register();
				Recipe.Create(ItemID.PaperAirplaneB, 5).AddIngredient<Paper>().Register();
				Recipe.Create(ItemID.Book).AddIngredient<Paper>().Register();
			}
			else
			{
				ScrollRecipe();
			}
		}
	}
}
