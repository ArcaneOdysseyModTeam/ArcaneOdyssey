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
	public class MagicCircle2 : AOPlayerProjectile
	{
		public static Texture2D MagicCircleSprite => ModContent.Request<Texture2D>($"{nameof(ArcaneOdyssey)}/Content/Projectiles/MagicCircle2").Value;

		public override void SetDefaults()
        {
            Projectile.height = Projectile.width = 64;
            Projectile.tileCollide = false;
		}

		public bool shouldBeAlive = true;

		public override void AI()
		{
			Projectile.rotation = (float)Math.PI * (FramesAlive / 120f);
            Player player = Main.player[Projectile.owner];
            aoPlayerOwner ??= player.AOPlayer();
			thisMagic ??= aoPlayerOwner.imbue;
			Projectile.ai[0] += player.channel && !player.dead && thisMagic is not null && shouldBeAlive ? 0 : 1;
			if (Projectile.ai[0] < 1)
			{
				aoPlayerOwner.myCircle = Projectile;
				if (Projectile.ai[1] == 2)
				{
					Projectile.position = player.MountedCenter;
					player.velocity = Vector2.Zero;
				}
				else
					Projectile.position = Main.MouseWorld;
			}
			if (thisMagic is not null)
			{
				float tempLightColorR = 0f;
				float tempLightColorG = 0f;
				float tempLightColorB = 0f;
				if (thisMagic.MagicColour.R != 0f)
				{
					tempLightColorR = 3f / thisMagic.MagicColour.R;
				}
				if (thisMagic.MagicColour.G != 0f)
				{
					tempLightColorG = 3f / thisMagic.MagicColour.G;
				}
				if (thisMagic.MagicColour.B != 0f)
				{
					tempLightColorB = 3f / thisMagic.MagicColour.B;
				}

				Lighting.AddLight(Projectile.position, tempLightColorR, tempLightColorG, tempLightColorB);

				if (Projectile.localAI[0] > 5 && !Main.dedServ)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.scale * Projectile.width * Main.rand.NextFloat()), Projectile.position.Y + (Projectile.scale * Projectile.height * Main.rand.NextFloat())), 0, 0, DustID.SilverFlame, 8f * (Main.rand.NextFloat() - 0.5f), (8f * (Main.rand.NextFloat() - 0.5f)), 0, thisMagic.MagicColour, 1f)];
					spawnedDust.noGravity = true;
					Projectile.localAI[0] = 0;
				}
				Projectile.localAI[0]++;

				if ((Projectile.ai[0] > 0) && !Main.dedServ)
				{
					shouldBeAlive = false;
					if (Projectile.alpha < 255)
					{
						Projectile.alpha += 255 / 60;
					}
					else Projectile.Kill();
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (thisMagic is not null)
			{
				Color drawColor = thisMagic.MagicColour;
				drawColor *= 1f - (Projectile.alpha / 255f);
				Main.EntitySpriteDraw(MagicCircleSprite, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, Projectile.width, Projectile.height), drawColor, Projectile.rotation, new Vector2(Projectile.height/2, Projectile.height / 2), thisMagic.AOMagicSize * Projectile.scale, SpriteEffects.None, 0);
			}
			return false;
		}

        public override void OnKill(int timeLeft)
        {
			Main.player[Projectile.owner].AOPlayer().myCircle = null;
			Main.player[Projectile.owner].channel = false;
        }
	}
}
