using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.Stuns;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal
{
	public class ThermoFist : FightingStyleBarred
	{
		public override bool? Cold => false;
		public override Color ImbueColour => Color.Orange;
		public override SoundStyle? ImbueSound => SoundID.Item20;

		public override float MaxImbueSpeed => 1.3f;
		public override float MaxImbueDamage => .85f;
		public override float MaxImbueSize => .833f;
		public override float MinImbueSpeed => 1f;
		public override float MinImbueDamage => .85f;
		public override float MinImbueSize => .833f;
		public override float MaxScrollSpeed => 1.3f;
		public override float MaxScrollDamage => .75f;
		public override float MaxScrollSize => .8f;
		public override float MinScrollSpeed => 1f;
		public override float MinScrollDamage => .75f;
		public override float MinScrollSize => .8f;
		public override Color DisplayColor => Color.Blue;
		public override float DashSpeed => BarValue > (BarMax / 2) ? 1.5f : 1f; // instant?

		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<SearedEffect>(), 60 * 10)];
		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override SynergyEffects Effects => new(
			[
				BuffID.Wet,
				ModContent.BuffType<AOBleed>(),
				ModContent.BuffType<FreezingEffect>()
			],
			[
				new(ModContent.BuffType<Crystallized>(),0.85f),
				new(ModContent.BuffType<SnowyEffect>(),0.95f),
				new(ModContent.BuffType<FreezingEffect>(),0.95f),
				new(ModContent.BuffType<AOBleed>(),1.15f),
				new(ModContent.BuffType<CharredEffect>(),1.1f),
				new(BuffID.OnFire3,1.075f),
				new(BuffID.Venom,1.075f),
				new(ModContent.BuffType<SearedEffect>(),1.1f),
				new(BuffID.ShadowFlame,1.1f),
				new(ModContent.BuffType<SandyEffect>(),0.8f),
				new(BuffID.OnFire,1.1f),
				new(ModContent.BuffType<AOScalding>(),1.1f),
			]
		);

		public override void SpawningEffects(Entity projectile)
		{
			BarValue += BarMax / 40f; // nerfed lmao
			if (projectile.TryGetOwner(out AOPlayer owner))
			{
				owner.SetCooldown(new Cooldown(Name, DisplayName, 60));
			}
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 10)), 1); n++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CrimsonTorch, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, 0, default, LerpValue);
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3 * 2)), 1); n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CrimsonTorch, 0f, 0f, 0, default, LerpValue * 2f)];
				spawnedDust.noGravity = true;
				spawnedDust.noLight = true;
			}
		}

		public override void ExplosionEffects(Entity projectile)
		{
			if (projectile.TryGetOwner(out AOPlayer owner))
			{
				owner.SetCooldown(new Cooldown(Name, DisplayName, 60));
			}
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3)), 1); n++)
			{
				Dust.NewDust(projectile.Center, 0, 0, DustID.CrimsonTorch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, LerpValue * 3f);
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CrimsonTorch, 2f * (Main.rand.NextFloat() - 0.5f), 2f * (Main.rand.NextFloat() - 0.5f), 0, default, LerpValue * 2f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center, null);
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.Hellstone, 10).Register();
		}

		public override void UpdateInventory(Player player)
		{
			if (player.GetModPlayer<ThermoFallOff>().resetBar)
			{
				BarValue = BarMin;
				player.GetModPlayer<ThermoFallOff>().resetBar = false;
			}
			if (!player.ArcaneOdyssey().OnCooldown(Name))
				BarValue -= BarMax / (BarMax * .6f * (BarMax / 10f));
			base.UpdateInventory(player);
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			BarValue = BarMin;
		}
	}

	public class ThermoBars : GlobalItem
	{
		public const float BarMax = FightingStyleBarred.BarMax;
		public const float BarMin = FightingStyleBarred.BarMin;

		public override void UseAnimation(Item item, Player player)
		{
			if (item.Imbue() is ThermoFist thermo)
			{
				thermo.BarValue += BarMax / 20f;
				player.ArcaneOdyssey().SetCooldown(new Cooldown(thermo.Name, thermo.DisplayName, 60));
			}
		}
	}

	public class ThermoFallOff : ModPlayer
	{
		public const float BarMax = FightingStyleBarred.BarMax;
		public const float BarMin = FightingStyleBarred.BarMin;
		public bool resetBar = false;

		public override void PostUpdate()
		{
			if (Player.TryGetImbue(out Imbuable imbue))
			{
				if (imbue is ThermoFist thermo && thermo.GetThisImbue(Player))
				{
					if (!Player.ArcaneOdyssey().OnCooldown(thermo.Name))
						thermo.BarValue -= BarMax / (BarMax * .6f * (BarMax / 10f));
				}
			}
		}
	}
}
