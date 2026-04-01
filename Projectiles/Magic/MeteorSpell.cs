using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Magic
{
	public class MeteorSpell : MagicSpell
	{
		// ai 2 is first frame bool

		public override float AOSize => 3f;
		public override float AOSpeed => .5f;

		public int ExplodingTime => ApplySpeed(60 * 8, true).Round();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.timeLeft = 600;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
		}

		public SlotId? sound = null;

		public override void AI()
		{
			if (Projectile.ai[2] == 0)
			{
				Projectile.ai[2] = 1;
				Projectile.netUpdate = true;
				Projectile.netSpam = 0;
			}

			if (!Main.dedServ)
			{
				if (!sound.HasValue || !SoundEngine.TryGetActiveSound(sound.Value, out var activeSound))
				{
					sound = SoundEngine.PlaySound(SoundID.DD2_BookStaffTwisterLoop with { Pitch = .25f, IsLooped = true }, Projectile.Center);
				}
				else
				{
					activeSound.Position = Projectile.Center;
				}
			}

			//if (Imbue is SoundMagic) // manually do sound magic
			//{
			//	var DustCount = 30;
			//	for (float i = 0; i < DustCount; i++)
			//	{
			//		var centre = (MathHelper.TwoPi / DustCount * (i + Main.rand.NextFloat())).ToRotationVector2() * (64 * Projectile.scale);
			//		var dust = Dust.NewDustPerfect(Projectile.Center, DustID.MushroomTorch, centre / (DustCount * .75f), Scale: Projectile.scale);
			//		dust.noGravity = true;
			//	}
			//}
			//if (Imbue is SlashMagic)
			//	Imbue.LingeringEffects(AOUtils.ScaleRectangleNotRef(Projectile.Hitbox, 2f));
		}

		public override void OnKill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				if (sound.HasValue)
				{
					if (SoundEngine.TryGetActiveSound(sound.Value, out var activeSound))
					{
						activeSound.Stop();
					}
				}
			}
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projHitbox.Intersects(targetHitbox))
			{
				var explode = Vector2.Lerp(targetHitbox.ClosestPointInRect(projHitbox.Center()), projHitbox.ClosestPointInRect(targetHitbox.Center()), .5f);
				for (int i = 0; i < 10; i++)
				{
					Imbue?.ExplosionEffects(explode, Projectile.scale / AOSize);
					SecondImbue?.ExplosionEffects(explode, Projectile.scale / AOSize);
				}
			}
			return null;
		}

		public override string Texture => typeof(WindMagic).FullName.Replace('.', '/').Replace(nameof(WindMagic), ModContent.GetInstance<WindMagic>().AttackPrefix + "Annihilation");

		public override Texture2D Sprite => ArcaneOdysseyMod.Sets.annihilationSprites[Imbue?.Type ?? ModContent.ItemType<WindMagic>()]?.Value ?? base.Sprite;

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is BlizzardMagic)
			{
				var texture = BlizzardMagic.trail;
				Main.EntitySpriteDraw(texture.Value, Projectile.Center - (Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()) * (Projectile.width / 2f)) - Main.screenPosition, new(0, texture.Height() / 7 * BlastSpell.TrailFrame, texture.Width(), texture.Height() / 7), Projectile.GetAlpha(lightColor), Projectile.velocity.SafeNormalize(Projectile.rotation.ToRotationVector2()).ToRotation(), new Vector2(texture.Width(), texture.Height() / 7) / 2f, Projectile.scale * .9f, SpriteEffects.None);
			}
			return base.PreDraw(ref lightColor);
		}
	}
}
