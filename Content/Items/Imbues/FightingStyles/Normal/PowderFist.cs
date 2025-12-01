using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.Audio;
using ArcaneOdyssey.Content.Projectiles;

namespace ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal
{
	public class PowderFist : FightingStyle
	{
		public override float DashSpeed => 1.2f;
		public override bool? Cold => false;
		public override Color ImbueColour => Color.DarkGray;
		public override SoundStyle? ImbueSound => SoundID.Item14;

		public override float AOImbueDamage => 1.085f;
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.056f;
		public override float AOScrollDamage => 0.7f;
		public override float AOScrollSize => 1f;
		public override float AOScrollSpeed => 1f;

		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<CharredEffect>(), 60 * 10)];
		public override SynergyEffects Effects => new(
			[],
			[
				new MagicBuffMultiplier(ModContent.BuffType<Crystallized>(),1.1f)
			]
		);
		public override void SpawningEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 3f)];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Ash, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 4f)];
				spawnedDust2.noGravity = true;
			}
		}
		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, 0f, 0f, 0, default, 1.6f)];
			spawnedDust.noGravity = true;
			Dust spawnedDust3 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, 0f, 0f, 0, default, 1.6f)];
			spawnedDust3.noGravity = true;
			Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Ash, 0f, 0f, 0, default, 2f)];
			spawnedDust2.noGravity = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust2.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(projectile.Center, 1, 1, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 4f)];
				spawnedDust3.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Pixie, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 3f)];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Ash, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, default, 4f)];
				spawnedDust2.noGravity = true;
			}
			if (projectile is Projectile)
			{
				Projectile proj = Projectile.NewProjectileDirect(Main.projectile[projectile.whoAmI].GetSource_FromThis(), projectile.Center, Vector2.Zero, ModContent.ProjectileType<PowderExplosion>(), 0, 3f, Main.projectile[projectile.whoAmI].owner, 0, Main.projectile[projectile.whoAmI].damage / 2f);
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.ExplosivePowder,15).Register();
		}
	}
}
