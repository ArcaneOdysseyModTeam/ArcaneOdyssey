using ArcaneOdyssey.Content.Buffs.Minions;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ArcaneOdyssey.Content.Projectiles.Relics
{
	public class SpiritMinion : SpiritProjectile, ILocalizedModType
	{
		public override string LocalizationCategory => base.LocalizationCategory + ".Minions";

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
		}

		public override string Texture => Mod.Name + "/Backgrounds/Blank";

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Player.defaultWidth;
			Projectile.height = Player.defaultHeight;
			Projectile.minionSlots = 1;
			Projectile.minion = true;
			Projectile.netImportant = true;
			Projectile.Opacity = .5f;
		}

		private NPC potentialTarget;

		public override bool? CanDamage() => false;

		public override void AI()
		{
			CheckActive();
			if (Projectile.velocity.Y < 15f)
				Projectile.velocity.Y += Gravity;

			Vector2 destination;
			potentialTarget = Projectile.Center.GetMinionTarget(900f, Owner);
			if (potentialTarget is null)
				destination = Owner.Center - Vector2.UnitX * (80f + (Projectile.identity * 28f) % 560f) * Owner.direction;
			else
			{
				Vector2 destA = potentialTarget.Center + Vector2.UnitX * (130f + (Projectile.identity * 28f) % 560f);
				Vector2 destB = potentialTarget.Center - Vector2.UnitX * (130f + (Projectile.identity * 28f) % 560f);
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
				AttackTimer++;
				if (AttackTimer == 1)
				{
					Projectile.netUpdate = true;
				}

				if (AttackTimer >= 30)
				{
					AttackTimer = 0;
					Projectile.netUpdate = true;
				}

				if (MathHelper.Distance(potentialTarget.Center.X, Projectile.Center.X) > 30f)
					Projectile.spriteDirection = (potentialTarget.Center.X - Projectile.Center.X < 0).ToDirectionInt();

				if (AttackTimer == 15 && Main.myPlayer == Projectile.owner)
				{
					Vector2 initialVelocity = Projectile.SafeDirectionTo(potentialTarget.Center + (potentialTarget.velocity * 15f)) * 7f;
					AOUtils.ShootProjectile(Projectile.GetSource_FromThis(), Projectile.Center, initialVelocity, ModContent.ProjectileType<SpiritBlast>(), Projectile.damage / 3, Projectile.knockBack, Projectile.owner, Imbue, SecondImbue, true);
				}
			}
			else if (potentialTarget is null && AttackTimer != 0)
			{
				AttackTimer = 0;
				Projectile.netUpdate = true;
			}
		}

		public bool CheckActive()
		{
			if (Owner.dead || !Owner.active)
			{
				Owner.ClearBuff(ModContent.BuffType<SpiritMinionBuff>());
				return false;
			}

			if (Owner.HasBuff(ModContent.BuffType<SpiritMinionBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			return true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Player player = Main.playerVisualClone[Projectile.owner] ??= new();
			player.CopyVisuals(Owner);
			player.isFirstFractalAfterImage = true;
			player.firstFractalAfterImageOpacity = Projectile.Opacity;
			player.ResetEffects();
			player.ResetVisibleAccessories();
			player.UpdateDyes();
			player.DisplayDollUpdate();
			player.UpdateSocialShadow();
			player.Center = Projectile.Center;
			player.position.Y -= 5;
			player.direction = (Projectile.velocity.X > 0f) ? 1 : -1;
			player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation - MathHelper.PiOver2);
			player.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
			player.PlayerFrame();
			player.socialIgnoreLight = true;
			Main.PlayerRenderer.DrawPlayer(Main.Camera, player, player.position, 0f, player.fullRotationOrigin, 0f, .9f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
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

					Projectile.netUpdate = true;
				}
				else if (tilesSearchedAhead > 0)
				{
					Projectile.velocity.X = 7f;
					Projectile.velocity.Y = -(5f + tilesSearchedAhead * 2f);
					Projectile.netUpdate = true;
				}
				else
				{
					StuckJumpSpeed = 0f;
					StuckWalkThroughWallsTimer -= 5f;
				}
			}

			Projectile.spriteDirection = (Owner.Center.X - Projectile.Center.X < 0).ToDirectionInt();
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity) => false;
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = Projectile.Bottom.Y < Owner.Top.Y;
			return true;
		}
	}
}
