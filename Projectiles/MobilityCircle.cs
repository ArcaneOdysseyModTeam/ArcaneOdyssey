using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Projectiles
{
	public class MobilityCircle : PlayerProjectile
	{
		public override string Texture => AOUtils.GetTexture<Circle>();

		public override bool CanHaveImbueVFX => false;

		public override bool? CanDamage() => false;

		public override float Size => .6f;
		public override void SetStaticDefaults()
		{
			ArcaneOdysseyMod.Sets.imbueEffect[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 125;
			Projectile.tileCollide = false;
			Projectile.Opacity = .75f;
		}

		public float Intensity => Projectile.Opacity * 1.2f;

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is not MagicType magic)
				return false;

			lightColor = Imbue?.Colour ?? Color.White;
			Lighting.AddLight(Projectile.Center, lightColor.ToVector3() * Intensity * (Projectile.scale / Size));
			if (Main.LocalPlayer.gravDir == 1)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				GameShaders.Misc[Mod.Name + ":MagicCircleBase"].UseImage1(magic.Circle.Texture);
				GameShaders.Misc[Mod.Name + ":MagicCircleBase"].UseImage1(magic.Circle.Texture);
				GameShaders.Misc[Mod.Name + ":MagicCircleBase"]
					.UseColor(lightColor)
					.UseSaturation(Intensity)
					.UseSecondaryColor(new Color((255 * ApplySpeed(MathHelper.TwoPi / 5f)).Round(), 0, 0));


				GameShaders.Misc[Mod.Name + ":MagicCircleBase"].Apply();


				Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, Sprite.Size() / 2f, Projectile.scale, SpriteEffects.FlipVertically);
			}
			else
			{
				Main.EntitySpriteDraw(Sprite, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, Sprite.Size() / 2f, Projectile.scale * (100 / 2000f), SpriteEffects.None);
			}
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			if (Main.LocalPlayer.gravDir == 1)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			}
		}

		public Vector2 dir;

		private void ProperDeath()
		{
			if (AOPlayerOwner.myMobilityCircle?.Projectile.identity == Projectile.identity)
			{
				AOPlayerOwner.myMobilityCircle = null;
			}
			MarkedForDeath = true;
		}

		public override void AI()
		{
			if (!MarkedForDeath)
			{
				AOPlayerOwner.myMobilityCircle = this;
			}

			if (!MarkedForDeath)
			{
				switch (Mode)
				{
					case MobilityCircleMode.Dash:
						if (AOPlayerOwner.dashing)
						{
							dir = -AOPlayerOwner.DashVelocity;
						}
						else
						{
							ProperDeath();
						}
						break;
					case MobilityCircleMode.Flight:
						if (Owner.controlJump && Owner.wingTime > 0 && Owner.wingsLogic != 0)
						{
							if (Owner.controlLeft && !Owner.controlRight)
							{
								dir.X = 10;
							}
							if (Owner.controlRight && !Owner.controlLeft)
							{
								dir.X = -10;
							}
							dir.Y = (MathF.Abs(dir.X) * 3f) + 1f;
						}
						else
						{
							ProperDeath();
						}
						break;
					case MobilityCircleMode.Hover:
						if (Owner.controlJump && Owner.carpetTime > 0 && Owner.carpet)
						{
							if (Owner.controlLeft && !Owner.controlRight)
							{
								dir.X = 10;
							}
							if (Owner.controlRight && !Owner.controlLeft)
							{
								dir.X = -10;
							}
							dir.Y = (MathF.Abs(dir.X) * 3f) + 1f;
						}
						else
						{
							ProperDeath();
						}
						break;
				}
			}
			else
			{
				dir = Projectile.rotation.ToRotationVector2();
			}

			if (Projectile.ai[0] == 0)
			{
				NetUpdate();
				Projectile.ai[0] = 1;
				Projectile.rotation = dir.ToRotation();
				if (!MarkedForDeath)
				{
					Projectile.alpha = 254;
				}
				if (Mode == MobilityCircleMode.Dash && AOPlayerOwner.dashing)
				{
					Projectile.rotation = (-AOPlayerOwner.DashVelocity).ToRotation();
				}
			}

			dir = Projectile.rotation.AngleTowards(dir.ToRotation(), ApplySpeed(MathHelper.TwoPi / 200f)).ToRotationVector2();


			if (Projectile.position != Projectile.oldPosition)
			{
				NetUpdate();
			}

			Imbue ??= ModContent.GetInstance<WindMagic>();

			if (Imbue is MagicType && !Main.dedServ && Projectile.Opacity >= .5f)
			{
				var hitbox = Projectile.Hitbox.Scaled(.5f);
				SecondImbue?.LingeringEffects(hitbox);
				Dust spawnedDust = Main.dust[Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.SilverFlame, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, Imbue.Colour)];
				spawnedDust.noGravity = true;
			}
			if (!MarkedForDeath)
			{
				Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + (dir.SafeNormalize() * 20f);
				Projectile.Opacity += Circle.GlobalChargeSpeed * 2f;
				Projectile.rotation = dir.ToRotation();
			}
			else
			{
				Projectile.Opacity -= Circle.GlobalChargeSpeed * 2f;
			}

			if (Projectile.alpha >= 255 && Main.myPlayer == Projectile.owner)
			{
				Kill();
			}
		}

		public override Texture2D Sprite
		{
			get
			{
				if (Main.LocalPlayer.gravDir != 1)
				{
					if (Imbue is MagicType magic)
					{
						return magic.Circle.Texture.Value;
					}
				}
				return TextureAssets.Projectile[Type].Value;
			}
		}

		public override void OnKill(int timeLeft)
		{
			ProperDeath();
		}

		public override bool? CanCutTiles() => false;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{

		}
		public bool MarkedForDeath = false;

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(MarkedForDeath);
			writer.Write(Projectile.rotation);
			writer.Write(dir);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			MarkedForDeath = reader.ReadBoolean();
			Projectile.rotation = reader.ReadSingle();
			dir = reader.ReadVector2();
		}

		public MobilityCircleMode Mode
		{
			get
			{
				return (MobilityCircleMode)Projectile.ai[2];
			}
			set
			{
				Projectile.ai[2] = (int)value;
			}
		}
	}

	public enum MobilityCircleMode
	{
		Dash,
		Flight,
		Hover
	}
}
