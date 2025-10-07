using ArcaneOdyssey.Content.Buffs;
using ArcaneOdyssey.Content.Items.Base;
using Steamworks;
using System.Linq.Expressions;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using System.Net.Mail;
using ArcaneOdyssey.Content.Items.Weapons.Bronze;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class SunkenSword : AORangedOrMeleeWeapon
    {
        public override bool? ColdWeapon => true;
        public override float AOSpeed => 1.2f;
		public override float AOSize => .9f;
		public override float AODamage => 1f;
		public override int AOValue => 900;
		public override AORarities AORarity => AORarities.Rare;
		public override AOWeaponTiers AOWeaponTier => AOWeaponTiers.Good;
		public override WeaponAbility Ability => new(Mod, "Rising Tide", "Launch yourself upwards", Color.Aqua);

		public override AODebuffRequirement? WeaponDebuff => new(BuffID.Wet, 60 * 5);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 50;
			Item.height = 54;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.SplashWeak;
			Item.DamageType = TrueMelee();
			Item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				var dash = new RisingTide();
				if (!dash.OnCooldown(player))
					player.ArcaneOdyssey().StartDash(dash, -2);
			}
			else if (!Main.dedServ) 
			{
				// Particles from swinging
				Dust.NewDust(player.MountedCenter+new Vector2(player.direction*3f,0f),3,3,DustID.Water,(player.direction*30f)*(0.8f-Main.rand.NextFloat()),30f*(0.5f-Main.rand.NextFloat()),255,default,1.3f);
			}   
			return null;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<RavennaSword>();
			recipe.AddIngredient<ArcaniumScrap>(2);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.Register();
		}
	}

	public class RisingTide : DashSystem
	{
		public override float DashSpeed => 20;
		public override int DashMax => 60;
		public override bool AnyDirection => false;
		public override bool Immune => false;
		public override int Cooldown => 60*5;
		public override bool OnHit(Player player, Entity target)
		{
			return false;
		}

		public override void DashEffect(Player player)
		{
			player.statDefense += 20;
			if (player.ArcaneOdyssey().DashLeft%5 == 0)
			{
				player.direction *= -1;
			}
		}

		public override void OnStart(Player player)
		{
			if (!Main.dedServ)
			{
				if (Main.LocalPlayer.whoAmI == player.whoAmI)
					player.velocity.Y *= 0.1f;
				SoundEngine.PlaySound(SoundID.Splash, player.position);
				// Adds dust
				for (int dustCountInt = 0; dustCountInt < 50; dustCountInt++)
				{
					Dust.NewDust(player.position + new Vector2(-20f + (40f * ((float)Math.Sin(dustCountInt * 3.0))), 0f), 3, 3, DustID.Water, player.velocity.X * dustCountInt * 0.02f, -1f * dustCountInt, 255, new Color(255, 255, 255, 255), 1.3f);
					Dust.NewDust(player.position + new Vector2(20f + (40f * ((float)Math.Sin((dustCountInt * 3.0) + (3.14)))), 0f), 3, 3, DustID.Water, player.velocity.X * dustCountInt * 0.02f, -1f * dustCountInt, 255, new Color(255, 255, 255, 255), 1.3f);
					Dust.NewDust(player.position + new Vector2(-20f + (40f * ((float)Math.Sin(dustCountInt * 3.0))), 0f), 3, 3, DustID.DungeonWater, player.velocity.X * dustCountInt * 0.02f, -0.5f * dustCountInt, 255, new Color(255, 255, 255, 255), 1f);
					Dust.NewDust(player.position + new Vector2(20f + (40f * ((float)Math.Sin((dustCountInt * 3.0) + (3.14)))), 0f), 3, 3, DustID.DungeonWater, player.velocity.X * dustCountInt * 0.02f, -0.5f * dustCountInt, 255, new Color(255, 255, 255, 255), 1f);
				}
				//Rising tide text
				CombatText.NewText(player.Hitbox, new Color(0, 105, 255, 255), Mod.CustomLocalization("PopupText.RisingTide").Value);
			}
		}
	}
}

