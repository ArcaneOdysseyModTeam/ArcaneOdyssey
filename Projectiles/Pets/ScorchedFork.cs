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

		public override float Size => .3f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.netImportant = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 100;
			Projectile.width = 44;
			Projectile.height = 154;
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
			Lighting.AddLight(Projectile.Center,0,1,1);
			if (Owner.HasBuff<ForkPetBuff>())
				Projectile.timeLeft = 2;
			targetPosition = Owner.Center + new Vector2(Owner.direction * -50f, -30f);
			if (Projectile.Center.Distance(targetPosition) > 5f)
			{
				Projectile.velocity = Vector2.Zero;
				Projectile.Center = Projectile.Center.MoveTowards(targetPosition+new Vector2(0f,(10f * MathF.Sin(Main.GameUpdateCount / 100f))), Projectile.Distance(targetPosition+new Vector2(0f,(10f * MathF.Sin(Main.GameUpdateCount / 100f)))) / 40f);
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
				Projectile.Center = Projectile.Center.MoveTowards(targetPosition+new Vector2(0f,(10f * MathF.Sin(Main.GameUpdateCount / 100f))), Projectile.Distance(targetPosition+new Vector2(0f,(10f * MathF.Sin(Main.GameUpdateCount / 100f)))) / 40f);
				//Projectile.rotation = Projectile.rotation.AngleTowards(0f, (MathHelper.Pi / 6f) / 20f);
				//Projectile.spriteDirection = Owner.direction;
			}
		}


		public override bool? CanDamage() => false;

		public override SpriteEffects FlippedMode => SpriteEffects.FlipHorizontally;
	}
}
