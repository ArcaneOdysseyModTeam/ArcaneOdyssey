using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Normal
{
	public class WaterMagic : MagicType
	{
		public override float Aura => .8f;
		public override void RegisterMutations()
		{
			RegisterMutation<CloudMagic>();
			RegisterMutation<LunarMagic>();
			RegisterMutation<OilMagic>();
			RegisterMutation<StormMagic>();
		}
		public override float DashSpeed => 1.2f; // burst
		public override bool? Cold => true;
		public override Color ImbueColour => new(0, 30, 255);
		public override float ImbueSpeed => 1f;
		public override float ImbueSize => 1.22f;
		public override float ImbueDamage => 0.975f;
		public override float ScrollSpeed => 1f;
		public override float ScrollSize => 1.25f;
		public override float ScrollDamage => 0.9f;
		public override SoundStyle? ImbueSound => SoundID.Splash;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Soaked>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<AOBurning>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<Corroding>(),
				ClearBuff.Create<Melting>(),
				ClearBuff.Create<Flammable>(),
				ClearBuff.Create<Singed>(),
				ClearBuff.Create<Scalding>(),
				ClearBuff.Create<SearedEffect>()
			],
			[
				Synergy.Create<Crystallized>(0.85f),
				Synergy.Create<AOBleed>(1.05f),
				
				Synergy.Create<AOBurning>(.8f),
				Synergy.Create<CharredEffect>(0.9f),
				
				Synergy.Create<Corroding>(.9f),
				Synergy.Create<FreezingEffect>(1.075f),
				
				Synergy.Create<Melting>(.9f),
				Synergy.Create<Flammable>(0.98f),
				Synergy.Create<SandyEffect>(0.8f),
				Synergy.Create<Scorched>(0.7f),
				Synergy.Create<SnowyEffect>(1.1f),
				Synergy.Create<SearedEffect>(0.7f),
				Synergy.Create<Singed>(0.8f),
			]
		);

		public override int BlastFrames => 5;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)

			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water, Scale: 1.2f * area.RelativeScale());
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Water, (Main.rand.NextFloat() - 0.5f) * (25f * intensity), (Main.rand.NextFloat() - 0.5f) * (35f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}