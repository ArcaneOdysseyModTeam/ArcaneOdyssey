using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Weapons.Old
{
	public class OldGreatsword : AORangedOrMeleeWeapon
	{
		public override int AOValue => 40;
		public override float AOSize => 1f;
		public override float AOSpeed => .9f;
		public override float AODamage => 1.05f;
		public override AOUtils.AORarities AORarity => AOUtils.AORarities.Common;
		public override AOUtils.AOWeaponTiers AOWeaponTier => AOUtils.AOWeaponTiers.Poor;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.height = 60;
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddRecipeGroup(RecipeGroupID.IronBar, 32).AddTile(TileID.Hellforge).Register();
		}
	}
}
