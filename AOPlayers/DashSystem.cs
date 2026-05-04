using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Imbues.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.AOPlayers
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		public void SetDash(ModDash dash, int dir = 0)
		{
			if (dash.LocksPlayer)
			{
				OmniDash = dash;
				OmniDashDir = dir;
			}
			else SideDash = dash;
		}

		private ModDash _dash;
		public ModDash OmniDash { get => _dash; set => _dash = !dashing ? value : _dash; }
		public int OmniDashDir = 0;
		private ModDash _dash2;
		public ModDash SideDash { get => _dash2; set => _dash2 = !dashing ? value : _dash2; }
		public ModDash CurrentDash;
		public int DashLeft;
		public Vector2 DashVelocity;
		public bool dashing;
		public int collisions;
		private int DashDir = 0;

		public float DashLerp
		{
			get
			{
				if (CurrentDash is not null)
				{
					if (DashLeft >= CurrentDash.DashMax / 2f)
					{
						return DashLeft / (CurrentDash.DashMax / 2f) - 1f;
					}
					else
					{
						return 1f - (DashLeft / (CurrentDash.DashMax / 2f));
					}
				}
				return 0f;
			}
		}

		public bool FirstFrame => CurrentDash is not null && DashLeft == CurrentDash.DashMax;

		private float dashmaxmult = 1f;

		/// <summary>
		/// Starts a dash, does not check for cooldowns but will use ExtraCheck
		/// </summary>
		/// <param name="dashToUse">The dash to use, otherwise use the already selected dash</param>
		/// <param name="direction">The direction of the normal dash, leave 0 for any direction<para>-1 is left, 1 is right</para><para>-2 is up, 2 is down</para><para>-3 is left up diagonal, 3 is right up diagonal</para><para>-4 is left down diagonal, 4 is right down diagonal</para></param>
		public void StartDash(ModDash dashToUse, int direction = 0, Imbuable imbue = null, bool imbueAffectsSpeed = false)
		{
			if (dashToUse.ExtraCheck(Player))
			{
				dashToUse.SetCooldown(Player);
				Player.timeSinceLastDashStarted = 0;
				CurrentDash = dashToUse;
				CurrentDashDir = direction;
				if (CurrentDash.UseScrollImbueStats.HasValue || (CurrentDash.Source is Item item1 && item1.ModItem is Imbuable))
				{
					CurrentDash.Imbue = imbue;
					if (CurrentDash.Source.TryGetSecondImbue(imbue, out var second))
						CurrentDash.SecondImbue = second;
					else if (CurrentDash.Source is Item item && item.ModItem is Imbuable imbue2)
					{
						CurrentDash.SecondImbue = imbue2.Imbue;
					}
				}
				collisions = 0;
				if (direction == 0)
				{
					DashVelocity = Player.SafeDirectionTo(Main.MouseWorld) * dashToUse.DashSpeed;
				}
				else
				{
					Vector2 standard;
					direction *= Math.Sign(Player.gravDir);
					if (Math.Abs(direction) == 2)
					{
						if (Math.Sign(Player.velocity.Y) != Math.Sign(direction))
						{
							Player.velocity.Y /= 4;
						}
						standard = Vector2.UnitY * Math.Sign(direction);
					}
					else if (Math.Abs(direction) == 3)
					{
						if ((Player.velocity.Y * Math.Abs(direction)) > 0)
						{
							Player.velocity.Y /= 4;
						}
						if (Math.Sign(Player.velocity.X) != Math.Sign(direction))
						{
							Player.velocity.X = 0f;
						}
						standard = Vector2.One * .707f;
						standard.X *= Math.Sign(direction);
						standard.Y *= -Player.gravDir;
					}
					else if (Math.Abs(direction) == 4)
					{
						if ((Player.velocity.Y * Math.Abs(direction)) < 0)
						{
							Player.velocity.Y /= 4;
						}
						if (Math.Sign(Player.velocity.X) != Math.Sign(direction))
						{
							Player.velocity.X = 0f;
						}
						standard = Vector2.One * .707f;
						standard.X *= Math.Sign(direction);
						standard.Y *= Player.gravDir;
					}
					else
					{
						if (Math.Sign(Player.velocity.X) != direction)
						{
							Player.velocity.X = 0f;
						}
						standard = Vector2.UnitX * direction;
					}
					DashVelocity = standard * dashToUse.DashSpeed;
				}
				dashmaxmult = 1f;
				if (imbueAffectsSpeed && CurrentDash.Imbue is not null)
				{
					DashVelocity *= CurrentDash.ApplySpeed(1f);
					dashmaxmult = CurrentDash.ApplySpeed(dashmaxmult);
				}
				DashLeft = dashToUse.DashMax;
				dashToUse.OnStart(Player);
				Player.velocity += DashVelocity;
				if (dashToUse.LocksPlayer)
				{
					Player.noFallDmg = true;
					Player.StopExtraJumpInProgress();
					Player.blockExtraJumps = true;
					Player.velocity.Y = MathHelper.Clamp(Player.velocity.Y, -CurrentDash.DashSpeed * dashmaxmult, CurrentDash.DashSpeed * dashmaxmult);
					Player.velocity.X = MathHelper.Clamp(Player.velocity.X, -CurrentDash.DashSpeed * dashmaxmult, CurrentDash.DashSpeed * dashmaxmult);
				}
				else
				{
					Player.velocity.Y = MathHelper.Clamp(Player.velocity.Y, -MaxDashSpeed, MaxDashSpeed);
					Player.velocity.X = MathHelper.Clamp(Player.velocity.X, -MaxDashSpeed, MaxDashSpeed);
				}
				dashing = true;
			}
		}

		public float MaxDashSpeed => CurrentDash.DashSpeed * (CurrentDash.Imbue is not null ? CurrentDash.Imbue.DashSpeed : 1.2f);

		public void HandleDashDetection()
		{
			if (!dashing)
			{
				OmniDash = null;
				OmniDashDir = 0;
				SideDash = null;
				CurrentDash = null;
			}
			if (Player.whoAmI == Main.myPlayer && ExternalModSupport.CanDoubleTapDash())
			{
				if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[2] < 15)
				{
					DashDir = 1;
				}
				else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[3] < 15)
				{
					DashDir = -1;
				}
				else
				{
					DashDir = 0;
				}
			}
			else if (Player.whoAmI == Main.myPlayer && ExternalModSupport.DashBind().JustPressed)
			{
				if (Player.controlRight && !Player.controlLeft)
				{
					DashDir = 1;
				}
				else if (Player.controlLeft && !Player.controlRight)
				{
					DashDir = -1;
				}
				else
				{
					if (MathF.Abs(Player.velocity.X) <= .01f)
						DashDir = Player.direction;
					else
						DashDir = Math.Sign(Player.velocity.X);
				}
			}
			else DashDir = 0;
		}

		public float DashBoxExtraBoost => CurrentDash.ApplySize(8f, player: Player);
		public int CurrentDashDir;

		public override void PreUpdateMovement()
		{
			FreezeMovement();
			dashing |= Player.solarDashing || Player.eocDash > 0;
			dashing &= !(Immobile || HeavySkillActive);

			if (OmniDash is not null)
			{
				if (!dashing && !OmniDash.OnCooldown(Player) && !Player.mount.Active && !Player.setSolar)
				{
					if (AOKeybinds.DashBind.JustPressed)
					{
						StartDash(OmniDash, OmniDashDir, Imbue, true);
					}
				}
			}

			if (SideDash is not null)
			{
				Player.dashType = DashID.None;
				if (!dashing && !SideDash.OnCooldown(Player) && !Player.mount.Active && !Player.setSolar)
				{
					if (DashDir != 0)
					{
						StartDash(SideDash, DashDir, Imbue);
					}
				}
			}

			if (CurrentDash is not null)
			{
				if (dashing)
				{
					if (CurrentDash.LocksPlayer)
					{
						Player.noFallDmg = true;
					}

					if (CurrentDash.LocksPlayer && DashVelocity.Y == 0)
					{
						Player.velocity.Y *= .01f;
					}

					if (DashVelocity.X != 0)
					{
						Player.ChangeDir(Math.Sign(DashVelocity.X));
					}
					else if (CurrentDash.LocksPlayer)
					{
						Player.velocity.X *= .01f;
					}

					Point upwardTilePoint = (Player.Center + new Vector2(MathHelper.Clamp(CurrentDashDir, -1f, 1f) * Player.width / 2 + 2, Player.gravDir * -Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
					Point aheadTilePoint = (Player.Center + new Vector2(MathHelper.Clamp(CurrentDashDir, -1f, 1f) * Player.width / 2 + 2, 0f)).ToTileCoordinates();
					if (WorldGen.SolidOrSlopedTile(upwardTilePoint.X, upwardTilePoint.Y) || WorldGen.SolidOrSlopedTile(aheadTilePoint.X, aheadTilePoint.Y) || (Player.velocity.Y < 1 && Player.velocity.Y > -1 && Player.velocity.X < 1 && Player.velocity.X > -1 && !FirstFrame && CurrentDash.LocksPlayer))
					{
						DashLeft = 0;
						Player.velocity /= 2f;
					}

					if (Player.mount.Active || Player.setSolar || (!CurrentDash.ExtraCheck(Player)) || DashLeft <= 0)
					{
						CurrentDash.SetCooldown(Player);
						dashing = false;
						CurrentDash.OnEnd(Player);
						if (collisions == 0)
						{
							CurrentDash.NaturalEnd(Player);
						}
						for (int i = 0; i < (DashLeft + 300) / 30; i++)
						{
							if (CurrentDash.Imbue is not null)
							{
								CurrentDash.Imbue.ExplosionEffects(Player.MountedCenter);
								CurrentDash.SecondImbue?.ExplosionEffects(Player.MountedCenter);
							}
						}
						return;
					}
					CurrentDash.Imbue?.LingeringEffects(Player.Hitbox.Scaled(1.5f), Player.velocity, Player);
					CurrentDash.SecondImbue?.LingeringEffects(Player.Hitbox.Scaled(1.5f), Player.velocity, Player);
					CurrentDash.DashEffect(Player);
					if (CurrentDash.LocksPlayer)
					{
						Player.velocity += DashVelocity;
						Player.velocity.Y = MathHelper.Clamp(Player.velocity.Y, -CurrentDash.DashSpeed * dashmaxmult, CurrentDash.DashSpeed * dashmaxmult);
						Player.velocity.X = MathHelper.Clamp(Player.velocity.X, -CurrentDash.DashSpeed * dashmaxmult, CurrentDash.DashSpeed * dashmaxmult);
						Player.blockExtraJumps = true;
						Player.controlLeft = false;
						Player.controlRight = false;
						Player.controlJump = false;
					}
					DashLeft--;
				}
			}
			else
			{
				DashLeft = 0;
				dashing = false;
			}
			Player.eocDash = DashLeft;
			DashStrikeCooldown--;
		}

		public override void PostUpdateMiscEffects()
		{
			if (CurrentDash is not null)
			{
				if (dashing)
				{
					if (CurrentDash.FallThrough && DashVelocity.Y > 0)
					{
						Player.GoingDownWithGrapple = true;
					}
				}
			}

			if (Player.InModBiome<EliusArena>() && InSpace)
			{
				Player.gravity = Player.defaultGravity;
				if (Player.wet)
				{
					if (Player.honeyWet)
					{
						Player.gravity = 0.1f;
					}
					else if (Player.merman)
					{
						Player.gravity = 0.3f;
					}
					else if (Player.trident && !Player.lavaWet)
					{
						Player.gravity = Player.controlUp ? 0.1f : 0.25f;
					}
					else
					{
						Player.gravity = 0.2f;
					}
				}
			}
		}

		internal int DashStrikeCooldown = 0;

		public void DashStrike()
		{
			if (CurrentDash is not null && dashing)
			{
				var hitbox = Utils.CenteredRectangle(Player.Center + (Player.velocity / 2f), new(Player.width + DashBoxExtraBoost, Player.height + DashBoxExtraBoost));
				foreach (NPC npc in Main.ActiveNPCs)
				{
					if (DashStrikeCooldown <= 0 && hitbox.Intersects(npc.Hitbox) && (npc.noTileCollide || Player.CanHit(npc)))
					{
						DashStrikeCooldown = 10;
						collisions++;
						if (CurrentDash.OnHit(Player, npc))
						{
							CurrentDash.OnEnd(Player);
							CurrentDash.SetCooldown(Player);
							dashing = false;
						}

						if (CurrentDash.Immune)
							Player.GiveImmuneTimeForCollisionAttack(12);

						if (CurrentDash.ContactDamage && Main.myPlayer == Player.whoAmI)
						{
							var damagetype = CurrentDash.DamageType;
							npc.HitNPC(CalculateDashDamage(npc), Player.direction, Imbue, Player, Main.rand.Next(100) < Player.GetTotalCritChance(damagetype), CalculateDashKnockback(), damagetype, true);
						}
					}
				}
			}
		}

		public int CalculateDashDamage(NPC target)
		{
			if (CurrentDash is null)
				return 0;
			var modifiers = new ModDamageHelper(null);
			modifiers = AOUtils.CalculateImbueDamage(CurrentDash.Imbue, target, modifiers);
			modifiers = AOUtils.CalculateImbueDamage(CurrentDash.SecondImbue, target, modifiers);

			return modifiers.GetDamage(CurrentDash.Damage);
		}

		public float CalculateDashKnockback()
		{
			if (CurrentDash is null)
				return 0;
			var knockback = Player.GetKnockback(CurrentDash.DamageType);
			if (Imbue is not null)
			{
				knockback *= Imbue.KBMulti;
				if (CurrentDash.UseScrollImbueStats.HasValue)
				{
					if (CurrentDash.UseScrollImbueStats.Value)
					{
						knockback += Imbue.ScrollSize.MultiToPercent();
					}
					else
					{
						knockback += Imbue.ImbueSize.MultiToPercent();
					}

					if (CurrentDash.SecondImbue is not null)
						knockback += CurrentDash.SecondImbue.ImbueSize.MultiToPercent();
				}
			}
			return knockback.ApplyTo(CurrentDash.Knockback);
		}
	}
}
