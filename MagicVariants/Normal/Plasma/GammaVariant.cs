using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Normal.Plasma
{
	public class GammaVariant : MagicVariant<PlasmaMagic>
	{
		public override bool LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.ShadowbeamStaff, Scale: 2f * area.RelativeScale())];
			spawnedDust2.noGravity = true;
			return true;
		}

		public override bool KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.ShadowbeamStaff, 5f * (Main.rand.NextFloat() - 0.5f), 5f * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale()).noGravity = true;
			}
			return true;
		}
	}
}
