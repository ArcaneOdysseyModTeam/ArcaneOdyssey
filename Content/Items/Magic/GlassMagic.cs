using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class GlassMagic : AOMagic
	{
		public override SoundStyle? MagicSound => SoundID.Shatter;
        public override Color MagicColour => new Color(255,255,255,0);
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.053f;
		public override float AOImbueDamage => 1f;
		public override float AOMagicSpeed => 1f;
		public override float AOMagicSize => 1.1f;
		public override float AOMagicDamage => 0.9f;
		public override AODebuffRequirement MagicDebuff => new AODebuffRequirement(ModContent.BuffType<AOBleed>(), 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.92f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f)
			]
			);
			public override void SpawningEffects(Projectile projectile)
		{ 
			for(int n = 0;n<10;n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*(float)Main.rand.NextDouble()),projectile.position.Y+(projectile.height*(float)Main.rand.NextDouble())),0,0,DustID.Glass,(projectile.velocity.X*0.4f),(projectile.velocity.Y*0.4f),0,default,1f)];
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
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.Glass, (2f * (float)(Main.rand.NextDouble() - 0.5)), (2f * (float)(Main.rand.NextDouble() - 0.5)), 0, default, 1f)];
			}
			SoundEngine.PlaySound(MagicSound, projectile.position, null);
		}
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<GlassBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}