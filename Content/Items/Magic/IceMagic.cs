using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Blasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Magic
{
	public class IceMagic : AOMagic
    {
		public override SoundStyle? MagicSound => SoundID.Item27;
        public override Color MagicColour => new Color(30,200,255,255);
        public override bool CanBeWet => false;
        public override float AOImbueSpeed => .925f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 1.05f;
		public override float AOMagicSpeed => 0.85f;
		public override float AOMagicSize => 1.2f;
		public override float AOMagicDamage => 0.975f;
		public override AODebuffRequirement MagicDebuff => new(ModContent.BuffType<FreezingEffect>(), 60 * 10);
		public override AODebuffRequirement MagicDebuff2 => new(ModContent.BuffType<AOFrozen>(), 60, 33);
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOFrozen>())];

		public override Dictionary<Type, int> Spells => new Dictionary<Type, int>(
			[KeyValuePair.Create(typeof(BlastSpell), ModContent.ProjectileType<IceBlast>()),
			// create more here as time passes
			]);

		public override MagicEffects Effects => new MagicEffects(
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
				new MagicBuffMultiplier(ModContent.BuffType<AOBleed>(), 1.2f), // bleeding
				new MagicBuffMultiplier(ModContent.BuffType<AOFrozen>(), 1.1f), // frozen
				new MagicBuffMultiplier(ModContent.BuffType<FreezingEffect>(), 1.1f), // freezing
				new MagicBuffMultiplier(BuffID.Wet, 1.1f), // (add stunning later!)
				new MagicBuffMultiplier(BuffID.OnFire, .9f), // burning
				new MagicBuffMultiplier(ModContent.BuffType<CharredEffect>(), .9f), // charred
				new MagicBuffMultiplier(BuffID.OnFire3, .8f), // scorched
				new MagicBuffMultiplier(BuffID.ShadowFlame, 1.15f),
				new MagicBuffMultiplier(ModContent.BuffType<SnowyEffect>(), 1.1f),
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.075f),
			]
			);
			public override void SpawningEffects(Projectile projectile) 
			{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.SnowflakeIce, (projectile.velocity.X * 0.5f), (projectile.velocity.Y * 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X+(projectile.width*(float)Main.rand.NextDouble()),projectile.position.Y+(projectile.height*(float)Main.rand.NextDouble())),0,0,DustID.Ice,(projectile.velocity.X*0.5f),(projectile.velocity.Y*0.5f),0,default,2f)];
			}
			}
		public override void LingeringEffects(Projectile projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 1, 1, DustID.Ice, 0f, 0f, 0, default, 1f)];
		}

		public override void KillEffects(Projectile projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.SnowflakeIce, (8f * (float)(Main.rand.NextDouble() - 0.5)), (8f * (float)(Main.rand.NextDouble() - 0.5)), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + (projectile.width * (float)Main.rand.NextDouble()), projectile.position.Y + (projectile.height * (float)Main.rand.NextDouble())), 0, 0, DustID.Ice, (8f * (float)(Main.rand.NextDouble() - 0.5)), (8f * (float)(Main.rand.NextDouble() - 0.5)), 0, default, 2f)];
			}
			SoundEngine.PlaySound(MagicSound, projectile.position, null);
		}
			public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient<HecateOrb>(1);
			recipe.Register();
		}
	}
}
