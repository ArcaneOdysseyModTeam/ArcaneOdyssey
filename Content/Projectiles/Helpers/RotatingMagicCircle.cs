using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Helpers
{
	public class RotatingMagicCircle : BaseMagicCircle
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.tileCollide = false;
			playedsound = false;
		}

		public override void AI()
		{
			if (Projectile.position != Projectile.oldPosition && Main.myPlayer == Projectile.owner)
			{
				Projectile.netUpdate = true;
				Projectile.netSpam = 0;
			}

			MarkedForDeath |= !(Owner.channel || Main.mouseRight) || Owner.dead || Imbue is null;
			if (!MarkedForDeath)
			{
				AOPlayerOwner.HeavySkillActive = true;
				AOPlayerOwner.myCircle = Projectile;
				if (Projectile.ai[1] != 2)
				{
					Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter);
				}
				else
				{
					Owner.itemAnimation = Owner.PlayerItem().useAnimation;
					Owner.itemTime = Owner.PlayerItem().useTime;
					if (Main.myPlayer == Projectile.owner)
					{
						Owner.itemRotation = Owner.RotatedRelativePoint(Owner.MountedCenter).DirectionTo(Vector2.Lerp(Projectile.Center, Main.MouseWorld, .5f)).ToRotation();
						if (Owner.direction != 1)
						{
							Owner.itemRotation += MathHelper.Pi;
						}
						if (Vector2.Distance(Main.MouseWorld, Owner.position) < 400)
						{
							Projectile.Center = Projectile.Center.MoveTowards(Main.MouseWorld, 10 * Imbue.AOScrollSpeed);
						}
						else
							Projectile.Center = Projectile.Center.MoveTowards(Owner.Center + Owner.Center.DirectionTo(Main.MouseWorld) * 400, 10 * Imbue.AOScrollSpeed);
					}
				}
			}
			Projectile.rotation += MathHelper.Pi / 120f * Imbue.AOScrollSpeed;
		}

		public override void OnKill(int timeLeft)
		{
			if (AOPlayerOwner.myCircle is not null)
			{
				AOPlayerOwner.myCircle = null;
				Owner.channel = false;
			}
		}
	}
}
