using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
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
	public class PlasmaMagic : AOMagic
	{
		public override bool? Cold => false;
		public override SoundStyle? ImbueSound => SoundID.Item91;
		public override Color ImbueColour => new Color(255, 100, 255, 255);
		public override bool CanBeWet => false;
		public override float AOImbueSpeed => 0.9f;
		public override float AOImbueSize => 0.948f;
		public override float AOImbueDamage => 0.9f;
		public override float AOScrollSpeed => 1.25f;
		public override float AOScrollSize => 1f;
		public override float AOScrollDamage => 0.825f;
		public override AODebuffRequirement ImbueDebuff => new AODebuffRequirement(BuffID.ShadowFlame, 60 * 10);
		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override SynergyEffects Effects => new SynergyEffects(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<AOBleed>(),
				ModContent.BuffType<CharredEffect>(),
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.15f),
				new MagicBuffMultiplier(BuffID.OnFire,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),0.97f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f),
				new MagicBuffMultiplier(BuffID.Poisoned,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(),0.99f),
				new MagicBuffMultiplier(BuffID.Wet,0.95f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),1.075f)
			]
			);
		public override void SpawningEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.PinkTorch, (projectile.velocity.X * 0.4f), (projectile.velocity.Y * 0.4f), 0, default, 1f)];
			}
		}

		public override void LingeringEffects(Projectile projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.PinkTorch, 0f, 0f, 0, default, 2f)];
			spawnedDust.noGravity = true;
			Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.ShadowbeamStaff, 0f, 0f, 0, default, 2f)];
			spawnedDust2.noGravity = true;
		}
		public override void ExplosionEffects(Projectile projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Firework_Pink, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
			}
		}
		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.ShadowbeamStaff, (5f * Main.rand.NextFloat() - 0.5f), (5f * Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<PlasmaBlast>()),]);
	}
}