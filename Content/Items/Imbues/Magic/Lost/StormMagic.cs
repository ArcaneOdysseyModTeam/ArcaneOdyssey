using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class StormMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float DashSpeed => 1.4f; // instant
		public override float KBMulti => 1.25f;
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningAuraZap;
		public override Color ImbueColour => Color.Gray;
		public override float AOImbueSpeed => 1.05f;
		public override float AOImbueSize => 1.265f;
		public override float AOImbueDamage => .95f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<CloudyEffect>(), 3 * 60), new(ModContent.BuffType<AOParalyzed>(), 60, 16)];
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOFrozen>()), new(ModContent.BuffType<SnowyEffect>(), ModContent.BuffType<AOFrozen>()), new(ModContent.BuffType<FreezingEffect>(), ModContent.BuffType<AOFrozen>())];

		public override SynergyEffects Effects => new(
			[
				BuffID.OnFire,
				BuffID.Venom,
				ModContent.BuffType<SandyEffect>(),
				BuffID.Wet,
				ModContent.BuffType<SnowyEffect>(),
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<AOScalding>(),
				BuffID.Oiled,
				ModContent.BuffType<AOPetrified>(), // petrified
				ModContent.BuffType<CharredEffect>(),
				ModContent.BuffType<AOBleed>(),
			],
			[
				new(ModContent.BuffType<CloudyEffect>(), 1.1f),
				new(ModContent.BuffType<Crystallized>(),0.9f),
				new(BuffID.OnFire,0.9f),
				new(ModContent.BuffType<CharredEffect>(),1.125f),
				new(ModContent.BuffType<FreezingEffect>(),1.1f),
				new(BuffID.Poisoned,0.9f),
				new(ModContent.BuffType<SandyEffect>(),0.9f),
				new(BuffID.ShadowFlame,1.15f),
				new(BuffID.Wet,0.9f),
				new(BuffID.Oiled,0.98f),
				new(ModContent.BuffType<AOScalding>(),0.9f),
				new(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, direction.X * 2f, direction.Y * 2f, 0, Color.DimGray, 4f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, direction.X * 0.2f, direction.Y * 0.2f, Scale: area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, 0f, 0f, 0, Color.DimGray, 1.5f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, Scale: 0.75f * area.RelativeScale());
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), 0, Color.DimGray, 4f * intensity)];
				spawnedDust.noGravity = true;
				Dust.NewDust(position, 0, 0, DustID.WitherLightning, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: intensity);
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 0, Color.DimGray, 4f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 1.2f * area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
