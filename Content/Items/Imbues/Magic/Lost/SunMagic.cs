using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Buffs.Helpers;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class SunMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float DashSpeed => 1.2f; // burst
		public override bool? Cold => false;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Color ImbueColour => new(252, 137, 18);
		public override bool CanBeWet => false;
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.3f;
		public override float AOImbueDamage => 1.2f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.3f;
		public override float AOScrollDamage => 1.2f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<ProminenceDebuff>(), 60 * 10)];
		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<AOBleed>(),
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet,
				ModContent.BuffType<CharredEffect>(),
				BuffID.Slimed
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.15f),
				new(ModContent.BuffType<CharredEffect>(),1.01f),
				new(BuffID.Venom,1.05f),
				new(ModContent.BuffType<Crystallized>(),0.85f),
				new(ModContent.BuffType<FreezingEffect>(),0.99f),
				new(ModContent.BuffType<SnowyEffect>(),0.99f),
				new(BuffID.Wet,0.99f),
				new(BuffID.OnFire3,1.05f),
				new(BuffID.Poisoned,1.05f),
				new(BuffID.ShadowFlame,1.1f),
				new(BuffID.Slimed,1.075f),
				new(BuffID.Oiled,1.075f),
				new(ModContent.BuffType<SandyEffect>(),0.98f),
				new(ModContent.BuffType<AOScalding>(),1.1f),
				new(ModContent.BuffType<SearedEffect>(),1.1f)

			]
			);

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Torch, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 5f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Torch, 0f, 0f, 0, default, 2f);
			}

		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Flare, (Main.rand.NextFloat() - 0.5f) * (30f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (30f * AOScrollSize), 0, default, 8f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Torch, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 8f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center);
		}



		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(FireMagic), typeof(PlasmaMagic),typeof(EarthMagic),typeof(MagmaMagic),typeof(AshMagic),typeof(ExplosionMagic));
		}
	}
}
