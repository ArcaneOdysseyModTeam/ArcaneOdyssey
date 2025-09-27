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
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class FireMagic : AOMagic
    {
        public override bool? Cold => false;
        public override SoundStyle? ImbueSound => SoundID.Item20;
        public override Color ImbueColour => new Color(252,107,3,0);
        public override bool CanBeWet => false;
        public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.11f;
		public override float AOImbueDamage => 0.925f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.15f;
		public override float AOScrollDamage => 0.85f;
		public override AODebuffRequirement ImbueDebuff => new AODebuffRequirement(BuffID.OnFire, 60*10);
		public override CombinedDebuff[] CombinedDebuffs => [new (ModContent.BuffType<CharredEffect>(), ModContent.BuffType<AOPetrified>())];
		public override SynergyEffects Effects => new SynergyEffects(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<AOBleed>(),
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<SnowyEffect>(),
				BuffID.Wet,
				ModContent.BuffType<CharredEffect>()
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.15f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.01f),
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.85f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),0.99f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(),0.99f),
				new MagicBuffMultiplier(BuffID.Wet,0.99f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f),
				new MagicBuffMultiplier(BuffID.Poisoned,1.05f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.1f),
				new MagicBuffMultiplier(BuffID.Slimed,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),0.98f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),1.1f)
				
			]
			);

		public override void SpawningEffects(Projectile projectile) 
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Torch, (projectile.velocity.X * 2f), (projectile.velocity.Y * 2f), 0, default, 5f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Pixie, (8f * Main.rand.NextFloat() - 0.5f), (8f * Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void LingeringEffects(Projectile projectile)
		{
			for (int n = 0; n < 2; n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.Torch, 0f, 0f, 0, default, 2f);
			}

        }
		public override void ExplosionEffects(Projectile projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Torch, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 8f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Torch, (8f * Main.rand.NextFloat() - 0.5f), (8f * Main.rand.NextFloat() - 0.5f), 0, default, 8f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Pixie, (8f * Main.rand.NextFloat() - 0.5f), (8f * Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<FireBlast>()),]);
	}
}