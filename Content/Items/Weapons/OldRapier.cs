using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class OldRapier : AOWeapon
	{
		public override int AOValue => 20;
		public override float AOSize => 1;
		public override float AOSpeed => 1;
		public override float AODamage => 1;
		public override AORarities AORarity => AORarities.Common;
		public override AOWeaponTiers AOWeaponTier => AOWeaponTiers.Old;

		public override void SetDefaultsWeapon()
		{
			Item.height = Item.height = 46;
			Item.useStyle = ItemUseStyleID.Thrust;
			Item.useTurn = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddRecipeGroup(RecipeGroupID.IronBar, 15).AddTile(TileID.Hellforge).Register();
		}
	}
}
