using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.PlayerClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Content.Projectiles.Base
{
	/// <summary>
	/// Projectile created by the player, usually via weapon
	/// </summary>
	public abstract class AOPlayerProjectile : AOBaseProjectile, IImbuable
	{
		public virtual bool CanHaveImbueVFX => true;

		public float ApplyScrollSpeed(float value, bool flipfloat = false)
		{
			if (Imbue is not null)
			{
				if (!flipfloat)
				{
					value *= Imbue.AOScrollSpeed;
					if (SecondImbue is not null)
						value *= SecondImbue.AOScrollSpeed;
				}
				else
				{
					value *= Imbue.AOScrollSpeed.FlipFloat();
					if (SecondImbue is not null)
						value *= SecondImbue.AOScrollSpeed.FlipFloat();
				}
			}
			return value;
		}

		public float ApplyImbueSpeed(float value, bool flipfloat = false)
		{
			if (Imbue is not null)
			{
				if (!flipfloat)
				{
					value *= Imbue.AOImbueSpeed;
					if (SecondImbue is not null)
						value *= SecondImbue.AOImbueSpeed;
				}
				else
				{
					value *= Imbue.AOImbueSpeed.FlipFloat();
					if (SecondImbue is not null)
						value *= SecondImbue.AOImbueSpeed.FlipFloat();
				}
			}
			return value;
		}

		public virtual bool CanHaveImbue => true;
		public virtual bool? Cold => null;

		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

		public AOPlayer AOPlayerOwner => Owner?.ArcaneOdyssey();

		public Vector2 VisualCentre => Projectile.VisualPosition + (Projectile.Size / 2f);

		public Player Owner
		{
			get
			{
				if (Projectile.owner != 255 && Main.player[Projectile.owner]?.active == true)
				{
					return Main.player[Projectile.owner];
				}
				return null;
			}
		}

		public Imbuable Imbue
		{
			get => Projectile.ArcaneOdyssey()?.Imbue;
			set => Projectile.ArcaneOdyssey().Imbue = value;
		}

		public Imbuable SecondImbue
		{
			get => Projectile.ArcaneOdyssey()?.SecondImbue;
			set => Projectile.ArcaneOdyssey().SecondImbue = value;
		}

		public bool? BenifitsFromScrollStats => Projectile.ArcaneOdyssey()?.BenifitsFromScrollStats;

		public override void SetDefaults()
		{
			Projectile.scale *= AOSize;
		}

		public virtual float AOSpeed => 1f;
		public virtual float AOSize => 1f;

		public virtual AODebuffRequirement? Debuff => new(ModContent.BuffType<AOBleed>(), 60 * 5);
		public virtual SoundStyle? HitSound => null;

		/// <summary>
		/// Kills the projectile.
		/// </summary>
		public void Kill()
		{
			Projectile.Kill();
		}

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
			Main.EntitySpriteDraw(Sprite, VisualCentre - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(GlowTexture, out var tex))
			{
				if (tex.Height() == Sprite.Height)
				{
					SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
					Main.EntitySpriteDraw(tex.Value, VisualCentre - Main.screenPosition, new(0, tex.Height() / Main.projFrames[Type] * Projectile.frame, tex.Width(), tex.Height() / Main.projFrames[Type]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				}
				else
				{
					SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
					Main.EntitySpriteDraw(tex.Value, VisualCentre - Main.screenPosition, new(0, 0, tex.Width(), tex.Height()), Projectile.GetAlpha(Color.White), Projectile.rotation, tex.Size() / 2f, Projectile.scale, mode);
				}
			}
		}
	}
}
