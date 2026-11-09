using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;
using Terraria.Audio;

namespace ArcaneOdyssey.MagicVariants.Normal.Acid
{
	public class CorrosiveVariant : MagicVariant<AcidMagic>
	{
		public override Color Colour => Color.LimeGreen;

		public override void BuffVisuals(ref int dust, ref Color drawColour, ref Color dustColour)
		{
			dust = DustID.Water_Jungle;
			drawColour = ImbueInstance.Colour;
		}

		public override bool SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_Jungle, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			return false;
		}

		public override bool LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_Jungle, Scale: 1.6f * area.RelativeScale());
			return false;
		}

		public override bool ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 6; n++)
			{
				Dust.NewDustDirect(position, 0, 0, DustID.Water_Jungle, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (35f * intensity), Scale: 3f * intensity).noGravity = true;
			}
			return false;
		}

		public override bool KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_Jungle, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueInstance.ImbueSound, area.Center());
			return false;
		}
	}
}
