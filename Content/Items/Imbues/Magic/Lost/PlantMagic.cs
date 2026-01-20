using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class PlantMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override Color ImbueColour => Color.ForestGreen;
		public override AODebuffRequirement[] ImbueDebuffs => [new(BuffID.Poisoned, 60 * 10),];
		public override SoundStyle? ImbueSound => SoundID.Grass;
		public override float AOImbueSpeed => 1.05f;
		public override float AOImbueSize => 1.2f;
		public override float AOImbueDamage => .95f;
		public override SynergyEffects Effects => new([],
			[
				new(BuffID.Poisoned, 1.2f),
				new(BuffID.ShadowFlame,1.15f),
				new(BuffID.OnFire,1.15f),
				new(BuffID.Venom,1.1f),
				new(BuffID.OnFire3,1.1f),
			]);

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
			Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GrassBlades, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 0, default, 1.5f);
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 0, 0, DustID.Pearlwood, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 2.5f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GrassBlades, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 1.5f)];
				spawnedDust2.noGravity = true;
			}
		}

		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pearlwood, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 2f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.GrassBlades, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 1.5f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.Center);
		}

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(WoodMagic),typeof(EarthMagic));
		}
	}
}
