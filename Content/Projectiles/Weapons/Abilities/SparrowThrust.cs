using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class SparrowThrust : AOPlayerProjectile
	{
		public Color Colour => Imbue?.GetColour(Color.MediumPurple) ?? Color.MediumPurple;
		public static int MaxTime => 60;
		public static int TrueMaxTime => MaxTime + (100 * 60);

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 10;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 186;
			Projectile.friendly = true;
			Projectile.timeLeft = TrueMaxTime;
			Projectile.extraUpdates = 100;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.ownerHitCheck = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
		}

		private Vector2 oldvelo;

		public override void AI()
		{
			if (Projectile.timeLeft > (TrueMaxTime - MaxTime))
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
				oldvelo = Projectile.velocity;
				Imbue?.LingeringEffects(AOUtils.ScaleRectangleNotRef(Projectile.Hitbox, 1f - (.75f * ((Projectile.timeLeft - (TrueMaxTime - MaxTime)) / (float)MaxTime))), Projectile.velocity, Projectile);
			}
			else
			{
				if (++Projectile.frameCounter > ((TrueMaxTime - MaxTime) / 10f))
				{
					Projectile.frameCounter = 0;
					if (++Projectile.frame > Main.projFrames[Type])
					{
						Projectile.frame = 0;
					}
				}
				Projectile.velocity = Vector2.Zero;
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var realkmax = 9;
			for (int k = realkmax; k >= 0; k--) 
			{
				Vector2 drawPos = Projectile.Center - (oldvelo * k * 7f) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(Colour * (1f - ((realkmax - k) / (float)realkmax)));
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, new(0, (Sprite.Height / Main.projFrames[Type]) * Projectile.frame, Sprite.Width, Sprite.Height / Main.projFrames[Type]), colour2, Projectile.rotation, new Vector2(Sprite.Width, Sprite.Height / Main.projFrames[Type]) / 2f, Projectile.scale - ((Projectile.scale * .075f) * k), SpriteEffects.None, 0);
			}
			return false;
		}
	}
}
