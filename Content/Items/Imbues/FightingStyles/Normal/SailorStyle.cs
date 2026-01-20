using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.Audio;
using ArcaneOdyssey.Content.Buffs.Stuns;

namespace ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal
{
	public class SailorStyle : FightingStyleBarred
	{
		public override float DashSpeed => BarValue > (BarMax / 2) ? 1.2f : 1f; // burst?
		public override bool? Cold => true;
		public override Color ImbueColour => Color.CornflowerBlue;
		public override SoundStyle? ImbueSound => SoundID.Splash;


		public override float MaxImbueSpeed => 1f;
		public override float MaxImbueDamage => .925f;
		public override float MaxImbueSize => 1.278f;
		public override float MinImbueSpeed => 1f;
		public override float MinImbueDamage => .85f;
		public override float MinImbueSize => .833f;
		public override float MaxScrollSpeed => 1f;
		public override float MaxScrollDamage => .85f;
		public override float MaxScrollSize => 1.2f;
		public override float MinScrollSpeed => 1f;
		public override float MinScrollDamage => .775f;
		public override float MinScrollSize => .8f;
		public override Color DisplayColor => Color.PaleVioletRed;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Wet, 60 * 10)];
		public override SynergyEffects Effects => new(
			[
				ModContent.BuffType<SearedEffect>(),
				ModContent.BuffType<CharredEffect>(),
				BuffID.OnFire,
				BuffID.OnFire3,
				BuffID.Venom,
				BuffID.ShadowFlame,
				ModContent.BuffType<Singed>(),
				ModContent.BuffType<AOScalding>(),
				ModContent.BuffType<AOPetrified>()
			],
			[
				new(ModContent.BuffType<Crystallized>(),1.1f),
				new(ModContent.BuffType<SnowyEffect>(),1.1f),
				new(ModContent.BuffType<FreezingEffect>(),1.075f),
				new(ModContent.BuffType<AOBleed>(),1.05f),
				new(ModContent.BuffType<CharredEffect>(),0.9f),
				new(BuffID.OnFire3,0.9f),
				new(ModContent.BuffType<Singed>(), 0.8f),
				new(BuffID.Venom,0.9f),
				new(ModContent.BuffType<SearedEffect>(),0.85f),
				new(BuffID.ShadowFlame,0.85f),
				new(ModContent.BuffType<SandyEffect>(),0.8f),
				new(BuffID.OnFire,0.8f)
			]
		);

		public override void SpawningEffects(Entity projectile)
		{
			BarValue -= BarMax / 100f;
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3)), 1); n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Water, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, LerpValue)];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3 * 2)), 1); n++)
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Water, 0f, 0f, 0, default, LerpValue * 2.2f);
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3)), 1); n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Water, (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), 0, default, LerpValue)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Water, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, LerpValue * 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center);
		}

		public override void AddRecipes()
		{
			var rec = CreateRecipe().AddIngredient<BasicCombat>();
			bool usecoral = true;
			if (ExternalModSupport.HasCalamity)
			{
				usecoral = false;
				rec.AddIngredient(ExternalModSupport.Calamity.Find<ModItem>("SeaRemains"), 5);
			}
			if (ExternalModSupport.HasThorium)
			{
				usecoral = false;
				rec.AddIngredient(ExternalModSupport.Thorium.Find<ModItem>("DepthScale"), 5);
			}
			if (usecoral)
			{
				rec.AddIngredient(ItemID.Coral, 15);
			}
			rec.Register();
		}

		public override void UpdateInventory(Player player)
		{
			if (player.wet && !player.honeyWet && !player.lavaWet)
			{
				BarValue += BarMax / (BarMax * .6f * 2.5f);
			}
			base.UpdateInventory(player);
		}
	}

	public class SailorBars : GlobalItem
	{
		public override void UseAnimation(Item item, Player player)
		{
			if (item.Imbue() is SailorStyle imbue)
			{
				imbue.BarValue -= FightingStyleBarred.BarMax / 100f;
			}
		}

		public override void OnConsumeItem(Item item, Player player)
		{
			if (item.potion)
			{
				if (player.Imbue() is SailorStyle imbue)
				{
					imbue.BarValue = FightingStyleBarred.BarMax;
				}
				if (player.PlayerItem()?.Imbue() is SailorStyle imbue2)
				{
					imbue2.BarValue = FightingStyleBarred.BarMax;
				}

			}
		}
	}
}
