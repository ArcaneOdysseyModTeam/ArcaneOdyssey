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
	public class WindMagic : AOMagic
	{
		public override void RegisterMutations()
		{
			RegisterMutation<BlizzardMagic>();
			RegisterMutation<CloudMagic>();
			RegisterMutation<GravityMagic>();
			RegisterMutation<SlashMagic>();
			RegisterMutation<HeatMagic>();
			RegisterMutation<SoundMagic>();
			RegisterMutation<StormMagic>();
		}
		public override float DashSpeed => 1.4f; // instant
		public override float KBMulti => 2f;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => new(232, 232, 232);
		public override float ImbueSpeed => 1.175f;
		public override float ImbueSize => 1.15f;
		public override float ImbueDamage => .9f;
		public override float ScrollSpeed => 1.35f;
		public override float ScrollSize => 1.2f;
		public override float ScrollDamage => .825f;
		public override Combo[] CombinedDebuffs => [Combo.Create<SnowyEffect, AOFrozen>(), Combo.Create<FreezingEffect, AOFrozen>()];
		public override SynergyEffects Effects => new(
			[
				ClearBuff.Create<AOBurning>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<Corroding>(),
				ClearBuff.Create<SandyEffect>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<Scalding>(),
				ClearBuff.Create<Flammable>()
			],
			[
				Synergy.Create<Crystallized>(0.9f),
				
				Synergy.Create<AOBurning>(.9f),
				Synergy.Create<CharredEffect>(1.125f),
				Synergy.Create<Singed>(.9f),
				Synergy.Create<FreezingEffect>(1.1f),
				
				Synergy.Create<SandyEffect>(0.9f),
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Soaked>(0.9f),
				Synergy.Create<Flammable>(0.98f),
				Synergy.Create<Scalding>(0.9f),
				Synergy.Create<SearedEffect>(1.15f)
			]
			);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, Scale: area.RelativeScale())];
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
