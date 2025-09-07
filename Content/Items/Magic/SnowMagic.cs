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
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class SnowMagic : AOMagic
    {
		public override SoundStyle? MagicSound => SoundID.Dig;
        public override Color MagicColour => new Color(255,255,255,255);
        public override bool CanBeWet => false;
        public override float AOImbueSpeed => 1.05f;
		public override float AOImbueSize => 1.11f;
		public override float AOImbueDamage => 1f;
		public override float AOMagicSpeed => 1.1f;
		public override float AOMagicSize => 1.15f;
		public override float AOMagicDamage => 0.925f;
		public override AODebuff MagicDebuff => new AODebuff(ModContent.BuffType<SnowyEffect>(), 60*10);
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOFrozen>()),new(ModContent.BuffType<FreezingEffect>(),ModContent.BuffType<AOFrozen>())];
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				BuffID.OnFire,
				ModContent.BuffType<CharredEffect>(),
				BuffID.Venom,
				BuffID.Wet,
				ModContent.BuffType<FreezingEffect>(),
				BuffID.OnFire3,
				BuffID.ShadowFlame
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackI>(),0.8f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackII>(),0.8f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIII>(),0.8f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackMid>(),0.8f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIIII>(),0.8f),
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.05f),
				new MagicBuffMultiplier(BuffID.OnFire,0.90f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),0.8f),
				new MagicBuffMultiplier(BuffID.Venom,0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire3,0.9f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,0.8f),
				new MagicBuffMultiplier(BuffID.Wet,1.1f)
			]
			);
		public override void SpawningEffects(Projectile projectile) 
		{
			for (int n = 0; n<3; n++)
			{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*(float)Main.rand.NextDouble()),projectile.position.Y+(projectile.height*(float)Main.rand.NextDouble())),0,0,DustID.Snow,(projectile.velocity.X*2f),(projectile.velocity.Y*2f),0,default,3f)];
					spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Projectile projectile)
		{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 1, 1, DustID.Snow, 0f, 0f, 0, default, 1f)];
		}

		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.SnowBlock, (8f * (float)(Main.rand.NextDouble() - 0.5)), (8f * (float)(Main.rand.NextDouble() - 0.5)), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(MagicSound, projectile.position, null);
		}
				public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<SnowBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
	}
}