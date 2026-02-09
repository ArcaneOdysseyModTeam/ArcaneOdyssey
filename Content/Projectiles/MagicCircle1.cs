using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

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

		public override float AOSize => .5f;

		public override void SetDefaults()
		{
			Projectile.scale = AOSize;
			Projectile.height = Projectile.width = 128;
			Projectile.tileCollide = false;
			Projectile.Opacity = .75f;
		}

		internal bool MarkedForDeath = false;
		internal bool originallyAltFire = false;

		public override void AI()
		{
			Projectile.scale = AOSize * charge * Imbue.AOScrollSize;
			var dir = Main.myPlayer == Projectile.owner ? Owner.RotatedRelativePoint(Owner.MountedCenter).DirectionTo(Main.MouseWorld) : Projectile.rotation.ToRotationVector2();
			if (Projectile.ai[0] == 0)
			{
				SoundEngine.PlaySound(SoundID.Item84 with { Pitch = Imbue.AOScrollSpeed.MultiToPercent().Clamp(-1, 1) }, Projectile.Center);
				Projectile.ai[0] = 1;
				Projectile.netUpdate = true;
				Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
			}
		
			SecondImbue?.LingeringEffects(Projectile.Hitbox);


			if (Projectile.position != Projectile.oldPosition)
			{
				Projectile.netUpdate = true;
			}

			if (Owner.channel && !MarkedForDeath)
			{
				Projectile.Opacity = .75f * charge;
				AOPlayerOwner.HeavySkillActive = true;
				Owner.heldProj = Projectile.whoAmI;
				Owner.itemAnimation = Owner.PlayerItem().useAnimation;
				Owner.itemTime = Owner.PlayerItem().useTime;
				Owner.itemRotation = dir.ToRotation();
				if (Owner.direction != 1)
				{
					Owner.itemRotation += MathHelper.Pi;
				}
				if (Main.myPlayer == Projectile.owner)
					charge += 1f / 120f;
				Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
				Projectile.rotation = dir.ToRotation();
				Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + (dir * 20f);
				if (charge >= 1.5f)
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
					var proj = AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, dir * 10, ChargingProjectile, (Projectile.damage * charge).Round(), Projectile.knockBack * charge, Projectile.owner, Imbue, SecondImbue, true);
					if (proj.ModProjectile is PulsarSpell && originallyAltFire)
					{
						proj.ai[1] = 1;
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
				if (Projectile.localAI[0]++ > 5)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.scale * Projectile.width * Main.rand.NextFloat()), Projectile.position.Y + (Projectile.scale * Projectile.height * Main.rand.NextFloat())), 0, 0, DustID.SilverFlame, 8f * (Main.rand.NextFloat() - 0.5f), (8f * (Main.rand.NextFloat() - 0.5f)), 0, Imbue.GetColour(), 1f)];
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
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Projectile.type])
				{
					Projectile.frame = 0;
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
	}
}
