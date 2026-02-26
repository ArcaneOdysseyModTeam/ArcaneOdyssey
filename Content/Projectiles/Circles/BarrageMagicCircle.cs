using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Circles
{
	public class BarrageMagicCircle : BaseMagicCircle
	{
		public int ChargingProjectile;
		public float ProjectileSpread = 0;

		public override string Texture => AOUtils.GetTexture<BasicMagicCircle>();

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}

		public override float AOSize => .5f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 128;
			Projectile.tileCollide = false;
			Projectile.Opacity = .75f;
		}

		public override void AI()
		{
			var dir = Main.myPlayer == Projectile.owner ? Owner.RotatedRelativePoint(Owner.MountedCenter).DirectionTo(Main.MouseWorld) : Projectile.rotation.ToRotationVector2();
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
			}


			if (Projectile.position != Projectile.oldPosition && Main.myPlayer == Projectile.owner)
			{
				Projectile.netUpdate = true;
				Projectile.netSpam = 0;
			}


			if (Owner.channel && !MarkedForDeath)
			{
				Projectile.Opacity = 1f;
				Owner.heldProj = Projectile.whoAmI;
				Owner.itemAnimation = Owner.itemAnimationMax;
				Owner.itemTime = Owner.itemTimeMax;
				Owner.itemRotation = dir.ToRotation();
				if (Owner.direction != 1)
				{
					Owner.itemRotation += MathHelper.Pi;
				}
				Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
				Projectile.rotation = dir.ToRotation();
				Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + (dir * 20f);

				//dir += (Main.rand.NextFloat(-ProjectileSpread, ProjectileSpread).ToRotationVector2());
				dir = (dir.ToRotation() + Main.rand.NextFloat(-ProjectileSpread, ProjectileSpread)).ToRotationVector2();

				if (Main.myPlayer == Projectile.owner && Main.GameUpdateCount % Owner.itemAnimationMax == 0)
				{
					if (Owner.CheckMana(Owner.GetManaCost(Owner.PlayerItem()), true))
					{
						if (ChargingProjectile != 0)
						{
							AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, dir * 10f, ChargingProjectile, Projectile.damage, Projectile.knockBack, Projectile.owner, Imbue, SecondImbue, true);
						}
					}
					else
					{
						MarkedForDeath = true;
					}
				}
			}
			else
			{
				MarkedForDeath = true;
			}

			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
		}
	}
}
