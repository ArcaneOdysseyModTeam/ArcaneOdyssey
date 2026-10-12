using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.MagicVariants.Normal.Acid;
using Terraria.Audio;

namespace ArcaneOdyssey.Imbues.Magic.Normal
{
	public sealed class AcidMagic : MagicType
	{
		public override void RegisterMutations()
		{
			RegisterMutation<OilMagic>();
		}

		public override float DashSpeed => 1.2f; // burst

		public override Color ImbueColour => Color.Purple;

		public override void Load()
		{
			base.Load();
			ID = ImbueID.Acid;
		}

		public static void GetVFX(AcidMagic imbue, out Color dustColour, out Color drawColour, out int dust)
		{
			dustColour = default;
			drawColour = imbue.Colour;
			if (imbue.magicVariant is MeltingVariant || (imbue.unloadedMagicVariant.IsNullOrWhiteSpace() && imbue.magicVariant is null))
			{
				dust = DustID.Water_Corruption;
			}
			else if (imbue.magicVariant is CorrosiveVariant)
			{
				dust = DustID.Water_Jungle;
				dustColour = imbue.Colour;
			}
			else
			{
				dust = DustID.Water_Desert;
				dustColour = imbue.Colour;
			}
		}
		
		public override float ScrollSpeed => 1f;
		public override float ScrollSize => 1.05f;
		public override float ScrollDamage => 0.875f;
		public override SoundStyle? ImbueSound => SoundID.Splash;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Corroding>(60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<SandyEffect>()
			],
			[
				Synergy.Create<Bleeding>(1.075f),

				Synergy.Create<Burning>(1.075f),
				Synergy.Create<CharredEffect>(1.1f),
				Synergy.Create<FreezingEffect>(1.2f),

				Synergy.Create<Melting>(1.05f),

				Synergy.Create<Poisoned>(1.05f),
				Synergy.Create<Scorched>(1.1f),
				Synergy.Create<Singed>(1.1f),
				Synergy.Create<Soaked>(0.9f),
				Synergy.Create<Flammable>(1.05f),
				Synergy.Create<Crystallized>(0.9f),
				Synergy.Create<SandyEffect>(0.99f),
				Synergy.Create<Scalding>(1.075f),
				Synergy.Create<SearedEffect>(1.1f)
			]
			);

		public override MagicCircleTypes CircleType => MagicCircleTypes.Tidal;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			GetVFX(this, out var colour, out _, out var dust);
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, dust, direction.X * 2f, direction.Y * 2f, newColor: colour, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			GetVFX(this, out var colour, out _, out var dust);
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, dust, newColor: colour, Scale: 1.6f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			GetVFX(this, out var colour, out _, out var dust);
			for (int n = 0; n < 6; n++)
			{
				Dust.NewDustDirect(position, 0, 0, dust, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (35f * intensity), newColor: colour, Scale: 3f * intensity).noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			GetVFX(this, out var colour, out _, out var dust);
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, dust, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), newColor: colour, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}