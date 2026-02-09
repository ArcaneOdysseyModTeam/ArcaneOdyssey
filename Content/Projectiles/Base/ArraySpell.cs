using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class ArraySpell : MagicSpell, ILocalizedModType
	{
		// ai 2 is first frame bool
		public override string Texture => GetType().FullName.Replace('.', '/').Replace("Array", "Blast");

		public override string LocalizationCategory => base.LocalizationCategory + ".Arrays." + Tier;

		public override float AOSize => .6f;

		public const int ShootDelay = 60 * 3;

		public const int ShootTime = 90;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = 75;
			Projectile.width = 100;
			Projectile.timeLeft = ShootTime + ShootDelay;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
		}

		public bool Hovering
		{
			get => Projectile.ai[0] == 0;
			set
			{
				if (value)
				{
					Projectile.ai[0] = 0;
				}
				else
				{
					Projectile.ai[0] = 1;
				}
			}
		}

		public int target = -1;
		internal Vector2 originalVelocity;

		public override void AI()
		{
			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				Projectile.netUpdate = true;
				originalVelocity = Projectile.velocity;
				Projectile.velocity = Vector2.Zero;
			}
			Animate();
			Rotate();
			if (Imbue is null || ((!Imbue.CanBeWet) && Projectile.wet))
			{
				Kill();
				return;
			}

			if (Hovering)
			{
				Projectile.scale = Imbue.AOScrollSize;
				if (SecondImbue is not null)
					Projectile.scale *= SecondImbue.AOScrollSize;
				Projectile.Center = Projectile.Center.MoveTowards(Owner.RotatedRelativePoint(Owner.MountedCenter) - new Vector2(0, (Player.defaultHeight * .75f) * Projectile.scale), AOPlayerOwner.MaxPossibleSpeed * .92f);
				Projectile.scale *= BaseScale;
				target = Projectile.FindTargetWithLineOfSight(originalVelocity.Length() * ShootTime);
				if (target != -1)
				{
					var targetnpc = Main.npc[target];
					if (ArcaneOdysseyConfig.Instance.PredictiveArray)
					{
						Projectile.rotation = Projectile.SafeDirectionTo(targetnpc.Center + (targetnpc.velocity * 40f)).ToRotation();
					}
					else
					{
						Projectile.rotation = Projectile.SafeDirectionTo(targetnpc.Center).ToRotation();
					}
				}
				else
				{
					Projectile.rotation = Projectile.Center.AngleTo(Main.MouseWorld);
				}

				if (++Projectile.ai[1] > ShootDelay)
				{
					Hovering = false;
					Projectile.velocity = Projectile.rotation.ToRotationVector2() * originalVelocity.Length();
				}
			}
		}

		public override bool? CanDamage() => !Hovering;

		public override bool OnTileCollide(Vector2 oldVelocity) => !Hovering;

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
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
			Main.EntitySpriteDraw(Sprite, Projectile.Center - (new Vector2(20, 0) * Projectile.scale) - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			Main.EntitySpriteDraw(Sprite, Projectile.Center - (new Vector2(-20, 0) * Projectile.scale) - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			Main.EntitySpriteDraw(Sprite, Projectile.Center - (new Vector2(50, -20) * Projectile.scale) - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			Main.EntitySpriteDraw(Sprite, Projectile.Center - (new Vector2(-50, -20) * Projectile.scale) - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			return false;
		}
	}
}
