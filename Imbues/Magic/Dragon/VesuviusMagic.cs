using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Magic.Dragon
{
	public class VesuviusMagic : MagicType
	{
		public override Color ImbueColour => new(0, 130, 255);
		public override float ImbueSpeed => 1.2f;
		public override float ImbueSize => 3f;
		public override float ImbueDamage => 2f;
		public override float ScrollSpeed => 1.2f;
		public override float ScrollSize => 3f;
		public override float ScrollDamage => 2f;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Dragon;
		public override float? DashResist => 1.3f;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<VesuvianBurn>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<FreezingEffect>(), // freezing
				ClearBuff.Create<Petrified>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<Bleeding>(),
				ClearBuff.Create<Corroding>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<SandyEffect>(),
				ClearBuff.Create<SnowyEffect>()
			],
			[
				Synergy.Create<Petrified>(2.2f), // petrified
				Synergy.Create<Bleeding>(2.15f), // bleeding
				Synergy.Create<Burning>(2.075f),
				Synergy.Create<Corroding>(2.1f),
				Synergy.Create<FreezingEffect>(1.95f),
				Synergy.Create<SnowyEffect>(1.99f),
				Synergy.Create<CharredEffect>(2.1f),
				Synergy.Create<SandyEffect>(1.99f),
				Synergy.Create<Soaked>(1.95f),
				Synergy.Create<Scorched>(2.1f),
				Synergy.Create<Flammable>(2.075f),
				Synergy.Create<Crystallized>(1.95f),
				Synergy.Create<Scalding>(2.075f)
			]
			);

		public override int BlastFrames => 4;
		public override void UpdateProjectile(Projectile Projectile)
		{
			Projectile.rotation += 0.1f * Projectile.direction;
		}

		public override MagicCircleTypes CircleType => MagicCircleTypes.Malignant;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UltraBrightTorch, direction.X * 2f, direction.Y * 2f, 0, new Color(0, 0, 255, 0), area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UltraBrightTorch, 0f, 0f, 0, new Color(0, 0, 255, 0), area.RelativeScale());
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.SolarFlare, 0f, 0f, 0, Color.Blue, area.RelativeScale());
			Lighting.AddLight(area.Center(), 1f, 0.19f, 0f);
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(position, 0, 0, DustID.UltraBrightTorch, (Main.rand.NextFloat() - 0.5f) * (5f * intensity), (Main.rand.NextFloat() - 0.5f) * (5f * intensity), 0, new Color(0, 0, 255, 0), intensity);
				Dust.NewDust(position, 0, 0, DustID.SolarFlare, (Main.rand.NextFloat() - 0.5f) * (5f * intensity), (Main.rand.NextFloat() - 0.5f) * (5f * intensity), 0, Color.Blue, intensity);
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.UltraBrightTorch, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 0, new Color(0, 0, 255, 0), area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{

		}
	}
}
