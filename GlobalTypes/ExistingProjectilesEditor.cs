using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Weapons.Bronze;
using ArcaneOdyssey.Projectiles.Abilities;
using ArcaneOdyssey.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.CameraModifiers;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.GlobalTypes
{
	public partial class AOProjectile : GlobalProjectile, IImbuable
	{
		public BeamSpell piercingShotBeam;

		public bool isPiercingShot = false;
		public bool isStormOfArrows;

		public void Spawn(Projectile projectile, IEntitySource source)
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

			if (source is EntitySource_Parent { Entity: Projectile storm} && storm.type == ModContent.ProjectileType<ArrowStorm>())
			{
				isStormOfArrows = true;
			}
		}

		public void Update(Projectile projectile)
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

		public void Death(Projectile projectile, int timeLeft)
		{
			if (isPiercingShot)
			{
				piercingShotBeam.Projectile.timeLeft = BeamSpell.LingerTime;
				piercingShotBeam.Projectile.rotation = projectile.rotation;
			}

			if (isStormOfArrows && !Main.dedServ)
			{
				PunchCameraModifier modifier = new(projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), ApplyKnockback(3f), ApplyKnockback(1f), 4, ApplyKnockback(500f), FullName);
				Main.instance.CameraModifiers.Add(modifier);
			}
		}
	}
}
