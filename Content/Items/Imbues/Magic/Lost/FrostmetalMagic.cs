using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Projectiles.Magic.MagicEffects;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class FrostmetalMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float DashResist => 1.45f;
		public override SoundStyle? ImbueSound => SoundID.Item27;
		public override Color ImbueColour => new(100, 100, 100);
		public override float AOImbueSpeed => 0.8f;
		public override float AOImbueSize => 1.3f;
		public override float AOImbueDamage => 1.4f;
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOFrozen>())];
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60 * 10), new(ModContent.BuffType<FreezingEffect>(), 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				BuffID.Wet,
				BuffID.Burning,
				BuffID.Venom,
				BuffID.OnFire3,
				BuffID.ShadowFlame,
				ModContent.BuffType<CharredEffect>()
			],
			[
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
				new MagicBuffMultiplier(ModContent.BuffType<AOFrozen>(), 1.1f), // frozen
				new MagicBuffMultiplier(BuffID.Wet, 1.1f), // (add stunning later!)
				new MagicBuffMultiplier(BuffID.OnFire, .9f), // burning
				new(BuffID.Oiled,1.03f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(), .9f), // charred
				new MagicBuffMultiplier(BuffID.ShadowFlame, 0.8f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(), 1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),0.8f)

			]
			);

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Mercury, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f);
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SnowflakeIce, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Ice, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, default, 2f);
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SilverFlame)];
			spawnedDust.noGravity = true;
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Ice);
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.SnowflakeIce, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 2f)];
				spawnedDust.noGravity = true;
				Dust.NewDust(projectile.Center, 1, 1, DustID.Ice, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize));
				Dust.NewDust(projectile.Center, 1, 1, DustID.Mercury, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 2f);
			}
		}

		public override void KillEffects(Entity entity)
		{
            if (entity is Projectile projectile) 
            {
                if (projectile.ModProjectile is FrostmetalShard)
                    return;
                for (int i = 0; i <= 3; i++)
                {
                    Projectile.NewProjectileDirect(entity.GetSource_Death(), entity.Center, Vector2.Zero, ModContent.ProjectileType<FrostmetalShard>(), projectile.damage / 6, projectile.knockBack / 6, projectile.owner);
                }
            }
			for (int n = 0; n < 15; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(entity.position, entity.width, entity.height, DustID.SnowflakeIce, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust.NewDust(entity.position, entity.width, entity.height, DustID.Ice, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 2f);
				Dust.NewDust(entity.position, entity.width, entity.height, DustID.Mercury, 2f * (Main.rand.NextFloat() - 0.5f), 2f * (Main.rand.NextFloat() - 0.5f), 0, default, 1f);
			}
			SoundEngine.PlaySound(ImbueSound, entity.position, null);
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(IceMagic), typeof(MetalMagic), typeof(SnowMagic));
		}
	}
}