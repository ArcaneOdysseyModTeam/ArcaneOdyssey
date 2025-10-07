using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
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
	public class WoodMagic : AOMagic
	{
		public override SoundStyle? ImbueSound => SoundID.Dig;
        public override Color ImbueColour => new Color(61,33,0,255);
		public override float AOImbueSpeed => 0.9f;
		public override float AOImbueSize => 1.162f;
		public override float AOImbueDamage => 1.025f;
		public override float AOScrollSpeed => 0.8f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 0.95f;
		public override AODebuffRequirement ImbueDebuff => new AODebuffRequirement(ModContent.BuffType<AOBleed>(), 60*10);
		public override SynergyEffects Effects => new SynergyEffects(
			[ // these are debuffs cleared on hit
				
			],
			[
				new MagicBuffMultiplier(BuffID.OnFire,1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<AOScalding>(),1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<SearedEffect>(),1.1f)
			]
			);
		public override void SpawningEffects(Entity projectile) 
		{
			for(int n = 0; n<3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*Main.rand.NextFloat()),projectile.position.Y+(projectile.height*Main.rand.NextFloat())),0,0,DustID.Pearlwood,(projectile.velocity.X*0.2f),(projectile.velocity.Y*0.2f),0,default,1.5f)];
			}
		}
		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Pearlwood, (projectile.velocity.X * 0.2f), (projectile.velocity.Y * 0.2f), 0, default, 1f)];
			Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*Main.rand.NextFloat()),projectile.position.Y+(projectile.height*Main.rand.NextFloat())),0,0,DustID.GrassBlades,(projectile.velocity.X*0.2f),(projectile.velocity.Y*0.2f),0,default,1.5f)];
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width / 2f), projectile.position.Y + (projectile.height / 2f)), 1, 1, DustID.Pearlwood, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 2.5f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.GrassBlades, (8f * (Main.rand.NextFloat() - 0.5f)), (8f * (Main.rand.NextFloat() - 0.5f)), 0, default, 1.5f)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Pearlwood, (8f * (Main.rand.NextFloat() - 0.5f)), (8f * (Main.rand.NextFloat() - 0.5f)), 0, default, 2f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.GrassBlades, (8f * (Main.rand.NextFloat() - 0.5f)), (8f * (Main.rand.NextFloat() - 0.5f)), 0, default, 1.5f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		public override Dictionary<Type, int> Skills => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<WoodBlast>()),]);
	}
}