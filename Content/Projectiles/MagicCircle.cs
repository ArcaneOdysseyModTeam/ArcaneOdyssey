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

namespace ArcaneOdyssey.Content.Projectiles
{
	public class MagicCircle : AOPlayerProjectile
	{
		public static Texture2D MagicCircleSprite => ModContent.Request<Texture2D>("ArcaneOdyssey/Content/Projectiles/MagicCircle").Value;

		public int ChargingProjectile;
		public float charge = 1f;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 128;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0f)
			{
				Projectile.ai[0] = 1f;
				Projectile.netUpdate = true; 
				if (Projectile.ai[1] == 1)
				{
					charge = .75f;
				}

			}
			aoPlayerOwner ??= Main.player[Projectile.owner].ArcaneOdyssey();


			Projectile.ai[2] += aoPlayerOwner.Player.channel && Projectile.ai[1] == 0 ? 0 : 1;


			var dir = aoPlayerOwner.Player.SafeDirectionTo(Main.MouseWorld);
			if (Projectile.ai[1] == 1 && Projectile.ai[2] == 0)
			{
				charge += 1 / 60;
				Projectile.rotation = dir.ToRotation();
				Projectile.position = dir * 30;
				if (charge >= 3)
				{
					Projectile.ai[2]++;
					aoPlayerOwner.Player.channel = false;
				}
			}
			else
			{
				Projectile.alpha += 255 / 60;
			}

			if (Projectile.TryGetImbue(out Imbuable Imbue) && !Main.dedServ)
			{
				float tempLightColorR = 0f;
				float tempLightColorG = 0f;
				float tempLightColorB = 0f;
				if (Imbue.ImbueColour.R != 0f)
				{
					tempLightColorR = 3f / Imbue.ImbueColour.R;
				}
				if (Imbue.ImbueColour.G != 0f)
				{
					tempLightColorG = 3f / Imbue.ImbueColour.G;
				}
				if (Imbue.ImbueColour.B != 0f)
				{
					tempLightColorB = 3f / Imbue.ImbueColour.B;
				}
				Lighting.AddLight(Projectile.position, tempLightColorR, tempLightColorG, tempLightColorB);
				if (Projectile.localAI[0] > 5)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.scale * Projectile.width * Main.rand.NextFloat()), Projectile.position.Y + (Projectile.scale * Projectile.height * Main.rand.NextFloat())), 0, 0, DustID.SilverFlame, 8f * (Main.rand.NextFloat() - 0.5f), (8f * (Main.rand.NextFloat() - 0.5f)), 0, Imbue.ImbueColour, 1f)];
					spawnedDust.noGravity = true;
					Projectile.localAI[0] = 0;
				}
			}

			if (Projectile.alpha >= 255)
			{
				Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.position, dir * 10 * this.Imbue.AOScrollSpeed, ChargingProjectile, (int)Math.Round(Projectile.damage * charge), 4.5f * this.Imbue.AOScrollSize * (this.Imbue is WindMagic ? 3f : 1f) * charge, Projectile.owner).scale = charge;
				Kill();
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
			if (Projectile.TryGetImbue(out Imbuable Imbue))
			{
				Color drawColor = Imbue.ImbueColour;
				drawColor *= 1f - (Projectile.alpha / 255f);
				Main.EntitySpriteDraw(MagicCircleSprite, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), drawColor, Projectile.rotation, new Vector2(Projectile.width/2, Projectile.width/2), Imbue.AOScrollSize * Projectile.scale / 2, SpriteEffects.None, 0);
				return false;
			}
			return true;
		}
	}
}
