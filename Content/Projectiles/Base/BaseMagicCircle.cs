using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class BaseMagicCircle : AOPlayerProjectile
	{
		public bool MarkedForDeath = false;
		public bool playedsound = false;

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is AOMagic)
			{
				lightColor = Imbue.GetColour();
				return base.PreDraw(ref lightColor);
			}
			else
				return false;
		}

		public override bool CanHaveImbueVFX => false;

		public override bool? CanDamage() => false;

		public override void PostAI()
		{
			if (Imbue is AOMagic)
				SecondImbue?.LingeringEffects(Projectile.Hitbox);
			if (!playedsound)
			{
				SoundEngine.PlaySound(SoundID.Item84 with { Pitch = (Imbue?.AOScrollSpeed ?? 0).MultiToPercent().Clamp(-1, 1) }, Projectile.Center);
				playedsound = true;
			}

			if (Imbue is not null)
			{
				float tempLightColorR = 0f;
				float tempLightColorG = 0f;
				float tempLightColorB = 0f;
				if (Imbue.GetColour().R != 0f)
				{
					tempLightColorR = 3f / Imbue.GetColour().R;
				}
				if (Imbue.GetColour().G != 0f)
				{
					tempLightColorG = 3f / Imbue.GetColour().G;
				}
				if (Imbue.GetColour().B != 0f)
				{
					tempLightColorB = 3f / Imbue.GetColour().B;
				}

				Lighting.AddLight(Projectile.Center, tempLightColorR, tempLightColorG, tempLightColorB);

				if (Projectile.localAI[0] > 5 && !Main.dedServ)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.scale * Projectile.width * Main.rand.NextFloat()), Projectile.position.Y + (Projectile.scale * Projectile.height * Main.rand.NextFloat())), 0, 0, DustID.SilverFlame, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, Imbue.GetColour(), 1f)];
					spawnedDust.noGravity = true;
					Projectile.localAI[0] = 0;
				}
				Projectile.localAI[0]++;

				if (MarkedForDeath && !Main.dedServ)
				{
					if (Projectile.alpha < 255)
					{
						Projectile.alpha += 255 / 60;
					}
					else Projectile.Kill();
				}
			}
		}
	}
}
