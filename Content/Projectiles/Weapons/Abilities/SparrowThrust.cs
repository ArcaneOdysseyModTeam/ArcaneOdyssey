using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class SparrowThrust : AOPlayerProjectile
	{
		public Color Colour => Imbue?.GetColour(Color.MediumPurple) ?? Color.MediumPurple;
		public static int MaxTime => 60;
		public static int TrueMaxTime => MaxTime + (100 * 60);
		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 150;
			Projectile.friendly = true;
			Projectile.timeLeft = TrueMaxTime;
			Projectile.extraUpdates = 100;
			Projectile.DamageType = DamageClass.Melee;
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
			}
			else
			{
				Projectile.velocity = Vector2.Zero;
				Projectile.Opacity = (Projectile.timeLeft - 1f) / (TrueMaxTime - MaxTime);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			var kmax = Imbue?.AOImbueSpeed ?? 1f;
			var realkmax = (10f * kmax).Round();
			for (int k = realkmax; k >= 0; k--)
			{
				Vector2 drawPos = Projectile.Center - (oldvelo * k * (7f * kmax.FlipFloat())) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(Colour * (1f - ((realkmax - k) / (float)realkmax)));
				var rotaitoneoffset = SpriteEffects.None;
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, null, colour2, Projectile.rotation, Sprite.Size() / 2, Projectile.scale - (.05f * k), rotaitoneoffset, 0);
			}
			return false;
		}
	}
}
