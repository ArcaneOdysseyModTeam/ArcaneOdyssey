using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
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
	public class WoodMagic : AOMagic
	{
		public override SoundStyle? MagicSound => SoundID.Dig;
        public override Color MagicColour => new Color(61,33,0,255);
		public override float AOImbueSpeed => 0.9f;
		public override float AOImbueSize => 1.162f;
		public override float AOImbueDamage => 1.025f;
		public override float AOMagicSpeed => 0.8f;
		public override float AOMagicSize => 1.2f;
		public override float AOMagicDamage => 0.95f;
		public override AODebuffRequirement MagicDebuff => new AODebuffRequirement(ModContent.BuffType<AOBleed>(), 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				new MagicBuffMultiplier(BuffID.OnFire,1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.1f)
			]
			);
		public override void SpawningEffects(Projectile projectile) 
		{
			for(int n = 0; n<3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*Main.rand.NextFloat()),projectile.position.Y+(projectile.height*Main.rand.NextFloat())),0,0,DustID.Pearlwood,(projectile.velocity.X*0.2f),(projectile.velocity.Y*0.2f),0,default,1.5f)];
			}
		}

		

		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Pearlwood, (8f * (Main.rand.NextFloat() - 0.5f)), (8f * (Main.rand.NextFloat() - 0.5f)), 0, default, 1.5f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(MagicSound, projectile.position, null);
		}
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<WoodBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}