using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Ancient
{
	public class IonMagic : AOMagic
	{
		public override float DashSpeed => 1.5f; // instant
		public override bool? Cold => false;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Ancient;
		public override SoundStyle? ImbueSound => SoundID.Item91;
		public override Color ImbueColour => new Color(0, 255, 0, 255);
		public override bool CanBeWet => false;
		public override float AOImbueSpeed => 1.5f;
		public override float AOImbueSize => 1.2f;
		public override float AOImbueDamage => 1.6f;
		public override float AOScrollSpeed => 1.5f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 1.6f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<IonizedEffect>(), 60 * 10)];
		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<AOBleed>(),
				ModContent.BuffType<CharredEffect>(),
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.15f),
				new(BuffID.OnFire,1.075f),
				new(ModContent.BuffType<CharredEffect>(),1.1f),
				new(BuffID.Venom,1.05f),
				new(ModContent.BuffType<Crystallized>(),0.99f),
				new(ModContent.BuffType<FreezingEffect>(),0.97f),
				new(BuffID.OnFire3,1.05f),
				new(BuffID.Poisoned,1.05f),
				new(ModContent.BuffType<SnowyEffect>(),0.99f),
				new(BuffID.Wet,0.95f),
				new(BuffID.Slimed,1.075f),
				new(BuffID.Oiled,1.075f),
				new(ModContent.BuffType<AOScalding>(),1.075f),
				new(ModContent.BuffType<SearedEffect>(),1.1f),
				new(BuffID.ShadowFlame,1.1f)
			]
			);
		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CursedTorch, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, 0, default, 3f)];
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CursedTorch, 0f, 0f, 0, default, 3f)];
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.CursedTorch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 4f)];
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CursedTorch, 5f * (Main.rand.NextFloat() - 0.5f), 5f * (Main.rand.NextFloat() - 0.5f), 0, default, 4f)];
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}

		public override void AddRecipes()
		{
			//CreateAncientRecipe(typeof(AetherMagic), typeof(HeatMagic), typeof(FlareMagic),typeof(ShadowflameMagic));
		}
	}
}