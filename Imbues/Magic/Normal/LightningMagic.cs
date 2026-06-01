using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Projectiles.Magic.Effects;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Magic.Normal
{
	public class LightningMagic : MagicType
	{
		public override void RegisterMutations()
		{
			RegisterMutation<AncientLightningMagic>();
			RegisterMutation<AetherLightningMagic>();
			RegisterMutation<EnergyMagic>();
			RegisterMutation<PoisonLightningMagic>();
			RegisterMutation<SoundMagic>();
			RegisterMutation<StormMagic>();
		}
		public override bool ImmuneDash => true; // instant
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningBugZap with { Volume = 2.25f };
		public override Color ImbueColour => new(255, 140, 255);
		public override float ImbueSpeed => 1.2f;
		public override float ImbueSize => .95f;
		public override float ImbueDamage => .95f;
		public override float ScrollSpeed => 1.4f;
		public override float ScrollSize => 1f;
		public override float ScrollDamage => .875f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Paralyzed>(60, 33)];
		public override Combo[] CombinedDebuffs => [Combo.Create<Soaked, Paralyzed>()];

		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Petrified>(), // petrified
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<SandyEffect>(),
				ClearBuff.Create<Bleeding>(),
				ClearBuff.Create<Frozen>()
			],
			[
				Synergy.Create<FreezingEffect>( 1.2f), // frozen
				Synergy.Create<Bleeding>(1.2f), // bleeding
				 // scalding
				 // melting/hellfire
				Synergy.Create<Melting>(1.075f),
				 // venom acid
				Synergy.Create<Corroding>(1.075f),
				Synergy.Create<Soaked>( 1.05f), // 
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Flammable>(0.96f),
				Synergy.Create<Crystallized>(1.075f),
				Synergy.Create<SearedEffect>(1.15f)
			]
			);

		public override int BlastFrames => 6;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Tesla;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, direction.X * 0.2f, direction.Y * 0.2f, Scale: 1.2f * area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			var updates = (float)Main.GameUpdateCount;
			if (source is Projectile projectile && projectile.extraUpdates > 0)
			{
				updates += projectile.numUpdates;
			}
			float waveVal = 10f * MathF.Abs(updates % 5f % 10f - 2.5f) - 12.5f;
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(direction.GetValueOrDefault(Vector2.One).ToRotation()), DustID.CrystalPulse, Vector2.Zero, Scale: 1.2f);
			spawnedDust.noGravity = true;

			Lighting.AddLight(area.Center(), 2, 1, 2);
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, Scale: 0.4f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDustDirect(position, 0, 0, DustID.WitherLightning, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 1.2f * intensity).noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 5; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 1.2f * area.RelativeScale());
			}
			if (source is Projectile proj && Main.myPlayer == proj.owner)
				Projectile.NewProjectile(source.GetSource_FromThis(), area.Center(), Vector2.Zero, ModContent.ProjectileType<LightningBurst>(), 0, 0, proj.owner, ai0: area.RelativeScale(AetherExplosion.SpriteSize) * 1.5f);
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
