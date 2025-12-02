using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class ExplosionMagic : AOMagic
    {
        public override float DashSpeed => 1.2f; // burst
        public override bool? Cold => false;
        public override Color ImbueColour => new(235, 146, 52);
		public override float AOImbueSpeed => 0.925f;
        public override bool CanBeWet => false;
        public override float AOImbueSize => 1.3f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 0.85f;
		public override float AOScrollSize => 1.3f;
		public override float AOScrollDamage => 0.925f;
        public override SoundStyle? ImbueSound => SoundID.Item14;
		public override AODebuffRequirement[] ImbueDebuffs => [new AODebuffRequirement(ModContent.BuffType<CharredEffect>(), 60*10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.01f),
				new MagicBuffMultiplier(BuffID.OnFire,1.125f),
				new MagicBuffMultiplier(BuffID.Venom,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.01f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(),0.99f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.15f),
				new MagicBuffMultiplier(BuffID.Wet,0.99f),
				new(BuffID.Oiled,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),1.125f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);
		public override void SpawningEffects(Entity projectile) 
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Ash, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 4f)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, 0f, 0f, 0, default, 1.6f)];
			spawnedDust.noGravity = true;
			Dust spawnedDust3 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, 0f, 0f, 0, default, 1.6f)];
			spawnedDust3.noGravity = true;
			Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Ash, 0f, 0f, 0, default, 2f)];
			spawnedDust2.noGravity = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (25f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (25f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * AOScrollSize), 0, default, 3f)];
				spawnedDust2.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (25f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (25f * AOScrollSize), 0, default, 4f)];
				spawnedDust3.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, 18f * (Main.rand.NextFloat() - 0.5f), 18f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, 18f * (Main.rand.NextFloat() - 0.5f), 18f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Ash, 18f * (Main.rand.NextFloat() - 0.5f), 18f * (Main.rand.NextFloat() - 0.5f), 0, default, 4f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}

        
	}
}