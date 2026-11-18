using ArcaneOdyssey.Dusts;
using ArcaneOdyssey.Gimmicks.Magic;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Imbues.Magic.Lost;
using System;

namespace ArcaneOdyssey.Imbues.Magic.Mythical
{
	public class WitherLightningMagic : DeathMagic
	{
		public override ImbueGimmick Gimmick => ModContent.GetInstance<DeathShock>();
		public override float ScrollDamage => 2.25f;
		public override float ScrollSpeed => 2.5f;
		public override float ScrollSize => 1.8f;

		public override bool AnimatedColours => true;

		public override ImbuableTiers ImbuableTier => ImbuableTiers.Mythical;

		public override SynergyEffects Effects => base.Effects + AOUtils.CopySynergiesFromImbue<AncientLightningMagic>();

		public override int BlastFrames => 6;

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			base.LingeringEffects(area, direction, source);

			float waveVal = 10f * MathF.Abs((float)Main.GameUpdateCount % 5 % 10f - 2.5f) - 12.5f;
			if (source is Projectile projectile && projectile.extraUpdates > 0)
			{
				waveVal = 10f * MathF.Abs(((float)Main.GameUpdateCount + (float)projectile.numUpdates) % 5 % 10f - 2.5f) - 12.5f;
			}
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(direction.GetValueOrDefault(Vector2.One).ToRotation()), DustID.Vortex, Vector2.Zero, Scale: 2f);
			spawnedDust.noGravity = true;
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			base.KillEffects(area, source);
			Dust.NewDustPerfect(area.Center(), ModContent.DustType<LightningBurst>(), newColor: Colour, Scale: area.RelativeBurstScale()).customData = Main.rand.Next(3, 6);
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1)
		{
			base.ExplosionEffects(position, intensity);
			Dust.NewDustPerfect(position, ModContent.DustType<LightningBurst>(), newColor: Colour, Scale: intensity * 1.6f);
		}

		public override void RegisterMutations()
		{

		}

		public override MagicCircleTypes CircleType => MagicCircleTypes.Tesla;
	}
}
