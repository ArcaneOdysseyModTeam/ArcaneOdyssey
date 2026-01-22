using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class KatanaSlash : AOPlayerProjectile
	{
		public Color Colour => Imbue?.GetColour(Color.Red) ?? Color.Red;

		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 68;
			Projectile.friendly = true;
			Projectile.stopsDealingDamageAfterPenetrateHits = true;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
			Projectile.ownerHitCheck = true;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 14;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.netUpdate = true;
				var distance = 300f;
				if (Imbue is not null)
					distance *= Imbue.AOImbueSpeed;
				Projectile.Center = Projectile.Center.MoveTowards(Main.MouseWorld, distance);
				Projectile.rotation = MathHelper.TwoPi / Main.rand.NextFloat();
				Projectile.ai[0] = 1;
			}

			if (++Projectile.frameCounter > 1)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Kill();
				}
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.Center = target.Center;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Colour;
			return base.PreDraw(ref lightColor);
		}
	}
}
