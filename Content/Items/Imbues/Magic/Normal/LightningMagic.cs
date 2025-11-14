using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Normal;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Normal;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Normal;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class LightningMagic : AOMagic
    {
        public override float DashSpeed => 1.5f; // instant
        public override SoundStyle? ImbueSound => SoundID.DD2_LightningAuraZap;
        public override Color ImbueColour => new(255, 140, 255, 255);
		public override float AOImbueSpeed => 1.2f;
		public override float AOImbueSize => .95f;
		public override float AOImbueDamage => .95f;
		public override float AOScrollSpeed => 1.4f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => .875f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOParalyzed>(), 60, 33)];
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
				new MagicBuffMultiplier(BuffID.Chilled, 1.2f), // frozen
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
				new MagicBuffMultiplier(BuffID.Burning, 1.15f), // scalding
				new MagicBuffMultiplier(BuffID.OnFire3, 1.075f), // melting/hellfire
				new MagicBuffMultiplier(BuffID.Venom, 1.075f), // venom acid
				new MagicBuffMultiplier(BuffID.Wet, 1.05f), // 
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.15f),
				new MagicBuffMultiplier(BuffID.Oiled,0.96f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);

		public override void SpawningEffects(Entity projectile) 
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.WitherLightning, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 0, default, 1.2f);
			}
		}

		public override void LingeringEffects(Entity projectile)
		{// WAHT IS  THIS IM SO CONFUSED
			if (projectile.velocity != Vector2.Zero)
			{
				float waveVal = 10f * MathF.Abs((float)Main.GameUpdateCount % 5 % 10f - 2.5f) - 12.5f; ;
				if (projectile is Projectile proj && proj.extraUpdates > 0)
				{
					waveVal = 10f * MathF.Abs(((float)Main.GameUpdateCount + (float)proj.numUpdates) % 5 % 10f - 2.5f) - 12.5f;
				}
				Vector2 baseVec = new(0f, waveVal);
				Dust spawnedDust = Dust.NewDustPerfect(projectile.position + baseVec.RotatedBy(projectile.velocity.ToRotation()) + new Vector2(projectile.width / 2f, projectile.height / 2f), DustID.CrystalPulse, new Vector2(0f, 0f), 255, default, 1.2f);
				spawnedDust.noGravity = true;
			}
			Lighting.AddLight(projectile.position,2,1,2);
			Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.WitherLightning, 0f, 0f, 0, default, 0.3f);
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(projectile.Center, 1, 1, DustID.WitherLightning, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 1.2f);
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.WitherLightning, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 1.2f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		
		public override List<Type> Skills => [typeof(LightningBlast), typeof(LightningPulsar), typeof(LightningCannon)];
	}
}
