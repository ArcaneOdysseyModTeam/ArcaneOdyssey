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
	public class EmptyTechniqueScroll : ModItem
	{
		public virtual int AOValue => 500;
		public virtual AORarities AORarity => AORarities.Uncommon;
		public virtual void SetDefaultsScroll() { }
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.noMelee = true;
			Item.knockBack = 4.5f;
			Item.noUseGraphic = true;
			Item.rare = (int)AORarity;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.DamageType = DamageClass.Melee;
			Item.value = GalleonToCopper(AOValue);
			SetDefaultsScroll();
		}

		public override void UpdateInventory(Player player)
		{
			AOPlayer playah = player.ArcaneOdyssey();
			if (playah.imbue is FightingStyle && Name != "EmptyTechniqueScroll")
			{
				Item.color = playah.imbue.ImbueColour;
				if (Item.color == Color.White || Item.color == Color.Black)
				{
					Item.color.A *= (byte).5f;
				}
			}
			else Item.color = default;
		}

		public virtual void ScrollRecipe() {}

		public override void AddRecipes()
		{
			if (Name == "EmptyTechniqueScroll")
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

		public override bool CanUseItem(Player player)
		{
			return player.ArcaneOdyssey().imbue is FightingStyle && Name != "EmptyTechniqueScroll";
		}
	}
}
