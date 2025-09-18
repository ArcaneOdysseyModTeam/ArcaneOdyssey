using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcaneOdyssey.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class ExplosionMagic : AOMagic
	{
        public override Color MagicColour => new Color(235,146,52,0);
		public override float AOImbueSpeed => 0.925f;
        public override bool CanBeWet => false;
        public override float AOImbueSize => 1.3f;
		public override float AOImbueDamage => 1f;
		public override float AOMagicSpeed => 0.85f;
		public override float AOMagicSize => 1.3f;
		public override float AOMagicDamage => 0.925f;
        public override SoundStyle? MagicSound => SoundID.Item14;
		public override AODebuffRequirement MagicDebuff => new AODebuffRequirement(ModContent.BuffType<CharredEffect>(), 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.01f),
				new MagicBuffMultiplier(BuffID.OnFire,1.125f),
				new MagicBuffMultiplier(BuffID.Venom,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.01f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(),0.99f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.15f),
				new MagicBuffMultiplier(BuffID.Wet,0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),0.99f)
			]
			);
		public override void SpawningEffects(Projectile projectile) 
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Pixie, (projectile.velocity.X * 2f), (projectile.velocity.Y * 2f), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Pixie, (projectile.velocity.X * 2f), (projectile.velocity.Y * 2f), 0, default, 3f)];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Ash, (projectile.velocity.X * 2f), (projectile.velocity.Y * 2f), 0, default, 4f)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void LingeringEffects(Projectile projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.Pixie, 0f, 0f, 0, default, 1.6f)];
			spawnedDust.noGravity = true;
			Dust spawnedDust3 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.Pixie, 0f, 0f, 0, default, 1.6f)];
			spawnedDust3.noGravity = true;
			Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.Ash, 0f, 0f, 0, default, 2f)];
			spawnedDust2.noGravity = true;
		}
		public override void ExplosionEffects(Projectile projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOMagicSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOMagicSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOMagicSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOMagicSize), 0, default, 3f)];
				spawnedDust2.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (15f * AOMagicSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOMagicSize), 0, default, 4f)];
				spawnedDust3.noGravity = true;
			}
		}
		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Pixie, (8f * Main.rand.NextFloat() - 0.5f), (8f * Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Pixie, (8f * Main.rand.NextFloat() - 0.5f), (8f * Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Ash, (8f * Main.rand.NextFloat() - 0.5f), (8f * Main.rand.NextFloat() - 0.5f), 0, default, 4f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(MagicSound, projectile.position, null);
		}
		public override Dictionary<Type, int> Spells => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<ExplosionBlast>()),]);
	}
}