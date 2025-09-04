using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
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
	public class MagicCircle : AOPlayerProjectile
	{
		Random rand = new System.Random();
		public override void SetStaticDefaults()
		{
			Projectile.friendly = false;
			Projectile.hostile = false;
            Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults() {
			Projectile.height = 62;
			Projectile.width = 64;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
		}
		private int currentFrame1 = 0;
		private int currentLifeFrame = 0;
        public override void AI() {
            if (Projectile.frame+1 >= Main.projFrames[Projectile.type]) {
					Projectile.frame = 0;
            }
			if(currentFrame1>5){
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X+((Projectile.scale)*Projectile.width*(float)rand.NextDouble()),Projectile.position.Y+((Projectile.scale)*Projectile.height*(float)rand.NextDouble())),0,0,DustID.SilverFlame,(8f*(float)(rand.NextDouble()-0.5)),(8f*(float)(rand.NextDouble()-0.5)),0,default,1f)];
				spawnedDust.noGravity = true;
				currentFrame1 = 0;
			}
			Projectile.alpha+=255/60;
			if(currentLifeFrame>60) {
				Projectile.Kill();
			}
            Projectile.frame++;
			currentFrame1++;
			currentLifeFrame++;
        }
	}
}
