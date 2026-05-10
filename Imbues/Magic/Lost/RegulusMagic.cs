using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Dusts;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class RegulusMagic : MagicType
	{
		public override MagicCircleTypes CircleType => MagicCircleTypes.Imperial;

		public override bool ImmuneDash => true;

		public override Color ImbueColour => Color.Gold;
		public override Color ImbueColour2 => Color.Yellow;
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Smooth;

		public override int BlastFrames => 7;

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<LightMagic>();
		}

		public override SynergyEffects Effects => new([ClearBuff.Create<DrainedEffect>()], [Synergy.Create<DrainedEffect>(1.2f), Synergy.Create<Crystallized>(1.1f)]);

		public override Debuff[] ImbueDebuffs => [Debuff.Create<BlindedEffect>(60 * 4)];

		public override SoundStyle? ImbueSound => SoundID.Item67;

		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;

		public override float ScrollDamage => .825f;
		public override float ScrollSize => 1.2f;
		public override float ScrollSpeed => 1.5f;

		public override bool Special => true;

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<RegulusDust>(), Alpha: 60, Scale: area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<RegulusDust>(), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Alpha: 60, Scale: 2f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (float i = 0; i < 70; i++)
			{
				var centre = (MathHelper.TwoPi / 25 * i).ToRotationVector2() * ((area.Width + area.Height) / 2);
				if (i % 2 == 0)
					AOUtils.NewDustImperfect(area.Center(), ModContent.DustType<RegulusDust>(), centre / (8 + (Main.rand.NextFloat() * 2)), Alpha: 60, Scale: .7f * area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (float i = 0; i < 5; i++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<RegulusDust>(), direction.X / 2f, direction.Y / 2f, Alpha: 60, Scale: .5f * area.RelativeScale());
			}
		}
	}
}
