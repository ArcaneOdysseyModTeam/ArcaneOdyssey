using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class PlasmaMagic : AOMagic
	{
		public override void RegisterMutations()
		{
			RegisterMutation<AetherMagic>();
			RegisterMutation<EnergyMagic>();
			RegisterMutation<HeatMagic>();
			RegisterMutation<PhoenixMagic>();
			RegisterMutation<SunMagic>();
		}
		public override bool Special => true;
		public override float DashSpeed => 1.4f; // instant
		public override bool? Cold => false;
		public override SoundStyle? ImbueSound => SoundID.Item91;
		public override Color ImbueColour => new Color(255, 100, 255, 255);
		public override bool CanBeWet => false;
		public override float AOImbueSpeed => 1.125f;
		public override float AOImbueSize => 0.948f;
		public override float AOImbueDamage => 0.9f;
		public override float AOScrollSpeed => 1.2f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 0.825f;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Scorched>(60 * 10)];
		public override Combo[] CombinedDebuffs => [new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<Petrified>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ClearBuff.Create<AOBleed>(),
				ClearBuff.Create < CharredEffect >(),
				ClearBuff.Create < FreezingEffect >(),
				ClearBuff.Create < SnowyEffect >(),
				new(BuffID.Wet),
				new(BuffID.Oiled)
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.15f),
				new(BuffID.OnFire,1.075f),
				Synergy.Create<AOBurning>(1.075f),
				new(ModContent.BuffType<CharredEffect>(),1.1f),
				new(BuffID.Venom,1.05f),
				Synergy.Create<Corroding>(1.05f),
				new(ModContent.BuffType<Crystallized>(),0.99f),
				new(ModContent.BuffType<FreezingEffect>(),0.97f),
				new(BuffID.OnFire3,1.05f),
				Synergy.Create<Melting>(1.05f),
				new(BuffID.Poisoned,1.05f),
				Synergy.Create<AOPoisoned>(1.05f),
				new(ModContent.BuffType<SnowyEffect>(),0.99f),
				new(ModContent.BuffType<Singed>(), 1.1f),
				new(BuffID.Wet,0.95f),
				new(BuffID.Slimed,1.075f),
				new(BuffID.Oiled,1.075f),
				new(ModContent.BuffType<Scalding>(),1.075f),
				new(ModContent.BuffType<SearedEffect>(),1.1f)
			]
			);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.PinkTorch, direction.X * 0.4f, direction.Y * 0.4f, Scale: area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.PinkTorch, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.ShadowbeamStaff, Scale: 2f * area.RelativeScale())];
			spawnedDust2.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(position, 0, 0, DustID.Firework_Pink, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity);
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.ShadowbeamStaff, 5f * (Main.rand.NextFloat() - 0.5f), 5f * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}