using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.CameraModifiers;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class PulsarSpell : MagicSpell
	{
		public override string Texture => (Mod.Name + "/" + ArcaneOdysseyMod.Sets.Assets.blasts[Imbue?.Type ?? WindMagic.ID]?.Name ?? typeof(WindMagic).FullName.Replace('.', '/').Replace(nameof(WindMagic), ModContent.GetInstance<WindMagic>().AttackPrefix + "Blast")).Replace("\\", "/");

		public override Texture2D Sprite => ArcaneOdysseyMod.Sets.Assets.blasts[Imbue?.Type ?? WindMagic.ID]?.Value ?? base.Sprite;
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
				Projectile.netUpdate = true;
				Projectile.netSpam = 0;
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
				if (Main.myPlayer == Projectile.owner)
				{
					AOUtils.SimulateAOE(130, Projectile.damage / frequency, Projectile.Center, 0f, Projectile, DamageClass.Magic, false);
				}
				if (!Main.dedServ)
				{
					for (int i = 0; i < 15; i++)
					{
						Imbue?.ExplosionEffects(Projectile.Center, Projectile.scale / Size);
						SecondImbue?.ExplosionEffects(Projectile.Center, Projectile.scale / Size);
					}
					PunchCameraModifier modifier = new(Projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), ApplyKnockback(10f), ApplyKnockback(4f), 10, ApplyKnockback(500f), FullName);
					Main.instance.CameraModifiers.Add(modifier);
				}
			}
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID])
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
				if (Imbue is BlizzardMagic)
				{
					var texture = BlizzardMagic.trail;
					Main.EntitySpriteDraw(texture.Value, Projectile.Center - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Projectile.width / 2f)) - Main.screenPosition, new(0, texture.Height() / 7 * BlastSpell.TrailFrame, texture.Width(), texture.Height() / 7), Projectile.GetAlpha(lightColor), Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()).ToRotation(), new Vector2(texture.Width(), texture.Height() / 7) / 2f, Projectile.scale * .9f, SpriteEffects.None);
				}
			}
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID]) / 2f, Projectile.scale, mode);
			return false;
		}

		public Asset<Texture2D> Glow;

		public override void PostDraw(Color lightColor)
		{
			if (AOUtils.RequestIfExists(GlowTexture, ref Glow))
			{
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
				if (Glow.Height() == Sprite.Height)
				{
					var Sprite = Glow.Value;
					Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? WindMagic.ID]) / 2f, Projectile.scale, mode);
				}
				else
				{
					var Sprite = Glow.Value;
					Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation, Sprite.Size() / 2f, Projectile.scale, mode);
				}
			}
		}
	}
}
