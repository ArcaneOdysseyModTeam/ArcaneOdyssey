using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class CloudMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float DashSpeed => 1.2f; // burst
		public override float? DashResist => 1.1f;
		public override float KBMulti => 1.25f;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => new(255, 255, 255, 255);
		public override float AOImbueSpeed => .9f;
		public override float AOImbueSize => 1.3f;
		public override float AOImbueDamage => .8f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<CloudyEffect>(), 5*60)];

		public override SynergyEffects Effects => new(
			[
				BuffID.OnFire,
				ModContent.BuffType<CharredEffect>(),
				BuffID.Venom,
				ModContent.BuffType<SandyEffect>(),
				BuffID.Wet,
				ModContent.BuffType<SnowyEffect>(),
				ModContent.BuffType<AOScalding>(),
				BuffID.Oiled
			],
			[
				new(ModContent.BuffType<CloudyEffect>(), 1.1f),
				new(ModContent.BuffType<Crystallized>(),0.9f),
				new(BuffID.OnFire,0.9f),
				new(ModContent.BuffType<CharredEffect>(),1.125f),
				new(ModContent.BuffType<FreezingEffect>(),1.1f),
				new(BuffID.Poisoned,0.9f),
				new(ModContent.BuffType<SandyEffect>(),0.9f),
				new(BuffID.ShadowFlame,1.15f),
				new(BuffID.Wet,0.9f),
				new(BuffID.Oiled,0.98f),
				new(ModContent.BuffType<AOScalding>(),0.9f),
				new(ModContent.BuffType<SearedEffect>(),1.15f)
			]
			);

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 6f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, 0f, 0f, 0, default, 2f)];
			spawnedDust.noGravity = true;
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 6f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.BubbleBurst_White, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 6f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center, null);
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(WindMagic));
		}
	}
}
