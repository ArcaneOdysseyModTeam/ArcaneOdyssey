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

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts
{
    public class DeliriumBlast : BlastSpell
    {
        public static Texture2D BlastSprite => ModContent.Request<Texture2D>($"{nameof(ArcaneOdyssey)}/Content/Projectiles/Magic/Blasts/{nameof(DeliriumBlast)}").Value;

        public float? timeLeftDefault = null;
        public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            timeLeftDefault ??= Projectile.timeLeft;
            Main.EntitySpriteDraw(BlastSprite, Projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Black, Color.White, (float)(FramesAlive / timeLeftDefault.Value)), Projectile.rotation, Projectile.GetDrawOriginCentre(), Projectile.scale, SpriteEffects.None);
            return false;
        }
    }
}
