using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.MagicVariants.Base;
using ArcaneOdyssey.MagicVariants.Lost.Cloud;
using Terraria.Audio;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class CloudMagic : MagicType
	{
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Lost;
		public override float DashSpeed => 1.2f; // burst
		public override float DashResist => 1.1f;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => Color.LightGray;
		public override Color? DefaultAnimatedColour => Color.DarkGray;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ImbueID.Sets.KBMulti[ID] = 1.25f;
			ImbueID.Sets.DefaultVariant[ID] = MagicVariant.GetID<CirrusVariant>();
		}

		public override float ScrollSpeed => .9f;
		public override float ScrollSize => 1.3f;
		public override float ScrollDamage => .8f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<CloudyEffect>(5 * 60)];


		public override MagicCircleTypes CircleType => MagicCircleTypes.Segmented;
		public override SynergyEffects Effects => new(
			[
				ClearBuff.Create<Burning>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<Corroding>(),
				ClearBuff.Create<SandyEffect>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<Scalding>(),
				ClearBuff.Create<Flammable>()
			],
			[
				Synergy.Create<Crystallized>(0.9f),
				Synergy.Create<Burning>(.9f),
				Synergy.Create<CharredEffect>(1.125f),
				Synergy.Create<FreezingEffect>(1.1f),
				Synergy.Create<Poisoned>(.9f),
				Synergy.Create<SandyEffect>(0.9f),
				Synergy.Create<Scorched>(1.15f),
				Synergy.Create<Soaked>(0.9f),
				Synergy.Create<Flammable>(0.98f),
				Synergy.Create<Scalding>(0.9f),
				Synergy.Create<SearedEffect>(1.15f)
			]
			);


		public static void GetVFX(CloudMagic imbue, out Color dustColour, out Color drawColour, out int dust)
		{
			dustColour = default;
			drawColour = imbue.Colour;
			if (imbue.magicVariant is CirrusVariant || (imbue.magicVariant is null && imbue.unloadedMagicVariant.IsNullOrWhiteSpace()))
			{
				dust = DustID.Cloud;
			}
			else if (imbue.magicVariant is StratusVariant)
			{
				dust = DustID.RainCloud;
			}
			else
			{
				dust = DustID.Cloud;
				dustColour = imbue.Colour;
			}
		}


		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			GetVFX(this, out var colour, out _, out var dust);
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, dust, direction.X * 2f, direction.Y * 2f, newColor: colour, Scale: 6f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			GetVFX(this, out var colour, out _, out var dust);
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, dust, newColor: colour, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			GetVFX(this, out var colour, out _, out var dust);
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, dust, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), newColor: colour, Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			GetVFX(this, out var colour, out _, out var dust);
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, dust, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), newColor: colour, Scale: 6f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void RegisterMutations()
		{
			RegisterDefaultMagic<WindMagic>();
		}
	}
}
