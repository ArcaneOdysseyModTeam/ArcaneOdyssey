using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class AcidMagic : AOMagic
	{
		public override float DashSpeed => 1.2f; // burst
		public override Color ImbueColour => new(245, 0, 240);
		public override float AOImbueSpeed => 0.925f;
		public override float AOImbueSize => 1f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.05f;
		public override float AOScrollDamage => 0.875f;
		public override SoundStyle? ImbueSound => SoundID.Splash;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Venom, 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				ModContent.BuffType<SandyEffect>()
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.075f),
				new(BuffID.OnFire,1.075f),
				new(ModContent.BuffType<CharredEffect>(),1.1f),
				new(ModContent.BuffType<FreezingEffect>(),1.2f),
				new(BuffID.OnFire3,1.05f),
				new(BuffID.Poisoned,1.05f),
				new(BuffID.ShadowFlame,1.1f),
				new(BuffID.Wet,0.9f),
				new(BuffID.Oiled,1.05f),
				new(ModContent.BuffType<Crystallized>(),0.9f),
				new(ModContent.BuffType<SandyEffect>(),0.99f),
				new(ModContent.BuffType<AOScalding>(),1.075f),
				new(ModContent.BuffType<SearedEffect>(),1.1f)
			]
			);



		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.UnholyWater, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Venom, 0f, 0f, 0, default, 1f)];
			Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.UnholyWater, 0f, 0f, 0, default, 1.6f)];
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Venom, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 1f)];
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.UnholyWater, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.UnholyWater, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
	}
}