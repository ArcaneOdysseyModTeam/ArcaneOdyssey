using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class PulsarSpell : MagicSpell
	{
		public override string Texture => GetType().FullName.Replace('.', '/').Replace("Pulsar", "Blast");
		public override float AOSize => .5f;
		public override float AOSpeed => .25f;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				Projectile.ai[0] = 1;
			}
			var frequency = Projectile.ai[1] == 0 ? 1f : 2f;
			if (Imbue is not null)
			{
				Projectile.localAI[0] += Imbue.AOScrollSpeed * frequency;
			}
			if (SecondImbue is not null)
			{
				Projectile.localAI[0] += MathHelper.Clamp(SecondImbue.AOScrollSpeed.MultiToPercent() * frequency, 0, 3);
			}
			if (Projectile.localAI[0] >= 30)
			{
				Projectile.localAI[0] = 0;
				for (int i = 0; i < 15; i++)
				{
					Imbue?.ExplosionEffects(Projectile.Center);
					SecondImbue?.ExplosionEffects(Projectile.Center);
					Imbue?.ExplosionEffects(Projectile.Center);
				}
				if (Main.myPlayer == Projectile.owner)
					AOUtils.SimulateAOE(130, Projectile.damage / frequency, Projectile.Center, 0f, Projectile, DamageClass.Magic, false);
			}
			if (Projectile.ai[2] == 0f)
			{
				Projectile.ai[2] = 1f;
				if (Main.myPlayer == Projectile.owner)
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

		public override bool? CanDamage() => false;

		public virtual void Rotate()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
	}
}
