using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.Projectiles.Relics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Enemies
{
	public class DuskRaindrop : ModProjectile
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.timeLeft = 90;
			Projectile.scale = .15f;
			Projectile.Opacity = .25f;
			Projectile.hostile = true;
		}

		public override string Texture => AOUtils.GetTexture<SpiritBlast>();
		public Imbuable Imbue = ModContent.GetInstance<NyxStaff>();

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 4;
		}

		public override bool PreKill(int timeLeft)
		{
			Imbue?.KillEffects(Projectile.Hitbox, Projectile);
			return base.PreKill(timeLeft);
		}

		public override void AI()
		{
			Imbue.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);

			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
				Projectile.netUpdate = true;
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}

			Projectile.velocity.X *= .95f;

			Projectile.velocity.Y += 0.13f;
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (ModContent.RequestIfExists<Texture2D>(Texture, out var tex))
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
				Main.EntitySpriteDraw(tex.Value, Projectile.Center - Main.screenPosition, new(0, tex.Height() / Main.projFrames[Type] * Projectile.frame, tex.Width(), tex.Height() / Main.projFrames[Type]), Projectile.GetAlpha(Imbue.ImbueColour), Projectile.rotation, new Vector2(tex.Width(), tex.Height() / Main.projFrames[Type]) / 2f, Projectile.scale, mode);
				return false;
			}
			return true;
		}
	}
}
