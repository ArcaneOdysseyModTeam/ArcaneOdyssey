using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons.Old
{
	public class OldSword : AORangedOrMeleeWeapon
	{
		public override int AOValue => 40;
		public override float AOSize => 1;
		public override float AOSpeed => 1.05f;
		public override float AODamage => .9f;
		public override AORarities AORarity => AORarities.Common;
		public override AOWeaponTiers AOWeaponTier => AOWeaponTiers.Poor;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = 42;
			Item.DamageType = TrueMelee();
			Item.height = 42;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Thrust;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddRecipeGroup(RecipeGroupID.IronBar, 24).AddTile(TileID.Hellforge).Register();
		}
	}
}
