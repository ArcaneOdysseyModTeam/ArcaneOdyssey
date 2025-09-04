using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class MagmaMagic : AOMagic
    {
        public override bool CanBeWet => false;
		public override Color MagicColour => new Color(255,50,0,0);
        public override float AOImbueSpeed => 0.85f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 0.975f;
		public override float AOMagicSpeed => 0.7f;
		public override float AOMagicSize => 1.2f;
		public override float AOMagicDamage => 0.9f;
		public override AODebuff MagicDebuff => new AODebuff(BuffID.OnFire3, 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				BuffID.Chilled, // freezing
				ModContent.BuffType<AOPetrified>(),
				BuffID.Wet,
				ModContent.BuffType<AOBleed>(),
				BuffID.Venom,
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SandyEffect>(),
				ModContent.BuffType<SnowyEffect>()
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOPetrified>(), 1.2f), // petrified
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(), 1.15f), // bleeding
				new MagicBuffMultiplier(BuffID.OnFire, 1.075f),
				new MagicBuffMultiplier(BuffID.Venom, 1.1f), // venom acid
				new MagicBuffMultiplier(BuffID.Burning, 1.075f),
				new MagicBuffMultiplier(BuffID.Poisoned, 1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(), .95f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(), .99f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(), 1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(), 0.99f),
				new MagicBuffMultiplier(BuffID.Wet, .95f),
				new MagicBuffMultiplier(BuffID.ShadowFlame, 1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackI>(),0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackII>(),0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIII>(),0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackMid>(),0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIIII>(),0.95f)
			]
			);
			
			public override void SpawningDust(Projectile projectile)
		{
			if (projectile.ModProjectile is not MagicCircle)
			{
				CreateMagicCircle(projectile);
				for (int n = 0; n < 3; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.InfernoFork, (projectile.velocity.X * 2f), (projectile.velocity.Y * 2f), 0, default, 2.5f)];
					spawnedDust.noGravity = true;
				}
			}
		}
			public override void LingeringDust(Projectile projectile) {
			if (projectile.ModProjectile is not MagicCircle)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 1, 1, DustID.InfernoFork, 0f, 0f, 0, default, 1.2f)];
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*(float)Main.rand.NextDouble()),projectile.position.Y+(projectile.height*(float)Main.rand.NextDouble())),1,1,DustID.SolarFlare,0f,0f,0,default,1.2f)];
				Lighting.AddLight(projectile.position,2f,0.38f,0f);
			}
				
			}
			public override void KillDust(Projectile projectile) {
				if(projectile.ModProjectile is not MagicCircle) {
					for(int n = 0;n<10;n++){
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*(float)Main.rand.NextDouble()),projectile.position.Y+(projectile.height*(float)Main.rand.NextDouble())),0,0,DustID.InfernoFork,(8f*(float)(Main.rand.NextDouble()-0.5)),(8f*(float)(Main.rand.NextDouble()-0.5)),0,default,3f)];
					spawnedDust.noGravity = true;
				}
				}
			}
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<MagmaBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}
