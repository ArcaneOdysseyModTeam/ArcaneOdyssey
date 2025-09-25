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
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class PoisonMagic : AOMagic
	{
		public override SoundStyle? ImbueSound => SoundID.Item17;
        public override Color ImbueColour => new(105,0,105,255);
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.11f;
		public override float AOImbueDamage => 0.825f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.15f;
		public override float AOScrollDamage => 0.75f;
		public override AODebuffRequirement ImbueDebuff => new(BuffID.Poisoned, 60*10);
		//public override AODebuff ImbueDebuff2 => new AODebuff(BuffID.Stinky, 60*10);
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.075f),
				new MagicBuffMultiplier(BuffID.OnFire,0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),0.9f)
			]
			);

		public override void SpawningEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Cloud, (projectile.velocity.X * 0.4f), (projectile.velocity.Y * 0.4f), 0, Color.Purple, 3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Projectile projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.Cloud, 0f, 0f, 0, Color.Purple, 2f)];
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Projectile projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Cloud, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, Color.Purple, 3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Cloud, (5f * Main.rand.NextFloat() - 0.5f), (5f * Main.rand.NextFloat() - 0.5f), 0, Color.Purple, 3f)];
				spawnedDust.noGravity = true;
				if (n/2 >= 10)
					Projectile.NewProjectile(projectile.GetSource_FromThis(), new(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), new(1.25f * Main.rand.NextFloat() - 0.5f, 1.25f * Main.rand.NextFloat() - 0.5f), Main.rand.Next([ProjectileID.SporeGas, ProjectileID.SporeGas2, ProjectileID.SporeGas3]), 2 + BossesKilled, 0f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}

		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<PoisonBlast>()),]);
	}
}