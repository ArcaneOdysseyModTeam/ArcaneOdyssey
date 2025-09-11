using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
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
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class PoisonMagic : AOMagic
	{
		public override SoundStyle? MagicSound => SoundID.Item17;
        public override Color MagicColour => new Color(105,0,105,255);
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.11f;
		public override float AOImbueDamage => 0.825f;
		public override float AOMagicSpeed => 1f;
		public override float AOMagicSize => 1.15f;
		public override float AOMagicDamage => 0.75f;
		public override AODebuffRequirement MagicDebuff => new AODebuffRequirement(BuffID.Poisoned, 60*10);
		//public override AODebuff MagicDebuff2 => new AODebuff(BuffID.Stinky, 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.075f),
				new MagicBuffMultiplier(BuffID.OnFire,0.99f)
			]
			);
			public override void SpawningEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.Cloud, (projectile.velocity.X * 0.4f), (projectile.velocity.Y * 0.4f), 0, Color.Purple, 3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Projectile projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 1, 1, DustID.Cloud, 0f, 0f, 0, Color.Purple, 2f)];
			spawnedDust.noGravity = true;
		}

		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.Cloud, (5f * (float)(Main.rand.NextDouble() - 0.5)), (5f * (float)(Main.rand.NextDouble() - 0.5)), 0, Color.Purple, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(MagicSound, projectile.position, null);
		}
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<PoisonBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}