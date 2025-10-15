using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using static System.Net.Mime.MediaTypeNames;

namespace ArcaneOdyssey.Content.Projectiles.Enemies
{
	public class EvanderSlash : ModProjectile
	{
		//public override float AOSpeed => .65f;
		//public override float AOSize => 1.2f;
		//public override float AODamage => 1.15f;
		//public override SoundStyle? DebuffApplySound => SoundID.NPCHit42;

		//public AOWeaponTiers AOWeaponTier = AOWeaponTiers.Good;

		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.damage = 25;
			Projectile.timeLeft = 60*3;
			Projectile.hostile = true;
			Projectile.height = Projectile.width = (234 + 74)/2; // average
			Projectile.knockBack = 4.5f;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}

		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new(Sprite.Width / 2f, Sprite.Height / 2f);
            for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;// + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(Sprite, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return false;
        }

		public override void AI()
		{
			if (Projectile.ai[0] == 0 || Projectile.timeLeft < 30)
			{
				Projectile.alpha += 255 / 30;
				Projectile.ai[0] = 1;
			}
			else
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
			}

			if (Projectile.localAI[0] > 60 && !Main.dedServ)
			{
				Projectile.localAI[0] = 0;
				SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.position);
			}
			Projectile.localAI[0]++;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			height = width = 1;
			fallThrough = true;
			return true;
		}

		public override bool? CanDamage()
		{
			return Projectile.ai[0] == 0;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = Vector2.Zero;
			Projectile.timeLeft = 30;
			Projectile.ai[0] = 1;
			return false;
		}
	}
}
