using Terraria.GameContent;

namespace ArcaneOdyssey.Projectiles.Base
{
	public abstract class BaseProjectile : ModProjectile, ILocalizedModType
	{
		public sealed override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.");

		public virtual Asset<Texture2D> Sprite => Texture != $"{Mod.Name}/{TextureAssets.Projectile[Type]?.Name.Replace("\\", "/") ?? Texture}" ? ModContent.Request<Texture2D>(Texture) : TextureAssets.Projectile[Type];

		/// <summary>
		/// Kills the projectile.
		/// </summary>
		public void Kill()
		{
			Projectile.Kill();
		}

		public virtual SpriteEffects FlippedMode => SpriteEffects.FlipVertically;


		/// <summary>
		/// The mod manually draws projectiles to avoid offsets that come with scaling
		/// <para/> Glowmasks must be manually handled
		/// </summary>
		/// <param name="lightColor"></param>
		/// <returns></returns>
		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			var frame = Sprite.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
			Main.EntitySpriteDraw(Sprite.Value, Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, frame.Size() / 2f, Projectile.scale, mode);
			return false;
		}
	}
}
