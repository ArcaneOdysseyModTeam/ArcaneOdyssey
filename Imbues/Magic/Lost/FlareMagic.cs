using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.VFX.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Magic.Lost
{
	public class FlareMagic : AOMagic
	{
		public override void RegisterMutations()
		{
			RegisterMutation<IonMagic>();
		}
		public override float DashSpeed => 1.2f; // burst
		public override Color ImbueColour => Color.OrangeRed;
		public override Color ImbueColour2 => Color.Orange;
		public override ColourTransitionStyle TransitionStyle => ColourTransitionStyle.Tangent;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollDamage => .925f;

		public override Debuff[] ImbueDebuffs => [Debuff.Create<Singed>(60 * 5)];
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;

		public override Combo[] CombinedDebuffs => [Combo.Create<CharredEffect, Petrified>()];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<AOBleed>(),
				ClearBuff.Create<FreezingEffect>(),
				ClearBuff.Create<SnowyEffect>(),
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<CharredEffect>(),
				ClearBuff.Create<Flammable>()
			],
			[
				Synergy.Create<AOBleed>(1.15f),
				Synergy.Create<Singed>(1.1f),
				Synergy.Create<CharredEffect>(1.01f),
				Synergy.Create<Corroding>(1.05f),
				Synergy.Create<Crystallized>(0.85f),
				Synergy.Create<FreezingEffect>(0.99f),
				Synergy.Create<SnowyEffect>(0.99f),
				Synergy.Create<Soaked>(0.99f),
				Synergy.Create<Melting>(1.05f),
				Synergy.Create<AOPoisoned>(1.05f),
				Synergy.Create<Scorched>(1.1f),
				Synergy.Create<Flammable>(1.075f),
				Synergy.Create<SandyEffect>(0.98f),
				Synergy.Create<Scalding>(1.1f),
				Synergy.Create<SearedEffect>(1.1f)

			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<FlareDust>(), direction.X * 2f, direction.Y * 2f, Alpha: (255 * .75f).Round(), Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedTorch, direction.X * 0.4f, direction.Y * 0.4f, Scale: 2f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			for (int n = 0; n < 2; n++)
			{
				var spawnedDust = Dust.NewDustDirect(area.TopLeft(), area.Width, area.Height, ModContent.DustType<FlareDust>(), Alpha: (255 * .75f).Round(), Scale: area.RelativeScale());
				spawnedDust.noGravity = true;
			}
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedTorch, direction.GetValueOrDefault().X * 0.2f, direction.GetValueOrDefault().Y * 0.2f, Scale: .05f * area.RelativeScale())];
			spawnedDust2.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 6; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, ModContent.DustType<FlareDust>(), (Main.rand.NextFloat() - 0.5f) * (30f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (30f * intensity * AOScrollSize), Alpha: (255 * .75f).Round(), Scale: intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.PinkFairy, (Main.rand.NextFloat() - 0.5f) * (22f * intensity * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (22f * intensity * AOScrollSize), 0, Color.Red, 0.8f * intensity)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 20; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, ModContent.DustType<FlareDust>(), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Alpha: (255 * .75f).Round(), Scale: area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedTorch, Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}