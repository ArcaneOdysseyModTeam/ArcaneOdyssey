using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class ArrowStorm : AOPlayerProjectile
	{
		public override string Texture => AOUtils.BlankTexture;

		public override Texture2D Sprite => ThisProjectileType != 0 ? TextureAssets.Projectile[ThisProjectileType].Value : base.Sprite;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.TrailingMode[Type] = 2;
		}

		public int ThisProjectileType
		{
			get
			{
				return (int)Projectile.ai[0];
			}
			set
			{
				Projectile.ai[0] = value;
			}
		}

		public ref float InitialVelocity => ref Projectile.ai[1];

		public override bool? CanDamage() => false;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Opacity = .75f;
			Projectile.timeLeft = 45;
			Projectile.width = Projectile.height = 4;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 2;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
			{
				Vector2 drawPos = Projectile.oldPos[k] + (Projectile.Size / 2f) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(lightColor * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length));
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, new(0, 0, Sprite.Width, Sprite.Height / Main.projFrames[ThisProjectileType]), colour2, Projectile.oldRot[k], new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[ThisProjectileType]) / 2f, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}

		public override void AI()
		{
			if (goal == Vector2.Zero)
			{
				var offsetX = Main.MouseWorld.X;
				var offsetY = Main.screenPosition.Y;
				goal = new Vector2(offsetX, offsetY);
			}
			if (Projectile.owner == Main.myPlayer)
			{
				if (Projectile.Distance(goal) <= Projectile.velocity.Length())
				{
					Kill();
					return;
				}
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}

		public override void OnKill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				for (int i = -2; i < 3; i++)
				{
					var direction = (MathHelper.PiOver2 + (MathHelper.PiOver4 / 10f * i)).ToRotationVector2();
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction * (Projectile.velocity.Length() * .75f), ThisProjectileType, Projectile.damage / 3, Projectile.knockBack / 5f, Projectile.owner);
				}
			}
		}

		public Vector2 goal;
	}
}
