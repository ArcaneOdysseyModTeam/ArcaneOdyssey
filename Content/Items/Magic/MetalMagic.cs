using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using ArcaneOdyssey.Content.Items.Base;
using System;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
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
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class MetalMagic : AOMagic
	{
		public override SoundStyle? MagicSound => SoundID.Item99;
        public override Color MagicColour => new Color(100,100,100,255);
		public override float AOImbueSpeed => 0.825f;
		public override float AOImbueSize => 1.158f;
		public override float AOImbueDamage => 1.1f;
		public override float AOMagicSpeed => 0.65f;
		public override float AOMagicSize => 1.2f;
		public override float AOMagicDamage => 1.025f;
		public override AODebuffRequirement MagicDebuff => new AODebuffRequirement(ModContent.BuffType<AOBleed>(), 60*10);
		public override MagicEffects Effects => new MagicEffects(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<FreezingEffect>()
			], 
			[
				new MagicBuffMultiplier(BuffID.Venom,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.02f),
				new MagicBuffMultiplier(BuffID.OnFire3,1.05f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),1.1f)

			]
			);
		public override void SpawningEffects(Projectile projectile)
		{ 
			for(int n = 0;n<10;n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*Main.rand.NextFloat()),projectile.position.Y+(projectile.height*Main.rand.NextFloat())),0,0,DustID.Mercury,(projectile.velocity.X*0.4f),(projectile.velocity.Y*0.4f),0,default,1f)];
			}
		}

		public override void LingeringEffects(Projectile projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 1, 1, DustID.SilverFlame, 0f, 0f, 0, default, 1f)];
			spawnedDust.noGravity = true;
		}

		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * Main.rand.NextFloat()), projectile.position.Y + (projectile.height * Main.rand.NextFloat())), 0, 0, DustID.Mercury, (2f * Main.rand.NextFloat() - 0.5f), (2f * Main.rand.NextFloat() - 0.5f), 0, default, 1f)];
			}
			SoundEngine.PlaySound(MagicSound, projectile.position, null);
		}

		public override Dictionary<Type, int> Spells => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<MetalBlast>()),]);
	}
}