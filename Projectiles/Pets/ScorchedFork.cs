using ArcaneOdyssey.Buffs.Pets;
using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Pets
{
	public class ScorchedFork : PlayerProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projPet[Type] = true;
			ProjectileID.Sets.LightPet[Type] = true;
		}

		private Vector2 targetPosition;

		public override float Size => .5f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.netImportant = true;
			Projectile.tileCollide = false;
			Projectile.width = 30;
			Projectile.height = 102;
		}

		public override bool PreAI()
		{
			if (Owner is not null && !Owner.DeadOrGhost)
			{
				return true;
			}
			Kill();
			return false;
		}

		public override void AI()
		{
			if (Owner.HasBuff<ForkPetBuff>())
				Projectile.timeLeft = 2;
			targetPosition = Owner.Center + new Vector2(Owner.direction * 50f, -17f);
			if (Projectile.Center.Distance(targetPosition) > 5f)
			{
				Projectile.velocity = Vector2.Zero;
				Projectile.Center = Projectile.Center.MoveTowards(targetPosition, Projectile.Distance(targetPosition) / 40f);
				var velocity = Projectile.SafeDirectionTo(targetPosition); // fake velocity
				Projectile.spriteDirection = (velocity.X < 0).ToDirectionInt();
				if (Projectile.Center.Distance(targetPosition) > 120f)
				{
					if (Projectile.spriteDirection == -1)
					{
						if (Projectile.rotation < (MathHelper.Pi / 6f))
						{
							Projectile.rotation += (MathHelper.Pi / 6f) / 20f;
						}
						if (Projectile.rotation > (MathHelper.Pi / 6f))
						{
							Projectile.rotation -= (MathHelper.Pi / 6f) / 20f;
						}
					}
					else
					{
						if (Projectile.rotation > (-(MathHelper.Pi / 6f)))
						{
							Projectile.rotation -= (MathHelper.Pi / 6f) / 20f;
						}
						if (Projectile.rotation < (-(MathHelper.Pi / 6f)))
						{
							Projectile.rotation += (MathHelper.Pi / 6f) / 20f;
						}
					}
				}
				else
				{
					if (Projectile.spriteDirection == -1)
					{
						if (Projectile.rotation < (MathHelper.Pi / 12f))
						{
							Projectile.rotation += (MathHelper.Pi / 12f) / 20f;
						}
						if (Projectile.rotation > (MathHelper.Pi / 12f))
						{
							Projectile.rotation -= (MathHelper.Pi / 12f) / 20f;
						}
					}
					else
					{
						if (Projectile.rotation > (-(MathHelper.Pi / 12f)))
						{
							Projectile.rotation -= (MathHelper.Pi / 12f) / 20f;
						}
						if (Projectile.rotation < (-(MathHelper.Pi / 12f)))
						{
							Projectile.rotation += (MathHelper.Pi / 12f) / 20f;
						}
					}
				}
				if (Projectile.Distance(Owner.position) > (Main.maxScreenW * .55f))
				{
					Projectile.Center = targetPosition;
				}
			}
			else
			{
				Projectile.rotation = Projectile.rotation.AngleTowards(0f, (MathHelper.Pi / 6f) / 20f);
				Projectile.spriteDirection = Owner.direction * -1;
			}
		}


		public override bool? CanDamage() => false;

		public override SpriteEffects FlippedMode => SpriteEffects.FlipHorizontally;
	}
}
