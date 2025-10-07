using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Equipment.MusicBoxes;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons;
using ArcaneOdyssey.Content.NPCS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Generation;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey
{
	public abstract class DashSystem
	{
		public string Name => GetType().Name;

		public Mod Mod { get => ModLoader.GetMod(nameof(ArcaneOdyssey)); }

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
			if (AnyDirection)
				player.ArcaneOdyssey().Cooldowns[GetType().Name] = Cooldown;
			else
				player.ArcaneOdyssey().Cooldowns["StandardDash"] = Cooldown;
		}

		/// <summary>
		/// Whether the dash is on cooldown
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool OnCooldown(Player player)
		{
			if (AnyDirection)
				return player.ArcaneOdyssey().Cooldowns.ContainsKey(GetType().Name) || player.ArcaneOdyssey().dashing;
			else
				return player.ArcaneOdyssey().Cooldowns.ContainsKey("StandardDash") || player.ArcaneOdyssey().dashing;
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
	}

	public partial class AOPlayer : ModPlayer 
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

		public bool FirstFrames;
		/// <summary>
		/// Starts a dash, does not check for cooldowns
		/// </summary>
		/// <param name="dashToUse">The dash to use, otherwise use the already selected dash</param>
		/// <param name="direction">The direction of the normal dash, -1 or 1 for horizontal and -2 or 2 for vertical</param>
		public void StartDash(DashSystem dashToUse, int direction = 0)
		{
			Player.timeSinceLastDashStarted = 0;
			CurrentDash = dashToUse;
			FirstFrames = true;
			collisions = 0;
			ExternalModSupport.SetCalamityDash(dashToUse.Name, Player, dashToUse.AnyDirection);
			if (dashToUse.AnyDirection && direction == 0)
			{
				DashVelocity = Player.Center.DirectionTo(Main.MouseWorld) * dashToUse.DashSpeed;
			}
			else
			{
				var standard = Vector2.UnitX;
				if (direction == 2 || direction == -2)
				{
					standard = Vector2.UnitY * (direction/2f);
				}
				else
				{ 
					standard *= direction; 
				}
				DashVelocity = standard * dashToUse.DashSpeed;
			}
			DashLeft = dashToUse.DashMax;
			dashToUse.OnStart(Player);
			dashing = true;
			if (dashToUse.Immune)
			{
				Player.immuneTime = dashToUse.DashMax;
			}
		}

		public void HandleDashing()
		{
			if (!dashing)
			{
				Dash = null;
				Dash2 = null;
				CurrentDash = null;
				DashDir = null;
			}
			if (ExternalModSupport.CanDoubleTapDash())
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
			else if (ExternalModSupport.DashBind().JustPressed)
			{
				if (Player.velocity.X > 1)
				{
					DashDir = 1;
				}
				else if (Player.velocity.X < -1)
				{
					DashDir = -1;
				}
				else DashDir = Player.direction;
			}
			else DashDir = null;
		}

		public override void PreUpdateMovement()
		{
			if (CompletelyFrozen)
			{
				Player.velocity = Vector2.Zero;
				Player.maxFallSpeed = 0f;
			}
			dashing |= Player.solarDashing || Player.eocDash > 0;
			dashing &= !Immobile;
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
				FirstFrames = CurrentDash.DashMax < DashLeft + 2;
				var dash = CurrentDash;
				if (dashing && !Player.mount.Active && !Player.setSolar)
				{
					ExternalModSupport.SetCalamityDash(dash.Name, Player, dash.AnyDirection);
					DashLeft--;
					Player.noFallDmg = true;

					if (DashVelocity.X != 0)
						Player.direction = (DashVelocity.X > 0).ToDirectionInt();

					if (DashLeft <= 0 || (Player.velocity.Y < 1 && Player.velocity.Y > -1 && Player.velocity.X < 1 && Player.velocity.X > -1 && !FirstFrames))
					{
						dash.SetCooldown(Player);
						dash.OnEnd(Player);
						dashing = false;
						if (collisions == 0)
						{
							dash.NaturalEnd(Player);
						}
					}
					else if (dash.AnyDirection || FirstFrames)
						Player.velocity = DashVelocity; // fly
					dash.DashEffect(Player);
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
						if (CurrentDash.OnHit(Player, npc))
						{
							CurrentDash.OnEnd(Player);
							CurrentDash.SetCooldown(Player);
							dashing = false;
						}

						if (CurrentDash.Damage > 0 && npc.immune[Player.whoAmI] <= 0 && Main.myPlayer == Player.whoAmI)
						{
							npc.SimpleStrikeNPC(DashDamage(npc), Player.direction, knockBack: CurrentDash.Knockback, damageType: CurrentDash.DamageType);
							npc.immune[Player.whoAmI] = 2;
						}
					}
				}
			}
		}

		public int DashDamage(NPC target)
		{
			var modifiers = new DashDamageHelper();
			if (Player.TryGetImbue(out var imbue))
			{
				if (CurrentDash.UseScrollImbue)
					modifiers.FinalDamage += imbue.AOScrollDamage.MultiToPercent();
				else modifiers.FinalDamage += imbue.AOImbueDamage.MultiToPercent();
				if (imbue is CrystalMagic && target.HasBuff<Crystallized>() && GetAOBuffStack(target, target.FindBuffIndex(ModContent.BuffType<Crystallized>())) == 4)
				{
					modifiers.FinalDamage += .3f;
				}

				foreach (var debuff in imbue.ImbueDebuffs) 
				{
					if ((debuff.debuffPercent == 0) || modifiers.GetDamage(CurrentDash.Damage) > (target.lifeMax / debuff.debuffPercent))
					{
						target.AddBuff(debuff.debuffID, debuff.debuffDuration);
					}
				}

				if (imbue.CombinedDebuffs is not null)
				{
					foreach (CombinedDebuff buffkeys in imbue.CombinedDebuffs)
					{
						if (target.HasBuff(buffkeys.requirement) || (buffkeys.requirement == BuffID.Wet && target.wet))
						{
							target.AddBuff(buffkeys.result, buffkeys.duration);
						}
					}
				}

				foreach (MagicBuffMultiplier multiplier in imbue.Effects.magicBuffMultipliers)
				{
					if (target.HasBuff(multiplier.buffID) || (multiplier.buffID == BuffID.Wet && target.wet))
					{
						modifiers.FinalDamage += multiplier.multiplier.MultiToPercent();
					}
				}

				if (Main.netMode == NetmodeID.SinglePlayer) // things would get chaotic in multiplayer if everyone kept clearing eachothers debuffs
				{
					foreach (int buffid in imbue.Effects.clearBuffs)
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
