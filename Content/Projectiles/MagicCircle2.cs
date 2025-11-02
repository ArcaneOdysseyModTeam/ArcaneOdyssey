using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Projectiles.Base;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class MagicCircle2 : AOPlayerProjectile
	{
		public Texture2D MagicCircleSprite => ModContent.Request<Texture2D>(Texture).Value;

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 64;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (Projectile.position != Projectile.oldPosition)
				Projectile.netUpdate = true;
			Player player = Main.player[Projectile.owner];
			aoPlayerOwner ??= player.ArcaneOdyssey();
			if (Projectile.ai[2] == 0 && aoPlayerOwner.Imbue is not null)
			{
				Projectile.ai[2] = aoPlayerOwner.Imbue.Type;
			}
			Imbue = (Imbuable)ModContent.GetModItem((int)Projectile.ai[2]);
			Projectile.ai[0] += (player.channel || Main.mouseRight) && !player.dead && Imbue is not null ? 0 : 1;
			if (Projectile.ai[0] < 1)
			{
				aoPlayerOwner.chargingSpell = true;
				aoPlayerOwner.myCircle = Projectile;
				if (Projectile.ai[1] != 2)
				{
					Projectile.Center = player.MountedCenter;
				}
				else
				{
					player.itemAnimation = player.itemTime = 2;
					if (Main.myPlayer == Projectile.owner)
					{
						player.itemRotation = player.MountedCenter.DirectionTo(Vector2.Lerp(Projectile.Center, Main.MouseWorld, .5f)).ToRotation();
						if (player.direction != 1)
						{
							player.itemRotation += MathHelper.Pi;
						}
						if (Vector2.Distance(Main.MouseWorld, player.position) < 400)
						{
							Projectile.Center = Projectile.Center.MoveTowards(Main.MouseWorld, 10 * Imbue.AOScrollSpeed);
						}
						else
							Projectile.Center = Projectile.Center.MoveTowards(player.Center + player.Center.DirectionTo(Main.MouseWorld) * 400, 10 * Imbue.AOScrollSpeed);
					}
				}
			}
			else
			{
				aoPlayerOwner.myCircle = null;
				aoPlayerOwner.chargingSpell = false;
			}

			if (Imbue is not null)
			{
				Projectile.rotation += MathHelper.Pi / 120f * Imbue.AOScrollSpeed;
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
				lightColor = Imbue.ImbueColour;
				lightColor *= 1f - (Projectile.alpha / 255f);
			}
			return true;
		}

		public override void OnKill(int timeLeft)
		{
			Main.player[Projectile.owner].ArcaneOdyssey().myCircle = null;
			Main.player[Projectile.owner].channel = false;
		}
	}
}
