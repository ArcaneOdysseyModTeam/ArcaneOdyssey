using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.AOPlayers.DrawLayers;
using ArcaneOdyssey.Projectiles.Abilities;
using ArcaneOdyssey.Projectiles.Base;
using Terraria.Graphics.CameraModifiers;

namespace ArcaneOdyssey.Projectiles.Enemies.Argos
{
	public class ArgosShockwave : BlockableProjectile
	{
		public override string Texture => AOUtils.GetTexture<DevastateShockwave>();
		
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 12;
		}

		public override float Size => 2f;

		public const int TimeLeft = 80;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 800;
			Projectile.height = 250;
			Projectile.timeLeft = TimeLeft + AOPlayer.MaxBlock;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.DamageType = DamageClass.Melee;
		}
		private bool drawIndicator;

		public override void AI()
		{
			drawIndicator = false;
			if (Projectile.timeLeft <= TimeLeft)
			{
				if (Projectile.ai[0] == 0)
				{
					Projectile.ai[0] = 1;
					Projectile.Bottom = Projectile.Center;
					if (!Main.dedServ)
					{
						PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), 20f, 6f, 20, 300f, FullName);
						Main.instance.CameraModifiers.Add(modifier);
					}
				}
				if (++Projectile.frameCounter >= 6)
				{
					Projectile.frameCounter = 0;
					if (++Projectile.frame >= Main.projFrames[Type])
					{
						Kill();
						return;
					}
				}
			}
			else
			{
				drawIndicator = true;
			}
		}

		public override bool? CanDamage()
		{
			if (Projectile.timeLeft <= TimeLeft)
				return null;
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			BlockingIndicator.Show |= drawIndicator;
			if (Projectile.timeLeft <= TimeLeft)
			{
				lightColor = Color.Orange;
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
				var frame = Sprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
				// draw from the bottom!
				Main.EntitySpriteDraw(Sprite, Projectile.Bottom - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(frame.Width / 2f, frame.Height), Projectile.scale, mode);
			}
			return false;
		}
	}
}
