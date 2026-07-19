using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Weapons.Bronze;
using ArcaneOdyssey.Projectiles.Abilities;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Weapons
{
	public class BronzeStaffProjectile : BaseStaffProjectile
	{
		public override float Speed => 1;
		public override float Size => .9f;

		public override void EffectBeforeSpin(Player player)
		{
			if (!player.ArcaneOdyssey().OnCooldown<PiercingGaleCooldown>())
			{
				player.ArcaneOdyssey().SetCooldown<PiercingGaleCooldown>();
				if (player.PlayerItem()?.ModItem is BronzeStaff weap)
				{
					weap.ActivateAbility(player, true);
				}
				if (Projectile.owner == Main.myPlayer)
					AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, 18f * player.SafeDirectionTo(Main.MouseWorld), ModContent.ProjectileType<PiercingGale>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Imbue, SecondImbue);
			}
		}
	}

	public class PiercingGaleCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<BronzeStaff>();

		public override int CooldownLength => 60;
	}
}
