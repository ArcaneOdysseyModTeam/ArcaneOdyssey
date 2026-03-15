using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class TwinCrescent : AOPlayerProjectile
	{
		public override string Texture => AOUtils.SlashTexture;
		public override float AOSize => .25f;
		public Color Colour => Imbue?.GetColour() ?? Color.Gold;

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

		public override void OnKill(int timeLeft)
		{
			if (!Main.dedServ && Imbue is null)
			{
				for (float i = 0; i < 10; i++)
				{
					var centre = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2();
					var dust = AOUtils.NewDustImperfect(centre + Projectile.Center, DustID.BubbleBurst_White, centre * (Projectile.width / 10f), 0, Colour, 1.5f);
					dust.noLight = true;
					dust.noGravity = true;
				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 6;
			height /= 6;
			fallThrough = true;
			return true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
			{
				Vector2 drawPos = Projectile.oldPos[k] + (Projectile.Size / 2f) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(Colour * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length));
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, null, colour2, Projectile.rotation, Sprite.Size() / 2, Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}
