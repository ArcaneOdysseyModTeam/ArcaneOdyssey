using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class BronzeStaffProjectile : BaseStaffProjectile
	{
		public override float AOSpeed => 1;
		public override float AOSize => .9f;

		public override void EffectBeforeSpin(Player player)
		{
			if (player.PlayerItem()?.ModItem is AOWeapon weap)
			{
				weap.ActivateAbility(player, true);
			}
			if (Projectile.owner == Main.myPlayer)
				AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 18f * player.SafeDirectionTo(Main.MouseWorld), ModContent.ProjectileType<PiercingGale>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner, Imbue, SecondImbue);
		}
	}
}
