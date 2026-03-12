using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Normal
{
	public class MagmaMagic : AOMagic
	{
		public override float Aura => 1f;
		public override void RegisterMutations()
		{
			RegisterMutation<DiamondMagic>();
			RegisterMutation<GravityMagic>();
			RegisterMutation<HeatMagic>();
			RegisterMutation<ShadowflameMagic>();
			RegisterMutation<SunMagic>();
			RegisterMutation<PhoenixMagic>();
		}
		public override bool Special => true;
		public override float? DashResist => 1.2f;
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override Color ImbueColour => new(255, 50, 0);
		public override float AOImbueSpeed => 0.85f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 0.975f;
		public override float AOScrollSpeed => 0.7f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 0.9f;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Melting>(60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<FreezingEffect>(), // freezing
				ClearBuff.Create<Petrified>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<AOBleed>(),
				
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<SandyEffect>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<Flammable>()
			],
			[
				Synergy.Create<Petrified>(1.2f), // petrified
				Synergy.Create<AOBleed>(1.15f), // bleeding
				
				Synergy.Create<AOBurning>(1.075f),
				 // venom acid
				Synergy.Create<Corroding>(1.1f),
				
				Synergy.Create<AOPoisoned>(1.05f),
				Synergy.Create<Singed>(1.1f),
				
				Synergy.Create<Flammable>(1.075f),
				Synergy.Create<FreezingEffect>(.95f),
				Synergy.Create<SnowyEffect>(.99f),
				Synergy.Create<CharredEffect>(1.1f),
				Synergy.Create<SandyEffect>(0.99f),
				Synergy.Create<Soaked>( .95f),
				Synergy.Create<Scorched>( 1.1f),
				Synergy.Create<Crystallized>(0.95f),
				Synergy.Create<Scalding>(1.075f),
				Synergy.Create<SearedEffect>(1.1f)
			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.InfernoFork, direction.X * 2f, direction.Y * 2f, Scale: 2.5f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.InfernoFork, Scale: 1.2f * area.RelativeScale());
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SolarFlare, Scale: 1.2f * area.RelativeScale());
			Lighting.AddLight(area.Center(), 1f, 0.19f, 0f);
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.InfernoFork, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust.NewDust(position, 0, 0, DustID.SolarFlare, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 1.4f * intensity);
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.InfernoFork, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
