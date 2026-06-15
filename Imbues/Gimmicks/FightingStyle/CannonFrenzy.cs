using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Imbues.Base;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Imbues.Gimmicks.FightingStyle
{
	public class CannonFrenzy : ImbueGimmick
	{
		public override void SpawningEffects(Projectile projectile)
		{
			if (projectile.TryGetOwner(out Player player))
			{
				if (!player.ArcaneOdyssey().OnCooldown(Name))
				{
					if (!projectile.DamageType.Name.Contains("TrueMelee") && projectile.type != ProjectileID.CannonballFriendly)
					{
						if (player.ConsumeItem(ItemID.Cannonball))
						{
							Projectile.NewProjectile(projectile.GetSource_FromThis(), player.MountedCenter, player.SafeDirectionTo(Main.MouseWorld) * 20, ProjectileID.CannonballFriendly, (projectile.damage * .5f).Round(), projectile.knockBack * .5f, player.whoAmI);
						}
						else
							Projectile.NewProjectile(projectile.GetSource_FromThis(), player.MountedCenter, player.SafeDirectionTo(Main.MouseWorld) * 10, ProjectileID.CannonballFriendly, (projectile.damage * .25f).Round(), projectile.knockBack * .25f, player.whoAmI);
						player.ArcaneOdyssey().SetCooldown(new Cooldown(Name, Mod, 60));
					}
				}
			}
		}

		public override void UseAnimation(Item item, Player player)
		{
			if (item.ArcaneOdyssey().WeaponsType == WeaponType.Strength)
			{
				if (!player.ArcaneOdyssey().OnCooldown(Name))
				{
					if (player.ConsumeItem(ItemID.Cannonball))
					{
						Projectile.NewProjectile(item.GetSource_ItemUse(player), player.MountedCenter, player.SafeDirectionTo(Main.MouseWorld) * 20, ProjectileID.CannonballFriendly, (item.damage * .5f).Round(), item.knockBack * .5f, player.whoAmI);
					}
					else
						Projectile.NewProjectile(item.GetSource_ItemUse(player), player.MountedCenter, player.SafeDirectionTo(Main.MouseWorld) * 10, ProjectileID.CannonballFriendly, (item.damage * .25f).Round(), item.knockBack * .25f, player.whoAmI);
					player.ArcaneOdyssey().SetCooldown(new Cooldown(Name, Mod, 60));
				}
			}
		}
	}
}
