using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class ColossalCleave : PlayerProjectile
	{
		public override float Speed => .65f;
		public override float Size => 1.2f;
		public override SoundStyle? HitSound => SoundID.NPCHit42;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 3;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 60 * 3;
			Projectile.friendly = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.height = Projectile.width = 234;
			Projectile.knockBack = 4.5f;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0; ;
				}
			}

			if (++Projectile.frameCounter > 6)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}

			if (++Projectile.localAI[0] >= 30 && !Main.dedServ)
			{
				Projectile.localAI[0] = 0;
				PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), ApplyKnockback(10f), ApplyKnockback(4f), 10, ApplyKnockback(500f), FullName);
				Main.instance.CameraModifiers.Add(modifier);
				for (int i = 0; i < 10; i++)
				{
					Imbue?.ExplosionEffects(Projectile.Center, 3f);
					SecondImbue?.ExplosionEffects(Projectile.Center);
				}
				if (Imbue is not null)
				{
					SoundEngine.PlaySound(Imbue.ImbueSound, Projectile.Center);
				}
				else
				{
					SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
				}
			}

			if (Projectile.timeLeft <= 30)
			{
				Projectile.ai[1] = 1;
			}

			if (Projectile.ai[1] != 0)
			{
				Projectile.alpha += 255 / 30;
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			height = width = 1;
			fallThrough = true;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = Vector2.Zero;
			Projectile.timeLeft = 30;
			Projectile.ai[1] = 1;
			Projectile.ai[2] = 1;
			return false;
		}

		public override bool? CanDamage()
		{
			return Projectile.ai[2] == 0;
		}

		public Color Colour => Imbue?.Colour ?? Color.White;

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Colour;
			return base.PreDraw(ref lightColor);
		}
	}
}
