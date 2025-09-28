using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts
{
	public class EarthBlast : BlastSpell
	{
		public override void AI()
		{
			if (Projectile.frameCounter > 5)
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
                if (Projectile.frame + 1 >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }
			Projectile.frameCounter++;
			if (Projectile.ai[0] == 0f)
			{
				Projectile.ai[0] = 1f;
				Projectile.netUpdate = true;
			}
			aoPlayerOwner ??= Main.player[Projectile.owner].ArcaneOdyssey();
			if (Projectile.velocity.X > 0) {
				Projectile.rotation += 0.1f;
			} else {
				Projectile.rotation -= 0.1f;
			}
			if (Projectile.TryGetImbue(out Imbuable imbue) && !imbue.CanBeWet && Projectile.wet)
			{
				Kill();
				return;
			}
		}
	}
}
