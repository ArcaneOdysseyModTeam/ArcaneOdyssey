using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.PlayerClasses
{
	public abstract class DashSystem(Entity source) : IImbuable
	{
		public Entity source = source;
		public Imbuable Imbue { get; set; }
		public Imbuable SecondImbue { get; set; }

		public string Name => GetType().Name;

		public static Mod Mod => ArcaneOdysseyMod.Instance;

		/// <summary>
		/// Whether the player is immune to contact damage while dashing, does not affect projectiles
		/// </summary>
		public abstract bool Immune { get; }

		/// <summary>
		/// Damage of the dash, set to 0 to disable damage
		/// </summary>
		public virtual int Damage
		{
			get
			{
				if (source is Projectile projectile)
				{
					return projectile.damage;
				}
				if (source is Item item)
				{
					return item.damage;
				}
				return 0;
			}
		}

		public virtual DamageClass DamageType => DamageClass.Default;

		/// <summary>
		/// Knockback of the dash
		/// </summary>
		public virtual float Knockback => 0;

		/// <summary>
		/// Whether the dash can be trigger via hotkey, and if it can be used to go directions other than left and right
		/// </summary>
		public abstract bool AnyDirection { get; }

		/// <summary>
		/// The cooldown between dash uses
		/// </summary>
		public abstract int Cooldown { get; }

		/// <summary>
		/// How long the dash lasts for
		/// </summary>
		public abstract int DashMax { get; }


		/// <summary>
		/// Sets the dash's cooldown
		/// </summary>
		/// <param name="player"></param>
		public void SetCooldown(Player player)
		{
			if (DisplayedCooldownID != -1)
			{
				player.ArcaneOdyssey()?.SetCooldown(DisplayedCooldownID, Cooldown);
			}
			else
				player.ArcaneOdyssey()?.SetCooldown(AOCooldown);
		}

		/// <summary>
		/// Whether the dash is on cooldown
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool OnCooldown(Player player)
		{
			if (DisplayedCooldownID != -1)
			{
				return player.ArcaneOdyssey().OnCooldown(DisplayedCooldownID) && !ArcaneOdysseyMod.DevMode;
			}
			if (AnyDirection)
				return (player.ArcaneOdyssey().OnCooldown(GetType().Name) || player.ArcaneOdyssey().dashing) && !ArcaneOdysseyMod.DevMode;
			else
				return (player.ArcaneOdyssey().OnCooldown("StandardDash") || player.ArcaneOdyssey().dashing) && !ArcaneOdysseyMod.DevMode;
		}

		/// <summary>
		/// Called every frame, and before the dash starts
		/// </summary>
		/// <param name="player"></param>
		/// <returns>Whether to keep dashing</returns>
		public virtual bool ExtraCheck(Player player) => true;

		/// <summary>
		/// The speed of the dash per tick
		/// </summary>
		public abstract float DashSpeed { get; }
		public bool? UseScrollImbueStats => source.AnyArcaneOdyssey()?.BenifitsFromScrollStats;


		/// <summary>
		/// called every frame
		/// </summary>
		/// <param name="player"></param>
		public virtual void DashEffect(Player player) { }

		/// <summary>
		/// called once at start of dash
		/// </summary>
		/// <param name="player"></param>
		public virtual void OnStart(Player player) { }

		/// <summary>
		/// Called when the dash collisions a target
		/// </summary>
		/// <param name="player"></param>
		/// <param name="target"></param>
		/// <returns>Whether to end the dash</returns>
		public abstract bool OnHit(Player player, Entity target);

		public virtual void OnEnd(Player player) { }

		/// <summary>
		/// Called if the dash ends naturally without hitting any enemies
		/// </summary>
		public virtual void NaturalEnd(Player player) { }

		public virtual int DisplayedCooldownID => -1;

		public Cooldown AOCooldown => new(AnyDirection ? Name : "StandardDash", Mod, Cooldown);
	}

	public partial class AOPlayer : ModPlayer, IImbuable
	{
		public void SetDash(DashSystem dash, int dir = 0)
		{
			if (dash.AnyDirection)
			{
				OmniDash = dash;
				OmniDashDir = dir;
			}
			else SideDash = dash;
		}

		private DashSystem _dash;
		public DashSystem OmniDash { get => _dash; set => _dash = !dashing ? value : _dash; }
		public int OmniDashDir = 0;
		private DashSystem _dash2;
		public DashSystem SideDash { get => _dash2; set => _dash2 = !dashing ? value : _dash2; }
		public DashSystem CurrentDash;
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
		public void StartDash(DashSystem dashToUse, int direction = 0, Imbuable imbue = null, bool imbueAffectsSpeed = false)
		{
			if (dashToUse.ExtraCheck(Player))
			{
				dashToUse.SetCooldown(Player);
				Player.timeSinceLastDashStarted = 0;
				CurrentDash = dashToUse;
				CurrentDashDir = direction;
				if (CurrentDash.UseScrollImbueStats.HasValue || (CurrentDash.source is Item item1 && item1.ModItem is Imbuable))
				{
					CurrentDash.Imbue = imbue;
					if (CurrentDash.source.TryGetSecondImbue(imbue, out var second))
						CurrentDash.SecondImbue = second;
					else if (CurrentDash.source is Item item && item.ModItem is Imbuable imbue2)
					{
						CurrentDash.SecondImbue = imbue2.Imbue;
					}
				}
				collisions = 0;
				if (direction == 0)
				{
					DashVelocity = Player.Center.DirectionTo(Main.MouseWorld) * dashToUse.DashSpeed;
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
					if (CurrentDash.UseScrollImbueStats.HasValue)
					{
						if (CurrentDash.UseScrollImbueStats.Value)
						{
							DashVelocity *= CurrentDash.Imbue.AOScrollSpeed;
							dashmaxmult *= CurrentDash.Imbue.AOScrollSpeed;
							if (CurrentDash.SecondImbue is not null)
							{
								DashVelocity *= CurrentDash.SecondImbue.AOScrollSpeed;
								dashmaxmult *= CurrentDash.SecondImbue.AOScrollSpeed;
							}
						}
						else
						{
							DashVelocity *= CurrentDash.Imbue.AOImbueSpeed;
							dashmaxmult *= CurrentDash.Imbue.AOImbueSpeed;
							if (CurrentDash.SecondImbue is not null)
							{
								DashVelocity *= CurrentDash.SecondImbue.AOImbueSpeed;
								dashmaxmult *= CurrentDash.SecondImbue.AOImbueSpeed;
							}
						}
					}
				}
				DashLeft = dashToUse.DashMax;
				dashToUse.OnStart(Player);
				Player.velocity += DashVelocity;
				if (dashToUse.AnyDirection)
				{
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

		public const int DashBoxExtraBoost = 8;
		public int CurrentDashDir;

		public override void PreUpdateMovement()
		{
			FreezeMovement();
			dashing |= Player.solarDashing || Player.eocDash > 0;
			dashing &= !(Immobile || HeavySkillActive);
			DashSystem[] dashes = [OmniDash, SideDash];
			foreach (DashSystem dash in dashes)
			{
				if (dash is not null)
				{
					if (!dash.AnyDirection)
						Player.dashType = DashID.None;
					if (!dashing && !dash.OnCooldown(Player) && !Player.mount.Active && !Player.setSolar)
					{
						if (dash.AnyDirection && AOKeybinds.DashBind.JustPressed)
						{
							StartDash(dash, OmniDashDir, Imbue, true);
						}
						else if (!dash.AnyDirection)
						{
							if (DashDir != 0)
							{
								StartDash(dash, DashDir, Imbue);
							}
						}
					}
				}
			}
			if (CurrentDash is not null)
			{
				if (dashing)
				{
					if (DashVelocity.X != 0)
						Player.ChangeDir(Math.Sign(DashVelocity.X));

					Point upwardTilePoint = (Player.Center + new Vector2(MathHelper.Clamp(CurrentDashDir, -1f, 1f) * Player.width / 2 + 2, Player.gravDir * -Player.height / 2f + Player.gravDir * 2f)).ToTileCoordinates();
					Point aheadTilePoint = (Player.Center + new Vector2(MathHelper.Clamp(CurrentDashDir, -1f, 1f) * Player.width / 2 + 2, 0f)).ToTileCoordinates();
					if (WorldGen.SolidOrSlopedTile(upwardTilePoint.X, upwardTilePoint.Y) || WorldGen.SolidOrSlopedTile(aheadTilePoint.X, aheadTilePoint.Y) || (Player.velocity.Y < 1 && Player.velocity.Y > -1 && Player.velocity.X < 1 && Player.velocity.X > -1 && !FirstFrame && CurrentDash.AnyDirection))
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

					CurrentDash.Imbue?.LingeringEffects(AOUtils.ScaleRectangleNotRef(Player.Hitbox, 1.5f), Player.velocity, Player);
					CurrentDash.SecondImbue?.LingeringEffects(AOUtils.ScaleRectangleNotRef(Player.Hitbox, 1.5f), Player.velocity, Player);

					CurrentDash.DashEffect(Player);
					if (CurrentDash.AnyDirection)
					{
						Player.noFallDmg = true;
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

		internal int DashStrikeCooldown = 0;

		public void DashStrike()
		{
			if (CurrentDash is not null && dashing)
			{
				var hitbox = new Rectangle((int)(Player.position.X + (Player.velocity.X * 0.5f) - (DashBoxExtraBoost / 2f)), (int)(Player.position.Y + (Player.velocity.Y * 0.5f) - (DashBoxExtraBoost / 2f)), Player.width + DashBoxExtraBoost, Player.height + DashBoxExtraBoost);
				foreach (NPC npc in Main.ActiveNPCs)
				{
					if (DashStrikeCooldown <= 0 && hitbox.Intersects(npc.getRect()) && (npc.noTileCollide || Player.CanHit(npc)))
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

						if (CurrentDash.Damage > 0 && Main.myPlayer == Player.whoAmI)
						{
							var damagetype = CurrentDash.DamageType.Imbued(Imbue, CurrentDash.source is Item item ? item : null);
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
						knockback += Imbue.AOScrollSize.MultiToPercent();
					}
					else
					{
						knockback += Imbue.AOImbueSize.MultiToPercent();
					}

					if (CurrentDash.SecondImbue is not null)
						knockback += CurrentDash.SecondImbue.AOImbueSize.MultiToPercent();
				}
			}
			return knockback.ApplyTo(CurrentDash.Knockback);
		}
	}
}
