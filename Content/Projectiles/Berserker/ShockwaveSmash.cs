using ArcaneOdyssey.Content.Items.Base;
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

namespace ArcaneOdyssey.Content.Projectiles.Berserker
{
	public class ShockwaveSmash : StrengthTechnique
	{
		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 100;
			Projectile.usesLocalNPCImmunity = true;
            Projectile.friendly = true;
            Projectile.ownerHitCheck = true;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.localNPCHitCooldown = -1;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 6;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.netUpdate = true;
                Projectile.velocity.Normalize();
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.ai[0] = 1;
			}
            aoPlayerOwner ??= Main.player[Projectile.owner].ArcaneOdyssey();
            Projectile.Center.MoveTowards(Main.MouseWorld, 30);

            if (++Projectile.frameCounter > 1)
			{
				Projectile.frameCounter = 0;
                BaseScale = 1 + (Projectile.frame * .1f);
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Kill();
				}
            }

            if (Projectile.TryGetImbue(out Imbuable imbue) && imbue is FightingStyle fs)
            {
                fs.ExplosionEffects(Projectile);
            }
        }

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is not null)
			{
				Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), Imbue is null ? Color.White : Imbue.ImbueColour, Projectile.rotation, Projectile.GetDrawOriginCentre(), Projectile.scale, SpriteEffects.None);
				return false;
			}
			return true;
		}
	}
}
