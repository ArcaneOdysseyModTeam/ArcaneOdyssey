using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class ArrowRain : AOPlayerProjectile
	{
		public override string Texture => AOUtils.BlankTexture;

		public override Texture2D Sprite => ThisProjectileType != 0 ? TextureAssets.Projectile[ThisProjectileType].Value : base.Sprite;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.TrailingMode[Type] = 0;
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
			Projectile.timeLeft = 60;
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
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, null, colour2, Projectile.rotation, Sprite.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}

		public override void AI()
		{
			if (!AOUtils.ScreenRect.Intersects(Projectile.Hitbox))
			{
				Kill();
				return;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}

		public override void OnKill(int timeLeft)
		{
			if (Projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.UnitY * Projectile.velocity.Length(), ThisProjectileType, Projectile.damage, Projectile.knockBack, Projectile.owner);
			}
		}
	}
}
