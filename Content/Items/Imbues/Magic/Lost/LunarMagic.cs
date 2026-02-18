using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class LunarMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float DashSpeed => 1.2f; // burst
		public override bool? Cold => true;
		public override Color ImbueColour => new(0, 30, 255);
		public override float AOImbueSpeed => 1.1f;
		public override float AOImbueSize => 1.25f;
		public override float AOImbueDamage => 0.95f;
		public override SoundStyle? ImbueSound => SoundID.Splash;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Wet, 60 * 7), new(ModContent.BuffType<BlindedEffect>(), 3 * 60)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				BuffID.OnFire,
				ModContent.BuffType<CharredEffect>(),
				BuffID.Venom,
				BuffID.OnFire3,
				BuffID.ShadowFlame,
				BuffID.Oiled,
				ModContent.BuffType<AOScalding>(),
				ModContent.BuffType<SearedEffect>()
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.05f),
				new(BuffID.OnFire,0.8f),
				new(ModContent.BuffType<CharredEffect>(),0.9f),
				new(ModContent.BuffType<DrainedEffect>(),0.9f),
				new(BuffID.Venom,0.9f),
				new(ModContent.BuffType<FreezingEffect>(),1.075f),
				new(BuffID.OnFire3,0.9f),
				new(BuffID.Oiled,0.98f),
				new(ModContent.BuffType<SandyEffect>(),0.8f),
				new(BuffID.ShadowFlame,0.7f),
				new(ModContent.BuffType<SnowyEffect>(),1.1f),
				new(ModContent.BuffType<SearedEffect>(),0.7f)
			]
		);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)

			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_GlowingMushroom, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust1 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedsWingsRun, direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust1.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.MushroomTorch, direction.X * 0.2f, direction.Y * 0.2f, Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_GlowingMushroom, Scale: 1.2f * area.RelativeScale());
			Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedsWingsRun, Scale: area.RelativeScale());
			Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.MushroomTorch, Scale: 2f * area.RelativeScale())];
			spawnedDust2.noGravity = true;
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Water_GlowingMushroom, (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust1 = Main.dust[Dust.NewDust(position, 0, 0, DustID.RedsWingsRun, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust1.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.RedsWingsRun, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Water_GlowingMushroom, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust1 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedsWingsRun, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust1.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.RedsWingsRun, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}
