using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Ancient;
using ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Ancient;
using ArcaneOdyssey.Content.Projectiles.Magic.Pulsars.Ancient;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Ancient
{
	public class IonMagic : AOMagic
	{
		public override bool? Cold => false;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Ancient;
		public override SoundStyle? ImbueSound => SoundID.Item91;
		public override Color ImbueColour => new Color(0, 255, 0, 255);
		public override bool CanBeWet => false;
		public override float AOImbueSpeed => 1.5f;
		public override float AOImbueSize => 1.2f;
		public override float AOImbueDamage => 1.6f;
		public override float AOScrollSpeed => 1.5f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 1.6f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<IonizedEffect>(), 60 * 10)];
		public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override SynergyEffects Effects => new(
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
				new MagicBuffMultiplier(BuffID.Slimed,1.075f),
				new MagicBuffMultiplier(BuffID.Oiled,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.1f)
			]
			);
		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.CursedTorch, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, 0, default, 3f)];
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.CursedTorch, 0f, 0f, 0, default, 3f)];
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 1, 1, DustID.CursedTorch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 4f)];
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.CursedTorch, 5f * Main.rand.NextFloat() - 0.5f, 5f * Main.rand.NextFloat() - 0.5f, 0, default, 4f)];
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<IonBlast>()), KeyValuePair.Create(typeof(PulsarSpell), ModContent.ProjectileType<IonPulsar>()), KeyValuePair.Create(typeof(CannonSpell), ModContent.ProjectileType<IonCannon>())]);
	}
}