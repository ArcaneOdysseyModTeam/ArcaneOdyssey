using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Old;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.Bronze
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
			CreateRecipe().AddIngredient<BronzeBar>(12).AddIngredient<OldSword>().AddTile(TileID.Anvils).Register();
		}

		public override bool AltFunctionUse(Player player)
		{
			if (!player.ArcaneOdyssey().ItemCooldowns.ContainsKey(Type))
			{
				player.ArcaneOdyssey().ItemCooldowns[Type] = 60 + Whirlwind.MaxTime;
				var proj = Projectile.NewProjectileDirect(Item.GetSource_FromThis(), player.Center, Vector2.UnitX * player.direction, ModContent.ProjectileType<Whirlwind>(), Item.damage, 0, player.whoAmI);
				((Whirlwind)proj.ModProjectile).color = Color.Orange;
			}
			return false;
		}
	}
}
