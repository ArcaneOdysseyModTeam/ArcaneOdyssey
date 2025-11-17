using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
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
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.85f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(),0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.15f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(BuffID.Venom,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),0.8f),
				new MagicBuffMultiplier(BuffID.OnFire,1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),1.1f),
			]
		);

		public override void SpawningEffects(Entity projectile)
		{
			BarValue += BarMax / 40f; // nerfed lmao
			if (projectile.TryGetOwner(out AOPlayer owner)) 
			{
				owner.SetCooldown(new(Name, DisplayName, true, 60));
			}
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 10)), 1); n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.CrimsonTorch, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, 0, default, (float)Math.Max(Math.Round((float)BarValue/50f),1));
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3 * 2)), 1); n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.CrimsonTorch, 0f, 0f, 0, default, (float)Math.Max((float)BarValue / 50f, 1))];
				spawnedDust.noGravity = true;
				spawnedDust.noLight = true;
			}
		}
		
		public override void ExplosionEffects(Entity projectile)
		{
			if (projectile.TryGetOwner(out AOPlayer owner))
			{
				owner.SetCooldown(new(Name, DisplayName, true, 60));
			}
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3)), 1); n++)
			{
				Dust.NewDust(projectile.Center, 1, 1, DustID.CrimsonTorch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, (float)Math.Max(Math.Round((float)BarValue * (BarMax * .286f)), 1));
			}
		}
		
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.CrimsonTorch, 2f * (Main.rand.NextFloat() - 0.5f), 2f * (Main.rand.NextFloat() - 0.5f), 0, default, 2f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
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

		public override void UpdateInventory(Item item, Player player)
		{
			if (item.Imbue() is ThermoFist thermo)
			{
			}
		}

		public override void UseAnimation(Item item, Player player)
		{
			if (item.Imbue() is ThermoFist thermo)
			{
				thermo.BarValue += BarMax / 20f;
				player.ArcaneOdyssey().SetCooldown(new(thermo.Name, thermo.DisplayName, true, 60));
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
