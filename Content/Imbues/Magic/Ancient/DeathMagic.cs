using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Imbues.Magic.Ancient
{
	public class DeathMagic : AOMagic
	{
		public override float DashSpeed => 1.2f; // burst
		public override bool Special => true;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Ancient;
		public override SoundStyle? ImbueSound => SoundID.NPCHit54;
		public override Color ImbueColour => Color.Lerp(new(0, 34, 41), Color.Black, Math.Abs(MathF.Tan(AOUtils.UpdateCount)));
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 1.5f;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<DeathDust>(), direction.X * 0.5f, direction.Y * 0.5f, Scale: 2f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<DeathDust>(), Scale: 2.3f)];
			spawnedDust.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<DeathDust>(), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<DeathDust>(), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 4f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}