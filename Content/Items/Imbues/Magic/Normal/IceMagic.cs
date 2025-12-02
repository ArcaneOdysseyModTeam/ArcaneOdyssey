using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class IceMagic : AOMagic
    {
        public override float DashResist => 1.3f;
        public override bool? Cold => true;
        public override SoundStyle? ImbueSound => SoundID.Item27;
        public override Color ImbueColour => new(30,200,255,255);
        public override bool CanBeWet => false;
        public override float AOImbueSpeed => .925f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 1.05f;
		public override float AOScrollSpeed => 0.85f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 0.975f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<FreezingEffect>(), 60 * 10), new(ModContent.BuffType<AOFrozen>(), 60, 33)];
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOFrozen>())];

        

		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				BuffID.Wet,
				ModContent.BuffType<AOBleed>(),
				BuffID.Burning, 
				BuffID.Venom,
				BuffID.OnFire3,
				BuffID.ShadowFlame,
				ModContent.BuffType<CharredEffect>()
			],
			[ // synergies
				new(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
				new(ModContent.BuffType<AOFrozen>(), 1.1f), // frozen
				new(ModContent.BuffType<FreezingEffect>(), 1.1f), // freezing
				new(BuffID.Wet, 1.1f), // (add stunning later!)
				new(BuffID.OnFire, .9f), // burning
				new(BuffID.Oiled,1.03f),
				new(ModContent.BuffType<CharredEffect>(), .9f), // charred
				new(BuffID.OnFire3, .8f), // scorched
				new(BuffID.ShadowFlame, 0.8f),
				new(ModContent.BuffType<SnowyEffect>(), 1.1f),
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<SearedEffect>(),0.8f)
			]
			);
		public override void SpawningEffects(Entity projectile) 
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SnowflakeIce, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+projectile.width*Main.rand.NextFloat(),projectile.position.Y+projectile.height*Main.rand.NextFloat()),0,0,DustID.Ice,projectile.velocity.X*0.5f,projectile.velocity.Y*0.5f,0,default,2f)];
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Ice, 0f, 0f, 0, default, 1f)];
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.SnowflakeIce, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Ice, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 2f)];
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SnowflakeIce, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Ice, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 2f)];
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
	}
}
