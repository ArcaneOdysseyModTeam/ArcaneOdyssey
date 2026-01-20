using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class BlizzardMagic : AOMagic
	{
		public override float? DashResist => 1.075f;
		public override bool? Cold => true;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => new(255, 255, 255, 255);
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float AOImbueSpeed => .925f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueDamage => 1f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<SnowyEffect>(), 60 * 10)];
		public override CombinedDebuff[] CombinedDebuffs => [new(BuffID.Wet, ModContent.BuffType<AOFrozen>()), new(ModContent.BuffType<FreezingEffect>(), ModContent.BuffType<AOFrozen>())];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				BuffID.OnFire,
				ModContent.BuffType<CharredEffect>(),
				BuffID.Venom,
				BuffID.Wet,
				BuffID.Oiled,
				ModContent.BuffType<FreezingEffect>(),
				BuffID.OnFire3,
				BuffID.ShadowFlame,
				ModContent.BuffType<AOScalding>(),
				ModContent.BuffType<SearedEffect>()
			],
			[
				new(ModContent.BuffType<Crystallized>(),0.8f),
				new(ModContent.BuffType<AOBleed>(),1.05f),
				new(BuffID.OnFire,0.90f),
				new(ModContent.BuffType<CharredEffect>(),0.8f),
				new(BuffID.Venom,0.9f),
				new(ModContent.BuffType<FreezingEffect>(),1.1f),
				new(BuffID.OnFire3,0.9f),
				new(BuffID.ShadowFlame,0.8f),
				new(BuffID.Wet,1.1f),
				new(ModContent.BuffType<SearedEffect>(),0.8f)
			]
			);

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Snow, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Snow, 0f, 0f, 0, default, 1f)];
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.SnowBlock, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SnowBlock, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center);
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(SnowMagic), typeof(IceMagic), typeof(WindMagic));
		}
	}
}
