using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class CannonSpell : MagicSpell, ILocalizedModType
	{
		public override string Texture => GetType().FullName.Replace('.', '/').Replace("Cannon", "Blast");
		public override string LocalizationCategory => base.LocalizationCategory + ".Cannons." + Tier;
		public int TileTimer = 0;

		public override float AOSize => 2f;
		public override float AOSpeed => .33f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 3 * 60;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{
			if (TileTimer > 0)
				TileTimer--;
			if (Projectile.ai[2] == 0f)
			{
				Projectile.ai[2] = 1f;
				Projectile.netUpdate = true;
			}
			Animate();
			Rotate();
			if (Imbue is null || ((!Imbue.CanBeWet) && Projectile.wet))
			{
				Kill();
				return;
			}
		}

		public virtual void Animate()
		{
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}
		}

		public virtual void Rotate()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (TileTimer <= 0)
			{
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
	}
}
