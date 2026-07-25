using ArcaneOdyssey.Projectiles.Abilities;
using ArcaneOdyssey.Projectiles.Base;
using Terraria.Audio;
using Terraria.Graphics.CameraModifiers;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class EvanderSlash : BaseProjectile
	{
		public override string Texture => AOUtils.GetTexture<ColossalCleave>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 60 * 3;
			Projectile.hostile = true;
			Projectile.penetrate = -1;
			Projectile.height = Projectile.width = 234;
			Projectile.ignoreWater = true;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 3;
		}


		public bool sentMessage = false;
		public override void AI()
		{
			if (ArcaneOdysseyClientConfig.Instance.AbilityText && !Main.dedServ && !sentMessage)
			{
				sentMessage = true;
				CombatText.NewText(Projectile.Hitbox, Color.Red, (DisplayName + "!").Trim(), true);
			}
			Projectile.localAI[0]++;
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
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

			if (Projectile.timeLeft <= 30)
			{
				Projectile.ai[1] = 1;
			}

			if (Projectile.ai[1] != 0)
			{
				Projectile.alpha += 255 / 30;
				Projectile.ai[2] += .075f;
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation();

				if (++Projectile.localAI[0] >= 30 && !Main.dedServ)
				{
					Projectile.localAI[0] = 0;
					SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
					for (int n = 0; n < 10; n++)
					{
						Dust spawnedDust = Main.dust[Dust.NewDust(Projectile.Center, 0, 0, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * 15f, (Main.rand.NextFloat() - 0.5f) * 15f, 255 / 2, default, 3f)];
						spawnedDust.noGravity = true;
					}
					PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), 10f, 4f, 10, 500f, FullName);
					Main.instance.CameraModifiers.Add(modifier);
				}
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
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Color.Red.MultiplyRGB(lightColor);
			return base.PreDraw(ref lightColor);
		}

		public override bool? CanDamage()
		{
			return Projectile.ai[2] < 1;
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			Projectile.ai[2] = 1;
		}
	}
}
