using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.MagicVariants.Base;
using Terraria.Audio;

namespace ArcaneOdyssey.MagicVariants.Lost.Cloud
{
	public class StratusVariant : MagicVariant<CloudMagic>
	{
		public override Color Colour => new(51, 51, 51);


		public override bool SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RainCloud, direction.X * 2f, direction.Y * 2f, Scale: 6f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			return false;
		}

		public override bool LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RainCloud, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			return false;
		}

		public override bool ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.RainCloud, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
			return false;
		}

		public override bool KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RainCloud, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 6f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueInstance.ImbueSound, area.Center());
			return false;
		}
	}
}
