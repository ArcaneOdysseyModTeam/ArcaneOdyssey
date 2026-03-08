using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;

namespace ArcaneOdyssey.Content.Projectiles.Circles
{
	public class BarrageMagicCircle : BaseMagicCircle
	{
		public int ChargingProjectile;
		public float ProjectileSpread = 0;

		public override string Texture => AOUtils.GetTexture<BasicMagicCircle>();

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}

		public override float AOSize => .5f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 128;
			Projectile.tileCollide = false;
		}

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
				Owner.heldProj = Projectile.whoAmI;
				Owner.itemAnimation = Owner.itemAnimationMax;
				Owner.itemTime = Owner.itemTimeMax;
				Owner.itemRotation = dir.ToRotation();
				if (Owner.direction != 1)
				{
					Owner.itemRotation += MathHelper.Pi;
				}
				Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
				Projectile.rotation = dir.ToRotation();
				Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + (dir * 30f);

				//dir += (Main.rand.NextFloat(-ProjectileSpread, ProjectileSpread).ToRotationVector2());
				dir = (dir.ToRotation() + Main.rand.NextFloat(-ProjectileSpread, ProjectileSpread)).ToRotationVector2();

				if (Main.myPlayer == Projectile.owner && Main.GameUpdateCount % Owner.itemAnimationMax == 0)
				{
					if (Owner.CheckMana(Owner.GetManaCost(Owner.PlayerItem()), true))
					{
						if (ChargingProjectile != 0)
						{
							AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, dir * 10f, ChargingProjectile, Projectile.damage, Projectile.knockBack, Projectile.owner, Imbue, SecondImbue, true);
						}
					}
					else
					{
						MarkedForDeath = true;
					}
				}
			}
			else
			{
				MarkedForDeath = true;
			}

			//if (Projectile.frameCounter++ > 5)
			//{
			//	Projectile.frameCounter = 0;
			//	if (++Projectile.frame >= Main.projFrames[Type])
			//	{
			//		Projectile.frame = 0;
			//	}
			//}
			circleRotation += ApplySpeed(MathHelper.PiOver4 / 200f);
		}

		public float Intensity => Projectile.Opacity * 1.2f;

		public float circleRotation = 0;

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

			GameShaders.Misc[Mod.Name + ":MagicCircleBase"].UseImage1(ArcaneOdysseyMod.MagicCircleSprite);
			GameShaders.Misc[Mod.Name + ":MagicCircleBase"].UseImage1(ArcaneOdysseyMod.MagicCircleSprite);
			GameShaders.Misc[Mod.Name + ":MagicCircleBase"]
				.UseColor(lightColor)
				.UseSaturation(Intensity)
				.UseSecondaryColor(new Color(circleRotation, 0, 0));


			GameShaders.Misc[Mod.Name + ":MagicCircleBase"].Apply();


			Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, Sprite.Size() / 2f, Projectile.scale, SpriteEffects.FlipVertically);
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
		}
	}
}
