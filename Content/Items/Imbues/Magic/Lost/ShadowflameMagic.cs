using ArcaneOdyssey.Content.Items.Base;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria.ID;
using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using Terraria;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;


namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class ShadowflameMagic : AOMagic
	{
		public override float DashSpeed => 1.2f; // burst
		public override bool? Cold => false;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Color ImbueColour => new(255, 100, 255);
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override bool CanBeWet => true;
		public override float AOImbueSpeed => 1.1f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 1.1f;
		public override float AOScrollSpeed => 1.1f;
		public override float AOScrollSize => 1.15f;
		public override float AOScrollDamage => 1.05f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.ShadowFlame, 60*10)];
		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<AOBleed>(),
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet,
				ModContent.BuffType<CharredEffect>()
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
				new(BuffID.OnFire,1.1f),
				new(BuffID.Slimed,1.075f),
				new(BuffID.Oiled,1.075f),
				new(ModContent.BuffType<SandyEffect>(),0.98f),
				new(ModContent.BuffType<AOScalding>(),1.1f),
				new(ModContent.BuffType<SearedEffect>(),1.1f)
				
			]
			);

		public override void SpawningEffects(Entity projectile)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.FireworkFountain_Pink, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 1f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 2.4f)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			if (Main.dedServ)
				return;
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, 0f, 0f, 0, default, 1.6f);
			Dust spawnedDust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.FireworkFountain_Pink, 0f, 0f, 0, default, 0.8f);
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Entity entity)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(entity.Center, 0, 0, DustID.FireworkFountain_Pink, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 1.3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(entity.Center, 0, 0, DustID.Shadowflame, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 2.8f)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.FireworkFountain_Pink, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 2f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Shadowflame, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 2.8f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		
		public override void AddRecipes() 
		{
			CreateLostRecipe(typeof(ShadowMagic), typeof(FireMagic),typeof(PlasmaMagic));
		}
	}
}