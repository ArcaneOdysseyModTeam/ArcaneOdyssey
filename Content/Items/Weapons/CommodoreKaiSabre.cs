using ArcaneOdyssey.Content.Buffs;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Steamworks;
using System.Collections.Generic;
using System.Linq.Expressions;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class CommodoreKaiSabre : AORangedOrMeleeWeapon
	{
		public override float AOSpeed => 1.1f;
		public override float AOSize => 1.1f;
		public override float AODamage => .925f;
		public override int AOValue => 200;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 52;
			Item.height = 54;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<KatanaSlash>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			var proj = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI);
			((KatanaSlash)proj.ModProjectile).color = this.Imbue() is not null ? Color.Lerp(Color.Red, this.Imbue().ImbueColour, .5f) : Color.Red;
			return false;
		}
	}
}
