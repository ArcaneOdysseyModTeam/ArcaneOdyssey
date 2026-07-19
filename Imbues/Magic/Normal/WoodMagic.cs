using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using Terraria.Audio;

namespace ArcaneOdyssey.Imbues.Magic.Normal
{
	public class WoodMagic : MagicType
	{
		public override float Aura => 1.2f;
		public override void RegisterMutations()
		{
			RegisterMutation<OilMagic>();
			RegisterMutation<PlantMagic>();
			RegisterMutation<SlashMagic>();
			RegisterMutation<ThreadMagic>();
		}
		public override bool Special => true;
		public override float? DashResist => 1.3f;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => new(61, 33, 0, 255);
		
		
		
		public override float ScrollSpeed => 0.8f;
		public override float ScrollSize => 1.2f;
		public override float ScrollDamage => 0.95f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Bleeding>()];
		public override SynergyEffects Effects => new([],
			[

				Synergy.Create<Burning>(1.1f),
				Synergy.Create<CharredEffect>(1.1f),
				Synergy.Create<Singed>(1.1f),

				Synergy.Create<Corroding>(1.05f),

				Synergy.Create<Melting>(1.05f),
				Synergy.Create<SandyEffect>(1.1f),
				Synergy.Create<Scorched>(1.1f),
				Synergy.Create<Scalding>(1.1f),
				Synergy.Create<SearedEffect>(1.1f)
			]
			);

		public override int BlastFrames => 4;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Imperial;

		public override void UpdateProjectile(Projectile Projectile)
		{
			Projectile.rotation += 0.1f * Projectile.direction;
		}

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pearlwood, direction.X * 0.2f, direction.Y * 0.2f, Scale: 1.5f * area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pearlwood, direction.GetValueOrDefault().X * 0.2f, direction.GetValueOrDefault().Y * 0.2f, Scale: 1f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Pearlwood, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 2.5f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pearlwood, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 2f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}