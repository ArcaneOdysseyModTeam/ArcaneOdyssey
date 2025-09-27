using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using ArcaneOdyssey.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class AshMagic : AOMagic
    {
        public override bool? Cold => false;
        public override bool CanBeWet => false;
        public override Color ImbueColour =>  new(235,40,0,0);
        public override float AOImbueSpeed => 0.975f;
		public override float AOImbueSize => 1.22f;
		public override float AOImbueDamage => 0.95f;
		public override float AOScrollSpeed => 0.95f;
		public override float AOScrollSize => 1.25f;
		public override float AOScrollDamage => 0.875f;
        public override SoundStyle? ImbueSound => SoundID.Dig;
		public override AODebuffRequirement ImbueDebuff => new(ModContent.BuffType<AOPetrified>(), 60*10,33);
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.OnFire3, ModContent.BuffType<AOPetrified>()),new(BuffID.OnFire, ModContent.BuffType<AOPetrified>()),new(BuffID.ShadowFlame, ModContent.BuffType<AOPetrified>()),new(ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>()),new(ModContent.BuffType<AOScalding>(), ModContent.BuffType<AOPetrified>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				BuffID.Wet,
				ModContent.BuffType<SnowyEffect>(),
				ModContent.BuffType<FreezingEffect>(),
				BuffID.OnFire,
				BuffID.OnFire3,
				ModContent.BuffType<CharredEffect>(),
				BuffID.ShadowFlame,
				ModContent.BuffType<AOScalding>()
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire,1.02f),
				new MagicBuffMultiplier(BuffID.Venom,1.075f),
				new MagicBuffMultiplier(BuffID.Slimed,1.075f),
new MagicBuffMultiplier(BuffID.Oiled,1.075f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.15f),
				new MagicBuffMultiplier(BuffID.Wet,0.995f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.01f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.125f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),1.2f)
			]
			);

		public override void SpawningEffects(Projectile projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Ash, (projectile.velocity.X * 2f), (projectile.velocity.Y * 2f), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.RedTorch, (projectile.velocity.X * 2f), (projectile.velocity.Y * 2f), 0, default, 2f)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Projectile projectile)
		{
			_ = Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.RedTorch, 0f, 0f, 0, default, 1f);
			Dust spawnedDust = Dust.NewDustDirect(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.Ash, 0f, 0f, 0, default, 2f);
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Projectile projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.RedTorch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 1f)];
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 2f)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Dust.NewDustDirect(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Ash, (8f * Main.rand.NextFloat() - 0.5f), (8f * Main.rand.NextFloat() - 0.5f), 0, default, 3f);
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Dust.NewDustDirect(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.RedTorch, (8f * Main.rand.NextFloat() - 0.5f), (8f * Main.rand.NextFloat() - 0.5f), 0, default, 2f);
				spawnedDust2.noGravity = true;
			}
			for (int n = 0; n < 10; n++)
			{
				Projectile.NewProjectile(projectile.GetSource_FromThis(), new(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), new(1.23f * Main.rand.NextFloat() - 0.5f, 1.23f * Main.rand.NextFloat() - 0.5f), ProjectileID.SporeCloud, 2 + BossesKilled, 0f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);

		}

        public override bool PreEffects(Projectile projectile)
        {
            return base.PreEffects(projectile) && projectile.type != ProjectileID.SporeCloud;
        }

		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<AshBlast>()),]);
	}
}