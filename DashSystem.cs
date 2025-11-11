using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Steamworks;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey
{
	public abstract class DashSystem
	{
		public string Name => GetType().Name;

		public Mod Mod { get => ModLoader.GetMod(ArcaneOdyssey.InternalName); }

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
			player.ArcaneOdyssey().SetCooldown(AOCooldown);
		}

		/// <summary>
		/// Whether the dash is on cooldown
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool OnCooldown(Player player)
		{
			if (AnyDirection)
				return (player.ArcaneOdyssey().OnCooldown(GetType().Name) || player.ArcaneOdyssey().dashing) && !ArcaneOdyssey.devMode;
			else
				return (player.ArcaneOdyssey().OnCooldown("StandardDash") || player.ArcaneOdyssey().dashing) && !ArcaneOdyssey.devMode;
		}

		/// <summary>
		/// Whether the dash is on cooldown
		/// </summary>
		/// <param name="dashType"></param>
		/// <param name="player"></param>
		/// <returns></returns>
		public static bool OnCooldown(Type dashType, Player player)
		{
			var dash = Activator.CreateInstance(dashType) as DashSystem;
			if (dash.AnyDirection)
				return (player.ArcaneOdyssey().OnCooldown(dashType.Name) || player.ArcaneOdyssey().dashing) && !ArcaneOdyssey.devMode;
			else
				return (player.ArcaneOdyssey().OnCooldown("StandardDash") || player.ArcaneOdyssey().dashing) && !ArcaneOdyssey.devMode;
		}

		/// <summary>
		/// The speed of the dash per tick
		/// </summary>
		public abstract float DashSpeed { get; }
		public virtual bool UseScrollImbue => true;


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
		public virtual void NaturalEnd(Player player)
		{

		}

		public Cooldown AOCooldown => new(AnyDirection ? Name : "StandardDash", Mod, true, Cooldown);
	}

	public partial class AOPlayer : ModPlayer, IImbuableEntity
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
		private int? DashDir;
		private float storedWingTime;

		public bool FirstFrame => CurrentDash is not null && DashLeft == CurrentDash.DashMax;

		/// <summary>
		/// Starts a dash, does not check for cooldowns
		/// </summary>
		/// <param name="dashToUse">The dash to use, otherwise use the already selected dash</param>
		/// <param name="direction">The direction of the normal dash, -1 or 1 for horizontal and -2 or 2 for vertical</param>
		public void StartDash(DashSystem dashToUse, int direction = 0)
		{
			storedWingTime = Player.wingTime;
			Player.noFallDmg = true;
			Player.timeSinceLastDashStarted = 0;
			CurrentDash = dashToUse;
			collisions = 0;
			ExternalModSupport.SetCalamityDash(dashToUse.Name, Player, dashToUse.AnyDirection);
			if (dashToUse.AnyDirection && direction == 0)
			{
				DashVelocity = Player.Center.DirectionTo(Main.MouseWorld) * dashToUse.DashSpeed;
			}
			else
			{
				var standard = Vector2.UnitX * direction;
                //if (Player.velocity.Y < 0)
                //standard.Y = -((Player.velocity.Y / 4f).Clamp(0, 20));
				if (direction == 2 || direction == -2)
				{
					standard = Vector2.UnitY * (direction/2f);
				}
				DashVelocity = standard * dashToUse.DashSpeed;
			}
			Player.ConsumeAllExtraJumps();
			DashLeft = dashToUse.DashMax;
			dashToUse.OnStart(Player);
			Player.velocity = DashVelocity;
			dashing = true;
			if (dashToUse.Immune)
			{
				Player.immuneTime = dashToUse.DashMax;
			}
		}

		public void HandleDashDetection()
		{
			if (!dashing)
			{
				Dash = null;
				Dash2 = null;
				CurrentDash = null;
				DashDir = null;
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
					DashDir = null;
				}
			}
			else if (Player.whoAmI == Main.myPlayer && ExternalModSupport.DashBind().JustPressed)
			{
				if (Player.controlRight || Player.direction == 1)
				{
					DashDir = 1;
				}
				else if (Player.controlLeft || Player.direction == -1)
				{
					DashDir = -1;
				}
				else DashDir = Player.direction;
			}
			else DashDir = null;
		}

		public override void PreUpdateMovement()
		{
			FreezeMovement();
			dashing |= Player.solarDashing || Player.eocDash > 0;
			dashing &= !(Immobile || SoftFrozen);
			DashSystem[] dashes = [Dash, Dash2];
			if (Dash2 is not null)
				ExternalModSupport.SetCalamityDash(Dash2.Name, Player);
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
							StartDash(dash);
						}
						else if (!dash.AnyDirection)
						{
							if (DashDir.HasValue)
							{
								StartDash(dash, DashDir.Value);
							}
						}
					}
				}
			}
			if (CurrentDash is not null)
			{
				if (dashing && !Player.mount.Active && !Player.setSolar)
				{
					ExternalModSupport.SetCalamityDash(CurrentDash.Name, Player, CurrentDash.AnyDirection);
					Player.noFallDmg = true;

					if (DashVelocity.X != 0)
						Player.direction = (DashVelocity.X > 0).ToDirectionInt();

					if (DashLeft <= 0 || (Player.velocity.Y < 1 && Player.velocity.Y > -1 && Player.velocity.X < 1 && Player.velocity.X > -1 && !FirstFrame))
					{
						Player.wingTime = storedWingTime;
						CurrentDash.SetCooldown(Player);
						CurrentDash.OnEnd(Player);
						dashing = false;
						if (collisions == 0)
						{
							CurrentDash.NaturalEnd(Player);
						}
						return;
					}

					CurrentDash.DashEffect(Player);
					if (CurrentDash.AnyDirection)
					{
						Player.velocity = DashVelocity;
					}
					else if (FirstFrame)
					{
						Player.velocity += DashVelocity;
					}
					Player.ConsumeAllExtraJumps();
					DashLeft--;
				}
			}
			else
			{
				DashLeft = 0;
				dashing = false;
			}
			Player.eocDash = DashLeft;
		}

		public void DashStrike()
		{
			if (CurrentDash is not null && dashing) 
			{
				foreach (NPC npc in Main.ActiveNPCs)
				{
					if (npc.Hitbox.Intersects(Player.Hitbox))
					{
						collisions++;
						if ((!npc.friendly) && CurrentDash.OnHit(Player, npc))
						{
							CurrentDash.OnEnd(Player);
							CurrentDash.SetCooldown(Player);
							dashing = false;
						}

						if (CurrentDash.Damage > 0 && Main.myPlayer == Player.whoAmI && !npc.friendly && npc.immune[Player.whoAmI] <= 0)
						{
							npc.HitNPC(CalculateDashDamage(npc), Player.direction, Imbue, Player, false, CalculateDashKnockback(), CurrentDash.DamageType, true);
							npc.immune[Player.whoAmI] = 5;
						}
					}
				}
			}
		}

		public int CalculateDashDamage(NPC target)
		{
			var modifiers = new DashDamageHelper();
			if (Imbue is not null)
			{
				if (CurrentDash.UseScrollImbue)
					modifiers.FinalDamage += Imbue.AOScrollDamage.MultiToPercent();
				else modifiers.FinalDamage += Imbue.AOImbueDamage.MultiToPercent();
				if (Imbue is CrystalMagic && target.HasBuff<Crystallized>() && GetAOBuffStack(target, target.FindBuffIndex(ModContent.BuffType<Crystallized>())) == 4)
				{
					modifiers.FinalDamage += .3f;
				}

				foreach (var debuff in Imbue.ImbueDebuffs) 
				{
					if ((debuff.debuffPercent == 0) || modifiers.GetDamage(CurrentDash.Damage) > (target.lifeMax / debuff.debuffPercent))
					{
						target.AddBuff(debuff.debuffID, debuff.debuffDuration);
					}
				}

				if (Imbue.CombinedDebuffs is not null)
				{
					foreach (CombinedDebuff buffkeys in Imbue.CombinedDebuffs)
					{
						if (target.HasBuff(buffkeys.requirement) || (buffkeys.requirement == BuffID.Wet && target.wet))
						{
							target.AddBuff(buffkeys.result, buffkeys.duration);
						}
					}
				}

				foreach (MagicBuffMultiplier multiplier in Imbue.Effects.magicBuffMultipliers)
				{
					if (target.HasBuff(multiplier.buffID) || (multiplier.buffID == BuffID.Wet && target.wet))
					{
						modifiers.FinalDamage += multiplier.multiplier.MultiToPercent();
					}
				}

				if (Main.netMode == NetmodeID.SinglePlayer) // things would get chaotic in multiplayer if everyone kept clearing eachothers debuffs
				{
					foreach (int buffid in Imbue.Effects.clearBuffs)
					{
						if (target.HasBuff(buffid))
						{
							target.DelBuff(target.FindBuffIndex(buffid));
						}
					}
				}
			}

			return modifiers.GetDamage(CurrentDash.Damage);
		}

		public float CalculateDashKnockback()
		{
			var knockback = 1f;
			if (this.TryGetImbue(out Imbuable imbue))
			{
				var extrakbmulti = 1;
				if (imbue is WindMagic or Boxing)
				{
					extrakbmulti = 3;
				}

				if (CurrentDash.UseScrollImbue)
				{
					knockback += imbue.AOScrollSize.MultiToPercent() * extrakbmulti;
				}
				else
				{
					knockback += imbue.AOImbueSize.MultiToPercent() * extrakbmulti;
				}
			}
			return knockback * CurrentDash.Knockback;
		}
	}

	/// <summary>
	/// used so i can copy paste code
	/// </summary>
	public struct DashDamageHelper()
	{
		public StatModifier FinalDamage = new();
		public int GetDamage(int damage)
		{
			return FinalDamage.ApplyTo(damage).Round();
		}
	}
}
