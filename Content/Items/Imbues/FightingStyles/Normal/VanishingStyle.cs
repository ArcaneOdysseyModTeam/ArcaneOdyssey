using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal
{
	public class VanishingStyle : FightingStyleBarred
	{
		public static bool HasYou => ModLoader.HasMod("YouBoss");

		public override Color ImbueColour => Color.Black;
		public override SoundStyle? ImbueSound => SoundID.Item64;
		public override Color DisplayColor => Color.White;
		public override float MinImbueSpeed => !HasYou ? 1.1f : 1.5f;

		public override AOImbuableTier ImbuableTier => !HasYou ? base.ImbuableTier : AOImbuableTier.Ancient;

		public override float MinImbueDamage => !HasYou ? .85f : 1f;
		public override float MinImbueSize => !HasYou ? 1.056f : 1.2f;
		public override float MinScrollSize => !HasYou ? 1.0f : 1.125f;
		public override float MaxScrollSpeed => MinScrollSpeed;
		public override float MaxScrollDamage => MinScrollDamage;
		public override float MaxScrollSize => MinScrollSize;
		public override float MinScrollSpeed => MinImbueSpeed;
		public override float MinScrollDamage => MinImbueDamage;
		public override float MaxImbueSpeed => MinImbueSpeed;
		public override float MaxImbueDamage => MinImbueDamage;
		public override float MaxImbueSize => MinImbueSize;

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Wraith, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Wraith, 0f, 0f, 0, default, 2f)];
			spawnedDust.noGravity = true;
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Wraith, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center, null);
		}

		public override void AddRecipes()
		{
			if (!HasYou)
				CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.SoulofNight, 5).Register();
			else
				CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ModLoader.GetMod("YouBoss").Find<ModItem>("FirstFractal")).Register();
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

	public class VanishingPlayer : ModPlayer
	{
		public const float BarMax = FightingStyleBarred.BarMax;
		public const float BarMin = FightingStyleBarred.BarMin;
		public override void PreUpdate()
		{
			if (Player.ArcaneOdyssey()?.Imbue is VanishingStyle vanish && vanish.GetThisImbue(Player))
			{
				Player.opacityForAnimation = vanish.LerpValue.FlipFloat() - 1f;
				if (!Player.ArcaneOdyssey().OnCooldown(vanish.Name))
					vanish.BarValue -= BarMax / (BarMax * .6f * (BarMax / 10f));
			}
			else if (Player.HasTypeInInventory(typeof(VanishingStyle), out var vanish1))
			{
				if (((VanishingStyle)vanish1.ModItem).GetThisImbue(Player))
				{
					Player.opacityForAnimation = ((VanishingStyle)vanish1.ModItem).LerpValue.FlipFloat() - 1f;
					if (!Player.ArcaneOdyssey().OnCooldown(((VanishingStyle)vanish1.ModItem).Name))
						((VanishingStyle)vanish1.ModItem).BarValue -= BarMax / (BarMax * .6f * (BarMax / 10f));
				}
			}
		}
	}
}
