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
	public class IceMagic : AOMagic
	{
		public override float Aura => 1.1f;
		public override void RegisterMutations()
		{
			RegisterMutation<BlizzardMagic>();
			RegisterMutation<FrostmetalMagic>();
		}
		public override bool Special => true;
		public override float? DashResist => 1.3f;
		public override bool? Cold => true;
		public override SoundStyle? ImbueSound => SoundID.Item27;
		public override Color ImbueColour => new(30, 200, 255, 255);
		public override bool CanBeWet => false;
		public override float ImbueSpeed => .925f;
		public override float ImbueSize => 1.15f;
		public override float ImbueDamage => 1.05f;
		public override float ScrollSpeed => 0.85f;
		public override float ScrollSize => 1.2f;
		public override float ScrollDamage => 0.975f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<FreezingEffect>(), Debuff.Create<AOFrozen>(60, 33)];
		public override Combo[] CombinedDebuffs => [Combo.Create<Soaked, AOFrozen>()];



		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<AOBleed>(),
				
				ClearBuff.Create<AOBurning>(),
				
				ClearBuff.Create<Corroding>(),
				
				ClearBuff.Create<Melting>(),
				ClearBuff.Create<Scorched>(),
				ClearBuff.Create<CharredEffect>()
			],
			[ // synergies
				Synergy.Create<AOBleed>(1.2f), // bleeding
				Synergy.Create<AOFrozen>(1.1f), // frozen
				Synergy.Create<FreezingEffect>(1.1f), // freezing
				Synergy.Create<Soaked>( 1.1f), // (add stunning later!)
				 // burning
				Synergy.Create<AOBurning>(.9f),
				Synergy.Create<Flammable>(1.03f),
				Synergy.Create<CharredEffect>(.9f), // charred
				 // scorched
				Synergy.Create<Melting>(.8f),
				Synergy.Create<Scorched>( 0.8f),
				Synergy.Create<SnowyEffect>(1.1f),
				Synergy.Create<Crystallized>(1.075f),
				Synergy.Create<SearedEffect>(0.8f),
				Synergy.Create<Singed>(0.85f)
			]
			);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SnowflakeIce, direction.X * 0.5f, direction.Y * 0.5f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ice, direction.X * 0.5f, direction.Y * 0.5f, Scale: 2f * area.RelativeScale())];
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ice, Scale: area.RelativeScale())];
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.SnowflakeIce, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Ice, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 2f * intensity)];
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SnowflakeIce, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ice, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2f * area.RelativeScale())];
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
