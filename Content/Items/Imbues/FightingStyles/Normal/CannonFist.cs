using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal
{
	public class CannonFist : FightingStyle
	{
		public override Color ImbueColour => Color.Black;
		public override SoundStyle? ImbueSound => SoundID.Item14;

		public override float AOImbueDamage => 1.085f;
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.056f;
		public override float AOScrollDamage => 0.7f;
		public override float AOScrollSize => 1f;
		public override float AOScrollSpeed => 1f;

		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<AOBleed>(), 60 * 10)];
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
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Ash, projectile.velocity.X * 2f, projectile.velocity.Y * 2f, 0, default, 4f)];
				spawnedDust.noGravity = true;
			}
		}
		public override void LingeringEffects(Entity projectile)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 1, 1, DustID.Ash, 0f, 0f, 0, default, 2f)];
			spawnedDust.noGravity = true;
		}
		public override void ExplosionEffects(Entity projectile)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 1, 1, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 1, 1, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 3f)];
				spawnedDust2.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width / 2f, projectile.position.Y + projectile.height / 2f), 1, 1, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize), 0, default, 4f)];
				spawnedDust3.noGravity = true;
			}
		}
		public override void KillEffects(Entity projectile)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Pixie, 8f * Main.rand.NextFloat() - 0.5f, 8f * Main.rand.NextFloat() - 0.5f, 0, default, 3f)];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Pixie, 8f * Main.rand.NextFloat() - 0.5f, 8f * Main.rand.NextFloat() - 0.5f, 0, default, 3f)];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(new Vector2(projectile.position.X + projectile.width * Main.rand.NextFloat(), projectile.position.Y + projectile.height * Main.rand.NextFloat()), 0, 0, DustID.Ash, 8f * Main.rand.NextFloat() - 0.5f, 8f * Main.rand.NextFloat() - 0.5f, 0, default, 4f)];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, projectile.position, null);
		}
		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.Bomb, 25).Register();
		}
	}

	public class CannonFistShooter : GlobalProjectile
	{
		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			if (source is not EntitySource_Parent { Entity: NPC })
			{
				var player = Main.player[projectile.owner].ArcaneOdyssey();
				if (!player.OnCooldown("CannonFistShot"))
				{
					if (projectile.TryGetImbue(out var imbue) && imbue is CannonFist && projectile.DamageType.Name != "TrueMeleeDamageClass" && projectile.DamageType.Name != "TrueMeleeNoSpeedDamageClass" && projectile.type != ProjectileID.CannonballFriendly)
					{
						if (player.Player.ConsumeItem(ItemID.Cannonball))
						{
							Projectile.NewProjectile(source, player.Player.MountedCenter, player.Player.SafeDirectionTo(Main.MouseWorld) * 20, ProjectileID.CannonballFriendly, (projectile.damage * .5f).Round(), projectile.knockBack * .5f, player.Player.whoAmI);
						}
						else
							Projectile.NewProjectile(source, player.Player.MountedCenter, player.Player.SafeDirectionTo(Main.MouseWorld) * 10, ProjectileID.CannonballFriendly, (projectile.damage * .25f).Round(), projectile.knockBack * .25f, player.Player.whoAmI);
                        player.SetCooldown(new CannonFistShotCooldown().AOCooldown);
					}
				}
			}
		}
	}

    public class CannonFistShotCooldown : CooldownSystem
    {
        public override int CooldownLength => 60;
        public override string Name => "Cannon Fist Shot Cooldown";
    }
}
