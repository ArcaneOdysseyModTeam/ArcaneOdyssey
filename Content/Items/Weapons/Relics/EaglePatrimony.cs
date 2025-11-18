using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles.Relics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.Relics
{
	public class EaglePatrimony : RelicWeapon
	{
		public override int AOValue => 500;
		public override AORarities AORarity => AORarities.Special;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.width = Item.height = 40;
			Item.shoot = ModContent.ProjectileType<SpiritBlast>();
			Item.shootSpeed = 15;
			Item.UseSound = SoundID.Item84 with { Pitch = .75f };
			Item.damage = 25;
			Item.autoReuse = true;
			Item.useTime = Item.useAnimation = 30;
			Item.knockBack = 3.75f;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<PoseidonChoice>().DisableDecraft().Register();
		}
	}
}
