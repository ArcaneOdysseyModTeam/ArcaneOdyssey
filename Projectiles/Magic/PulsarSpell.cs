using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class PulsarSpell : MagicSpell
	{

		public override string Texture => typeof(WindMagic).FullName.Replace('.', '/').Replace(nameof(WindMagic), ModContent.GetInstance<WindMagic>().AttackPrefix + "Blast");

		public override Texture2D Sprite => ArcaneOdysseyMod.Sets.blasts[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;
		public override float Size => .5f;
		public override float Speed => .25f;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				Projectile.ai[0] = 1;
			}
			var frequency = Projectile.ai[1] == 0 ? 1f : 2f;
			if (Imbue is not null)
			{
				Projectile.localAI[0] += Imbue.ScrollSpeed * frequency;
			}
			if (SecondImbue is not null)
			{
				Projectile.localAI[0] += MathHelper.Clamp(SecondImbue.ScrollSpeed.MultiToPercent() * frequency, 0, 3);
			}
			if (Projectile.localAI[0] >= 30)
			{
				Projectile.localAI[0] = 0;
				for (int i = 0; i < 15; i++)
				{
					Imbue?.ExplosionEffects(Projectile.Center);
					SecondImbue?.ExplosionEffects(Projectile.Center);
					Imbue?.ExplosionEffects(Projectile.Center);
				}
				if (Main.myPlayer == Projectile.owner)
					AOUtils.SimulateAOE(130, Projectile.damage / frequency, Projectile.Center, 0f, Projectile, DamageClass.Magic, false);
			}
			if (Projectile.ai[2] == 0f)
			{
				Projectile.ai[2] = 1f;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()])
				{
					Projectile.frame = 0;
				}
			}
			Imbue?.UpdateProjectile(Projectile);
		}

		public override bool? CanDamage() => false;
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
