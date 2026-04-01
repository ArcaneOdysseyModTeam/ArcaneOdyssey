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
	public class Iris : PlayerProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projPet[Type] = true;
			ProjectileID.Sets.LightPet[Type] = true;
		}

		private Vector2 targetPosition;

		public override void SetDefaults()
		{
			Projectile.scale = .75f;
			base.SetDefaults();
			Projectile.netImportant = true;
			Projectile.tileCollide = false;
			Projectile.width = 38;
			Projectile.height = 50;
		}

		public override void AI()
		{
			if (Owner.HasBuff<IrisBuff>())
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

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = ModContent.GetInstance<FlareMagic>().Colour;
			Main.EntitySpriteDraw(ArcaneOdysseyMod.MagicCircleSprite.Value, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), MathHelper.Lerp(0f, MathHelper.PiOver2, AOUtils.UpdateCount * 2), ArcaneOdysseyMod.MagicCircleSprite.Size() / 2f, Projectile.scale * .75f * (100 / 2000f), SpriteEffects.None);
			Lighting.AddLight(Projectile.Center, lightColor.ToVector3() * MathHelper.Lerp(2f, 4f, Math.Abs(MathF.Sin(AOUtils.UpdateCount))));
			lightColor = Color.White;
			return base.PreDraw(ref lightColor);
		}

		public override bool? CanDamage() => false;

		public override SpriteEffects FlippedMode => SpriteEffects.FlipHorizontally;
	}
}
