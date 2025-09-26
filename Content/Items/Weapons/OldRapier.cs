using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using static ArcaneOdyssey.AOUtils;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class OldRapier : AORangedOrMeleeWeapon
	{
		public override int AOValue => 20;
		public override float AOSize => .9f;
		public override float AOSpeed => 1.025f;
		public override float AODamage => 1.025f;
		public override AORarities AORarity => AORarities.Common;
		public override AOWeaponTiers AOWeaponTier => AOWeaponTiers.Poor;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.height = 46;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useTurn = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddRecipeGroup(RecipeGroupID.IronBar, 15).AddTile(TileID.Hellforge).Register();
		}

		private bool canSwing = true;
		public override bool CanUseItem(Player player)
		{
			canSwing = !canSwing;
			if (canSwing)
			{
				if (Item.useStyle == ItemUseStyleID.Thrust)
					Item.useStyle = ItemUseStyleID.Swing;
				else
					Item.useStyle = ItemUseStyleID.Thrust;
			}
			return base.CanUseItem(player) && !canSwing;
		}
	}
}
