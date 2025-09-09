using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
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
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Magic
{
    public class LightningMagic : AOMagic
    {
        public override SoundStyle? MagicSound => SoundID.DD2_LightningAuraZap;
        public override Color MagicColour => new Color(255,140,255,255);
        public override float AOImbueSpeed => 1.2f;
        public override float AOImbueSize => .95f;
        public override float AOImbueDamage => .95f;
        public override float AOMagicSpeed => 1.4f;
		public override float AOMagicSize => 1f;
		public override float AOMagicDamage => .875f;
        public override AODebuff MagicDebuff => new AODebuff(ModContent.BuffType<AOParalyzed>(), 60, 33);
        public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOParalyzed>())];
        public override MagicEffects Effects => new MagicEffects(
            [ // these are debuffs cleared on hit
                ModContent.BuffType<AOPetrified>(), // petrified
                ModContent.BuffType<CharredEffect>(),
                ModContent.BuffType<SandyEffect>(),
                ModContent.BuffType<AOBleed>(),
                ModContent.BuffType<AOFrozen>()
            ], 
            [
                new MagicBuffMultiplier(BuffID.Chilled, 1.2f), // frozen
                new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
                new MagicBuffMultiplier(BuffID.Burning, 1.15f), // scalding
                new MagicBuffMultiplier(BuffID.OnFire3, 1.075f), // melting/hellfire
                new MagicBuffMultiplier(BuffID.Venom, 1.075f), // venom acid
                new MagicBuffMultiplier(BuffID.Wet, 1.05f), // (add stunning later!)
                new MagicBuffMultiplier(BuffID.ShadowFlame,1.15f),
                new MagicBuffMultiplier(ModContent.BuffType<CrystalStackI>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackII>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIII>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackMid>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<CrystalStackIIII>(),1.075f)
            ]
            );

        public override void SpawningEffects(Projectile projectile) 
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.WitherLightning, (projectile.velocity.X * 0.2f), (projectile.velocity.Y * 0.2f), 0, default, 1.2f)];
			}
		}

		public override void LingeringEffects(Projectile projectile)
		{
            Dust spawnedDust = Dust.NewDustPerfect(projectile.position+(new Vector2(Vector2.Normalize(projectile.velocity).Y,(Vector2.Normalize(projectile.velocity).X*-1)*(((float)Main.time%2))*projectile.height*8f)+new Vector2(projectile.width/2f,projectile.height/2f)), DustID.CrystalPulse, null, 255, default, 1.2f);
            //Dust spawnedDust = Dust.NewDustPerfect(projectile.Center * (float)(Main.time%2f), DustID.CrystalPulse, null, 255, default, 1.2f);
            spawnedDust.noGravity = true;
            Lighting.AddLight(projectile.position,2,1,2);
			_ = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 1, 1, DustID.WitherLightning, 0f, 0f, 0, default, 0.3f)];
		}

		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.WitherLightning, (8f * (float)(Main.rand.NextDouble() - 0.5)), (8f * (float)(Main.rand.NextDouble() - 0.5)), 0, default, 1.2f)];
			}
			SoundEngine.PlaySound(MagicSound, projectile.position, null);
		}
            public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<LightningBlast>()),]);
		
		public override void AddRecipes() {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient<HecateOrb>(1);
            recipe.Register();
        }
    }
}
