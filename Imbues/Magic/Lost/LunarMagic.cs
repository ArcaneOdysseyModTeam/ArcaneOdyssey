using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;
using ArcaneOdyssey.MagicVariants.Lost.Lunar;
using Terraria.Audio;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class LunarMagic : MagicType
	{
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float DashSpeed => 1.2f; // burst
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.cold[Type] = true;
			ImbueID.Sets.AuraPower[ID] = .8f;
			ImbueID.Sets.Bright[ID] = true;
			ImbueID.Sets.DefaultVariant[ID] = MagicVariant.GetID<MidnightVariant>();
		}

		public override Color ImbueColour => new(0, 10, 87);
		public override Color? DefaultAnimatedColour => new(137, 64, 255);

		public override float ScrollSpeed => 1.1f;
		public override float ScrollSize => 1.25f;
		public override float ScrollDamage => 0.95f;
		public override SoundStyle? ImbueSound => SoundID.Splash;

		public override MagicCircleTypes CircleType => MagicCircleTypes.Tidal;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Soaked>(60 * 7), Debuff.Create<BlindedEffect>(3 * 60)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
				ClearBuff.Create<Burning>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<Melting>(),
				ClearBuff.Create<Scorched>(),
				ClearBuff.Create<Flammable>(),
				ClearBuff.Create<Scalding>(),
				ClearBuff.Create<SearedEffect>()
			],
			[
				Synergy.Create<Bleeding>(1.05f),
				Synergy.Create<Burning>(.8f),
				Synergy.Create<CharredEffect>(0.9f),
				Synergy.Create<DrainedEffect>(0.9f),
				Synergy.Create<Corroding>(.9f),
				Synergy.Create<FreezingEffect>(1.075f),
				Synergy.Create<Melting>(.9f),
				Synergy.Create<Flammable>(0.98f),
				Synergy.Create<SandyEffect>(0.8f),
				Synergy.Create<Scorched>(0.7f),
				Synergy.Create<SnowyEffect>(1.1f),
				Synergy.Create<SearedEffect>(0.7f)
			]
		);

		public int[] DustIDs
		{
			get
			{
				if (magicVariant is BloodVariant)
				{
					return [DustID.Water_BloodMoon, DustID.VampireHeal, DustID.CrimsonTorch];
				}
				return [DustID.Water_GlowingMushroom, DustID.RedsWingsRun, DustID.MushroomTorch];
			}
		}

		public static void GetVFX(LunarMagic imbue, out int[] dust)
		{
			dust = [DustID.Water_GlowingMushroom, DustID.RedsWingsRun, DustID.MushroomTorch];
			if (imbue.magicVariant is BloodVariant)
			{
				dust = [DustID.Water_BloodMoon, DustID.VampireHeal, DustID.CrimsonTorch];
			}
		}

		protected override void BaseSpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustIDs[0], direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust1 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustIDs[1], direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust1.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustIDs[2], direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		protected override void BaseLingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustIDs[0], Scale: 1.2f * area.RelativeScale());
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustIDs[1], Scale: area.RelativeScale());
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustIDs[2], Scale: 2f * area.RelativeScale())];
			spawnedDust2.noGravity = true;
		}
		protected override void BaseExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustIDs[1], (Main.rand.NextFloat() - 0.5f) * (25f * ScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * ScrollSize), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust1 = Main.dust[Dust.NewDust(position, 0, 0, DustIDs[2], (Main.rand.NextFloat() - 0.5f) * (20f * intensity), (Main.rand.NextFloat() - 0.5f) * (20f * intensity), 150, Scale: 3f * intensity)];
				spawnedDust1.noGravity = true;
			}
		}
		protected override void BaseKillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustIDs[1], 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust1 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustIDs[2], 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust1.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustIDs[2], 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<WaterMagic>();
		}
	}
}
