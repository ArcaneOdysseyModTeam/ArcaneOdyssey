using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class PoisonLightningMagic : AOMagic
	{
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningAuraZap;
		public override Color ImbueColour => new Color(240,140,255,255);
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 1f;
        public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Poisoned, 60 * 10), new(ModContent.BuffType<AOParalyzed>(), 60, 33)];
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOParalyzed>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<AOPetrified>(), // petrified
				ModContent.BuffType<CharredEffect>(),
				ModContent.BuffType<SandyEffect>(),
				ModContent.BuffType<AOBleed>(),
				ModContent.BuffType<AOFrozen>()
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.075f),
				new MagicBuffMultiplier(BuffID.OnFire,0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),0.9f),
				new MagicBuffMultiplier(BuffID.Chilled, 1.2f), // frozen
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
				new MagicBuffMultiplier(BuffID.Burning, 1.15f), // scalding
				new MagicBuffMultiplier(BuffID.OnFire3, 1.075f), // melting/hellfire
				new MagicBuffMultiplier(BuffID.Venom, 1.075f), // venom acid
				new MagicBuffMultiplier(BuffID.Wet, 1.05f), // 
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.15f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);
		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Cloud, (projectile.velocity.X * 0.4f), (projectile.velocity.Y * 0.4f), 0, Color.Purple, 3f)];
				spawnedDust.noGravity = true;
				Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.WitherLightning, (projectile.velocity.X * 0.2f), (projectile.velocity.Y * 0.2f), 0, default, 1.2f);
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.Cloud, 0f, 0f, 0, Color.Purple, 2f)];
			spawnedDust.noGravity = true;
			if (projectile.velocity != Vector2.Zero)
			{
				float waveVal = 0f;
				if (projectile is Projectile) {
					if (((Projectile)projectile).type == ModContent.ProjectileType<BeamSpell>())
					{
						waveVal = 10f * MathF.Abs((((float)((Projectile)projectile).numUpdates) % 5 % 10f) - 2.5f) - 12.5f;
					}
					else
					{
						waveVal = 10f * MathF.Abs((((float)Main.GameUpdateCount) % 5 % 10f) - 2.5f) - 12.5f;
					}
				} else
                {
                    waveVal = 10f * MathF.Abs((((float)Main.GameUpdateCount) % 5 % 10f) - 2.5f) - 12.5f;
                }
				Vector2 baseVec = new(0f, waveVal);
				Dust spawnedDust2 = Dust.NewDustPerfect(projectile.position + (baseVec.RotatedBy(projectile.velocity.ToRotation())) + new Vector2(projectile.width / 2f, projectile.height / 2f), DustID.CrystalPulse, new Vector2(0f, 0f), 255, default, 1.2f);
				spawnedDust2.noGravity = true;
			}
			Lighting.AddLight(projectile.position,2,1,2);
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Cloud, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, Color.Purple, 3f)];
				spawnedDust.noGravity = true;
				Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.WitherLightning, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 1.2f);
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Cloud, (5f * Main.rand.NextFloat() - 0.5f), (5f * Main.rand.NextFloat() - 0.5f), 0, Color.Purple, 3f)];
				spawnedDust.noGravity = true;
				Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.WitherLightning, (8f * Main.rand.NextFloat() - 0.5f), (8f * Main.rand.NextFloat() - 0.5f), 0, default, 1.2f);
				if (n/2 >= 10)
					Projectile.NewProjectile(projectile.GetSource_FromThis(), new(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), new(1.25f * Main.rand.NextFloat() - 0.5f, 1.25f * Main.rand.NextFloat() - 0.5f), Main.rand.Next([ProjectileID.SporeGas, ProjectileID.SporeGas2, ProjectileID.SporeGas3]), 2 + BossesKilled, 0f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
        public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<PoisonLightningBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<PoisonLightningPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<PoisonLightningCannon>())]);
		
		public override void AddRecipes() {
            
        }
	}
}