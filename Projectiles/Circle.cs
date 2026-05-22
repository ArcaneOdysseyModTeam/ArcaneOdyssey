using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Projectiles.Relics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Projectiles
{
	public class Circle : PlayerProjectile
	{
		public int ChargingProjectile;
		public float charge = 1f;
		public bool MarkedForDeath = false;
		public bool playedsound = false;
		internal bool originallyAltFire = false;
		public float ProjectileSpread = 0;

		public const float GlobalChargeSpeed = 1f / 120f;
		public const float GlobalMaxCharge = 1.75f;

		public override float Size => .6f;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (Mode != MagicCircleMode.Rotating)
			{
				overPlayers.Add(index);
			}
			else
			{
				behindProjectiles.Add(index);
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.WriteFlags(MarkedForDeath, playedsound, originallyAltFire);
			writer.Write(ChargingProjectile);
			writer.Write(charge);
			writer.Write(ProjectileSpread);
			writer.Write(Projectile.rotation);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			reader.ReadFlags(out MarkedForDeath, out playedsound, out originallyAltFire);
			ChargingProjectile = reader.ReadInt32();
			charge = reader.ReadSingle();
			ProjectileSpread = reader.ReadSingle();
			Projectile.rotation = reader.ReadSingle();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 125;
			Projectile.tileCollide = false;
			Projectile.hide = true;
			Projectile.Opacity = .75f;
			playedsound = false;
		}

		public MagicCircleMode Mode
		{
			get
			{
				return (MagicCircleMode)Projectile.ai[2];
			}
			set
			{
				Projectile.ai[2] = (int)value;
			}
		}

		public override bool CanHaveImbueVFX => false;

		protected Vector2 dir;

		public override bool? CanDamage() => false;

		private float ExplosionSizeMult => !originallyAltFire ? 1.25f : 1f;

		public override void AI()
		{
			if ((ChargingProjectile != 0) || (!MarkedForDeath))
			{
				AOPlayerOwner.myCircle = this;
			}

			if (!MarkedForDeath)
			{
				dir = Main.myPlayer == Projectile.owner ? Owner.RotatedRelativePoint(Owner.MountedCenter).DirectionTo(Main.MouseWorld) : Projectile.DirectionFrom(Owner.RotatedRelativePoint(Owner.MountedCenter));
			}
			else
			{
				dir = Projectile.rotation.ToRotationVector2();
			}

			if (Projectile.ai[0] == 0)
			{
				NetUpdate();
				if (!MarkedForDeath)
				{
					Projectile.Opacity = .1f;
				}
				else if (Mode == MagicCircleMode.Rotating)
				{
					Projectile.Opacity = 1f;
				}

				Projectile.ai[0] = 1;

				if (Mode != MagicCircleMode.Rotating)
				{
					dir = Main.myPlayer == Projectile.owner ? Owner.RotatedRelativePoint(Owner.MountedCenter).DirectionTo(Main.MouseWorld) : Projectile.DirectionFrom(Owner.RotatedRelativePoint(Owner.MountedCenter));
					Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
					if (Owner.ItemAnimationActive)
					{
						Owner.itemRotation = dir.ToRotation();
						if (Owner.direction != 1)
						{
							Owner.itemRotation += MathHelper.Pi;
						}
					}
				}
			}

			if (Mode == MagicCircleMode.Barrage)
			{
				if (!MarkedForDeath)
					dir = Projectile.rotation.AngleTowards(dir.ToRotation(), ApplySpeed(MathHelper.TwoPi / 200f)).ToRotationVector2();
			}

			if (Mode == MagicCircleMode.Rotating)
			{
				Projectile.rotation += ApplySpeed(MathHelper.Pi / 120f);
				if (ChargingProjectile == ModContent.ProjectileType<ExplosionSpell>() || ChargingProjectile == ModContent.ProjectileType<SpiritExplosion>())
				{
					// Outline vfx
					if (Imbue is not null)
					{
						for (int n = 0; n < 360; n += 4)
						{
							Vector2 currentDustPos = new Vector2((float)Math.Cos(n * (MathHelper.Pi / 180f)), (float)Math.Sin(n * (MathHelper.Pi / 180f))) * ApplySize(109f * ExplosionSizeMult);
							currentDustPos.X = Utils.Clamp(currentDustPos.X, -1 * ApplySize(100f * ExplosionSizeMult), ApplySize(100f * ExplosionSizeMult));
							currentDustPos.Y = Utils.Clamp(currentDustPos.Y, -1 * ApplySize(100f * ExplosionSizeMult), ApplySize(100f * ExplosionSizeMult));
							Dust.NewDustPerfect(Projectile.Center + currentDustPos, DustID.ShimmerSpark, Vector2.Zero, 0, Imbue.Colour, ApplySize(1f * ExplosionSizeMult));
						}
					}
				}
			}


			if (Projectile.position != Projectile.oldPosition)
			{
				NetUpdate();
			}

			Imbue ??= ModContent.GetInstance<WindMagic>();

			if (!playedsound && Imbue is MagicType)
			{
				SoundEngine.PlaySound(SoundID.Item84 with { Pitch = ApplySpeed(1f).MultiToPercent().Clamp(-1, 1) }, Projectile.Center);
				playedsound = true;
			}

			if (Imbue is MagicType && !Main.dedServ && Projectile.Opacity >= .5f)
			{
				var hitbox = Projectile.Hitbox.Scaled(.5f);
				SecondImbue?.LingeringEffects(hitbox);
				Dust spawnedDust = Main.dust[Dust.NewDust(hitbox.TopLeft(), hitbox.Width, hitbox.Height, DustID.SilverFlame, 8f * (Main.rand.NextFloat() - 0.5f), 8f * (Main.rand.NextFloat() - 0.5f), 0, Imbue.Colour)];
				spawnedDust.noGravity = true;
			}

			MarkedForDeath |= !((Owner.channel && !originallyAltFire) || (Main.mouseRight && originallyAltFire)) || Owner.DeadOrGhost;

			if (!MarkedForDeath)
			{
				if (Mode != MagicCircleMode.Barrage)
				{
					AOPlayerOwner.HeavySkillActive = true;
				}

				if (Mode == MagicCircleMode.Rotating)
				{
					if (!originallyAltFire)
					{
						Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter);
					}
					else
					{
						if (Main.myPlayer == Projectile.owner)
						{
							Owner.itemRotation = Owner.RotatedRelativePoint(Owner.MountedCenter).DirectionTo(Vector2.Lerp(Projectile.Center, Main.MouseWorld, .5f)).ToRotation();
							if (Owner.direction != 1)
							{
								Owner.itemRotation += MathHelper.Pi;
							}
							if (Vector2.Distance(Main.MouseWorld, Owner.position) < 400)
							{
								Projectile.Center = Projectile.Center.MoveTowards(Main.MouseWorld, ApplySpeed(10f));
							}
							else
								Projectile.Center = Projectile.Center.MoveTowards(Owner.RotatedRelativePoint(Owner.MountedCenter) + Owner.RotatedRelativePoint(Owner.MountedCenter).DirectionTo(Main.MouseWorld) * 400, ApplySpeed(10f));
						}
					}
				}
				else
				{
					Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + (dir * 30f);
					Projectile.rotation = dir.ToRotation();
				}

				Owner.itemAnimation = Owner.itemAnimationMax;
				Owner.itemTime = Owner.itemTimeMax;
				Owner.itemRotation = dir.ToRotation();
				if (Owner.direction != 1)
				{
					Owner.itemRotation += MathHelper.Pi;
				}
				charge += GlobalChargeSpeed;
				Projectile.Opacity += GlobalChargeSpeed;
				Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
				if (charge >= GlobalMaxCharge)
				{
					if (Mode == MagicCircleMode.Barrage)
					{
						ShootProjectile();
					}
					else
					{
						Owner.channel = false;
						MarkedForDeath = true;
						NetUpdate();
					}
				}
			}
			else
			{
				if (Mode != MagicCircleMode.Barrage)
				{
					ShootProjectile();
				}
				Projectile.Opacity -= GlobalChargeSpeed * 2f;
			}

			if (Projectile.alpha >= 255 && Main.myPlayer == Projectile.owner)
			{
				Kill();
			}
		}

		public override void OnKill(int timeLeft)
		{
			if (AOPlayerOwner.myCircle?.Projectile.identity == Projectile.identity)
			{
				AOPlayerOwner.myCircle = null;
			}
		}

		public float Intensity
		{
			get
			{
				if (Mode == MagicCircleMode.Basic)
				{
					return Projectile.Opacity * charge;
				}
				if (Mode == MagicCircleMode.Barrage)
				{
					return Projectile.Opacity * 1.2f;
				}
				return Projectile.Opacity;
			}
		}

		public override Texture2D Sprite
		{
			get
			{
				if ((Mode == MagicCircleMode.Rotating) || (Main.LocalPlayer.gravDir != 1))
				{
					if (Imbue is MagicType magic)
					{
						return magic.Circle.Texture.Value;
					}
				}
				return TextureAssets.Projectile[Type].Value;
			}
		}

		public virtual void ShootProjectile()
		{
			dir = (dir.ToRotation() + Main.rand.NextFloat(-ProjectileSpread, ProjectileSpread)).ToRotationVector2();
			if (Mode == MagicCircleMode.Barrage)
			{
				if (Projectile.Opacity == 1f && Main.myPlayer == Projectile.owner && Main.GameUpdateCount % Owner.itemAnimationMax == 0)
				{
					if (Owner.CheckMana(Owner.PlayerItem(), -1, true))
					{
						NetUpdate();
						if (ChargingProjectile != 0)
						{
							if (Main.rand.NextBool(5))
							{
								playedsound = false;
							}
							AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, dir * 12f, ChargingProjectile, Projectile.damage, Projectile.knockBack, Projectile.owner, Imbue, SecondImbue, true);
						}
					}
					else
					{
						MarkedForDeath = true;
						NetUpdate();
					}
				}
			}
			else
			{
				if (Main.myPlayer == Projectile.owner && ChargingProjectile != 0)
				{
					if (ArcaneOdysseyClientConfig.Instance.AbilityText && Owner is not null && Owner?.active == true && !Owner.DeadOrGhost)
					{
						var name = Lang.GetProjectileName(ChargingProjectile).Value;
						if (ModContent.GetModProjectile(ChargingProjectile) is MagicSpell spell)
						{
							name = (Imbue.PrettySpellPrefix + " " + name).Trim();
						}
						if (SecondImbue is not null)
						{
							name = (SecondImbue.PrettyAttackPrefix + " " + name).Trim();
						}
						CombatText.NewText(Owner.Hitbox, Imbue?.Colour ?? Color.White, (name + "!").Trim(), ModContent.GetModProjectile(ChargingProjectile) is not BlastSpell);
					}

					var velo = dir * 10f;
					if (charge != 1f)
					{
						velo = dir * 15f * (charge / 2f);
					}
					var proj = AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, velo, ChargingProjectile, (Projectile.damage * charge).Round(), Projectile.knockBack * charge, Projectile.owner, Imbue, SecondImbue, true);

					if (ChargingProjectile == ModContent.ProjectileType<ExplosionSpell>() || ChargingProjectile == ModContent.ProjectileType<SpiritExplosion>())
					{
						proj.damage = (Projectile.damage * charge * ExplosionSizeMult).Round();
						proj.Hitbox = proj.Hitbox.Scaled(ExplosionSizeMult);
						proj.scale *= ExplosionSizeMult;
					}

					if (proj.ModProjectile is PulsarSpell && originallyAltFire)
					{
						proj.ai[1] = 1;
					}
					ChargingProjectile = 0;
					NetUpdate();
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is not MagicType magic)
				return false;

			lightColor = Imbue?.Colour ?? Color.White;
			Lighting.AddLight(Projectile.Center, lightColor.ToVector3() * Intensity * (Projectile.scale / Size));

			if ((Mode != MagicCircleMode.Rotating) && (Main.LocalPlayer.gravDir == 1))
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
			if ((Mode != MagicCircleMode.Rotating) && (Main.LocalPlayer.gravDir == 1))
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			}
		}

		public override bool? CanCutTiles() => false;
	}

	public enum MagicCircleMode
	{
		Rotating,
		Basic,
		Barrage
	}
}
