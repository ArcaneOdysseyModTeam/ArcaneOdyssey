using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles.Berserker;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.Scrolls
{
	public class ShotScroll : TechniqueScroll
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = 30;
			Item.damage = 50;
			Item.shoot = ModContent.ProjectileType<ShotTechnique>();
			Item.shootSpeed = 5f;
			Item.DamageType = DamageClass.Melee;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.SlapHand).Register();
		}
	}
}
