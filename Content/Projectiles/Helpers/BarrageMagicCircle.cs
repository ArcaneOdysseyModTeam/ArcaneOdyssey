using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Helpers
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
				Owner.itemAnimation = Owner.PlayerItem().useAnimation;
				Owner.itemTime = Owner.PlayerItem().useTime;
				Owner.itemRotation = dir.ToRotation();
				if (Owner.direction != 1)
				{
					Owner.itemRotation += MathHelper.Pi;
				}
				Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
				Projectile.rotation = dir.ToRotation();
				Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + (dir * 20f);

				dir = (dir.ToRotation() + Main.rand.NextFloat(-ProjectileSpread, ProjectileSpread)).ToRotationVector2();

				if (Main.myPlayer == Projectile.owner && ChargingProjectile != 0 && Main.GameUpdateCount % MathHelper.Clamp(ApplyScrollSpeed(Owner.itemAnimationMax, true).Round(), 1, 500) == 0)
				{
					if (Owner.CheckMana(Owner.GetManaCost(Owner.PlayerItem()), true))
					{
						AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, dir * 10, ChargingProjectile, Projectile.damage, Projectile.knockBack, Projectile.owner, Imbue, SecondImbue, true);
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
