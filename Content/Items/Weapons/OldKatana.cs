using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class OldKatana : AORangedOrMeleeWeapon
	{
		public override int AOValue => 40;
		public override float AOSize => 1;
		public override float AOSpeed => 1.05f;
		public override float AODamage => .9f;
		public override AOUtils.AORarities AORarity => AOUtils.AORarities.Common;
		public override AOUtils.AOWeaponTiers AOWeaponTier => AOUtils.AOWeaponTiers.Poor;

		public override void SetDefaultsWeapon()
		{
			Item.height = 74;
			Item.height = 64;
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddRecipeGroup(RecipeGroupID.IronBar, 30).AddTile(TileID.Hellforge).Register();
		}
	}
}
