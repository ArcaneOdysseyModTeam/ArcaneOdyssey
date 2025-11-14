using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
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
	public class GlassMagic : AOMagic
    {
        public override float DashResist => 1.05f;
        public override float ItemInvisibility => .5f;
		public override SoundStyle? ImbueSound => SoundID.Shatter;
        public override Color ImbueColour => new(255,255,255);
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.053f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollDamage => 0.9f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60*10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			], 
			[
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.92f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f)
			]
			);
		public override void SpawningEffects(Entity projectile)
		{
            for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X+projectile.width*Main.rand.NextFloat(),projectile.position.Y+projectile.height*Main.rand.NextFloat()),0,0,DustID.Glass,projectile.velocity.X*0.4f,projectile.velocity.Y*0.4f,0,default,1f);
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.SilverFlame, 0f, 0f, 0, default, 1f)];
			spawnedDust.noGravity = true;
			spawnedDust.noLight = true;
		}
	public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(projectile.Center, 1, 1, DustID.Glass, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 1f);
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Glass, 2f * (Main.rand.NextFloat() - 0.5f), 2f * (Main.rand.NextFloat() - 0.5f), 0, default, 1f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		public override List<Type> Skills => [typeof(GlassBlast), typeof(GlassPulsar), typeof(GlassCannon)];
	}
}