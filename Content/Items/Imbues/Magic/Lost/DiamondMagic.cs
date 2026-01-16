using ArcaneOdyssey.Content.Items.Base;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Terraria.ID;
using Terraria;
using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class DiamondMagic : AOMagic
	{
		public override float? DashResist => 1.6f;
		public override float AOImbueSpeed => 1.13f;
		public override float AOImbueSize => 1.11f;
		public override float AOImbueDamage => 1.3f;
		public override float AOScrollSpeed => 1.13f;
		public override float AOScrollSize => 1.15f;
		public override float AOScrollDamage => 1.3f;
		public override Color ImbueColour => new(50, 255, 255);
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override SoundStyle? ImbueSound => SoundID.Shatter;
		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60 * 10)];
		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			],
			[
				new(ModContent.BuffType<FreezingEffect>(),1.01f),
				new(ModContent.BuffType<AOBleed>(),1.01f),
				new(BuffID.Venom,1.01f),
				new(BuffID.OnFire3,1.075f),
				new(ModContent.BuffType<SandyEffect>(),1.125f),
				new(ModContent.BuffType<Crystallized>(),1.125f)
			]
			);
		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GemSapphire, projectile.velocity.X * 0.4f, projectile.velocity.Y * 0.4f, 0, default, 1f);
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
				Dust.NewDust(projectile.Center, 0, 0, DustID.GemSapphire, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f);
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 30; n++)
			{
				Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GemSapphire, 2f * (Main.rand.NextFloat() - 0.5f), 2f * (Main.rand.NextFloat() - 0.5f), 0, default, 1f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center, null);
		}



		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(CrystalMagic), typeof(EarthMagic), typeof(MetalMagic), typeof(SandMagic),typeof(MagmaMagic));
		}
	}
}