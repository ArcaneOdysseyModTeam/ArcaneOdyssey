using ArcaneOdyssey.Buffs.Minions;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Relics.Minions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ArcaneOdyssey.Projectiles.Magic.Minions
{
	public class Elemental : MagicSpell
	{
		public ref float AttackTimer => ref Projectile.ai[2];
		public bool Stuck => StuckWalkThroughWallsTimer >= 40f || Collision.SolidCollision(Projectile.Center, 2, 2);
		public ref float StuckWalkThroughWallsTimer => ref Projectile.ai[0];
		public ref float StuckJumpSpeed => ref Projectile.ai[1];
		public const float Gravity = 0.35f;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.MinionTargettingFeature[Type] = true;
			Main.projPet[Type] = true;
			ProjectileID.Sets.MinionSacrificable[Type] = true;
			Main.projFrames[Type] = 4;
		}

		public override string Texture => AOUtils.GetTexture<SpiritMinion>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 24;
			Projectile.height = 46;
			Projectile.minionSlots = 1;
			Projectile.minion = true;
			Projectile.netImportant = true;
			Projectile.Opacity = .75f;
		}

		private NPC potentialTarget;

		public override bool? CanDamage() => false;

		public override void AI()
		{
			CheckActive();
			if (Projectile.frameCounter++ > 5)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
			}
			CheckActive();
			if (Projectile.velocity.Y < 15f)
				Projectile.velocity.Y += Gravity;

			Vector2 destination;
			potentialTarget = Projectile.Center.GetMinionTarget(900f, Owner);
			if (potentialTarget is null)
				destination = Owner.Center - Vector2.UnitX * (80f + Projectile.identity * 28f % 560f) * Owner.direction;
			else
			{
				Vector2 destA = potentialTarget.Center + Vector2.UnitX * (130f + Projectile.identity * 28f % 560f);
				Vector2 destB = potentialTarget.Center - Vector2.UnitX * (130f + Projectile.identity * 28f % 560f);
				if ((Projectile.Center - destA).Length() < (Projectile.Center - destB).Length())
					destination = destA;
				else
					destination = destB;
			}

			try
			{
				Vector2 upwardCheck = destination - Vector2.UnitY * 2400f;
				upwardCheck.Y = Utils.Clamp(upwardCheck.Y, 0f, Main.maxTilesY * 16f - 100f);
				WorldUtils.Find(upwardCheck.ToTileCoordinates(), Searches.Chain(new Searches.Down(200), new Conditions.IsSolid()), out Point loweredPoint);
				destination = loweredPoint.ToWorldCoordinates();
			}
			catch (NullReferenceException) { }

			StuckWalkThroughWallsTimer = Utils.Clamp(StuckWalkThroughWallsTimer, 0, 160);

			if (Projectile.Distance(Owner.Center) > 3500f)
			{
				Projectile.Center = Owner.Center;
				StuckWalkThroughWallsTimer = 0;
				Projectile.netImportant = true;
			}
			if ((MoveToDestination(destination) || AttackTimer > 0) && potentialTarget != null)
			{
				AttackTimer += ApplySpeed(1f);

				if (MathHelper.Distance(potentialTarget.Center.X, Projectile.Center.X) > 30f)
				{
					if (potentialTarget.Center.X - Projectile.Center.X > 0)
					{
						Projectile.spriteDirection = 1;
					}
					else
					{
						Projectile.spriteDirection = -1;
					}
				}

				if (AttackTimer >= 25)
				{
					AttackTimer = 0;
					if (Main.myPlayer == Projectile.owner)
					{
						Vector2 initialVelocity = Projectile.SafeDirectionTo(potentialTarget.Center + (potentialTarget.velocity * 15f)) * 7f;
						AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, initialVelocity, ModContent.ProjectileType<MinionMinionBeam>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Imbue, SecondImbue, true);
						Projectile.netUpdate = true;
						Projectile.netSpam = 0;
					}
				}
			}
			else if (potentialTarget is null && AttackTimer != 0)
			{
				AttackTimer = 0;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}
		}

		public bool CheckActive()
		{
			if (Owner?.active == true)
			{
				if (Owner.DeadOrGhost)
				{
					Owner.ClearBuff(ModContent.BuffType<ElementalBuff>());
					return false;
				}

				if (Owner.HasBuff(ModContent.BuffType<ElementalBuff>()))
				{
					Projectile.timeLeft = 2;
				}

				return true;
			}
			return false;
		}

		public bool MoveToDestination(Vector2 destination)
		{
			Tile tileBelow = AOUtils.GetTile((int)(Projectile.Bottom.X / 16), (int)(Projectile.Bottom.Y / 16));
			if (Stuck)
			{
				StuckJumpSpeed = 0f;
				Projectile.tileCollide = false;

				if (Projectile.DistanceSQ(destination - Vector2.UnitY * 16f) > 10f * 10f)
					Projectile.velocity = Projectile.SafeDirectionTo(destination - Vector2.UnitY * 16f) * 6f;
				else
					StuckWalkThroughWallsTimer = 0;

				StuckWalkThroughWallsTimer -= 4;
				return false;
			}

			Projectile.tileCollide = true;

			if (Math.Abs(Projectile.Center.X - destination.X) < 55 + Math.Abs(Projectile.velocity.X))
			{
				StuckJumpSpeed = 0f;
				Projectile.velocity.X *= 0.8f;
				return true;
			}

			int currentWalkDirection = Math.Sign(Projectile.velocity.X);
			int tilesSearchedAhead = 0;

			Tile tileBelowAhead;

			while (tilesSearchedAhead < 4)
			{
				tileBelowAhead = AOUtils.GetTile((int)(Projectile.Bottom.X / 16) + currentWalkDirection, (int)(Projectile.Bottom.Y / 16));

				if (tileBelowAhead.IsTileSolidGround())
					break;

				tilesSearchedAhead++;
			}

			int directionToWalk = Math.Sign(destination.X - Projectile.Center.X);
			float idealWalkSpeed = 10f * directionToWalk;
			float walkAcceleration = directionToWalk != currentWalkDirection ? 0.325f : 0.2f;
			Projectile.velocity.X = MathHelper.Lerp(Projectile.velocity.X, idealWalkSpeed, walkAcceleration);

			if (tileBelow.IsTileSolidGround() || Collision.SolidCollision(Projectile.Center, 10, 10))
			{
				if (Math.Abs(Projectile.oldPosition.X - Projectile.position.X) < 2f || Collision.SolidCollision(Projectile.Center, 2, 2) || !Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, Owner.position, Owner.width, Owner.height))
				{
					Projectile.velocity.Y = -7f - StuckJumpSpeed;
					StuckJumpSpeed += 3.5f;
					StuckJumpSpeed = Utils.Clamp(StuckJumpSpeed, 0f, 14f);

					StuckWalkThroughWallsTimer += 10f;

					if (Main.myPlayer == Projectile.owner)
					{
						Projectile.netUpdate = true;
						Projectile.netSpam = 0;
					}
				}
				else if (tilesSearchedAhead > 0)
				{
					Projectile.velocity.X = 7f;
					Projectile.velocity.Y = -(5f + tilesSearchedAhead * 2f);
					if (Main.myPlayer == Projectile.owner)
					{
						Projectile.netUpdate = true;
						Projectile.netSpam = 0;
					}
				}
				else
				{
					StuckJumpSpeed = 0f;
					StuckWalkThroughWallsTimer -= 5f;
				}
			}

			if (Projectile.velocity.X > 0)
			{
				Projectile.spriteDirection = 1;
			}
			else
			{
				Projectile.spriteDirection = -1;
			}
			return false;
		}

		public override SpriteEffects FlippedMode => SpriteEffects.FlipHorizontally;

		public override bool DrawWithImbueColours => true;

		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = Projectile.Bottom.Y < Owner.Top.Y;
			return true;
		}

		public override bool TouchingWater()
		{
			Projectile.Center = Owner.Center;
			return true;
		}
	}
}
