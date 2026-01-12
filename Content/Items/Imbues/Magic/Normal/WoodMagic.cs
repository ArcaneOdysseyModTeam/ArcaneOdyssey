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
	public class WoodMagic : AOMagic
	{
		public override float? DashResist => 1.3f;
		public override SoundStyle? ImbueSound => SoundID.Dig;
		public override Color ImbueColour => new(61, 33, 0, 255);
		public override float AOImbueSpeed => 0.9f;
		public override float AOImbueSize => 1.162f;
		public override float AOImbueDamage => 1.025f;
		public override float AOScrollSpeed => 0.8f;
		public override float AOScrollSize => 1.2f;
		public override float AOScrollDamage => 0.95f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			],
			[
				new(BuffID.OnFire,1.1f),
				new(ModContent.BuffType<CharredEffect>(),1.1f),
				new(BuffID.Venom,1.05f),
				new(BuffID.OnFire3,1.05f),
				new(ModContent.BuffType<SandyEffect>(),1.1f),
				new(BuffID.ShadowFlame,1.1f),
				new(ModContent.BuffType<AOScalding>(),1.1f),
				new(ModContent.BuffType<SearedEffect>(),1.1f)
			]
			);
		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pearlwood, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 0, default, 1.5f);
			}
		}
		public override void LingeringEffects(Entity projectile)
		{
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pearlwood, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 0, default, 1f);
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Pearlwood, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 2.5f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pearlwood, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 2f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center, null);
		}
	}
}