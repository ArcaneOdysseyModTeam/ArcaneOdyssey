using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class BlastSpell : MagicSpell
	{
		// ai 2 is first frame bool

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.timeLeft = 60;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
		}

		public override void AI()
		{
			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}

			Imbue?.UpdateProjectile(Projectile);

			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= ImbueID.Sets.BlastFrames[Imbue?.ID ?? WindMagic.StaticID])
				{
					Projectile.frame = 0;
				}
			}
		}

		public override string Texture => AOUtils.BlankTexture;

		public override Asset<Texture2D> Sprite => Imbue?.Blast ?? base.Sprite;

		public static int TrailFrame => (int)(Main.GameUpdateCount / 15 % 7);

		public override bool PreDraw(ref Color lightColor)
		{
			Rectangle frame;
			GetDrawColour(ref lightColor);
			if (Imbue is BlizzardMagic)
			{
				var texture = BlizzardMagic.trail;
				frame = texture.Frame(1, 7, 0, TrailFrame);
				Main.EntitySpriteDraw(texture.Value, Projectile.Center - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Projectile.width / 2f)) - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()).ToRotation(), frame.Size() / 2f, Projectile.scale * .9f, SpriteEffects.None);
			}
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;

			frame = Sprite.Frame(1, ImbueID.Sets.BlastFrames[Imbue?.ID ?? WindMagic.StaticID], 0, Projectile.frame);

			Main.EntitySpriteDraw(Sprite.Value, Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, frame.Size() / 2f, Projectile.scale, mode);
			return false;
		}
	}
}
