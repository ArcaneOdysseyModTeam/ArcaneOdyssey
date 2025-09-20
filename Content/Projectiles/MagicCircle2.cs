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

namespace ArcaneOdyssey.Content.Projectiles
{
	public class MagicCircle2 : AOPlayerProjectile
	{
		public static Texture2D MagicCircleSprite => ModContent.Request<Texture2D>($"{nameof(ArcaneOdyssey)}/Content/Projectiles/{nameof(MagicCircle2)}").Value;

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 64;
			Projectile.tileCollide = false;
		}

		public bool shouldBeAlive = true;

		public override void AI()
		{
			if (Projectile.position != Projectile.oldPosition)
                Projectile.netUpdate = true;
            Projectile.rotation = MathHelper.Pi * (Projectile.ArcaneOdyssey().FramesAlive / 120f);
			Player player = Main.player[Projectile.owner];
			aoPlayerOwner ??= player.ArcaneOdyssey();
			if (Projectile.ai[2] == 0)
			{
				if (Projectile.TryGetImbue(out Imbuable imbue))
					Projectile.ai[2] = imbue.Type;
			}
			Imbue = (AOMagic)ModContent.GetModItem((int)Projectile.ai[2]);
			Projectile.ai[0] += (player.channel || Main.mouseRight) && !player.dead && Imbue is not null && shouldBeAlive ? 0 : 1;
			if (Projectile.ai[0] < 1)
			{
				aoPlayerOwner.myCircle = Projectile;
				if (Projectile.ai[1] == 2)
				{
					Projectile.Center = player.Center;
				}
				else
					Projectile.position = Main.MouseWorld - new Vector2(Projectile.width/2, Projectile.height/2);
			}
			else
				aoPlayerOwner.myCircle = null;

			if (Imbue is not null)
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

				if (Projectile.localAI[0] > 5 && !Main.dedServ)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.scale * Projectile.width * Main.rand.NextFloat()), Projectile.position.Y + (Projectile.scale * Projectile.height * Main.rand.NextFloat())), 0, 0, DustID.SilverFlame, 8f * (Main.rand.NextFloat() - 0.5f), (8f * (Main.rand.NextFloat() - 0.5f)), 0, Imbue.ImbueColour, 1f)];
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
			if (Imbue is not null)
			{
				Color drawColor = Imbue.ImbueColour;
				drawColor *= 1f - (Projectile.alpha / 255f);
				Main.EntitySpriteDraw(MagicCircleSprite, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, Projectile.width, Projectile.height), drawColor, Projectile.rotation, new Vector2(Projectile.height/2, Projectile.height / 2), Imbue.AOScrollSize * Projectile.scale, SpriteEffects.None);
			}
			return false;
		}

		public override void OnKill(int timeLeft)
		{
			Main.player[Projectile.owner].ArcaneOdyssey().myCircle = null;
			Main.player[Projectile.owner].channel = false;
		}
	}
}
