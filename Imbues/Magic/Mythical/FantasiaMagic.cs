using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Dusts;
using ArcaneOdyssey.Gimmicks.Magic;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Terraria.Audio;

namespace ArcaneOdyssey.Imbues.Magic.Mythical
{
	public class FantasiaMagic : MagicType<FantasiaRifts>
	{
		public override int BlastFrames => 5;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Mythical;
		public override MagicCircleTypes CircleType => MagicCircleTypes.Singularity;
		public override float DashSpeed => 1.2f; // burst
		public override float Aura => 1f;
		public override SoundStyle? ImbueSound => SoundID.NPCHit52;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<HeavyBleed>()];
		public override bool AnimatedColours => true;
		public override Color ImbueColour => Color.Teal;
		public override Color ImbueColour2 => Color.PaleVioletRed;
		public override float ScrollSpeed => 1.25f;
		public override float ScrollSize => 1.2f;
		public override float ScrollDamage => 1f;

		public override SynergyEffects Effects => AOUtils.CopySynergiesFromImbue<GravityMagic>() + AOUtils.CopySynergiesFromImbue<SlashMagic>();

		public override void RegisterMutations()
		{

		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<FantasiaDust>(), direction.X * 0.5f, direction.Y * 0.5f, Scale: 2f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<FantasiaDust>(), Scale: 2.3f)];
			spawnedDust.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<FantasiaDust>(), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<FantasiaDust>(), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 4f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
