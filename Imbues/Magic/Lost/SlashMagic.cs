using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class SlashMagic : MagicType
	{
		public override float Aura => .5f;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float ScrollDamage => 1.2f;
		public override float ScrollSpeed => 1.1f;
		public override float ScrollSize => .8f;
		public override Color ImbueColour => new(0, 255, 0);
		public override Color ImbueColour2 => new(0, 120, 0);
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Tangent;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<AOBleed>(60 * 15)];
		public override SoundStyle? ImbueSound => SoundID.Item71;
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<FreezingEffect>()
			],
			[
				
				Synergy.Create<Corroding>(1.05f),
				Synergy.Create<Crystallized>(1.05f),
				Synergy.Create<FreezingEffect>(1.02f),
				
				Synergy.Create<Melting>(1.05f),
				Synergy.Create<SandyEffect>(1.1f)
			]
			);

		public override int BlastFrames => 4;

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<SlashDust>(), Alpha: 60, Scale: area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<SlashDust>(), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Alpha: 60, Scale: 2f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (float i = 0; i < 70; i++)
			{
				var centre = (MathHelper.TwoPi / 25 * i).ToRotationVector2() * ((area.Width + area.Height) / 2);
				if (i % 2 == 0)
					AOUtils.NewDustImperfect(area.Center(), ModContent.DustType<SlashDust>(), centre / (8 + (Main.rand.NextFloat() * 2)), Alpha: 60, Scale: .7f * area.RelativeScale()).noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (float i = 0; i < 5; i++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<SlashDust>(), direction.X / 2f, direction.Y / 2f, Alpha: 60, Scale: .5f * area.RelativeScale());
			}
		}
	}
}
