using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Normal
{
	public class WaterMagic : AOMagic
	{
		public override float DashSpeed => 1.2f; // burst
		public override bool? Cold => true;
		public override Color ImbueColour => new(0, 30, 255);
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.22f;
		public override float AOImbueDamage => 0.975f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.25f;
		public override float AOScrollDamage => 0.9f;
		public override SoundStyle? ImbueSound => SoundID.Splash;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Wet, 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				BuffID.OnFire,
				ModContent.BuffType<CharredEffect>(),
				BuffID.Venom,
				BuffID.OnFire3,
				BuffID.ShadowFlame,
				BuffID.Oiled,
				ModContent.BuffType<Singed>(),
				ModContent.BuffType<AOScalding>(),
				ModContent.BuffType<SearedEffect>()
			],
			[
				new(ModContent.BuffType<Crystallized>(),0.85f),
				new(ModContent.BuffType<AOBleed>(),1.05f),
				new(BuffID.OnFire,0.8f),
				new(ModContent.BuffType<CharredEffect>(),0.9f),
				new(BuffID.Venom,0.9f),
				new(ModContent.BuffType<FreezingEffect>(),1.075f),
				new(BuffID.OnFire3,0.9f),
				new(BuffID.Oiled,0.98f),
				new(ModContent.BuffType<SandyEffect>(),0.8f),
				new(BuffID.ShadowFlame,0.7f),
				new(ModContent.BuffType<SnowyEffect>(),1.1f),
				new(ModContent.BuffType<SearedEffect>(),0.7f),
				new(ModContent.BuffType<Singed>(), 0.8f),
			]
		);

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)

			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Water, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Water, 0f, 0f, 0, default, 1.2f);
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Water, (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (35f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Water, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center);
		}
	}
}