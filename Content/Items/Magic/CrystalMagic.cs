using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Projectiles;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using Terraria.Audio;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class CrystalMagic : AOMagic
	{
		public override Color MagicColour => new Color(255,0,0,0);
		public override float AOImbueSpeed => 0.95f;
		public override float AOImbueSize => 1.11f;
		public override float AOImbueDamage => 1.025f;
		public override float AOMagicSpeed => 0.9f;
		public override float AOMagicSize => 1.15f;
		public override float AOMagicDamage => 1.05f;
		public override SoundStyle? MagicSound => SoundID.Shatter;
		public override AODebuff MagicDebuff => new AODebuff(ModContent.BuffType<CrystalStackI>(), 60*10);
		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<CrystalStackIII>(),ModContent.BuffType<CrystalStackIIII>()),new(ModContent.BuffType<CrystalStackII>(),ModContent.BuffType<CrystalStackMid>()),new(ModContent.BuffType<CrystalStackI>(),ModContent.BuffType<CrystalStackII>())];
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIII>(),1.3f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.01f),
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.01f),
				new MagicBuffMultiplier(BuffID.Venom,1.01f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.125f)
			]
			);
		public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<CrystalBlast>()),]);
		public override void SpawningEffects(Projectile projectile)
		{ 
			for(int n = 0;n<10;n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*(float)Main.rand.NextDouble()),projectile.position.Y+(projectile.height*(float)Main.rand.NextDouble())),0,0,DustID.GemRuby,(projectile.velocity.X*0.4f),(projectile.velocity.Y*0.4f),0,default,1f)];
			}
		}

		public override void LingeringEffects(Projectile projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 1, 1, DustID.SilverFlame, 0f, 0f, 0, default, 1f)];
			spawnedDust.noGravity = true;
		}
			
		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.GemRuby, (2f * (float)(Main.rand.NextDouble() - 0.5)), (2f * (float)(Main.rand.NextDouble() - 0.5)), 0, default, 1f)];
			}
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<HecateOrb>(1);
			recipe.Register();
		}
	}
}