using ArcaneOdyssey.Items.Weapons;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class SanguineThrow : PlayerProjectile
	{
		public override string Texture => AOUtils.GetTexture<Sanguine>();

		public override float AOSize => .85f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 44;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
		}

		public override void AI()
		{
			if (Projectile.timeLeft < (600 - 60))
			{
				Projectile.rotation += Projectile.velocity.Length() / 100f * Projectile.direction;
				Projectile.velocity.Y += 0.2f;
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
				Projectile.velocity.Y += 0.1f;
			}
			if (Projectile.velocity.Y > 18f)
			{
				Projectile.velocity.Y = 18f;
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return true;
		}
	}
}
