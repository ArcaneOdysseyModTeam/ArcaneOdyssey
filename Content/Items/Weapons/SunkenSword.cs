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

		public override AODebuffRequirement WeaponDebuff => new(BuffID.Wet, 60 * 5);

		public override void SetDefaultsWeapon()
		{
			Item.width = 50;
			Item.height = 54;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.SplashWeak;
			Item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return !player.HasBuff<RisenTide>();
		}

		public override bool? UseItem(Player player)
		{
			if (player.altFunctionUse == 2 && !player.HasBuff<RisenTide>())
			{
				player.AddBuff(ModContent.BuffType<RisingTide>(), 60);
				player.AddBuff(ModContent.BuffType<RisenTide>(), (int)(60 * (player.ArcaneOdyssey().imbue is not null ? player.ArcaneOdyssey().imbue.AOImbueSpeed.FlipFloat() : 1) * AOSpeed * 5));
				SoundEngine.PlaySound(SoundID.Splash, player.position);
				player.velocity.Y *= 0.1f;
				player.velocity.Y -= 20;
				// Adds dust
				for(int dustCountInt = 0;dustCountInt<50;dustCountInt++)
				{
					Dust.NewDust(player.position+new Vector2(-20f+(40f*((float)Math.Sin(dustCountInt*3.0))),0f),3,3,DustID.Water,player.velocity.X*dustCountInt*0.02f,-1f * dustCountInt,255,new Color(255,255,255,255),1.3f);
					Dust.NewDust(player.position+new Vector2(20f+(40f*((float)Math.Sin((dustCountInt*3.0)+(3.14)))),0f),3,3,DustID.Water,player.velocity.X*dustCountInt*0.02f,-1f * dustCountInt,255,new Color(255,255,255,255),1.3f);
					Dust.NewDust(player.position+new Vector2(-20f+(40f*((float)Math.Sin(dustCountInt*3.0))),0f),3,3,DustID.DungeonWater,player.velocity.X*dustCountInt*0.02f,-0.5f * dustCountInt,255,new Color(255,255,255,255),1f);
					Dust.NewDust(player.position+new Vector2(20f+(40f*((float)Math.Sin((dustCountInt*3.0)+(3.14)))),0f),3,3,DustID.DungeonWater,player.velocity.X*dustCountInt*0.02f,-0.5f * dustCountInt,255,new Color(255,255,255,255),1f);
				}
				//Rising tide text
				CombatText.NewText(player.Hitbox, new Color(0,105,255,255), Mod.CustomLocalization("PopupText.RisingTide").Value);
			} 
			else if (!Main.dedServ) 
			{
				// Particles from swinging
				Dust.NewDust(player.position+new Vector2(player.direction*3f,0f),3,3,DustID.Water,(player.direction*30f)*(0.8f-Main.rand.NextFloat()),30f*(0.5f-Main.rand.NextFloat()),255,default,1.3f);
			}   
			return null;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.DD2SquireBetsySword);
			recipe.AddIngredient<ArcaniumScrap>(2);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}

