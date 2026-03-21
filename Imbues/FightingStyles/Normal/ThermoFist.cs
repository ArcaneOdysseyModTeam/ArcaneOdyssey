using ArcaneOdyssey.AOPlayers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;

namespace ArcaneOdyssey.Imbues.FightingStyles.Normal
{
	public class ThermoFist : FightingStyleBarred
	{
		public override float Aura => .75f;
		public override bool? Cold => false;
		public override Color ImbueColour => Color.Orange;
		public override SoundStyle? ImbueSound => SoundID.Item20;

		public override float BarValueMulti => 1f;

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
		public override float DashSpeed => BarValue > (BarMax / 2) ? 1.4f : 1f; // instant?

		public override Debuff[] ImbueDebuffs => [Debuff.Create<SearedEffect>()];
		public override Combo[] CombinedDebuffs => [Combo.Create<CharredEffect, Petrified>()];
		public override SynergyEffects Effects => new(
			[
				ClearBuff.Create<Soaked>(),
				ClearBuff.Create<AOBleed>(),
				ClearBuff.Create<FreezingEffect>()
			],
			[
				Synergy.Create<Crystallized>(0.85f),
				Synergy.Create<SnowyEffect>(0.95f),
				Synergy.Create<FreezingEffect>(0.95f),
				Synergy.Create<AOBleed>(1.15f),
				Synergy.Create<CharredEffect>(1.1f),
				Synergy.Create<Melting>(1.075f),
				Synergy.Create<Corroding>(1.075f),
				Synergy.Create<SearedEffect>(1.1f),
				Synergy.Create<Scorched>(1.1f),
				Synergy.Create<SandyEffect>(0.8f),
				Synergy.Create<AOBurning>(1.1f),
				Synergy.Create<Scalding>(1.1f),
			]
		);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			BarValue += BarMax / 40f; // nerfed lmao
			Item.ArcaneOdyssey()?.owner?.ArcaneOdyssey()?.SetCooldown(new Cooldown(Name, DisplayName, 60));
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 10)), 1); n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.CrimsonTorch, direction.X * 0.4f, direction.Y * 0.4f, Scale: LerpValue * area.RelativeScale());
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3 * 2)), 1); n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.CrimsonTorch, Scale: LerpValue * 2f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				spawnedDust.noLight = true;
			}
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			Item.ArcaneOdyssey()?.owner?.ArcaneOdyssey()?.SetCooldown(new Cooldown(Name, DisplayName, 60));
			for (int n = 0; n < (int)Math.Max(Math.Round((float)BarValue / (BarMax / 3)), 1); n++)
			{
				Dust.NewDust(position, 0, 0, DustID.CrimsonTorch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: LerpValue * 3f * intensity);
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.CrimsonTorch, 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 2f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: LerpValue * 2f * area.RelativeScale());
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
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
				player.opacityForAnimation = 1f;
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
			if (Player.HasTypeInInventory<ThermoFist>(out var thermo))
			{
				if (!Player.ArcaneOdyssey().OnCooldown(thermo.Name))
					thermo.BarValue -= BarMax / (BarMax * .6f * (BarMax / 10f));
			}
		}
	}
}
