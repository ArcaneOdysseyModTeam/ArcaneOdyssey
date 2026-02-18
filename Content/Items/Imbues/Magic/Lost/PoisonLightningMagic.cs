using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class PoisonLightningMagic : AOMagic
	{
		public override float DashSpeed => 1.5f; // instant
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningAuraZap;
		public override Color ImbueColour => new(105, 0, 105, 255);
		public override float AOImbueSpeed => 1.4f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 0.9f;
		public override float AOScrollSpeed => 1.4f;
		public override float AOScrollSize => 1.15f;
		public override float AOScrollDamage => 0.9f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<ElectrifiedToxins>(), 60 * 10), new(ModContent.BuffType<AOParalyzed>(), 60, 25)];
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOParalyzed>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<AOPetrified>(), // petrified
				ModContent.BuffType<CharredEffect>(),
				ModContent.BuffType<SandyEffect>(),
				ModContent.BuffType<AOBleed>(),
				ModContent.BuffType<AOFrozen>()
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.075f),
				new(BuffID.OnFire,0.99f),
				new(ModContent.BuffType<AOScalding>(),0.9f),
				new(BuffID.Chilled, 1.2f), // frozen
				new(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
				new(BuffID.Burning, 1.15f), // scalding
				new(BuffID.OnFire3, 1.075f), // melting/hellfire
				new(BuffID.Venom, 1.075f), // venom acid
				new(BuffID.Wet, 1.05f), //
				new(BuffID.Oiled,0.98f),
				new(BuffID.ShadowFlame,1.15f),
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);
		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Cloud, direction.X * 0.4f, direction.Y * 0.4f, 0, Color.Purple, 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, direction.X * 0.2f, direction.Y * 0.2f, Scale: 1.2f * area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Cloud, 0f, 0f, 0, Color.Purple, 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
			float waveVal = 10f * MathF.Abs((float)Main.GameUpdateCount % 5 % 10f - 2.5f) - 12.5f;
			if (source is Projectile projectile && projectile.extraUpdates > 0)
			{
				waveVal = 10f * MathF.Abs(((float)Main.GameUpdateCount + (float)projectile.numUpdates) % 5 % 10f - 2.5f) - 12.5f;
			}
			Vector2 baseVec = new(0f, waveVal);
			Dust spawnedDust2 = Dust.NewDustPerfect(area.Center() + baseVec.RotatedBy(direction.GetValueOrDefault(Vector2.One).ToRotation()), DustID.CrystalPulse, Scale: 1.2f);
			spawnedDust2.noGravity = true;
			Lighting.AddLight(area.Center(), 2, 1, 2);
		}
		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Cloud, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), 0, Color.Purple, 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust.NewDust(position, 0, 0, DustID.WitherLightning, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 1.2f * intensity);
			}
		}
		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Cloud, 5f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 5f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 0, Color.Purple, 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.WitherLightning, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 1.2f * area.RelativeScale());
				if (source is Projectile projectile && n / 2 >= 10)
					Projectile.NewProjectile(projectile.GetSource_FromThis(), new(area.X + area.Width * Main.rand.NextFloat(), area.Y + area.Height * Main.rand.NextFloat()), new(1.25f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 1.25f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f)), Main.rand.Next([ProjectileID.SporeGas, ProjectileID.SporeGas2, ProjectileID.SporeGas3]), 2 + BossesKilled, 0f);
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}
	}
}