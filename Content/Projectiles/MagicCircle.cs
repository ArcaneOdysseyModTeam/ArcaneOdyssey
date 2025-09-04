using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
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

using static System.Net.Mime.MediaTypeNames;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class MagicCircle : AOPlayerProjectile
	{
		public static Texture2D MagicCircleSprite => ModContent.Request<Texture2D>("ArcaneOdyssey/Content/Projectiles/MagicCircle").Value;
		Random rand = new System.Random();
		public override void SetStaticDefaults()
		{
			Projectile.friendly = false;
			Projectile.hostile = false;
			Main.projFrames[Projectile.type] = 4;
		}
		public override void SetDefaults()
		{
			Projectile.height = 62;
			Projectile.width = 64;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
			Projectile.frameCounter = 0;
		}
		private int currentFrame1 = 0;
		private int currentLifeFrame = 0;
		public override void AI()
		{
			aoPlayerOwner ??= Main.player[Projectile.owner].AOPlayer();
			thisMagic ??= aoPlayerOwner.imbue;
			
			if (currentFrame1 > 5)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + ((Projectile.scale) * Projectile.width * (float)rand.NextDouble()), Projectile.position.Y + ((Projectile.scale) * Projectile.height * (float)rand.NextDouble())), 0, 0, DustID.SilverFlame, (8f * (float)(rand.NextDouble() - 0.5)), (8f * (float)(rand.NextDouble() - 0.5)), 0, thisMagic.MagicColour, 1f)];
				spawnedDust.noGravity = true;
				currentFrame1 = 0;
			}
			Projectile.alpha += 255 / 60;
			if (currentLifeFrame > 60)
			{
				Projectile.Kill();
			}
			if (Projectile.frameCounter > 5)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame + 1 >= Main.projFrames[Projectile.type])
				{
				Projectile.frame = 0;
				}
			 }
			currentFrame1++;
			Projectile.frameCounter++;
			currentLifeFrame++;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Color drawColor = thisMagic.MagicColour;
			drawColor *= 1f - (Projectile.alpha / 255f);
			Main.EntitySpriteDraw(MagicCircleSprite,Projectile.Center-Main.screenPosition,new Rectangle(0, 64*Projectile.frame,62,64),drawColor,Projectile.rotation,new Vector2(31f,32f),1f,SpriteEffects.None,0);
			return false;
		}
	}
}
