using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

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
		/// Damage of the dash, keep at 0 to deal no damage
		/// </summary>
		public virtual int Damage => 0;

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
		public void SetDash(DashSystem dash)
		{
			if (dash.AnyDirection)
			{
				Dash = dash;
			}
			else Dash2 = dash;
		}

		private DashSystem _dash;
		public DashSystem Dash { get => _dash; set => _dash = !dashing ? value : _dash; }
		private DashSystem _dash2;
		public DashSystem Dash2 { get => _dash2; set => _dash2 = !dashing ? value : _dash2; }
		public DashSystem CurrentDash;
		public int DashLeft;
		public Vector2 DashVelocity;
		public bool dashing;
		public int collisions;
		private int DashDir = 0;
		private float storedWingTime;

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

		/// <summary>
		/// Starts a dash, does not check for cooldowns but will use ExtraCheck
		/// </summary>
		/// <param name="dashToUse">The dash to use, otherwise use the already selected dash</param>
		/// <param name="direction">The direction of the normal dash, -1 or 1 for horizontal and -2 or 2 for vertical</param>
		public void StartDash(DashSystem dashToUse, int direction = 0, Imbuable imbue = null, bool imbueAffectsSpeed = false)
		{
			if (dashToUse.ExtraCheck(Player))
			{
				dashToUse.SetCooldown(Player);
				storedWingTime = Player.wingTime;
				Player.noFallDmg = true;
				Player.timeSinceLastDashStarted = 0;
				CurrentDash = dashToUse;
				CurrentDashDir = direction;
				CurrentDash.Imbue = imbue;
				if (CurrentDash.source.TryGetSecondImbue(imbue, out var second))
					CurrentDash.SecondImbue = second;
				collisions = 0;
				if (dashToUse.AnyDirection && direction == 0)
				{
					DashVelocity = Player.Center.DirectionTo(Main.MouseWorld) * dashToUse.DashSpeed * (imbueAffectsSpeed ? (Imbue is not null ? (CurrentDash.UseScrollImbueStats.HasValue ? (CurrentDash.UseScrollImbueStats.Value ? Imbue.AOScrollSpeed : Imbue.AOImbueSpeed) : 1f) : 1f) : 1f);
				}
				else
				{
					var standard = Vector2.UnitX * direction;
					//if (Player.velocity.Y < 0)
					//standard.Y = -((Player.velocity.Y / 4f).Clamp(0, 20));
					if (direction == 2 || direction == -2)
					{
						standard = Vector2.UnitY * MathHelper.Clamp(direction, -1f, 1f) * Player.gravDir;
					}
					DashVelocity = standard * dashToUse.DashSpeed;
				}
				if (imbueAffectsSpeed && imbue is not null && CurrentDash.UseScrollImbueStats.HasValue)
				{
					if (CurrentDash.UseScrollImbueStats.Value)
					{
						DashVelocity *= imbue.AOScrollSpeed;
						if (CurrentDash.SecondImbue is not null)
						{
							DashVelocity *= CurrentDash.SecondImbue.AOScrollSpeed.MultiToPercent();
						}
					}
					else
					{
						DashVelocity *= imbue.AOImbueSpeed;
						if (CurrentDash.SecondImbue is not null)
						{
							DashVelocity *= CurrentDash.SecondImbue.AOImbueSpeed.MultiToPercent();
						}
					}
				}
				Player.ConsumeAllExtraJumps();
				DashLeft = dashToUse.DashMax;
				dashToUse.OnStart(Player);
				if (dashToUse.AnyDirection)
				{
					Player.StopExtraJumpInProgress();
					Player.blockExtraJumps = true;
					Player.velocity = DashVelocity;
				}
				dashing = true;
				if (dashToUse.Immune)
				{
					Player.immuneTime = dashToUse.DashMax;
				}
			}
		}

		public void HandleDashDetection()
		{
			if (!dashing)
			{
				Dash = null;
				Dash2 = null;
				CurrentDash = null;
				DashDir = 0;
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
			dashing &= !(Immobile || SoftFrozen);
			DashSystem[] dashes = [Dash, Dash2];
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
							StartDash(dash, imbue: Imbue, imbueAffectsSpeed: true);
						}
						else if (!dash.AnyDirection)
						{
							if (DashDir != 0)
							{
								StartDash(dash, DashDir, Imbue, true);
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
						if (!Player.mount.Active)
							Player.wingTime = storedWingTime;
						CurrentDash.SetCooldown(Player);
						CurrentDash.OnEnd(Player);
						dashing = false;
						if (collisions == 0)
						{
							CurrentDash.NaturalEnd(Player);
						}
						for (int i = 0; i < (DashLeft + 300) / 30; i++)
						{
							if (CurrentDash.UseScrollImbueStats.HasValue && CurrentDash.Imbue is not null)
							{
								CurrentDash.Imbue.ExplosionEffects(Player);
								CurrentDash.SecondImbue?.ExplosionEffects(Player);
							}
						}
						return;
					}

					CurrentDash.Imbue?.LingeringEffects(Player);
					if (CurrentDash.UseScrollImbueStats.GetValueOrDefault() && CurrentDash.SecondImbue is not null)
					{
						CurrentDash.SecondImbue.LingeringEffects(Player);
					}
					CurrentDash.DashEffect(Player);
					if (CurrentDash.AnyDirection)
					{
						Player.noFallDmg = true;
						Player.velocity = DashVelocity;
						Player.blockExtraJumps = true;
					}
					else if (FirstFrame)
					{
						Player.velocity = Vector2.Clamp(Player.velocity + DashVelocity, (Player.velocity + DashVelocity).SafeNormalize(Vector2.Zero) * (-MaxDashSpeed), (Player.velocity + DashVelocity).SafeNormalize(Vector2.Zero) * MaxDashSpeed);
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
			if (Main.myPlayer == Player.whoAmI && CurrentDash is not null && dashing)
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

						if (CurrentDash.Damage > 0)
						{
							var damagetype = CurrentDash.DamageType.Imbued(Imbue);
							npc.HitNPC(CalculateDashDamage(npc), Player.direction, Imbue, Player, Main.rand.Next(100) < Player.GetTotalCritChance(damagetype), CalculateDashKnockback(), damagetype, true);
						}
					}
				}
			}
		}

		public int CalculateDashDamage(NPC target)
		{
			var modifiers = new ModDamageHelper(null);
			if (CurrentDash is null)
				return 0;
			modifiers.FinalDamage += Player.GetDamage(CurrentDash.DamageType).Additive.MultiToPercent();
			modifiers.FinalDamage *= Player.GetDamage(CurrentDash.DamageType).Multiplicative;
			if (CurrentDash?.Imbue is not null)
			{
				modifiers = CalculateImbueDamage(CurrentDash.Imbue, target, modifiers);
				if (CurrentDash.UseScrollImbueStats.HasValue)
				{
					if (CurrentDash.UseScrollImbueStats.Value)
					{
						modifiers.FinalDamage += CurrentDash.Imbue.AOScrollDamage.MultiToPercent();

						if (CurrentDash.SecondImbue is not null)
						{
							modifiers.FinalDamage += CurrentDash.SecondImbue.AOScrollDamage.MultiToPercent();
							modifiers = CalculateImbueDamage(CurrentDash.SecondImbue, target, modifiers);
						}
					}
					else
					{
						modifiers.FinalDamage += CurrentDash.Imbue.AOImbueDamage.MultiToPercent();

						if (CurrentDash.SecondImbue is not null)
						{
							modifiers.FinalDamage += CurrentDash.SecondImbue.AOImbueDamage.MultiToPercent();
							modifiers = CalculateImbueDamage(CurrentDash.SecondImbue, target, modifiers);
						}
					}
				}
			}

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
