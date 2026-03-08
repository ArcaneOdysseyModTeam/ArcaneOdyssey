using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class AOBaseProjectile : ModProjectile, ILocalizedModType
	{
		public override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.Content.");

		public bool VariableTexture = false;

		public override void PostAI()
		{
			VariableTexture |= Texture != $"{Mod.Name}/{TextureAssets.Projectile[Type].Name.Replace("\\", "/")}";
		}

		public virtual Texture2D Sprite => (VariableTexture ? ModContent.Request<Texture2D>(Texture) : TextureAssets.Projectile[Type]).Value;

		/// <summary>
		/// Kills the projectile.
		/// </summary>
		public void Kill()
		{
			Projectile.Kill();
		}

		public virtual SpriteEffects FlippedMode => SpriteEffects.FlipVertically;

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new(0, Sprite.Height / Main.projFrames[Type] * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(GlowTexture, out var tex))
			{
				if (tex.Height() == Sprite.Height)
				{
					SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
					Main.EntitySpriteDraw(tex.Value, Projectile.Center - Main.screenPosition, new(0, tex.Height() / Main.projFrames[Type] * Projectile.frame, tex.Width(), tex.Height() / Main.projFrames[Type]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				}
				else
				{
					SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
					Main.EntitySpriteDraw(tex.Value, Projectile.Center - Main.screenPosition, new(0, 0, tex.Width(), tex.Height()), Projectile.GetAlpha(Color.White), Projectile.rotation, tex.Size() / 2f, Projectile.scale, mode);
				}
			}
		}
	}
}
