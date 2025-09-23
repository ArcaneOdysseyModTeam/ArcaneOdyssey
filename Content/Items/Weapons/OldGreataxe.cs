using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class OldGreataxe : AOWeapon
	{
		public override int AOValue => 50;
		public override float AOSize => 1;
		public override float AOSpeed => 1;
		public override float AODamage => 1;
		public override AOUtils.AORarities AORarity => AOUtils.AORarities.Common;
		public override AOUtils.AOWeaponTiers AOWeaponTier => AOUtils.AOWeaponTiers.Old;

		public override void SetDefaultsWeapon()
		{
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddRecipeGroup(RecipeGroupID.IronBar, 30).AddTile(TileID.Hellforge).Register();
		}
	}
}
