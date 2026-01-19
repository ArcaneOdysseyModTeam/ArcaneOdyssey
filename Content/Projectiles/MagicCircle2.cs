using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ArcaneOdyssey.Content.Projectiles.Base;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class MagicCircle2 : AOPlayerProjectile
	{
		public override bool? CanDamage() => false;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.tileCollide = false;
		}

		public override void AI()
		{
			if (Projectile.position != Projectile.oldPosition)
				Projectile.netUpdate = true;

			if (Imbue is RelicImbue)
			{
				Imbue.LingeringEffects(Projectile);
			}

			Projectile.ai[0] += (Owner.channel || Main.mouseRight) && !Owner.dead && Imbue is not null ? 0 : 1;
			if (Projectile.ai[0] < 1)
			{
				AOPlayerOwner.chargingSpell = true;
				AOPlayerOwner.myCircle = Projectile;
				if (Projectile.ai[1] != 2)
				{
					Projectile.Center = Owner.MountedCenter;
				}
				else
				{
					Owner.itemAnimation = Owner.itemTime = 2;
					if (Main.myPlayer == Projectile.owner)
					{
						Owner.itemRotation = Owner.MountedCenter.DirectionTo(Vector2.Lerp(Projectile.Center, Main.MouseWorld, .5f)).ToRotation();
						if (Owner.direction != 1)
						{
							Owner.itemRotation += MathHelper.Pi;
						}
						if (Vector2.Distance(Main.MouseWorld, Owner.position) < 400)
						{
							Projectile.Center = Projectile.Center.MoveTowards(Main.MouseWorld, 10 * Imbue.AOScrollSpeed);
						}
						else
							Projectile.Center = Projectile.Center.MoveTowards(Owner.Center + Owner.Center.DirectionTo(Main.MouseWorld) * 400, 10 * Imbue.AOScrollSpeed);
					}
				}
			}

			if (Imbue is not null)
			{
				Projectile.rotation += MathHelper.Pi / 120f * Imbue.AOScrollSpeed;
				float tempLightColorR = 0f;
				float tempLightColorG = 0f;
				float tempLightColorB = 0f;
				if (Imbue.GetColour().R != 0f)
				{
					tempLightColorR = 3f / Imbue.GetColour().R;
				}
				if (Imbue.GetColour().G != 0f)
				{
					tempLightColorG = 3f / Imbue.GetColour().G;
				}
				if (Imbue.GetColour().B != 0f)
				{
					tempLightColorB = 3f / Imbue.GetColour().B;
				}

				Lighting.AddLight(Projectile.position, tempLightColorR, tempLightColorG, tempLightColorB);

				if (Projectile.localAI[0] > 5 && !Main.dedServ)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.scale * Projectile.width * Main.rand.NextFloat()), Projectile.position.Y + (Projectile.scale * Projectile.height * Main.rand.NextFloat())), 0, 0, DustID.SilverFlame, 8f * (Main.rand.NextFloat() - 0.5f), (8f * (Main.rand.NextFloat() - 0.5f)), 0, Imbue.GetColour(), 1f)];
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
			if (Imbue is AOMagic)
			{
				lightColor = Imbue.GetColour();
				return base.PreDraw(ref lightColor);
			}
			else
				return false;
		}

		public override void OnKill(int timeLeft)
		{
			if (AOPlayerOwner.myCircle is not null)
			{
				AOPlayerOwner.myCircle = null;
				Owner.channel = false;
			}
		}
	}
}
