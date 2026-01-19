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
			if (Projectile.owner == Main.myPlayer)
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 15 * player.SafeDirectionTo(Main.MouseWorld), ModContent.ProjectileType<PiercingGale>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
		}
	}
}
