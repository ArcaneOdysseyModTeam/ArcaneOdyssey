using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using ArcaneOdyssey.Content.Buffs.Stuns;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Projectiles;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class VesuviusMagic : AOMagic
	{
        public override Color MagicColour => new Color(0,0,255,0);
		public override float AOImbueSpeed => 0.9f;
		public override float AOImbueSize => 3f;
		public override float AOImbueDamage => 2f;
		public override float AOMagicSpeed => 0.9f;
		public override float AOMagicSize => 3f;
		public override float AOMagicDamage => 2f;
        public override AOMagicTier MagicTier => AOMagicTier.Custom;
        public override SoundStyle? MagicSound => SoundID.Item21;
        public override AODebuffRequirement MagicDebuff => new AODebuffRequirement(ModContent.BuffType<AOPetrified>(),10*60);
		public override AODebuffRequirement MagicDebuff2 => new AODebuffRequirement(BuffID.OnFire3,10*60);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				
			], 
			[
				
			]
			);
		public override Dictionary<Type, int> Spells => new Dictionary<Type, int>([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<VesuviusBlast>()),]);
		public override void SpawningEffects(Projectile projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.UltraBrightTorch, (projectile.velocity.X * 2f), (projectile.velocity.Y * 2f), 0, new Color(0,0,255,0), 2.5f)];
				spawnedDust.noGravity = true;
			}
		}
		
		public override void LingeringEffects(Projectile projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 1, 1, DustID.UltraBrightTorch, 0f, 0f, 0, new Color(0,0,255,0), 1.2f)];
			Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 1, 1, DustID.SolarFlare, 0f, 0f, 0, Color.Blue, 1.2f)];
			Lighting.AddLight(projectile.position, 1f, 0.19f, 0f);
		}

		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.UltraBrightTorch, (8f * (float)(Main.rand.NextDouble() - 0.5)), (8f * (float)(Main.rand.NextDouble() - 0.5)), 0, new Color(0, 0, 255, 0), 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(MagicSound, projectile.position, null);
		}
		public override void AddRecipes() {
            
        }
	}
}
