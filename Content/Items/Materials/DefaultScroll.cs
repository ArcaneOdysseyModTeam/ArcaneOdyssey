using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography.X509Certificates;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
	public class DefaultScroll : ModItem
	{
		public virtual int AOValue => 500;
		public virtual int AORarity => AORarities.Rare;
		public virtual void SetDefaultsScroll() { }
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.noMelee = true;
			Item.knockBack = 4.5f;
			Item.noUseGraphic = true;
			Item.rare = AORarity;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.DamageType = DamageClass.Magic;
			Item.value = GalleonToCopper(AOValue, Item.rare);
			SetDefaultsScroll();
		}

		public override void UpdateInventory(Player player)
		{
			AOPlayer playah = player.AOPlayer();
			if (playah.imbue is not null && GetType().IsSubclassOf(typeof(DefaultScroll)))
			{
				Item.color = playah.imbue.MagicColour;
			}
			else Item.color = default;
		}

		public virtual void ScrollRecipe()
		{
			
		}

		public override void AddRecipes()
		{
			if (Name == "DefaultScroll")
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
