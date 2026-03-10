using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class CannonSpell : MagicSpell
	{
		public override string Texture => GetType().FullName.Replace('.', '/').Replace("Cannon", "Blast");
		public int TileTimer = 0;

		public override float AOSize => 2f;

		public bool DoneCharging = false;
		public float charge = 1f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.penetrate = -1;
			Projectile.localNPCHitCooldown = 30;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.timeLeft = 3 * 60;
			Projectile.hide = true;
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (!DoneCharging)
				overWiresUI.Add(index);
			else
				behindNPCsAndTiles.Add(index);
		}

		public override bool? CanDamage()
		{
			if (!DoneCharging)
			{
				return false;
			}
			return null;
		}

		public override void AI()
		{
			if (Projectile.wet && DoneCharging)
			{
				Kill();
				return;
			}
			if (Projectile.ai[2] == 0f)
			{
				Projectile.ai[2] = 1f;
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0; ;
				}
			}
			Animate();
			Rotate();

			var dir = Main.myPlayer == Projectile.owner ? Owner.RotatedRelativePoint(Owner.MountedCenter).DirectionTo(Main.MouseWorld) : Projectile.DirectionFrom(Owner.RotatedRelativePoint(Owner.MountedCenter));

			if (Owner.channel && !DoneCharging)
			{
				if (Main.myPlayer == Projectile.owner && Projectile.position != Projectile.oldPosition)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter) + (dir * 94f);
				charge += BaseMagicCircle.GlobalChargeSpeed;
				Projectile.timeLeft = 3 * 60;
				Projectile.rotation = dir.ToRotation();
				if (charge >= BaseMagicCircle.GlobalMaxCharge)
				{
					Owner.channel = false;
					DoneCharging = true;
				}
			}
			else
			{
				DoneCharging = true;
				if (Projectile.velocity == Vector2.Zero)
				{
					Projectile.velocity = dir * ApplySpeed(5f);
					if (Main.myPlayer == Projectile.owner)
					{
						Projectile.netUpdate = true;
						Projectile.netSpam = 0;
					}
				}
				if (TileTimer > 0)
					TileTimer--;
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.SourceDamage *= charge;
		}

		public virtual void Animate()
		{
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
		}

		public virtual void Rotate()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (DoneCharging)
			{
				if (TileTimer <= 0)
				{
					Imbue?.KillEffects(Projectile.Hitbox);
				}
				if (TileTimer < 60 && TileTimer > 0)
				{
					return true;
				}
				TileTimer = 65;
			}
			Projectile.position = Projectile.oldPosition;
			Projectile.velocity = oldVelocity;
			return false;
		}
	}
}
