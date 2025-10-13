using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.Stuns;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.FightingStyles
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
			BarValue += 5;
			if (projectile.GetOwner(out var owner)) 
			{
				owner.ItemCooldowns[Type] = 60;
			}
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue/10f),1); n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.CrimsonTorch, (projectile.velocity.X * 0.4f), (projectile.velocity.Y * 0.4f), 0, default, (float)Math.Max(Math.Round((float)BarValue/50f),1));
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / 66.6f), 1); n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.CrimsonTorch, 0f, 0f, 0, default, (float)Math.Max((float)BarValue / 50f, 1))];
				spawnedDust.noGravity = true;
				spawnedDust.noLight = true;
			}
		}
		public override void ExplosionEffects(Entity projectile)
		{
			BarValue++;
			if (projectile.GetOwner(out var owner))
			{
				owner.ItemCooldowns[Type] = 60;
			}
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / 33.3f), 1); n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.CrimsonTorch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, (float)Math.Max(Math.Round((float)BarValue / 28.6f), 1));
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.CrimsonTorch, (2f * Main.rand.NextFloat() - 0.5f), (2f * Main.rand.NextFloat() - 0.5f), 0, default, 2f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.Hellstone, 10).Register();
		}
	}

	public class ThermoBars : GlobalItem
	{
		public override void UseAnimation(Item item, Player player)
		{
			if (item.TryGetImbue(out var im) && ImbueClassCheck(item) && im is ThermoFist thermo && thermo.GetThisImbue(player))
			{
				thermo.BarValue += 5;
				player.ArcaneOdyssey().ItemCooldowns[thermo.Type] = 60;
			}
		}
	}

	public class ThermoFallOff : ModPlayer
	{
		public bool resetBar = false;
		public override void PostUpdate()
		{
			if (Player.TryGetImbue(out Imbuable imbue))
			{
				if (imbue is ThermoFist thermo && thermo.GetThisImbue(Player))
				{
					if (resetBar)
					{
						resetBar = false;
						thermo.BarValue = 0;
					}
					if (!Player.ArcaneOdyssey().ItemCooldowns.ContainsKey(thermo.Type))
						thermo.BarValue -= 100f / (60 * 10f);
				}
			}
		}
	}
}
