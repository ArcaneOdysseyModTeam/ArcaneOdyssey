using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class PlantMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override Color ImbueColour => Color.ForestGreen;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Poisoned, 60 * 10),];
		public override SoundStyle? ImbueSound => SoundID.Grass;
		public override float AOImbueSpeed => 1.05f;
		public override float AOImbueSize => 1.2f;
		public override float AOImbueDamage => .95f;
		public override SynergyEffects Effects => new([],
			[
				new(BuffID.Poisoned, 1.2f),
				new(BuffID.ShadowFlame,1.15f),
				new(BuffID.OnFire,1.15f),
				new(BuffID.Venom,1.1f),
				new(BuffID.OnFire3,1.1f),
			]);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pearlwood, direction.X * 0.2f, direction.Y * 0.2f, Scale: 1.5f * area.RelativeScale());
			}
		}
		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pearlwood, direction.GetValueOrDefault().X * 0.2f, direction.GetValueOrDefault().Y * 0.2f, Scale: 1f * area.RelativeScale());
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.GrassBlades, direction.GetValueOrDefault().X * 0.2f, direction.GetValueOrDefault().Y * 0.2f, Scale: 1.5f * area.RelativeScale());
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Pearlwood, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 2.5f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.GrassBlades, 8f * intensity * (Main.rand.NextFloat() - 0.5f), 8f * intensity * (Main.rand.NextFloat() - 0.5f), Scale: 1.5f * intensity)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pearlwood, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.GrassBlades, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 1.5f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
