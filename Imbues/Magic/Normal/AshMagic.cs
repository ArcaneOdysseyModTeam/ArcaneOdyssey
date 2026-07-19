using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Gimmicks.Magic;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Terraria.Audio;

namespace ArcaneOdyssey.Imbues.Magic.Normal
{
	public class AshMagic : MagicType
	{
		public override ImbueGimmick Gimmick => ModContent.GetInstance<AshClouds>();
		public override void RegisterMutations()
		{
			RegisterMutation<AetherMagic>();
			RegisterMutation<HeatMagic>();
			RegisterMutation<PhoenixMagic>();
			RegisterMutation<SunMagic>();
			RegisterMutation<CursedAshMagic>();
		}
		public override bool Special => true;
		public override void SetStaticDefaults() { base.SetStaticDefaults(); ArcaneOdysseyMod.Sets.cold[Type] = false; }
		public override bool CanBeWet => false;
		public override Color ImbueColour => new(235, 40, 0, 0);
		
		
		
		public override float ScrollSpeed => 0.95f;
		public override float ScrollSize => 1.25f;
		public override float ScrollDamage => 0.875f;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Petrified>(60, 33)];
		public override Combo[] CombinedDebuffs => [Combo.Create<Melting, Petrified>(), Combo.Create<Burning, Petrified>(), Combo.Create<Scorched, Petrified>(), Combo.Create<CharredEffect, Petrified>(), Combo.Create<Scalding, Petrified>(), Combo.Create<Singed, Petrified>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<FreezingEffect>(),

				ClearBuff.Create<Burning>(),

				ClearBuff.Create<Melting>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<Scorched>(),
				ClearBuff.Create<Singed>(),
				ClearBuff.Create<Scalding>()
			],
			[
				Synergy.Create<Bleeding>(1.1f),

				Synergy.Create<Burning>(1.02f),

				Synergy.Create<Corroding>(1.075f),
				Synergy.Create<Singed>(1.2f),

				Synergy.Create<Flammable>(1.075f),

				Synergy.Create<Melting>(1.075f),
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Soaked>(0.995f),
				Synergy.Create<FreezingEffect>(0.99f),
				Synergy.Create<CharredEffect>(1.01f),
				Synergy.Create<SandyEffect>(1.125f),
				Synergy.Create<Scalding>(1.2f),
				Synergy.Create<SearedEffect>(1.15f)
			]
			);

		public override int BlastFrames => 7;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Malignant;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedTorch, direction.X * 2f, direction.Y * 2f, Scale: 2f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			_ = Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedTorch, Scale: 1f * area.RelativeScale());
			Dust spawnedDust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.Ash, Scale: 2f * area.RelativeScale());
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 2f * intensity)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.Ash, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale());
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, DustID.RedTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2f * area.RelativeScale());
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}