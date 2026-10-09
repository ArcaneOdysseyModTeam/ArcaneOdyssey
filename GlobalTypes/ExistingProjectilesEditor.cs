using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Weapons.Bronze;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Abilities;
using ArcaneOdyssey.Projectiles.Magic;
using System.IO;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.CameraModifiers;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.GlobalTypes
{
	public partial class AOProjectile : GlobalProjectile, IImbuable
	{
		public bool isPiercingShot;
		public bool isStormOfArrows;

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.DrawScreenCheckFluff[ProjectileID.BulletHighVelocity] = 99999999;
		}

		internal Vector2 origin = default;

		public void Spawn(Projectile projectile, IEntitySource source)
		{
			if (projectile.type == ProjectileID.BulletHighVelocity)
			{
				if (source is EntitySource_ItemUse_WithAmmo { Item: Item item, AmmoItemIdUsed: int bullet })
				{
					if (item.type == ModContent.ItemType<BronzeMusket>() && item.Imbue() is MagicType or SpiritEnergy && bullet != ItemID.HighVelocityBullet)
					{
						isPiercingShot = true;
					}
				}
			}

			if (source is EntitySource_Parent { Entity: Projectile storm } && storm.type == ModContent.ProjectileType<ArrowStorm>())
			{
				isStormOfArrows = true;
			}
		}

		public void SendEvenMoreAI(Projectile projectile, BitWriter bit, BinaryWriter writer)
		{
			bit.WriteBit(isPiercingShot);
			if (isPiercingShot)
			{
				bit.WriteBit(piercingShotHit);
				writer.Write(origin);
			}
			bit.WriteBit(isStormOfArrows);
		}

		public void ReceiveEvenMoreAI(Projectile projectile, BitReader bit, BinaryReader reader)
		{
			isPiercingShot = bit.ReadBit();
			if (isPiercingShot)
			{
				piercingShotHit = bit.ReadBit();
				origin = reader.ReadVector2();
			}
			isStormOfArrows = bit.ReadBit();
		}

		private int shotFrame;

		public Texture2D Sprite => TextureAssets.Projectile[thisProjectile?.type ?? 0]?.Value ?? Asset<Texture2D>.DefaultValue;
		public Texture2D MidSprite => ImbueID.Sets.Assets.raySprites[Imbue.ID].Value;
		public Texture2D EndSprite => ImbueID.Sets.Assets.rayEndSprites[Imbue.ID].Value;
		public Texture2D StartSprite => ImbueID.Sets.Assets.rayStartSprites[Imbue.ID].Value;

		public override bool PreDraw(Projectile projectile, ref Color lightColor)
		{
			if (isPiercingShot)
			{
				if (origin != default)
				{
					var colour = Imbue is MagicType ? Color.White : Imbue.Colour;
					var scale = projectile.scale / 2f;
					var maxFrames = Main.projFrames[ModContent.ProjectileType<BeamSpell>()];
					SpriteEffects mode = projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
					var info = AOUtils.DrawChain(origin, projectile.Center, MidSprite, scale, maxFrames, shotFrame, colour * projectile.Opacity, mode);
					var frame = StartSprite.Frame(1, maxFrames, 0, shotFrame);
					Main.EntitySpriteDraw(StartSprite, origin - Main.screenPosition, frame, colour * projectile.Opacity, info.Rotation, frame.Size() / 2f, scale, mode);
					var ending = info.Ending + new Vector2(EndSprite.Width * scale, 0).RotatedBy(info.Rotation);
					Main.EntitySpriteDraw(EndSprite, ending - Main.screenPosition, EndSprite.Frame(1, maxFrames, 0, info.FinalFrame), colour * projectile.Opacity, info.Rotation, new Vector2(EndSprite.Width, EndSprite.Height / maxFrames) / 2f, scale, mode);
					return false;
				}
			}
			return base.PreDraw(projectile, ref lightColor);
		}

		private bool piercingShotHit = false;

		public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
		{
			if (isPiercingShot)
			{
				if (!piercingShotHit)
				{
					PiercingShotHit(ref projectile);
				}
				return false;
			}
			return base.OnTileCollide(projectile, oldVelocity);
		}

		public void Update(Projectile projectile)
		{
			if (isPiercingShot)
			{
				if (origin == default)
				{
					shotFrame = Main.rand.Next(Main.projFrames[ModContent.ProjectileType<BeamSpell>()]);
					origin = projectile.Center;
					projectile.spriteDirection = (projectile.velocity.X > 0).ToDirectionInt();
					projectile.netUpdate = true;
					projectile.netSpam = 0;
				}

				if (piercingShotHit)
				{
					CanBeAffected = false;
					projectile.velocity = Vector2.Zero;

					if (projectile.numUpdates == 0)
						projectile.Opacity -= Circle.GlobalChargeSpeed * 2f;

					if (projectile.alpha >= 255 && Main.myPlayer == projectile.owner)
					{
						projectile.Kill();
					}
				}
			}
		}

		private void PiercingShotHit(ref Projectile projectile)
		{
			projectile.velocity = Vector2.Zero;
			projectile.timeLeft = 60 * (projectile.extraUpdates + 1);
			projectile.alpha = 0;
			projectile.penetrate++;
			piercingShotHit = true;
		}

		private static void DontKillPiercingShot(On_Projectile.orig_Kill orig, Projectile projectile)
		{
			if (projectile.ArcaneOdyssey().isPiercingShot)
			{
				if (!projectile.ArcaneOdyssey().piercingShotHit)
				{
					projectile.ArcaneOdyssey().PiercingShotHit(ref projectile);
					return;
				}
			}
			orig(projectile);
		}

		public override bool? CanDamage(Projectile projectile)
		{
			if (piercingShotHit)
			{
				return false;
			}
			return base.CanDamage(projectile);
		}

		public void Death(Projectile projectile, int timeLeft)
		{
			if (isStormOfArrows && !Main.dedServ)
			{
				PunchCameraModifier modifier = new(projectile.Center, (Main.rand.NextFloat() * MathHelper.TwoPi).ToRotationVector2(), ApplyKnockback(3f), ApplyKnockback(1f), 4, ApplyKnockback(500f), FullName);
				Main.instance.CameraModifiers.Add(modifier);
				isStormOfArrows = false;
			}
		}
	}
}
