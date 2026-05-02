using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class PlantMagic : MagicType
	{
		public override float Aura => .8f;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float? DashResist => 1.05f;
		public override Color ImbueColour => Color.ForestGreen;
		public override Color ImbueColour2 => Color.PaleGreen;
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Tangent;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Poisoned>(60 * 10),];
		public override SoundStyle? ImbueSound => SoundID.Grass;
		public override float ImbueSpeed => 1.05f;
		public override float ImbueSize => 1.2f;
		public override float ImbueDamage => .95f;
		public override SynergyEffects Effects => new([],
			[
				Synergy.Create<Scorched>(1.15f),
				
				Synergy.Create<Burning>(1.15f),
				
				Synergy.Create<Corroding>(1.1f),
				
				Synergy.Create<Melting>(1.1f),
				Synergy.Create<Tangled>(.9f),
			]);

		public override int BlastFrames => 2;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Demonic;

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
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Pearlwood, (Main.rand.NextFloat() - 0.5f) * (10f * intensity), (Main.rand.NextFloat() - 0.5f) * (10f * intensity), Scale: 1.5f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.GrassBlades, 15f * intensity * (Main.rand.NextFloat() - 0.5f), 15f * intensity * (Main.rand.NextFloat() - 0.5f), Scale: 2.5f * intensity)];
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

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<WoodMagic>();
		}
	}
}
