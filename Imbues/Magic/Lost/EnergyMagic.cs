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
	public class EnergyMagic : MagicType
	{
		public override ImbueGimmick Gimmick => ModContent.GetInstance<InfiniteMana>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.mana = 0;
		}

		public override bool ImmuneDash => true; // instant
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningBugZap with { Volume = 2.25f };
		public override Color ImbueColour => Color.Yellow;
		public override Color ImbueColour2 => Color.LightYellow;
		public override bool AnimatedColours => true;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float ScrollSpeed => 1.275f;
		public override float ScrollSize => 1.15f;
		public override float ScrollDamage => .75f;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Reminiscent;



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
				Synergy.Create<Melting>(1.075f),
				Synergy.Create<Corroding>(1.075f),
				Synergy.Create<Soaked>( 1.05f), // 
				Synergy.Create<Flammable>(0.96f),
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Crystallized>(1.075f),
				Synergy.Create<SearedEffect>(1.15f)
			]
			);

		public override int BlastFrames => 3;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.IchorTorch, direction.X * 0.2f, direction.Y * 0.2f, Scale: 1.2f * area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			float waveVal = 10f * MathF.Abs((float)Main.GameUpdateCount % 5 % 10f - 2.5f) - 12.5f;
			if (source is Projectile projectile && projectile.extraUpdates > 0)
			{
				waveVal = 10f * MathF.Abs(((float)(Main.GameUpdateCount + projectile.numUpdates)) % 5 % 10f - 2.5f) - 12.5f;
			}
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(direction.GetValueOrDefault(Vector2.One).ToRotation()), DustID.SolarFlare, Vector2.Zero, Scale: 1.2f);
			spawnedDust.noGravity = true;
			Lighting.AddLight(area.Center(), 2, 0, 0);
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.IchorTorch, Scale: .7f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust dust = Dust.NewDustDirect(position, 0, 0, DustID.Firework_Yellow, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 2.3f * intensity);
				dust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 5; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.IchorTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2.5f * area.RelativeScale());
			}
			if (source is Projectile proj && Main.myPlayer == proj.owner)
				Projectile.NewProjectile(source.GetSource_FromThis(), area.Center(), Vector2.Zero, ModContent.ProjectileType<LightningBurst>(), 0, 0, proj.owner, ai0: area.RelativeScale(AetherExplosion.SpriteSize) * 1.5f);
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<LightningMagic>();
		}
	}
}
