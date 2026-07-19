using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Terraria.Audio;

namespace ArcaneOdyssey.Imbues.Magic.Normal
{
	public class ShadowMagic : MagicType
	{
		public override float Aura => 1f;
		public override void RegisterMutations()
		{
			RegisterMutation<CursedAshMagic>();
			RegisterMutation<DarknessMagic>();
			RegisterMutation<ShadowflameMagic>();
		}
		public override float DashSpeed => 1.2f; // burst
		public override SoundStyle? ImbueSound => SoundID.Item8;
		public override Color ImbueColour => Color.Black;
		
		
		
		public override float ScrollSpeed => 1.25f;
		public override float ScrollSize => 1.1f;
		public override float ScrollDamage => 0.95f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<DrainedEffect>(60 * 5)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				Synergy.Create<Crystallized>(0.7f),
				Synergy.Create<BlindedEffect>(0.7f),
			]
			);

		public override int BlastFrames => 7;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Singularity;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Wraith, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}