using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class BlastSpell : MagicSpell
	{
		// ai 2 is first frame bool

		public override float AOSize => .6f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.timeLeft = 60;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
		}

		public override bool HasMagicVariant => true;

		public override void AI()
		{
			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}
			Animate();
			Rotate();
		}

		public virtual void Animate()
		{
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
		}

		public virtual void Rotate()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
	}
}
