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
	public class EarthMagic : AOMagic
	{
		public override float DashResist => 1.4f;
		public override Color ImbueColour => new(69, 42, 1);
		public override float AOImbueSpeed => 0.85f;
		public override float AOImbueSize => 1.26f;
		public override float AOImbueDamage => 1.075f;
		public override float AOScrollSpeed => 0.7f;
		public override float AOScrollSize => 1.3f;
		public override float AOScrollDamage => 1f;
		public override SoundStyle? ImbueSound => SoundID.Item110;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				ModContent.BuffType<FreezingEffect>()
			],
			[
				new(ModContent.BuffType<AOBleed>(),1.1f),
				new(BuffID.Venom,1.075f),
				new(ModContent.BuffType<Crystallized>(),1.075f),
				new(ModContent.BuffType<FreezingEffect>(),1.02f),
				new(BuffID.OnFire3,1.075f),
				new(ModContent.BuffType<SandyEffect>(),1.1f)
			]
			);



		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Dirt, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Entity projectile)
		{
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Dirt, 0f, 0f, 0, default, 1f);
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(projectile.Center, 0, 0, DustID.Dirt, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f);
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Dirt, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
	}
}