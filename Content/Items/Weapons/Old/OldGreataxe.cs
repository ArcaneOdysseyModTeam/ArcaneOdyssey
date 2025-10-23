using ArcaneOdyssey.Content.Items.Base;
using Terraria.ID;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons.Old
{
	public class OldGreataxe : AORangedOrMeleeWeapon
	{
		public override int AOValue => 50;
		public override float AOSize => 1.05f;
		public override float AOSpeed => .9f;
		public override float AODamage => 1;
		public override AORarities AORarity => AORarities.Common;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Poor;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.height = 70;
			Item.axe = 70 / 5;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = TrueMelee();
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddRecipeGroup(RecipeGroupID.IronBar, 32).AddTile(TileID.Hellforge).Register();
		}
	}
}
