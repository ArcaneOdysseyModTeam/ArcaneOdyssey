using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.AOPlayers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Imbues.FightingStyles.Normal
{
	public class CannonFist : FightingStyle
	{
		public override float Aura => .875f;
		public override Color ImbueColour => Color.Black;
		public override SoundStyle? ImbueSound => SoundID.Item14;

		public override float AOImbueDamage => 1.085f;
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.056f;
		public override float AOScrollDamage => 0.7f;
		public override float AOScrollSize => 1f;
		public override float AOScrollSpeed => 1f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.shoot = ProjectileID.CannonballFriendly;
			Item.shootSpeed = 8f * AOScrollSpeed;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.knockBack = 2f;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (player.ConsumeItem(ItemID.Cannonball))
			{
				velocity *= 2;
				damage *= 2;
				knockback *= 2;
			}
		}

		public override Debuff[] ImbueDebuffs => [Debuff.Create<AOBleed>()];
		public override SynergyEffects Effects => new(
			[],
			[
				Synergy.Create<Crystallized>(1.1f)
			]
		);

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, direction.X * 2f, direction.Y * 2f, Scale: 4f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, Scale: 2f * area.RelativeScale())];
			spawnedDust.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Pixie, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 3f * intensity)];
				spawnedDust2.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(position, 0, 0, DustID.Ash, (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * AOScrollSize * intensity), Scale: 4f * intensity)];
				spawnedDust3.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
				Dust spawnedDust3 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Pixie, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust3.noGravity = true;
				Dust spawnedDust2 = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.Ash, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 4f * area.RelativeScale())];
				spawnedDust2.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BasicCombat>().AddIngredient(ItemID.Bomb, 15).Register();
		}
	}

	public class CannonFistShooter : GlobalProjectile
	{
		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			if (source is not EntitySource_Parent { Entity: NPC })
			{
				if (projectile.TryGetOwner(out Player player))
				{
					if (Main.myPlayer == player.whoAmI)
					{
						if (projectile.TryGetImbue(out var imbue) && imbue is CannonFist cfist)
						{
							if (!player.ArcaneOdyssey().OnCooldown(cfist.Name))
							{
								if (!projectile.DamageType.Name.Contains("TrueMelee") && projectile.type != ProjectileID.CannonballFriendly)
								{
									if (player.ConsumeItem(ItemID.Cannonball))
									{
										Projectile.NewProjectile(source, player.MountedCenter, player.SafeDirectionTo(Main.MouseWorld) * 20, ProjectileID.CannonballFriendly, (projectile.damage * .5f).Round(), projectile.knockBack * .5f, player.whoAmI);
									}
									else
										Projectile.NewProjectile(source, player.MountedCenter, player.SafeDirectionTo(Main.MouseWorld) * 10, ProjectileID.CannonballFriendly, (projectile.damage * .25f).Round(), projectile.knockBack * .25f, player.whoAmI);
									player.ArcaneOdyssey().SetCooldown(new Cooldown(cfist.Name, Mod, 60));
								}
							}
						}
					}
				}
			}
		}
	}

	public class CannonFistItemShot : GlobalItem
	{
		public override void UseAnimation(Item item, Player player)
		{
			if (Main.myPlayer == player.whoAmI)
			{
				if (item.Imbue() is CannonFist cfist && item.ArcaneOdyssey().WeaponsType == WeaponType.Arcanium)
				{
					if (!player.ArcaneOdyssey().OnCooldown(cfist.Name))
					{
						if (player.ConsumeItem(ItemID.Cannonball))
						{
							Projectile.NewProjectile(item.GetSource_ItemUse(player), player.MountedCenter, player.SafeDirectionTo(Main.MouseWorld) * 20, ProjectileID.CannonballFriendly, (item.damage * .5f).Round(), item.knockBack * .5f, player.whoAmI);
						}
						else
							Projectile.NewProjectile(item.GetSource_ItemUse(player), player.MountedCenter, player.SafeDirectionTo(Main.MouseWorld) * 10, ProjectileID.CannonballFriendly, (item.damage * .25f).Round(), item.knockBack * .25f, player.whoAmI);
						player.ArcaneOdyssey().SetCooldown(new Cooldown(cfist.Name, Mod, 60));
					}
				}
			}
		}
	}
}
