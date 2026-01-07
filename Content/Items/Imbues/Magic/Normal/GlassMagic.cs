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
	public class GlassMagic : AOMagic
	{
		public override float? DashResist => 1.05f;
		public override float ItemInvisibility => .5f;
		public override SoundStyle? ImbueSound => SoundID.Shatter;
		public override Color ImbueColour => new(255, 255, 255);
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.053f;
		public override float AOImbueDamage => 1f;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollDamage => 0.9f;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			],
			[
				new(BuffID.Venom,1.05f),
				new(ModContent.BuffType<Crystallized>(),0.92f),
				new(ModContent.BuffType<FreezingEffect>(),1.075f),
				new(ModContent.BuffType<SandyEffect>(),1.1f),
				new(BuffID.OnFire3,1.05f)
			]
			);
		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Glass, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, 0, default, 1f);
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.SilverFlame, 0f, 0f, 0, default, 1f)];
			spawnedDust.noGravity = true;
			spawnedDust.noLight = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDust(projectile.Center, 0, 0, DustID.Glass, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 1f);
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Glass, 2f * (Main.rand.NextFloat() - 0.5f), 2f * (Main.rand.NextFloat() - 0.5f), 0, default, 1f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center, null);
		}
	}
}