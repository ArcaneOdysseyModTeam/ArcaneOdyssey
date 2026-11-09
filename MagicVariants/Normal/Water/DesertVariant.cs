using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;
using Terraria.Audio;

namespace ArcaneOdyssey.MagicVariants.Normal.Water
{
	public class DesertVariant : MagicVariant<WaterMagic>
	{
		public override Color Colour => Color.Brown;

		public override bool SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.DesertWater2, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			return false;
		}

		public override bool LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.DesertWater2, Scale: 1.2f * area.RelativeScale());
			return false;
		}

		public override bool ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.DesertWater2, (Main.rand.NextFloat() - 0.5f) * (25f * intensity), (Main.rand.NextFloat() - 0.5f) * (35f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
			return false;
		}

		public override bool KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.DesertWater2, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueInstance.ImbueSound, area.Center());
			return false;
		}
	}
}
