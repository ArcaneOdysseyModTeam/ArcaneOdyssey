using ArcaneOdyssey.Content.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class BaseMagicCircle : AOPlayerProjectile
	{
		public bool MarkedForDeath = false;
		public bool playedsound = false;

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is null or AOMagic)
			{
				lightColor = Imbue?.GetColour(Color.White) ?? Color.White;
				return base.PreDraw(ref lightColor);
			}
			else
				return false;
		}

		public override bool CanHaveImbueVFX => false;

		public override bool? CanDamage() => false;

		public override void PostAI()
		{
			base.PostAI();
			Imbue ??= ModContent.GetInstance<WindMagic>();

			if (!playedsound)
			{
				SoundEngine.PlaySound(SoundID.Item84 with { Pitch = (Imbue?.AOScrollSpeed ?? 0).MultiToPercent().Clamp(-1, 1) }, Projectile.Center);
				playedsound = true;
			}

			if (Imbue is AOMagic && !Main.dedServ)
			{
				var hitbox = AOUtils.ScaleRectangleNotRef(Projectile.Hitbox, .5f);
				SecondImbue?.LingeringEffects(hitbox);
				Dust spawnedDust = Main.dust[Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.SilverFlame, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, Imbue.GetColour())];
				spawnedDust.noGravity = true;
			}

			if (MarkedForDeath && !Main.dedServ)
			{
				if (Projectile.alpha < 255)
				{
					Projectile.alpha += 255 / 60;
				}
			}

			if (Projectile.alpha >= 255 && Main.myPlayer == Projectile.owner)
			{
				Kill();
			}
		}
	}
}
