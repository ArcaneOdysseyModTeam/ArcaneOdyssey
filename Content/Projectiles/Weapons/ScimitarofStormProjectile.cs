using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class ScimitarofStormProjectile : AOPlayerProjectile
	{
		public override bool? CanDamage() => false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.TrailingMode[Type] = 4;
		}

		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
			Projectile.NewProjectile(Projectile.GetSource_FromThis(), Owner.itemLocation, 9f * Owner.MountedCenter.DirectionTo(Main.MouseWorld), ModContent.ProjectileType<TwinCrescent>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.friendly = true;
			Projectile.height = 234;
			Projectile.width = 74;
			Projectile.AverageDimensions();
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
			Projectile.tileCollide = false;
		}

		internal bool started = false;

		public override void AI()
		{
			if (!started)
			{
				if (Owner.itemTime > Owner.itemAnimation / 2)
				{
					Projectile.Center = Owner.MountedCenter + (Vector2.UnitY * -(MathF.Sqrt((Projectile.height ^ 2) + (Projectile.width ^ 2)) / 3f));
					Projectile.rotation = 0.95493f;
				}
				else
				{
					Projectile.Center = Owner.MountedCenter + (Vector2.UnitY * (MathF.Sqrt((Projectile.height ^ 2) + (Projectile.width ^ 2)) / 3f));
					Projectile.rotation = 0.95493f + MathHelper.Pi;
				}
			}

			if (Owner.itemTime > Owner.itemAnimation / 2)
			{
				Owner.ChangeDir(1);
				Projectile.rotation += MathHelper.Pi / 30;
				Projectile.Center = Owner.MountedCenter + (Projectile.rotation.ToRotationVector2() * -(MathF.Sqrt((Projectile.height ^ 2) + (Projectile.width ^ 2)) / 3f));
			}
			else
			{

				Owner.ChangeDir(-1);
				Projectile.rotation -= MathHelper.Pi / 30; 
				Projectile.Center = Owner.MountedCenter + (Projectile.rotation.ToRotationVector2() * (MathF.Sqrt((Projectile.height ^ 2) + (Projectile.width ^ 2)) / 3f));
			}
		}

		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

		public override bool PreDraw(ref Color lightColor)
		{
			SpriteEffects effects = Owner.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			//for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
			//{
			//	Vector2 drawPos = Projectile.oldPos[k] + (Projectile.Size / 2f) + new Vector2(0f, Projectile.gfxOffY);
			//	var colour2 = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
			//	Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, null, colour2, Projectile.oldRot[k], Sprite.Size() / 2, Projectile.scale, effects, 0);
			//}
			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, Sprite.Size() / 2, Projectile.scale, effects, 0);
			return false;
		}
	}
}
