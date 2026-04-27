using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class HomingArray : MagicSpell
	{
		public override float Size => .75f;

		public override string Texture => (Mod.Name + "/" + ArcaneOdysseyMod.Sets.Assets.blasts[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Name ?? typeof(WindMagic).FullName.Replace('.', '/').Replace(nameof(WindMagic), ModContent.GetInstance<WindMagic>().AttackPrefix + "Blast")).Replace("\\", "/");

		public override Texture2D Sprite => ArcaneOdysseyMod.Sets.Assets.blasts[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;

		public int Target { get => (int)Projectile.ai[0]; set => Projectile.ai[0] = value; }
		public int oldTarget;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = 120;
			Projectile.width = Projectile.height = 64;
			Projectile.ArmorPenetration += 5;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is BlizzardMagic)
			{
				var texture = BlizzardMagic.trail;
				Main.EntitySpriteDraw(texture.Value, Projectile.Center - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Projectile.width / 2f)) - Main.screenPosition, new(0, texture.Height() / 7 * BlastSpell.TrailFrame, texture.Width(), texture.Height() / 7), Projectile.GetAlpha(lightColor), Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()).ToRotation(), new Vector2(texture.Width(), texture.Height() / 7) / 2f, Projectile.scale * .9f, SpriteEffects.None);
			}
			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(lightColor), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
			return false;
		}

		public override void AI()
		{
			if (Main.myPlayer == Projectile.owner)
			{
				Target = AOUtils.ClosestNPCAt(Projectile.Center, ApplySpeed(12f) * 120, false, true)?.whoAmI ?? -1;
				if (Target != oldTarget)
				{
					NetUpdate();
				}
			}
			if (Target != -1)
			{
				var targetnpc = Main.npc[Target];
				Projectile.velocity = Projectile.velocity.ToRotation().AngleTowards(Projectile.SafeDirectionTo(targetnpc.Center).ToRotation(), ApplySpeed(MathHelper.TwoPi) / 100f).ToRotationVector2() * Projectile.velocity.Length();
			}
			Imbue?.UpdateProjectile(Projectile);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
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
					Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new(0, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()] * Projectile.frame, Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]), Projectile.GetAlpha(Color.White), Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / ArcaneOdysseyMod.Sets.BlastMaxFrames[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]) / 2f, Projectile.scale, mode);
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
