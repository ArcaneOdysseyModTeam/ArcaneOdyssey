using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.Helpers;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class AncientLightningMagic : AOMagic
	{
		public override float DashSpeed => 1.5f; // instant
		public override SoundStyle? ImbueSound => SoundID.DD2_LightningAuraZap;
		public override Color ImbueColour => new(255, 0, 0, 255);
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float AOImbueSpeed => 1.4f;
		public override float AOImbueSize => 1.1f;
		public override float AOImbueDamage => 1.3f;
		public override float AOScrollSpeed => 1.4f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollDamage => 1.3f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOParalyzed>(), 60, 16), new(ModContent.BuffType<AncientLightingChain>(), 60)];
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
				new(BuffID.Chilled, 1.2f), // frozen
				new(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
				new(BuffID.Burning, 1.15f), // scalding
				new(BuffID.OnFire3, 1.075f), // melting/hellfire
				new(BuffID.Venom, 1.075f), // venom acid
				new(BuffID.Wet, 1.05f), // 
				new(BuffID.Oiled,0.96f),
				new(BuffID.ShadowFlame,1.15f),
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);

		public override void SpawningEffects(Entity projectile)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CrimsonTorch, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 0, default, 1.2f);
			}
		}

		public override void LingeringEffects(Entity projectile)
		{// WAHT IS  THIS IM SO CONFUSED
			if (Main.dedServ)
				return;
			if (projectile.velocity != Vector2.Zero)
			{
				float waveVal = 10f * MathF.Abs((float)Main.GameUpdateCount % 5 % 10f - 2.5f) - 12.5f;
				if (projectile is Projectile proj && proj.extraUpdates > 0)
				{
					waveVal = 10f * MathF.Abs(((float)Main.GameUpdateCount + (float)proj.numUpdates) % 5 % 10f - 2.5f) - 12.5f;
				}
				Vector2 baseVec = new(0f, waveVal);
				Dust spawnedDust = Dust.NewDustPerfect(projectile.position + baseVec.RotatedBy(projectile.velocity.ToRotation()) + new Vector2(projectile.width / 2f, projectile.height / 2f), DustID.TheDestroyer, new Vector2(0f, 0f), 255, Color.Red, 1.2f);
				spawnedDust.noGravity = true;
			}
			Lighting.AddLight(projectile.position, 2, 0, 0);
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CrimsonTorch, 0f, 0f, 0, default, .7f);
		}

		public override void ExplosionEffects(Entity projectile)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 3; n++)
			{
				Dust dust = Dust.NewDustDirect(projectile.Center, 0, 0, DustID.Firework_Red, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, Color.Red, 2.3f);
				dust.noGravity = true;
			}
		}

		public override void KillEffects(Entity projectile)
		{
			if (Main.dedServ)
				return;
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.CrimsonTorch, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 2.5f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(LightningMagic));
		}
	}
}
