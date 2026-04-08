using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Weapons.Bronze;
using ArcaneOdyssey.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public class ExistingProjectilesEditor : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

		public BeamSpell piercingShotBeam;

		public bool isPiercingShot = false;

		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			if (projectile.type == ProjectileID.BulletHighVelocity)
			{
				if (source is EntitySource_ItemUse_WithAmmo { Item: Item item, AmmoItemIdUsed: int bullet })
				{
					if (item.type == ModContent.ItemType<BronzeMusket>() && bullet != ItemID.HighVelocityBullet)
					{
						isPiercingShot = true;
						piercingShotBeam = Projectile.NewProjectileDirect(source, projectile.Center, projectile.velocity.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<BeamSpell>(), 0, 0, projectile.owner).ModProjectile as BeamSpell;
					}
				}
			}
		}

		public override void AI(Projectile projectile)
		{
			if (isPiercingShot)
			{
				piercingShotBeam.dying = true;
				piercingShotBeam.end = projectile.Center;
				piercingShotBeam.Projectile.timeLeft = BeamSpell.LingerTime + BeamSpell.TravelTime;
				piercingShotBeam.Projectile.position -= piercingShotBeam.Projectile.velocity;
				piercingShotBeam.Projectile.rotation = projectile.rotation;
			}
		}

		public override bool PreKill(Projectile projectile, int timeLeft)
		{
			if (isPiercingShot)
			{
				piercingShotBeam.Projectile.timeLeft = BeamSpell.LingerTime;
				piercingShotBeam.Projectile.rotation = projectile.rotation;
			}
			return base.PreKill(projectile, timeLeft);
		}
	}
}
