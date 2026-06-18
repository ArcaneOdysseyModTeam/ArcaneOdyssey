using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Gimmicks.Magic;
using ArcaneOdyssey.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class WaveMagic : MagicType
	{
		public override MagicCircleTypes CircleType => MagicCircleTypes.Ornamental;

		public override ImbueGimmick Gimmick => ModContent.GetInstance<ManaSiphon>();

		public override float ScrollSpeed => 1f;

		public override float ScrollSize => 1.15f;

		public override float ScrollDamage => .8f;

		public override Color ImbueColour => Color.AliceBlue;

		public override Color ImbueColour2 => Color.DarkBlue;

		public override SoundStyle? ImbueSound => SoundID.Item91;

		public override bool AnimatedColours => true;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float DashSpeed => 1.2f;
		public override float Aura => .4f;
		public override int BlastFrames => 3;

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<PlasmaMagic>();
		}

		public const int DustCount = 30;
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			if (!Main.dedServ)
			{
				for (float i = 0; i < (DustCount * 2); i++)
				{
					var centre = (MathHelper.TwoPi / (DustCount * 2) * (i + Main.rand.NextFloat())).ToRotationVector2() * (64 * area.RelativeScale() * 2.5f);
					var dust = Dust.NewDustPerfect(area.Center(), DustID.BlueTorch, centre / (DustCount * 2 * .75f), Scale: area.RelativeScale());
					dust.noGravity = true;
				}
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (float i = 0; i < DustCount; i++)
			{
				float e = Main.rand.Next(13, 18);
				var centre = (MathHelper.TwoPi / DustCount * (i + Main.rand.NextFloat())).ToRotationVector2() * (300f * intensity);
				var dust = Dust.NewDustPerfect(position, DustID.BlueTorch, (centre / e) * .4f, Scale: 2f * intensity);
				dust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			if (!Main.dedServ)
			{
				for (float i = 0; i < DustCount; i++)
				{
					var centre = (MathHelper.TwoPi / DustCount * (i + Main.rand.NextFloat())).ToRotationVector2() * (64 * area.RelativeScale());
					var dust = Dust.NewDustPerfect(area.Center(), DustID.BlueTorch, centre / (DustCount * .75f), Scale: area.RelativeScale());
					dust.noGravity = true;
				}
			}
		}

		public override SynergyEffects Effects => new(
			[

			],
			[
				Synergy.Create<SandyEffect>(.9f),
				Synergy.Create<Crystallized>(1.1f),
				Synergy.Create<Frozen>(1.2f),
			]);
	}
}
