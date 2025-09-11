using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
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
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class WindMagic : AOMagic
	{
		public override SoundStyle? MagicSound => SoundID.Dig;
        public override Color MagicColour => new Color(255,255,255,255);
		public override float AOImbueSpeed => 1.175f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => .9f;
		public override float AOMagicSpeed => 1.35f;
		public override float AOMagicSize => 1.2f;
		public override float AOMagicDamage => .825f;
        public override CombinedDebuff[] CombinedDebuffs => [new(ModContent.BuffType<SnowyEffect>(), ModContent.BuffType<AOFrozen>()), new(ModContent.BuffType<FreezingEffect>(), ModContent.BuffType<AOFrozen>())];
		public override MagicEffects Effects => new MagicEffects(
			[
				BuffID.OnFire,
				ModContent.BuffType<FreezingEffect>(),
				ModContent.BuffType<CharredEffect>(),
				BuffID.Venom,
				ModContent.BuffType<SandyEffect>(),
				BuffID.Wet,
				ModContent.BuffType<SnowyEffect>()
			], 
			[
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),0.9f),
				new MagicBuffMultiplier(BuffID.OnFire,0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(),1.125f),
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(),1.1f),
				new MagicBuffMultiplier(BuffID.Poisoned,0.9f),
				new MagicBuffMultiplier(ModContent.BuffType<SandyEffect>(),0.9f),
				new MagicBuffMultiplier(BuffID.ShadowFlame,1.15f),
				new MagicBuffMultiplier(BuffID.Wet,0.9f)
			]
			);
			public override void SpawningEffects(Projectile projectile) 
		{
			for (int n = 0; n<3; n++)
			{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*(float)Main.rand.NextDouble()),projectile.position.Y+(projectile.height*(float)Main.rand.NextDouble())),0,0,DustID.BubbleBurst_White,(projectile.velocity.X*2f),(projectile.velocity.Y*2f),0,default,3f)];
					spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Projectile projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 1, 1, DustID.BubbleBurst_White, 0f, 0f, 0, default, 1f)];
			spawnedDust.noGravity = true;
		}

		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.BubbleBurst_White, (8f * (float)(Main.rand.NextDouble() - 0.5)), (8f * (float)(Main.rand.NextDouble() - 0.5)), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(MagicSound, projectile.position, null);
		}
		public override Dictionary<Type, int> Spells => new([KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<WindBlast>()),]);
	}
}
