using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Relics
{
	public class SpiritHound : SpiritProjectile
	{
		public int TileTimer = 0;
		public override float AOSpeed => .9f;

		public const int TimeLeftMax = 60 * 5;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = 84;
			Projectile.width = 104;
			Projectile.AverageDimensions();
			Projectile.penetrate = 2;
			Projectile.localNPCHitCooldown = 60;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.timeLeft = TimeLeftMax;
			Projectile.ignoreWater = true;
			Projectile.Opacity = .75f;
		}

		public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction;

			if (TileTimer > 0)
				TileTimer--;

			if (Projectile.timeLeft == (TimeLeftMax / 2))
			{
				Projectile.penetrate--;
				Imbue?.KillEffects(Projectile.Hitbox);
			}

			if (Projectile.ai[2] == 0f)
			{
				Projectile.ai[2] = 1f;
				Projectile.netUpdate = true;
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Projectile.penetrate == Projectile.maxPenetrate)
			{
				Imbue?.KillEffects(Projectile.Hitbox);
				Projectile.timeLeft -= TimeLeftMax / 2;
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (TileTimer <= 0)
			{
				Projectile.penetrate--;
				Imbue?.KillEffects(Projectile.Hitbox);
			}
			if (TileTimer < 60 && TileTimer > 0)
			{
				return true;
			}
			Projectile.velocity = Projectile.oldVelocity;
			Projectile.position = Projectile.oldPosition;
			TileTimer = 65;
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue?.GetColour() ?? Color.White;
			return base.PreDraw(ref lightColor);
		}
	}
}
