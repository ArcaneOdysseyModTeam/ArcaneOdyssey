using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Lost;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Terraria.ID;
using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using static ArcaneOdyssey.AOUtils;
using Terraria;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class OilMagic : AOMagic
	{
		public override bool CanBeWet => false;
		public override Color ImbueColour => new(20,20,20);
        public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 1f;
        public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
        public override SoundStyle? ImbueSound => SoundID.Splash;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Oiled, 60*10)];
		public override SynergyEffects Effects => new SynergyEffects(
			[ // these are debuffs cleared on hit
				
			],
			[
				new(BuffID.OnFire,1.15f),
				new(BuffID.OnFire3,1.15f),
				new(BuffID.ShadowFlame,1.15f),
				new(ModContent.BuffType<AOBleed>(),1.1f),
				new(ModContent.BuffType<HeavyBleed>(),1.1f),
				new(ModContent.BuffType<SandyEffect>(),0.96f),
				new(ModContent.BuffType<SnowyEffect>(),0.96f),
				new(ModContent.BuffType<CharredEffect>(),1.05f),
				new(ModContent.BuffType<SearedEffect>(),1.1f)
			]
			);
		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<OilBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<OilPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<OilCannon>())]);
		public override void SpawningEffects(Entity projectile) 
		{
            for (int n = 0; n < 3; n++)

            {
                Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Water_Corruption, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, Color.Black, 3f)];
                spawnedDust.noGravity = true;
            }
		}

		public override void LingeringEffects(Entity projectile) 
		{
            Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.Water_Corruption, 0f, 0f, 0, Color.Black, 1.2f);
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 1, 1, DustID.Water_Corruption, (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), 0, Color.Black, 3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Water_Corruption, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, Color.Black, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		public override void AddRecipes() {
			CreateLostRecipe(typeof(WaterMagic),typeof(EarthMagic),typeof(WoodMagic));
        }
	}
}