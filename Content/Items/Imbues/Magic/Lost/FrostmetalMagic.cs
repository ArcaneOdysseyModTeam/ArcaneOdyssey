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
        public override bool? Cold => true;
		public override float DashResist => 1.45f;
		public override SoundStyle? ImbueSound => SoundID.Item27;
		public override Color ImbueColour => Color.Lerp(new(100, 100, 100), new(30, 200, 255), .5f);
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
				new(BuffID.Venom,1.05f),
				new(ModContent.BuffType<FreezingEffect>(),1.1f),
				new(BuffID.OnFire3,1.05f),
				new(ModContent.BuffType<SandyEffect>(),1.1f),
				new(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
				new(ModContent.BuffType<AOFrozen>(), 1.1f), // frozen
				new(BuffID.Wet, 1.1f), // (add stunning later!)
				new(BuffID.OnFire, .9f), // burning
				new(BuffID.Oiled,1.03f),
				new(ModContent.BuffType<CharredEffect>(), .9f), // charred
				new(BuffID.ShadowFlame, 0.8f),
				new(ModContent.BuffType<SnowyEffect>(), 1.1f),
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<SearedEffect>(),0.8f)

			]
			);

		public override void SpawningEffects(Entity projectile)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Mercury, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, Scale: 2f);
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SnowflakeIce, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f)];
				spawnedDust.noGravity = true;
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Ice, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			if (Main.dedServ)
				return;
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SilverFlame, Scale: 2f)];
			spawnedDust.noGravity = true;
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Ice);
		}

		public override void ExplosionEffects(Entity projectile)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.SnowflakeIce, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize))];
				spawnedDust.noGravity = true;
				Dust.NewDust(projectile.Center, 0, 0, DustID.Ice, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize));
				Dust.NewDust(projectile.Center, 0, 0, DustID.Mercury, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), Scale: 2f);
			}
		}

		public override bool PreEffects(Entity entity)
		{
			if (entity is Projectile projectile)
				if (projectile.ModProjectile is FrostmetalShard)
					return false;
			return base.PreEffects(entity);
		}

		public override void KillEffects(Entity entity)
		{
			if (Main.dedServ)
				return;
			if (entity is Projectile projectile && Main.myPlayer == projectile.owner)
			{
				for (int i = 0; i < 3; i++)
				{
					var angle = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * 7f;
					angle.Y *= 0.35f;
					var proj = Projectile.NewProjectileDirect(entity.GetSource_FromThis(), entity.Center, angle, ModContent.ProjectileType<FrostmetalShard>(), projectile.damage / 6, projectile.knockBack / 6, projectile.owner);
					proj.frame = i;
				}
			}
			for (int n = 0; n < 15; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(entity.position, entity.width, entity.height, DustID.SnowflakeIce, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f))];
				spawnedDust.noGravity = true;
				Dust.NewDust(entity.position, entity.width, entity.height, DustID.Ice, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f));
				Dust.NewDust(entity.position, entity.width, entity.height, DustID.Mercury, 2f * (Main.rand.NextFloat() - 0.5f), 2f * (Main.rand.NextFloat() - 0.5f), Scale: 2f);
			}
			SoundEngine.PlaySound(ImbueSound, entity.position, null);
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(IceMagic), typeof(MetalMagic), typeof(SnowMagic));
		}
	}
}