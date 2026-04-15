using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class DiamondMagic : MagicType
	{
		public override float Aura => 1.5f;
		public override float? DashResist => 1.6f;
		public override float ScrollSpeed => .65f;
		public override float ScrollSize => 1.2f;
		public override float ScrollDamage => 1.1f;
		public override Color ImbueColour => new(0, 210, 217);
		public override Color ImbueColour2 => new(158, 252, 255);
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Smooth;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override SoundStyle? ImbueSound => SoundID.Shatter;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<AOBleed>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				Synergy.Create<FreezingEffect>(1.01f),
				Synergy.Create<AOBleed>(1.01f),
				Synergy.Create<Corroding>(1.01f),
				Synergy.Create<Melting>(1.075f),
				Synergy.Create<SandyEffect>(1.125f),
				Synergy.Create<Crystallized>(1.125f)
			]
			);

		public override int BlastFrames => 4;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.GemSapphire, direction.X * 0.4f, direction.Y * 0.4f, Scale: area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SilverFlame, Scale: area.RelativeScale())];
			spawnedDust.noGravity = true;
			spawnedDust.noLight = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDustDirect(position, 0, 0, DustID.GemSapphire, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity).noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.GemSapphire, 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}