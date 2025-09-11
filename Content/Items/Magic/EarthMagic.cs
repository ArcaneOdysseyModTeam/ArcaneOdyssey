using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using ArcaneOdyssey.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Buffs.DOT;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class EarthMagic : AOMagic
	{
        public override Color MagicColour => new Color(69,42,1,0);
		public override float AOImbueSpeed => 0.85f;
		public override float AOImbueSize => 1.26f;
		public override float AOImbueDamage => 1.075f;
		public override float AOMagicSpeed => 0.7f;
		public override float AOMagicSize => 1.3f;
		public override float AOMagicDamage => 1f;
        public override SoundStyle? MagicSound => SoundID.Item110;
		public override AODebuffRequirement MagicDebuff => new(ModContent.BuffType<AOBleed>(), 60*10);
		public override MagicEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<FreezingEffect>()
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(),1.1f),
				new MagicBuffMultiplier(BuffID.Venom,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.02f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.075f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.1f)
			]
			);
		public override Dictionary<Type, int> Spells => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<EarthBlast>()),]);
		
		public override void SpawningEffects(Projectile projectile) 
		{
			for (int n = 0; n < 3; n++){
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*(float)Main.rand.NextDouble()),projectile.position.Y+(projectile.height*(float)Main.rand.NextDouble())),0,0,DustID.Dirt,(projectile.velocity.X*2f),(projectile.velocity.Y*2f),0,default,3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Projectile projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 1, 1, DustID.Dirt, 0f, 0f, 0, default, 1f)];
		}

		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.Dirt, (8f * (float)(Main.rand.NextDouble() - 0.5)), (8f * (float)(Main.rand.NextDouble() - 0.5)), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(MagicSound, projectile.position, null);
		}
	}
}