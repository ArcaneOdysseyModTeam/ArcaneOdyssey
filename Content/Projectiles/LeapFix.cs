using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Items.Magic;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class LeapFix : ModProjectile
	{
        public override void AI()
        {
            Main.player[Projectile.owner].direction = (int)Projectile.ai[0];
            Projectile.Kill();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
	}
}
