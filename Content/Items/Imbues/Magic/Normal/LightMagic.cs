using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class LightMagic : AOMagic
	{
		public override float DashSpeed => 1.5f; // instant
		public override SoundStyle? ImbueSound => SoundID.Item9;
		public override Color ImbueColour => new(255, 255, 0, 255);
		public override float AOImbueSpeed => 1.3f;
		public override float AOImbueSize => 0.946f;
		public override float AOImbueDamage => 0.9f;
		public override float AOScrollSpeed => 1.6f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 0.87f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<BlindedEffect>(), 60 * 5)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			],
			[
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<DrainedEffect>(),0.8f)
			]
			);
		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.YellowStarDust, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.YellowTorch, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 0, default, 3f)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.YellowStarDust, 0f, 0f, 0, default, 1f)];
			Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.YellowTorch, 0f, 0f, 0, default, 2f)];
			spawnedDust2.noGravity = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.YellowStarDust, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.YellowTorch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.YellowStarDust, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.YellowTorch, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center);
		}
	}
}