using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class MagicCircle : ModProjectile
	{
		
		public override void SetStaticDefaults()
		{
            Main.projFrames[Projectile.type] = 4;
		}
        public override void AI() {
            if (++Projectile.frame >= Main.projFrames[Projectile.type]) {
					Projectile.frame = 0;
            }
            Projectile.frame++;
        }
	}
}
