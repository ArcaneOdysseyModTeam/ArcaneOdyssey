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
	public class SandMagic : MagicType
	{
		public override float Aura => 1f;
		public override void RegisterMutations()
		{
			RegisterMutation<DiamondMagic>();
			RegisterMutation<GravityMagic>();
		}
		public override bool Special => true;
		public override float? DashResist => 1.1f;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => new(255, 255, 60, 255);
		public override bool CanBeWet => false;
		public override float ImbueSpeed => 0.975f;
		public override float ImbueSize => 1.053f;
		public override float ImbueDamage => 1.05f;
		public override float ScrollSpeed => 0.95f;
		public override float ScrollSize => 1.1f;
		public override float ScrollDamage => 0.975f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<SandyEffect>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<Flammable>()
			],
			[
				Synergy.Create<Bleeding>(1.1f),
				
				Synergy.Create<Burning>(1.125f),
				Synergy.Create<CharredEffect>(1.01f),
				
				Synergy.Create<Corroding>(1.075f),
				Synergy.Create<Crystallized>(0.8f),
				
				Synergy.Create<Melting>(1.075f),
				Synergy.Create<Soaked>(0.8f),
				Synergy.Create<Flammable>(0.9f),
				Synergy.Create<Singed>(1.1f),
				Synergy.Create<Scalding>(1.125f)
			]
			);

		public override int BlastFrames => 7;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Reminiscent;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Sand, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Sand, Scale: 1f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Sand, (Main.rand.NextFloat() - 0.5f) * (20f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Sand, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}