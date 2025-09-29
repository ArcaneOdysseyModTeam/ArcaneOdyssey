using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Weapons.Old
{
	public class OldGreataxe : AORangedOrMeleeWeapon
	{
		public override int AOValue => 50;
		public override float AOSize => 1.05f;
		public override float AOSpeed => .9f;
		public override float AODamage => 1;
		public override AOUtils.AORarities AORarity => AOUtils.AORarities.Common;
		public override AOUtils.AOWeaponTiers AOWeaponTier => AOUtils.AOWeaponTiers.Poor;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.height = 70;
			Item.axe = 70 / 5;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddRecipeGroup(RecipeGroupID.IronBar, 30).AddTile(TileID.Hellforge).Register();
		}
	}
}
