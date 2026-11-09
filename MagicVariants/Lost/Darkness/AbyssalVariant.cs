using ArcaneOdyssey.Dusts;
using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.MagicVariants.Base;

namespace ArcaneOdyssey.MagicVariants.Lost.Darkness
{
	public class AbyssalVariant : MagicVariant<DarknessMagic>
	{
		public override Color? Colour2 => new(41, 0, 66);

		public override bool SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<AbyssalDust>(), direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			return true;
		}

		public override bool LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			if (Main.GameUpdateCount % 2 == 0)
			{
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<AbyssalDust>(), Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			return true;
		}

		public override bool ExplosionEffects(Vector2 position, float intensity = 1)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<AbyssalDust>(), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust2.noGravity = true;
			}
			return true;
		}

		public override bool KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 6; n++)
			{
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<AbyssalDust>(), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			return true;
		}
	}
}
