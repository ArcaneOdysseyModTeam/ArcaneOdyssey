using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Gimmicks.Magic;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Magic.Effects;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class AetherLightningMagic : MagicType
	{
		public override ImbueGimmick Gimmick => ModContent.GetInstance<AetherLightningShocks>();
		public override bool ImmuneDash => true; // instant
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningBugZap with { Volume = 2.25f };
		public override Color ImbueColour => Color.Turquoise;
		public override Color ImbueColour2 => Color.White;
		public override bool AnimatedColours => true;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float ScrollSpeed => 1.3f;
		public override float ScrollSize => 1.2f;
		public override float ScrollDamage => .85f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Paralyzed>(60, 15), Debuff.Create<CharredEffect>()];
		public override Combo[] CombinedDebuffs => [Combo.Create<Soaked, Paralyzed>(), Combo.Create<Bleeding, HeavyBleed>()];

		public override int BlastFrames => 6;

		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Petrified>(), // petrified
				ClearBuff.Create<SandyEffect>(),
				ClearBuff.Create<Bleeding>(),
				ClearBuff.Create<Frozen>()
			],
			[
				Synergy.Create<FreezingEffect>(1.2f), // frozen
				Synergy.Create<Bleeding>(1.2f), // bleeding
				Synergy.Create<Melting>(1.075f),
				Synergy.Create<Corroding>(1.075f),
				Synergy.Create<Soaked>( 1.05f), // 
				Synergy.Create<Flammable>(0.96f),
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Crystallized>(1.075f),
				Synergy.Create<SearedEffect>(1.15f)
			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UltraBrightTorch, direction.X * 0.2f, direction.Y * 0.2f, Scale: 1.2f * area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{// WAHT IS  THIS IM SO CONFUSED
			float waveVal = 10f * MathF.Abs((float)Main.GameUpdateCount % 5 % 10f - 2.5f) - 12.5f;
			if (source is Projectile projectile && projectile.extraUpdates > 0)
			{
				waveVal = 10f * MathF.Abs(((float)Main.GameUpdateCount + (float)projectile.numUpdates) % 5 % 10f - 2.5f) - 12.5f;
			}
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(direction.GetValueOrDefault(Vector2.One).ToRotation()), DustID.Vortex, Vector2.Zero, Scale: 1.2f);
			spawnedDust.noGravity = true;
			Lighting.AddLight(area.Center(), 2, 0, 0);
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UltraBrightTorch, Scale: .4f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			if (Main.projectile.Find(e => e.active && (e.type == ModContent.ProjectileType<AetherLightningAftershock>()) && (e.Center.ToTileCoordinates16() == position.ToTileCoordinates16()) && (e.ai[0] != 0)) is null)
				Projectile.NewProjectile(Item.GetSource_FromThis(), position, Vector2.Zero, ModContent.ProjectileType<AetherLightningAftershock>(), 0, 0, ai0: intensity * .8f);
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UltraBrightTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2.5f * area.RelativeScale());
			}
			if (source is Projectile proj && Main.myPlayer == proj.owner)
				Projectile.NewProjectile(source.GetSource_FromThis(), area.Center(), Vector2.Zero, ModContent.ProjectileType<LightningBurst>(), 0, 0, proj.owner, ai0: area.RelativeScale(AetherExplosion.SpriteSize) * 1.5f);
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<LightningMagic>();
		}

		public override MagicCircleTypes CircleType => MagicCircleTypes.Reminiscent;
	}
}
