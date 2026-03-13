using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class MountainWind : AOPlayerProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.WeatherPainShot}";
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = Main.projFrames[ProjectileID.WeatherPainShot];
		}
		public override float AOSize => 1.05f;
		public override float AOSpeed => .9f;

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 60;
			Projectile.friendly = true;
			Projectile.timeLeft = 120;
			Projectile.DamageType = DamageClass.Melee;
		}

		public SlotId? sound = null;

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();

			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}

			if (!Main.dedServ)
			{
				if (!sound.HasValue || !SoundEngine.TryGetActiveSound(sound.Value, out var activeSound))
				{
					sound = SoundEngine.PlaySound(SoundID.DD2_BookStaffTwisterLoop with { Pitch = .25f }, Projectile.Center);
				}
				else
				{
					activeSound.Position = Projectile.Center;
					activeSound.Volume = 1f / Owner.ownedProjectileCounts[Type];
				}
			}

			if (++Projectile.frameCounter > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
		}

		public override void OnKill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				if (sound.HasValue)
				{
					if (SoundEngine.TryGetActiveSound(sound.Value, out var activeSound))
					{
						activeSound.Stop();
					}
				}

				SoundEngine.PlaySound(SoundID.Item66, Projectile.Center);

				if (!Main.dedServ && Imbue is null)
				{
					for (float i = 0; i < 30; i++)
					{
						var centre = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2();
						var dust = AOUtils.NewDustImperfect(centre + Projectile.Center, DustID.BubbleBurst_White, centre * (Projectile.width / 10f), 0, Colour, 1.5f);
						dust.noLight = true;
						dust.noGravity = true;
					}
				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 2;
			height /= 2;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public Color Colour => Imbue?.GetColour(Color.Gold) ?? Color.White;

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Colour;
			return base.PreDraw(ref lightColor);
		}
	}
}
