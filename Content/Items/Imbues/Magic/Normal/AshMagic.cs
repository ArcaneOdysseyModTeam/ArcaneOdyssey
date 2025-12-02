using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class AshMagic : AOMagic
    {
        public override bool? Cold => false;
        public override bool CanBeWet => false;
        public override Color ImbueColour =>  new(235,40,0,0);
        public override float AOImbueSpeed => 0.975f;
		public override float AOImbueSize => 1.22f;
		public override float AOImbueDamage => 0.95f;
		public override float AOScrollSpeed => 0.95f;
		public override float AOScrollSize => 1.25f;
		public override float AOScrollDamage => 0.875f;
        public override SoundStyle? ImbueSound => SoundID.Dig;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOPetrified>(), 60*10,33)];
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.OnFire3, ModContent.BuffType<AOPetrified>()),new(BuffID.OnFire, ModContent.BuffType<AOPetrified>()),new(BuffID.ShadowFlame, ModContent.BuffType<AOPetrified>()),new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>()),new(ModContent.BuffType<AOScalding>(), ModContent.BuffType<AOPetrified>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				BuffID.Wet,
				ModContent.BuffType<SnowyEffect>(),
				ModContent.BuffType<FreezingEffect>(),
				BuffID.OnFire,
				BuffID.OnFire3,
				ModContent.BuffType<CharredEffect>(),
				BuffID.ShadowFlame,
				ModContent.BuffType<AOScalding>()
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.1f),
				new(BuffID.OnFire,1.02f),
				new(BuffID.Venom,1.075f),
				new(BuffID.Slimed,1.075f),
				new(BuffID.Oiled,1.075f),
				new(BuffID.OnFire3,1.075f),
				new(BuffID.ShadowFlame,1.15f),
				new(BuffID.Wet,0.995f),
				new(ModContent.BuffType<FreezingEffect>(),0.99f),
				new(ModContent.BuffType<CharredEffect>(),1.01f),
				new(ModContent.BuffType<SandyEffect>(),1.125f),
				new(ModContent.BuffType<AOScalding>(),1.2f),
				new(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Ash, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.RedTorch, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 2f)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			_ = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.RedTorch, 0f, 0f, 0, default, 1f);
			Dust spawnedDust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Ash, 0f, 0f, 0, default, 2f);
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.RedTorch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 1f)];
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 2f)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Ash, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f);
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.RedTorch, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 2f);
				spawnedDust2.noGravity = true;
			}
			for (int n = 0; n < 10; n++)
			{
				Projectile.NewProjectile(projectile.GetSource_FromThis(), new(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), new(1.23f * (Main.rand.NextFloat() - 0.5f), 1.23f * (Main.rand.NextFloat() - 0.5f)), ProjectileID.SporeCloud, 2 + BossesKilled, 0f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);

		}

        public override bool PreEffects(Entity projectile)
        {
			if (projectile is Projectile proj)
				return base.PreEffects(projectile) && proj.type != ProjectileID.SporeCloud;
			return base.PreEffects(projectile);
        }

        
	}
}