using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Bronze;
using Terraria.ModLoader;
using ArcaneOdyssey.PlayerClasses;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class SunkenSword : AORangedOrMeleeWeapon
	{
		public override bool? Cold => true;
		public override float AOSpeed => 1.2f;
		public override float AOSize => .9f;
		public override float AODamage => 1f;
		public override int AOValue => 900;
		public override AORarities AORarity => AORarities.Rare;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
		public override WeaponAbility? Ability => new(Mod, "Rising Tide", "Launch yourself upwards", Color.Aqua);
		public override SoundStyle UseSound => SoundID.SplashWeak;
		public override AODebuffRequirement? WeaponDebuff => new(BuffID.Wet, 60 * 5);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 50;
			Item.height = 54;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = TrueMelee();
			Item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool? UseItem(Player player)
		{
			if (player.AltUse())
			{
				var dash = new RisingTide(Item);
				if (!dash.OnCooldown(player))
					player.ArcaneOdyssey().StartDash(dash, -2, Imbue, true);
			}
			if (!Main.dedServ)
			{
				// Particles from swinging
				Dust.NewDust(player.MountedCenter + new Vector2(player.direction * 3f * (Imbue?.AOImbueSize ?? 1f), 0f), 3, 3, DustID.Water, (player.direction * 30f) * (0.8f - Main.rand.NextFloat()) * (Imbue?.AOImbueSize ?? 1f), 30f * (0.5f - Main.rand.NextFloat()) * (Imbue?.AOImbueSpeed ?? 1f), 255, default, 1.3f);
			}
			return null;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<RavennaSword>();
			recipe.AddIngredient<SunkenScrap>(2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	public class RisingTide(Entity source) : DashSystem(source)
	{
		
		public override float DashSpeed => 23;
		public override int DashMax => 60;
		public override bool AnyDirection => false;
		public override bool Immune => false;
		public override int Cooldown => 60 * 3;
		public override bool OnHit(Player player, Entity target)
		{
			return false;
		}

		public override void DashEffect(Player player)
		{
			player.statDefense += 20;
			if (player.ArcaneOdyssey().DashLeft % 5 == 0)
			{
				player.direction *= -1;
			}
		}

		public override void OnStart(Player player)
		{
			if (!Main.dedServ)
			{
				SoundEngine.PlaySound(SoundID.Splash, player.position);
				// Adds dust
				for (int dustCountInt = 0; dustCountInt < 50; dustCountInt++)
				{
					Dust.NewDust(player.position + new Vector2(-20f + (40f * ((float)Math.Sin(dustCountInt * 3.0))), 0f), 3, 3, DustID.Water, player.velocity.X * dustCountInt * 0.02f, -1f * dustCountInt, 255, new Color(255, 255, 255, 255), 1.3f);
					Dust.NewDust(player.position + new Vector2(20f + (40f * ((float)Math.Sin((dustCountInt * 3.0) + (3.14)))), 0f), 3, 3, DustID.Water, player.velocity.X * dustCountInt * 0.02f, -1f * dustCountInt, 255, new Color(255, 255, 255, 255), 1.3f);
					Dust.NewDust(player.position + new Vector2(-20f + (40f * ((float)Math.Sin(dustCountInt * 3.0))), 0f), 3, 3, DustID.DungeonWater, player.velocity.X * dustCountInt * 0.02f, -0.5f * dustCountInt, 255, new Color(255, 255, 255, 255), 1f);
					Dust.NewDust(player.position + new Vector2(20f + (40f * ((float)Math.Sin((dustCountInt * 3.0) + (3.14)))), 0f), 3, 3, DustID.DungeonWater, player.velocity.X * dustCountInt * 0.02f, -0.5f * dustCountInt, 255, new Color(255, 255, 255, 255), 1f);
				}
			}
		}

		public override int DisplayedCooldownID => ModContent.BuffType<RisingTideCooldown>();
	}

	public class RisingTideCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => typeof(SunkenSword).FullName.Replace('.', '/');
	}
}

