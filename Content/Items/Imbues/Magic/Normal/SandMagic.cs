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
	public class SandMagic : AOMagic
	{
		public override float DashResist => 1.1f;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => new(255, 255, 60, 255);
		public override bool CanBeWet => false;
		public override float AOImbueSpeed => 0.975f;
		public override float AOImbueSize => 1.053f;
		public override float AOImbueDamage => 1.05f;
		public override float AOScrollSpeed => 0.95f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollDamage => 0.975f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<SandyEffect>(), 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				BuffID.Wet,
				BuffID.Oiled
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.1f),
				new(BuffID.OnFire,1.125f),
				new(ModContent.BuffType<CharredEffect>(),1.01f),
				new(BuffID.Venom,1.075f),
				new(ModContent.BuffType<Crystallized>(),0.8f),
				new(BuffID.OnFire3,1.075f),
				new(BuffID.Wet,0.8f),
				new(BuffID.Oiled,0.9f),
				new(ModContent.BuffType<AOScalding>(),1.125f)
			]
			);

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Sand, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Sand, 0f, 0f, 0, default, 1f);
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Sand, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Sand, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
	}
}