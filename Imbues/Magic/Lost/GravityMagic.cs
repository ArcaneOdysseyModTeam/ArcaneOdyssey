using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Dusts;
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
	public class GravityMagic : MagicType
	{
		public override ImbueGimmick Gimmick => ModContent.GetInstance<ReverseGravity>();
		public override float Aura => 1f;
		public override float DashSpeed => 1.2f; // burst
		public override float KBMulti => 3f;
		public override SoundStyle? ImbueSound => SoundID.NPCHit52;
		public override Color ImbueColour => new(31, 0, 48);
		public override Color ImbueColour2 => new(140, 0, 217);
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Smooth;
		public override float ScrollSpeed => 1.1f;
		public override float ScrollSize => 1.2f;
		public override float ScrollDamage => 1f;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;

		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				Synergy.Create<Burning>(.9f),
				Synergy.Create<CharredEffect>(1.125f),
				Synergy.Create<FreezingEffect>(1.1f),
				Synergy.Create<Poisoned>(.9f),
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Soaked>(0.9f),
				Synergy.Create<Flammable>(0.9f),
				Synergy.Create<Scalding>(0.9f),
				Synergy.Create<SearedEffect>(1.15f),
				Synergy.Create<Bleeding>(1.1f),
				Synergy.Create<Corroding>(1.075f),
				Synergy.Create<Melting>(1.075f),
				Synergy.Create<SandyEffect>(1.1f)
			]
			);

		public override int BlastFrames => 4;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Singularity;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<GravityDust>(), direction.X * 0.5f, direction.Y * 0.5f, Scale: 2f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<GravityDust>(), Scale: 2.3f)];
			spawnedDust.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<GravityDust>(), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<GravityDust>(), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 4f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<EarthMagic>();
		}
	}
}