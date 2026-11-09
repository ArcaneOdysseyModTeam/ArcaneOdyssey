using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Normal.Plasma
{
	public class NovaVariant : MagicVariant<PlasmaMagic>
	{
		public override Color Colour => new(255, 87, 101);
		public override bool LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.ViciousPowder, Scale: 2f * area.RelativeScale())];
			spawnedDust2.noGravity = true;
			return true;
		}

		public override bool ExplosionEffects(Vector2 position, float intensity = 1)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDustDirect(position, 0, 0, DustID.Firework_Red, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity).noGravity = true;
			}
			return false;
		}

		public override bool KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.ViciousPowder, 5f * (Main.rand.NextFloat() - 0.5f), 5f * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale()).noGravity = true;
			}
			return true;
		}
	}
}
