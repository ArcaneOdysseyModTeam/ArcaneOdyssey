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
	public class DuskRaincloud : ModProjectile
	{
		public override string Texture => AOUtils.GetTexture<SpiritRaincloud>();


		public const int MaxTimeLeft = SpiritRaincloud.MaxTimeLeft;

		public Imbuable Imbue = ModContent.GetInstance<NyxStaff>();
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.DamageType = OracleDamage.Instance;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = MaxTimeLeft;
			Projectile.scale = 1.5f;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 7;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Rectangle fakebox = AOUtils.ScaleRectangleNotRef(new(Projectile.Hitbox.Center.X - 190, Projectile.Hitbox.Center.Y, 190 * 2, 700), Imbue.AOScrollSize, true, true);
			return targetHitbox.Intersects(fakebox);
		}

		public override void AI()
		{
			Imbue.LingeringEffects(Projectile.Hitbox, Projectile.velocity, Projectile);
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
				Projectile.netUpdate = true;
				Projectile.velocity = -Vector2.UnitY * 5;
			}

			if (Projectile.timeLeft <= (MaxTimeLeft - 60))
			{
				Projectile.velocity = Vector2.Zero;
				AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(Main.rand.NextFloat(-10f, 10f), Main.rand.NextFloat(2f)), ModContent.ProjectileType<DuskRaindrop>(), Projectile.damage / 2, 0f, Projectile.owner, Imbue, null, true);
			}


			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
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
