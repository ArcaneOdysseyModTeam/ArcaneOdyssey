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
	public class ThreadMagic : AOMagic
	{
		public override Color ImbueColour => Color.Gray;
		public override SoundStyle? ImbueSound => SoundID.Grass;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;

		public override float AOImbueDamage => .7f;
		public override float AOImbueSize => 1.15f;
		public override float AOImbueSpeed => .85f;

		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<Tangled>(), 60)];

		public override SynergyEffects Effects => new([],
			[
				new(BuffID.OnFire, .9f), // burning
				new(ModContent.BuffType<CharredEffect>(), .9f), // charred
				new(ModContent.BuffType<SearedEffect>(),0.8f),
				new(BuffID.OnFire3, .95f),
				new(BuffID.ShadowFlame, 0.8f),
			]);

		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Web, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Entity projectile)
		{
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Web);
		}

		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Web, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Web, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center);
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(WoodMagic));
		}
	}
}
