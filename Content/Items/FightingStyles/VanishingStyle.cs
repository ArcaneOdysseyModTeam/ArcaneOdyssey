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
	public class VanishingStyle : FightingStyle
	{
		public override Color ImbueColour => Color.Black;
		public override SoundStyle? ImbueSound => SoundID.Item64;

		public override float AOImbueDamage => 0.9f;
		public override float AOImbueSpeed => 1.2f;
		public override float AOImbueSize => 1.056f;
		public override float AOScrollDamage => .8f;
		public override float AOScrollSize => 1f;
		public override float AOScrollSpeed => 1.2f;

		public override SynergyEffects Effects => new(
			[],
			[
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.15f)
			]
		);
		public override void SpawningEffects(Entity projectile) 
			{
				for (int n = 0; n<3; n++)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*Main.rand.NextFloat()),projectile.position.Y+(projectile.height*Main.rand.NextFloat())),0,0,DustID.Wraith,(projectile.velocity.X*2f),(projectile.velocity.Y*2f),0,default,3f)];
					spawnedDust.noGravity = true;
				}
			}
		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.Wraith, 0f, 0f, 0, default, 2f)];
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Wraith, (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Wraith, (8f * Main.rand.NextFloat() - 0.5f), (8f * Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
	}
}
