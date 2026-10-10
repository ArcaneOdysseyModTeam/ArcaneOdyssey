using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.MagicVariants.Base;
using ArcaneOdyssey.MagicVariants.Crystal;
using Terraria.Audio;

namespace ArcaneOdyssey.Imbues.Magic.Normal
{
	public sealed class CrystalMagic : MagicType
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ImbueID.Sets.DefaultVariant[ID] = MagicVariant.GetID<RubyVariant>();
		}

		public override void RegisterMutations()
		{
			RegisterMutation<DiamondMagic>();
			RegisterMutation<PrismMagic>();
		}
		public override float DashResist => 1.3f;

		public override Color ImbueColour => Color.Red;

		public override void Load()
		{
			base.Load();
			ID = ImbueID.Crystal;
		}

		public override float ScrollSpeed => 0.9f;
		public override float ScrollSize => 1.15f;
		public override float ScrollDamage => 1.05f;
		public override SoundStyle? ImbueSound => SoundID.Shatter;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Crystallized>(60 * 5)];
		public override Combo[] CombinedDebuffs => [];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
			
			],
			[
				Synergy.Create<FreezingEffect>(1.01f),
				Synergy.Create<Bleeding>(1.01f),

				Synergy.Create<Corroding>(1.01f),

				Synergy.Create<Melting>(1.075f),
				Synergy.Create<SandyEffect>(1.125f)
			]
			);

		public override MagicCircleTypes CircleType => MagicCircleTypes.Ornamental;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			GetVFX(this, out var colour, out _, out var dust);
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, dust, direction.X * 0.4f, direction.Y * 0.4f, newColor: colour, Scale: area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			GetVFX(this, out var colour, out _, out var dust);
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, dust, newColor: colour, Scale: area.RelativeScale())];
			spawnedDust.noGravity = true;
			spawnedDust.noLight = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			GetVFX(this, out var colour, out _, out var dust);
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDustDirect(position, 0, 0, dust, (Main.rand.NextFloat() - 0.5f) * (18f * intensity), (Main.rand.NextFloat() - 0.5f) * (18f * intensity), newColor: colour, Scale: 2f * intensity).noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			GetVFX(this, out var colour, out _, out var dust);
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, dust, 2f * (Main.rand.NextFloat() - 0.5f), 2f * (Main.rand.NextFloat() - 0.5f), newColor: colour, Scale: area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public static void GetVFX(CrystalMagic imbue, out Color dustColour, out Color drawColour, out int dust)
		{
			dustColour = default;
			drawColour = imbue.Colour;
			if (imbue.magicVariant is RubyVariant || (imbue.unloadedMagicVariant.IsNullOrWhiteSpace() && imbue.magicVariant is null))
			{
				dust = DustID.GemRuby;
			}
			else if (imbue.magicVariant is SapphireVariant)
			{
				dust = DustID.GemSapphire;
			}
			else if (imbue.magicVariant is EmeraldVariant)
			{
				dust = DustID.GemEmerald;
			}
			else if (imbue.magicVariant is AmethystVariant)
			{
				dust = DustID.GemAmethyst;
			}
			else
			{
				dust = DustID.GemDiamond;
				dustColour = imbue.Colour;
			}
		}
	}
}