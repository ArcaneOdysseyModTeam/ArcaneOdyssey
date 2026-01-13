using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using ArcaneOdyssey.Content.Items.Base;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class MagicCircle1 : AOPlayerProjectile
	{
		public int ChargingProjectile;
		public float charge = 1f;

		public override bool? CanDamage() => false;

		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 4;
		}

		public override float AOSize => .15f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 128;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
		}

		internal bool MarkedForDeath = false;

		public override void AI()
		{
			var dir = Main.myPlayer == Projectile.owner ? Owner.MountedCenter.DirectionTo(Main.MouseWorld) : Projectile.rotation.ToRotationVector2();
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
				if (Owner.channel)
				{
					charge = .75f;
				}
				Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
			}
			
			if (Imbue is RelicImbue)
			{
				Imbue.LingeringEffects(Projectile);
			}

			if (Projectile.position != Projectile.oldPosition)
			{
				Projectile.netUpdate = true;
			}

			if (Owner.channel && !MarkedForDeath)
			{
				if (Projectile.ai[2] != 0)
				{
					AOPlayerOwner.chargingSpell = true;
					Owner.heldProj = Projectile.whoAmI;
					Owner.itemAnimation = Owner.itemTime = 2;
					Owner.itemRotation = dir.ToRotation();
					if (Owner.direction != 1)
					{
						Owner.itemRotation += MathHelper.Pi;
					}
					if (Main.myPlayer == Projectile.owner)
						charge += 1f / 60f;
				}
				Projectile.ai[2] = 1;
				Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
				Projectile.rotation = dir.ToRotation();
				Projectile.Center = Owner.MountedCenter + (dir * 20f);
				if (charge >= 3f)
				{
					Owner.channel = false;
					MarkedForDeath = true;
				}
			}
			else
			{
				Projectile.alpha += (255f / 60f).Round();
				MarkedForDeath = true;
				if (Projectile.ai[1] == 0 && Main.myPlayer == Projectile.owner && ChargingProjectile != 0)
				{
					var proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, dir * 10 * Imbue.AOScrollSpeed, ChargingProjectile, Projectile.damage, Projectile.knockBack * charge, Projectile.owner);
					if (proj.ModProjectile is BlastSpell or BeamSpell)
					{
						proj.ArcaneOdyssey().BaseScale = charge / 2;
						proj.damage = (Projectile.damage * (charge * charge)).Round();
					}
					proj.netUpdate = true;
					Projectile.ai[1] = 1;
				}
			}

			if (Imbue is not null && !Main.dedServ)
			{
				float tempLightColorR = 0f;
				float tempLightColorG = 0f;
				float tempLightColorB = 0f;
				if (Imbue.GetColor().R != 0f)
				{
					tempLightColorR = 3f / Imbue.GetColor().R;
				}
				if (Imbue.GetColor().G != 0f)
				{
					tempLightColorG = 3f / Imbue.GetColor().G;
				}
				if (Imbue.GetColor().B != 0f)
				{
					tempLightColorB = 3f / Imbue.GetColor().B;
				}
				Lighting.AddLight(Projectile.position, tempLightColorR, tempLightColorG, tempLightColorB);
				if (Projectile.localAI[0]++ > 5)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.scale * Projectile.width * Main.rand.NextFloat()), Projectile.position.Y + (Projectile.scale * Projectile.height * Main.rand.NextFloat())), 0, 0, DustID.SilverFlame, 8f * (Main.rand.NextFloat() - 0.5f), (8f * (Main.rand.NextFloat() - 0.5f)), 0, Imbue.GetColor(), 1f)];
					spawnedDust.noGravity = true;
					Projectile.localAI[0] = 0;
				}
			}

			if (Projectile.alpha >= 255)
			{
				Kill();
			}

			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
				if (Projectile.frame + 1 >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is AOMagic)
			{
				lightColor = Imbue.GetColor();
				return base.PreDraw(ref lightColor);
			}
			else
				return false;
		}
	}
}
