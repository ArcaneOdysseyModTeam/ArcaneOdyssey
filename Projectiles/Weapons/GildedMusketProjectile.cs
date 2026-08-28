using ArcaneOdyssey.Items.Weapons;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Berserker;
using System;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.GameContent;

namespace ArcaneOdyssey.Projectiles.Weapons
{
	public class GildedMusketProjectile : PlayerProjectile
	{
		public static int AfterimagesType => ModContent.ProjectileType<Crescendo>();
		public static Texture2D Afterimages => TextureAssets.Projectile[AfterimagesType].Value;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.AllowsContactDamageFromJellyfish[Type] = true;
			ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[Type] = true;
			ProjectileID.Sets.TrailingMode[Type] = 2;
			ProjectileID.Sets.TrailCacheLength[Type] = 10;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			var size = new Vector2(118, 124);
			Projectile.width = Projectile.height = (int)size.Length();
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.ownerHitCheck = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.DamageType = DamageClass.Generic;
		}

		public ref float RotationVelocity => ref Projectile.ai[1];
		private float savedRot;
		private bool thrusting;

		public override void AI()
		{
			Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter);
			Owner.heldProj = Projectile.whoAmI;
			Projectile.direction = Owner.direction;
			Projectile.spriteDirection = Projectile.direction;

			Owner.itemRotation = Projectile.rotation - (MathHelper.PiOver4 * Projectile.spriteDirection);
			if (Owner.direction != 1)
			{
				Owner.itemRotation += MathHelper.Pi;
			}

			if (Projectile.ai[0] == 0)
			{
				Projectile.rotation = Projectile.velocity.ToRotation() + (MathHelper.PiOver4 * Projectile.direction);
				savedRot = Projectile.rotation;
				NetUpdate();
				Projectile.ai[0]++;
				Projectile.velocity.Normalize();
			}

			if (Projectile.ai[2] == 0) // swinging
			{
				if (Projectile.ai[0] == 1)
				{
					Owner.GetModPlayer<GildedPlayer>().swingCount++;
				}
				if (Owner.GetModPlayer<GildedPlayer>().swingCount > 3)
					Owner.GetModPlayer<GildedPlayer>().swingCount = 1;
				if (Owner.GetModPlayer<GildedPlayer>().swingCount == 1)
				{
					if (Projectile.ai[0] == 1)
					{
						Projectile.rotation = Projectile.velocity.ToRotation() - (MathHelper.Pi / 8f * Projectile.direction);
						RotationVelocity = MathHelper.Pi / Owner.itemAnimationMax;
						Projectile.ai[0]++;
						Projectile.localAI[1] = -Projectile.width / 4f;
					}
					Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + ((Projectile.rotation - (MathHelper.PiOver4 * Projectile.spriteDirection)).ToRotationVector2() * Projectile.localAI[1]);

					Projectile.localAI[1] += BaseSpearProjectile.SpearSpeed * Projectile.scale * .8f;
					RotationVelocity *= .95f;
				}
				else if (Owner.GetModPlayer<GildedPlayer>().swingCount == 2)
				{
					Projectile.spriteDirection *= -1;
					if (Projectile.ai[0] == 1)
					{
						Projectile.rotation = Projectile.velocity.ToRotation() + (MathHelper.Pi / 8f * Projectile.direction);
						RotationVelocity = -MathHelper.Pi / Owner.itemAnimationMax;
						Projectile.ai[0]++;
						Projectile.localAI[1] = -Projectile.width / 4f;
					}
					Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + ((Projectile.rotation - (MathHelper.PiOver4 * Projectile.spriteDirection)).ToRotationVector2() * Projectile.localAI[1]);

					Projectile.localAI[1] += BaseSpearProjectile.SpearSpeed * Projectile.scale * .8f;
					RotationVelocity *= .95f;
				}
				else if (Owner.GetModPlayer<GildedPlayer>().swingCount == 3)
				{
					thrusting = true;
					if (Projectile.ai[0] == 1)
					{
						Projectile.ai[0]++;
						Projectile.localAI[1] = -Projectile.width / 3f;
					}
					Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + (Projectile.velocity * Projectile.localAI[1]);
	
					Projectile.localAI[1] += BaseSpearProjectile.SpearSpeed * Projectile.scale * .8f;
				}
				else
				{
					if (Projectile.ai[0] == 1)
					{
						Owner.GetModPlayer<GildedPlayer>().swingCount = 0;
					}
				}
			}
			else
			{
				if (Projectile.ai[0] == 3)
				{
					Projectile.rotation = Projectile.velocity.ToRotation() + (MathHelper.PiOver4 * Projectile.direction);
					RotationVelocity = -MathHelper.PiOver2 / (Owner.itemAnimationMax / 4f);
					if (Projectile.ai[2] == ProjectileID.Bullet)
					{
						Projectile.ai[2] = ProjectileID.GoldenBullet;
					}

					if (Main.myPlayer == Owner.whoAmI)
					{
						var proj = Projectile.NewProjectileDirect(Owner.PlayerItem().GetSource_ItemUse(Owner), Projectile.Center, (Projectile.rotation - (MathHelper.PiOver4 * Projectile.direction)).ToRotationVector2() * 7f, (int)Projectile.ai[2], Projectile.damage * 3, Projectile.knockBack, Projectile.owner);
						proj.scale *= 5f;
						proj.Hitbox = proj.Hitbox.Scaled(5f);
						Owner.velocity += proj.velocity * -1f;
					}
					SoundEngine.PlaySound(SoundID.Item11, Projectile.Center);

					Projectile.ai[0]++;
					Owner.GetModPlayer<GildedPlayer>().BarProgress = 0f;
					Owner.GetModPlayer<GildedPlayer>().swingCount = 0;
					NetUpdate();
				}
				else if (Projectile.ai[0] == 1)
				{
					Projectile.rotation = savedRot - (MathHelper.PiOver2 * Projectile.direction);
					RotationVelocity = MathHelper.PiOver2 / (Owner.itemAnimationMax * .75f);
					Projectile.ai[0]++;
				}
				else if (Projectile.ai[0] == 2)
				{
					if (Owner.itemAnimation <= (Owner.itemAnimationMax / 4f))
					{
						Projectile.ai[0]++;
					}
				}
			}

			if (Owner.ItemAnimationEndingOrEnded && Projectile.owner == Main.myPlayer)
			{
				Kill();
			}
			Projectile.rotation += RotationVelocity * Projectile.direction;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			hitbox.X += ((Projectile.rotation - (MathHelper.PiOver4 * Projectile.spriteDirection)).ToRotationVector2().X * (Projectile.width / 2f)).Round();
			hitbox.Y += ((Projectile.rotation - (MathHelper.PiOver4 * Projectile.spriteDirection)).ToRotationVector2().Y * (Projectile.width / 2f)).Round();
			AOUtils.ScaleRectangle(ref hitbox, 1 / 3f);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			base.OnHitNPC(target, hit, damageDone);
			Owner.GetModPlayer<GildedPlayer>().BarProgress += .1f;
		}

		public Color Colour => Imbue?.Colour ?? Color.Gold;

		public override bool PreDraw(ref Color lightColor)
		{
			if (thrusting)
			{
				lightColor = Colour.MultiplyRGB(lightColor);
				SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically;
				for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
				{
					if (k % 3 == 0)
					{
						Vector2 drawPos = Projectile.oldPos[k] + (Projectile.Size / 2f) + new Vector2(0f, Projectile.gfxOffY);
						var colour2 = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
						Main.EntitySpriteDraw(Afterimages, drawPos + (Projectile.velocity * (Projectile.width / 4f)) - (Projectile.velocity * 5f * k) - Main.screenPosition, new(0, 0, Afterimages.Width, Afterimages.Height / Main.projFrames[AfterimagesType]), colour2 * .5f, Projectile.rotation - (MathHelper.PiOver4 * Projectile.direction), new Vector2(Afterimages.Width, Afterimages.Height / Main.projFrames[AfterimagesType]) / 2f, Projectile.scale - (k * .05f), mode);
					}
				}
			}
			return base.PreDraw(ref lightColor);
		}
	}
}
