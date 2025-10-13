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
using ArcaneOdyssey.Content.Items.FightingStyles;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class MagicCircle1 : AOPlayerProjectile
	{
		public Texture2D MagicCircleSprite => ModContent.Request<Texture2D>(Texture).Value;

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
			Projectile.alpha = 0;
		}

		public bool MarkedForDeath = false;

		public override void AI()
		{
			aoPlayerOwner ??= Main.player[Projectile.owner].ArcaneOdyssey();
			var dir = aoPlayerOwner.Player.MountedCenter.DirectionTo(Main.MouseWorld);
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
				if (aoPlayerOwner.Player.channel)
				{
					charge = .75f;
				}
				aoPlayerOwner.Player.direction = (dir.X > 0f).ToDirectionInt();
			}

			if (Projectile.position != Projectile.oldPosition || Projectile.rotation != Projectile.oldRot[0])
			{
				Projectile.netUpdate = true;
			}

			if (aoPlayerOwner.Player.channel && !MarkedForDeath)
			{
				if (Projectile.ai[2] != 0)
				{
					aoPlayerOwner.chargingSpell = true;
					aoPlayerOwner.Player.itemAnimation = aoPlayerOwner.Player.itemTime = 2;
					aoPlayerOwner.Player.itemRotation = dir.ToRotation();
					if (aoPlayerOwner.Player.direction != 1)
					{
						aoPlayerOwner.Player.itemRotation += MathHelper.Pi;
					}
					if (Main.myPlayer == Projectile.owner)
						charge += 1f / 60f;
				}
				Projectile.ai[2] = 1;
				aoPlayerOwner.Player.ChangeDir((dir.X > 0f).ToDirectionInt());
				Projectile.rotation = dir.ToRotation();
				Projectile.Center = aoPlayerOwner.Player.MountedCenter + (dir * 30f);
				if (charge >= 3f)
				{
					aoPlayerOwner.Player.channel = false;
					MarkedForDeath = true;
				}
			}
			else
			{
				Projectile.alpha += (255f / 60f).Round();
				MarkedForDeath = true;
				if (Projectile.ai[1] == 0 && Main.myPlayer == Projectile.owner && ChargingProjectile != 0)
				{
					var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center - (dir * 30f), dir * 10 * this.Imbue.AOScrollSpeed, ChargingProjectile, (int)Math.Round(Projectile.damage * (charge * charge)), 4.5f * this.Imbue.AOScrollSize * (this.Imbue is WindMagic or Boxing ? 3f : 1f) * charge, Projectile.owner);
					proj.ArcaneOdyssey().BaseScale = charge/2;
					proj.netUpdate = true;
					Projectile.ai[1] = 1;
				}
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
				Main.EntitySpriteDraw(MagicCircleSprite, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), drawColor, Projectile.rotation, Projectile.GetDrawOriginCentre(), Imbue.AOScrollSize * Projectile.scale / 2, SpriteEffects.None, 0);
				return false;
			}
			return true;
		}
	}
}
