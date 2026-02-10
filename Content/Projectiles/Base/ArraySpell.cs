using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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

		public override bool CanHaveImbueVFX => false;


		public Rectangle Proj1 => new(Projectile.Center.X.Round(), Projectile.position.Y.Round() - (20 * Projectile.scale).Round(), (64 * Projectile.scale).Round(), (64 * Projectile.scale).Round());
		public Rectangle Proj2 => new(Proj1.X - (64 * Projectile.scale).Round(), Projectile.position.Y.Round() - (20 * Projectile.scale).Round(), (64 * Projectile.scale).Round(), (64 * Projectile.scale).Round());
		public Rectangle Proj3 => new(Proj1.X + (64 * Projectile.scale).Round(), Projectile.position.Y.Round() + (20 * Projectile.scale).Round(), (64 * Projectile.scale).Round(), (64 * Projectile.scale).Round());
		public Rectangle Proj4 => new(Proj2.X - (64 * Projectile.scale).Round(), Projectile.position.Y.Round() + (20 * Projectile.scale).Round(), (64 * Projectile.scale).Round(), (64 * Projectile.scale).Round());


		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Proj1.Height + 40;
			Projectile.width = Proj1.Width + Proj2.Width + Proj3.Width + Proj4.Width;
			Projectile.timeLeft = ShootTime + ShootDelay;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Proj1.Height / 4;
			height = Proj1.Width / 4;
			fallThrough = true;
			return true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (targetHitbox.Intersects(Proj1))
				return true;
			if (targetHitbox.Intersects(Proj2))
				return true;
			if (targetHitbox.Intersects(Proj3))
				return true;
			if (targetHitbox.Intersects(Proj4))
				return true;
			return false;
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
				if (Imbue is not PhoenixMagic)
					Projectile.spriteDirection = Owner.direction;
				Projectile.scale = Imbue.AOScrollSize;
				if (SecondImbue is not null)
					Projectile.scale *= SecondImbue.AOScrollSize;
				Projectile.Center = Projectile.Center.MoveTowards(Owner.RotatedRelativePoint(Owner.MountedCenter) - new Vector2(0, (Player.defaultHeight * .75f) * Projectile.scale), AOPlayerOwner.MaxPossibleSpeed * Imbue.AOScrollSpeed);
				Projectile.scale *= AOSize;
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
			if (!Hovering || Imbue is SoundMagic)
			{
				Imbue?.LingeringEffects(Proj1, Projectile.velocity, Projectile);
				Imbue?.LingeringEffects(Proj2, Projectile.velocity, Projectile);
				Imbue?.LingeringEffects(Proj3, Projectile.velocity, Projectile);
				Imbue?.LingeringEffects(Proj4, Projectile.velocity, Projectile);
				SecondImbue?.LingeringEffects(Proj1, Projectile.velocity, Projectile);
				SecondImbue?.LingeringEffects(Proj2, Projectile.velocity, Projectile);
				SecondImbue?.LingeringEffects(Proj3, Projectile.velocity, Projectile);
				SecondImbue?.LingeringEffects(Proj4, Projectile.velocity, Projectile);
			}
		}

		public override bool PreKill(int timeLeft)
		{
			Imbue?.KillEffects(Proj1, Projectile);
			Imbue?.KillEffects(Proj2, Projectile);
			Imbue?.KillEffects(Proj3, Projectile);
			Imbue?.KillEffects(Proj4, Projectile);
			SecondImbue?.KillEffects(Proj1, Projectile);
			SecondImbue?.KillEffects(Proj2, Projectile);
			SecondImbue?.KillEffects(Proj3, Projectile);
			SecondImbue?.KillEffects(Proj4, Projectile);
			return base.PreKill(timeLeft);
		}

		public override bool? CanDamage() => !Hovering;

		public override bool OnTileCollide(Vector2 oldVelocity) => !Hovering;

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

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
			Main.EntitySpriteDraw(Sprite, Proj1.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			Main.EntitySpriteDraw(Sprite, Proj2.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			Main.EntitySpriteDraw(Sprite, Proj3.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			Main.EntitySpriteDraw(Sprite, Proj4.Center() - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			return false;
		}
	}
}
