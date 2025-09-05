using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using ArcaneOdyssey.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class AshMagic : AOMagic
    {
        public override bool CanBeWet => false;
        public override Color MagicColour =>  new Color(235,40,0,0);
        public override float AOImbueSpeed => 0.975f;
		public override float AOImbueSize => 1.22f;
		public override float AOImbueDamage => 0.95f;
		public override float AOMagicSpeed => 0.95f;
		public override float AOMagicSize => 1.25f;
		public override float AOMagicDamage => 0.875f;
		public override AODebuff MagicDebuff => new AODebuff(ModContent.BuffType<AOPetrified>(), 60*10,33);
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.OnFire3, ModContent.BuffType<AOPetrified>()),new(BuffID.OnFire, ModContent.BuffType<AOPetrified>()),new(BuffID.ShadowFlame, ModContent.BuffType<AOPetrified>()),new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				BuffID.Wet,
				ModContent.BuffType<SnowyEffect>(),
				ModContent.BuffType<FreezingEffect>(),
				BuffID.OnFire,
				BuffID.OnFire3,
				ModContent.BuffType<CharredEffect>(),
				BuffID.ShadowFlame
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire,1.02f),
				new MagicBuffMultiplier(BuffID.Venom,1.075f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.15f),
				new MagicBuffMultiplier(BuffID.Wet,0.995f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.01f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.125f)
			]
			);
		public override void SpawningDust(Projectile projectile)
		{
			if (projectile.ModProjectile is not MagicCircle)
			{
				CreateMagicCircle(projectile);
				for (int n = 0; n < 3; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.Ash, (projectile.velocity.X * 2f), (projectile.velocity.Y * 2f), 0, default, 3f)];
					spawnedDust.noGravity = true;
					Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.RedTorch, (projectile.velocity.X * 2f), (projectile.velocity.Y * 2f), 0, default, 2f)];
					spawnedDust2.noGravity = true;
				}
			}
		}
			public override void LingeringDust(Projectile projectile) {
			if (projectile.ModProjectile is not MagicCircle)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 1, 1, DustID.RedTorch, 0f, 0f, 0, default, 1f)];
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 1, 1, DustID.Ash, 0f, 0f, 0, default, 2f)];
				spawnedDust2.noGravity = true;
			}
			}
			public override void KillDust(Projectile projectile) {
			if (projectile.ModProjectile is not MagicCircle)
			{
				for (int n = 0; n < 10; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.Ash, (8f * (float)(Main.rand.NextDouble() - 0.5)), (8f * (float)(Main.rand.NextDouble() - 0.5)), 0, default, 3f)];
					spawnedDust.noGravity = true;
					Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.RedTorch, (8f * (float)(Main.rand.NextDouble() - 0.5)), (8f * (float)(Main.rand.NextDouble() - 0.5)), 0, default, 2f)];
					spawnedDust2.noGravity = true;
				}
				SoundEngine.PlaySound(SoundID.Dig,projectile.position,null);
				}
			}
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<AshBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}