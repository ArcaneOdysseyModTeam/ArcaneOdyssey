using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;

namespace ArcaneOdyssey.Content.Projectiles.Magic
{
	public class Javelin : MagicSpell
	{
		public JavelinMode Mode = JavelinMode.Charging;
		public int PiercingNPC = -1;
		public float charge = 1f;
		public static int TimeLeft => 60 * 4;

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			if (Mode == JavelinMode.Grounded)
			{
				behindNPCsAndTiles.Add(index);
			}
			else if (Mode == JavelinMode.Piercing)
			{
				behindNPCs.Add(index);
			}
			else
			{
				overPlayers.Add(index);
			}
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.penetrate = 4;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.width = 96;
			Projectile.height = 32;
			Projectile.AverageDimensions();
			Projectile.localNPCHitCooldown = (TimeLeft / 4) + 1;
			Projectile.hide = true;
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

			if (Projectile.position.ToTileCoordinates() != Projectile.oldPosition.ToTileCoordinates() && Main.myPlayer == Projectile.owner)
			{
				Projectile.netUpdate = true;
				Projectile.netSpam = 0;
			}

			if (Mode == JavelinMode.Charging)
			{
				if (Owner.channel && charge < BaseMagicCircle.GlobalMaxCharge)
				{
					charge += BaseMagicCircle.GlobalChargeSpeed;
					Owner.ChangeDir((dir.X > 0f).ToDirectionInt());
					Projectile.spriteDirection = Owner.direction;
					Owner.heldProj = Projectile.whoAmI;
					AOPlayerOwner.HeavySkillActive = true;
					Owner.itemAnimation = Owner.itemAnimationMax;
					Owner.itemTime = Owner.itemTimeMax;
					Owner.itemRotation = dir.ToRotation();
					if (Owner.direction != 1)
					{
						Owner.itemRotation += MathHelper.Pi;
					}
					Projectile.rotation = dir.ToRotation();
					Projectile.Center = Owner.RotatedRelativePoint(Owner.MountedCenter);
					Projectile.position.Y -= 15f;
					Projectile.timeLeft = TimeLeft;
				}
				else
				{
					Projectile.velocity = dir * 20f;
					Mode = JavelinMode.Flying;
					Owner.channel = false;
					Projectile.timeLeft = TimeLeft;
				}
			}
			if (Mode == JavelinMode.Flying)
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.velocity.Y += ApplySpeed(0.13f, true);
				if (Projectile.velocity.Y > 16f)
				{
					Projectile.velocity.Y = 16f;
				}
			}
			if (Mode == JavelinMode.Piercing)
			{
				if (PiercingNPC != -1 && Main.npc.IndexInRange(PiercingNPC))
				{
					var npc = Main.npc[PiercingNPC];
					if (npc.active && npc.life > 0)
					{
						Projectile.Center = npc.Center;
					}
					else
					{
						Kill();
					}
				}
			}
			if (Mode == JavelinMode.Grounded)
			{
				if (Projectile.timeLeft % (TimeLeft / 4) == 0)
					SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
			}
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Mode == JavelinMode.Flying)
			{
				Projectile.velocity = Vector2.Zero;
				Mode = JavelinMode.Piercing;
				Projectile.timeLeft = TimeLeft;
				PiercingNPC = target.whoAmI;
			}
			SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.SourceDamage *= charge;
		}

		public override bool DrawWithImbueColours => true;

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Mode == JavelinMode.Flying)
			{
				Projectile.velocity = Vector2.Zero;
				Mode = JavelinMode.Grounded;
				Projectile.timeLeft = TimeLeft;
			}
			return false;
		}
	}

	public enum JavelinMode
	{
		Charging,
		Flying,
		Grounded,
		Piercing
	}
}
