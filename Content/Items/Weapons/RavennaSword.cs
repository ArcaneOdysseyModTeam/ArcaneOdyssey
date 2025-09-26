using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class RavennaSword : AORangedOrMeleeWeapon
	{
		public override int AOValue => 50;
		public override float AOSize => 1;
		public override float AOSpeed => .925f;
		public override float AODamage => 1.05f;
		public override AOUtils.AORarities AORarity => AOUtils.AORarities.Common;
		public override AOUtils.AOWeaponTiers AOWeaponTier => AOUtils.AOWeaponTiers.Average;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = 40;
			Item.height = 40;
			Item.useTurn = true;
			Item.useStyle = ItemUseStyleID.Thrust;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(12).AddTile(TileID.Anvils).Register(); // ravenna sword will be only pre-hardmode bronze weapon
		}

		public override bool AltFunctionUse(Player player)
		{
			// whirlwind here
			return false;
		}
	}
}
