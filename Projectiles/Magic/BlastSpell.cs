using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class BlastSpell : MagicSpell
	{
		// ai 2 is first frame bool

		public override float AOSize => .6f;

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
				if (++Projectile.frame >= ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()])
				{
					Projectile.frame = 0;
				}
			}
		}

		public override string Texture => typeof(WindMagic).FullName.Replace('.', '/').Replace(nameof(WindMagic), ModContent.GetInstance<WindMagic>().AttackPrefix + "Blast");

		public override Texture2D Sprite => ArcaneOdysseyMod.Sets.blasts[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is BlizzardMagic)
			{
				//if (ModContent.RequestIfExists<Texture2D>(Texture + "_Overlay", out var texture))
				//{
				//	Main.EntitySpriteDraw(texture.Value, Projectile.Center - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Projectile.width / 2f)) - Main.screenPosition, new(0, texture.Width() * overlayFrame, texture.Width(), texture.Height() / OverlayFrames), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(texture.Width(), texture.Height() / OverlayFrames) / 2f, Projectile.scale * .9f, SpriteEffects.None);
				//}
			}
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
			return false;
		}
	}
}
