using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.FightingStyles
{
	public class IronLeg : FightingStyle
	{
		public override Color ImbueColour => Color.LightGray;
		public override SoundStyle? ImbueSound => SoundID.Item99;

		public override float AOImbueDamage => 1.125f;
		public override float AOImbueSpeed => 0.75f;
		public override float AOImbueSize => 1.1f;
		public override float AOScrollDamage => .95f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollSpeed => 0.75f;

		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60 * 10)];
		public override SynergyEffects Effects => new(
			[
				ModContent.BuffType<FreezingEffect>()
			],
			[
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.2f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.1f),
				new MagicBuffMultiplier(BuffID.Venom,1.1f)
			]
		);
		public override void SpawningEffects(Entity projectile)
		{ 
			for(int n = 0;n<10;n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*Main.rand.NextFloat()),projectile.position.Y+(projectile.height*Main.rand.NextFloat())),0,0,DustID.Mercury,(projectile.velocity.X*0.4f),(projectile.velocity.Y*0.4f),0,default,1f)];
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.SilverFlame, 0f, 0f, 0, default, 1f)];
			spawnedDust.noGravity = true;
			spawnedDust.noLight = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Mercury, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 2f)];
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Mercury, (2f * Main.rand.NextFloat() - 0.5f), (2f * Main.rand.NextFloat() - 0.5f), 0, default, 1f)];
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
	}
}
