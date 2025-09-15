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

namespace ArcaneOdyssey.Content.Projectiles
{
	public class MagicCircle : AOPlayerProjectile
	{
		public static Texture2D MagicCircleSprite => ModContent.Request<Texture2D>("ArcaneOdyssey/Content/Projectiles/MagicCircle").Value;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.height = 64;
			Projectile.width = 64;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
			Projectile.frameCounter = 0;
			Projectile.friendly = false;
			Projectile.hostile = false;
		}

		public override void AI()
		{
			aoPlayerOwner ??= Main.player[Projectile.owner].AOPlayer();
			thisMagic ??= aoPlayerOwner.imbue;
			float tempLightColorR = 0f;
			float tempLightColorG = 0f;
			float tempLightColorB = 0f;
			if (!(thisMagic.MagicColour.R == 0f))
			{
				tempLightColorR = 3f / thisMagic.MagicColour.R;
			}
			if (!(thisMagic.MagicColour.G == 0f))
			{
				tempLightColorG = 3f / thisMagic.MagicColour.G;
			}
			if (!(thisMagic.MagicColour.B == 0f))
			{
				tempLightColorB = 3f / thisMagic.MagicColour.B;
			}
			Lighting.AddLight(Projectile.position,tempLightColorR,tempLightColorG,tempLightColorB);
			if (Projectile.localAI[0] > 5 && !Main.dedServ)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.scale * Projectile.width * Main.rand.NextFloat()), Projectile.position.Y + (Projectile.scale * Projectile.height * Main.rand.NextFloat())), 0, 0, DustID.SilverFlame, 8f * (Main.rand.NextFloat() - 0.5f), (8f * (Main.rand.NextFloat() - 0.5f)), 0, thisMagic.MagicColour, 1f)];
				spawnedDust.noGravity = true;
				Projectile.localAI[0] = 0;
			}
			Projectile.alpha += 255 / 60;
			if (FramesAlive > 60)
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
			Projectile.localAI[0]++;
			Projectile.frameCounter++;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Color drawColor = thisMagic.MagicColour;
			drawColor *= 1f - (Projectile.alpha / 255f);
			Main.EntitySpriteDraw(MagicCircleSprite, Projectile.Center-Main.screenPosition, new Rectangle(0, 64 * Projectile.frame, 64, 64), drawColor, Projectile.rotation, new Vector2(31f, 32f), thisMagic.AOMagicSize*Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}
