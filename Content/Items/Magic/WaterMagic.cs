using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class WaterMagic : AOMagic
	{
		public override Color MagicColour => new Color(0,30,255,0);
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.22f;
		public override float AOImbueDamage => 0.975f;
		public override float AOMagicSpeed => 1f;
		public override float AOMagicSize => 1.25f;
		public override float AOMagicDamage => 0.9f;
		public override SoundStyle? MagicSound => SoundID.Splash;
		public override AODebuffRequirement MagicDebuff => new AODebuffRequirement(BuffID.Wet, 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				BuffID.OnFire,
				ModContent.BuffType<CharredEffect>(),
				BuffID.Venom,
				BuffID.OnFire3,
				BuffID.ShadowFlame
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.85f),
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.05f),
				new MagicBuffMultiplier(BuffID.OnFire,0.8f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),0.9f),
				new MagicBuffMultiplier(BuffID.Venom,0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.075f),
				new MagicBuffMultiplier(BuffID.OnFire3,0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),0.8f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,0.7f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(),1.1f)
			]
		);

		public override void SpawningEffects(Projectile projectile) 
		{
			for(int n = 0; n<3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*Main.rand.NextFloat()),projectile.position.Y+(projectile.height*Main.rand.NextFloat())),0,0,DustID.Water,(projectile.velocity.X*2f),(projectile.velocity.Y*2f),0,default,3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Projectile projectile) 
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*Main.rand.NextFloat()),projectile.position.Y+(projectile.height*Main.rand.NextFloat())),1,1,DustID.Water,0f,0f,0,default,1.2f)];
		}

		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Water, (8f * (Main.rand.NextFloat() - 0.5f)), (8f * (Main.rand.NextFloat() - 0.5f)), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(MagicSound, projectile.position, null);
		}
		public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<WaterBlast>()),]);
		
		public override void AddRecipes() 
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<HecateOrb>(1);
			recipe.Register();
		}
	}
}