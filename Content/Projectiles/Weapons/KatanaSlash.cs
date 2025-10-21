using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class KatanaSlash : AOPlayerProjectile
	{
		public Color color = default;

		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = 5;
			Projectile.width = 68;
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
				Projectile.Center = Projectile.Center.MoveTowards(Main.MouseWorld, 300);
				Projectile.rotation = MathHelper.Pi*4 / Main.rand.Next(50);
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

		public override void PostDraw(Color lightColor)
		{
			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), Color.Lerp(Color.White, color, .5f), Projectile.rotation, Projectile.GetDrawOriginCentre(), Projectile.scale * .95f, SpriteEffects.None);
			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color, Projectile.rotation, Projectile.GetDrawOriginCentre(), Projectile.scale * .90f, SpriteEffects.None);
		}
	}
}
