using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Projectiles.Circles
{
	public class BasicMagicCircle : BaseMagicCircle
	{
		public int ChargingProjectile;
		public float charge = 1f;

		public override void SetStaticDefaults()
		{
			//Main.projFrames[Type] = 4;
		}

		public override float AOSize => .5f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 128;
			Projectile.tileCollide = false;
		}

		internal bool originallyAltFire = false;

		public override void AI()
		{
			var dir = Main.myPlayer == Projectile.owner ? Owner.RotatedRelativePoint(Owner.MountedCenter).DirectionTo(Main.MouseWorld) : Projectile.rotation.ToRotationVector2();
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
			}


			if (Projectile.position != Projectile.oldPosition && Main.myPlayer == Projectile.owner)
			{
				Projectile.netUpdate = true;
				Projectile.netSpam = 0;
			}
			

			if (Owner.channel && !MarkedForDeath)
			{
				AOPlayerOwner.HeavySkillActive = true;
				Owner.heldProj = Projectile.whoAmI;
				Owner.itemAnimation = Owner.itemAnimationMax;
				Owner.itemTime = Owner.itemTimeMax;
				Owner.itemRotation = dir.ToRotation();
				if (Owner.direction != 1)
				{
					Owner.itemRotation += MathHelper.Pi;
				}
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
				MarkedForDeath = true;
				if (Projectile.ai[1] == 0 && Main.myPlayer == Projectile.owner && ChargingProjectile != 0)
				{
					var proj = AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, dir * 10, ChargingProjectile, (Projectile.damage * charge).Round(), Projectile.knockBack * charge, Projectile.owner, Imbue, SecondImbue, true);
					if (proj.ModProjectile is PulsarSpell && originallyAltFire)
					{
						proj.ai[1] = 1;
					}
					Projectile.ai[1] = 1;
				}
			}

			if (Imbue is not null && !Main.dedServ)
			{
				if (Projectile.localAI[0]++ > 5)
				{
					Dust spawnedDust = Main.dust[Dust.NewDust(new Vector2(Projectile.position.X + (Projectile.scale * Projectile.width * Main.rand.NextFloat()), Projectile.position.Y + (Projectile.scale * Projectile.height * Main.rand.NextFloat())), 0, 0, DustID.SilverFlame, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, Imbue.GetColour(), 1f)];
					spawnedDust.noGravity = true;
					Projectile.localAI[0] = 0;
				}
			}

			if (Projectile.alpha >= 255)
			{
				Kill();
			}

			//if (Projectile.frameCounter++ > 5)
			//{
			//	Projectile.frameCounter = 0;
			//	if (++Projectile.frame >= Main.projFrames[Type])
			//	{
			//		Projectile.frame = 0;
			//	}
			//}
			circleRotation = Intensity * ApplySpeed(MathHelper.Pi / 120f);
		}


		public float circleRotation = 0;

		public float Intensity => Projectile.Opacity * (charge * charge);

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is null or AOMagic)
			{
				lightColor = Imbue?.GetColour(Color.White) ?? Color.White;
				Lighting.AddLight(Projectile.Center, lightColor.ToVector3());
			}
			else
				lightColor = Color.Transparent;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			var asset = Mod.Assets.Request<Texture2D>("Effects/MagicCircles/Ancient");
			GameShaders.Misc[Mod.Name + ":MagicCircleBase"].UseImage1(asset);
			GameShaders.Misc[Mod.Name + ":MagicCircleBase"].UseImage1(asset);
			GameShaders.Misc[Mod.Name + ":MagicCircleBase"]
				.UseColor(lightColor)
				.UseSaturation(Intensity)
				.UseSecondaryColor(new Color(circleRotation, 0, 0));
			

			GameShaders.Misc[Mod.Name + ":MagicCircleBase"].Apply();


			SpriteEffects mode = Projectile.spriteDirection > 0 ? SpriteEffects.None : FlippedMode;
			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, Sprite.Size() / 2f, Projectile.scale, mode);
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		}
	}
}
