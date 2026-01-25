using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class TwinCrescent : AOPlayerProjectile
	{
		public override string Texture => Mod.Name + "/Assets/BasicSlash";
		public override float AOSize => .15f;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.friendly = true;
			Projectile.height = 234;
			Projectile.width = 74;
			Projectile.AverageDimensions();
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 90;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 6;
			height /= 6;
			fallThrough = true;
			return true;
		}
		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

		public override bool PreDraw(ref Color lightColor)
		{
			for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
			{
				Vector2 drawPos = Projectile.oldPos[k] + (Projectile.Size / 2f) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(Imbue?.GetColour() ?? Color.Gold) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, null, colour2, Projectile.rotation, Sprite.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}
