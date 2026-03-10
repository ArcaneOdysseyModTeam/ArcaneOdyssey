using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Berserker;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Usable.Common
{
	public class SmashScroll : CommonScroll
	{
		public override bool CanHaveFS => true;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = 30;
			Item.damage = 30;
			Item.shoot = ModContent.ProjectileType<ShockwaveSmash>();
			Item.DamageType = AOUtils.TrueMeleeNoSpeed();
			Item.shootSpeed = 5f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, type);
			return true;
		}
	}
}
