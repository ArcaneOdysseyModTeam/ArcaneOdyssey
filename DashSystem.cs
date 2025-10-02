using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Equipment.MusicBoxes;
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

namespace ArcaneOdyssey
{
	public abstract class DashSystem
	{
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
			player.ArcaneOdyssey().Cooldowns[GetType().Name] = Cooldown + DashMax;
		}

		/// <summary>
		/// Whether the dash is on cooldown
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool OnCooldown(Player player)
		{
			return player.ArcaneOdyssey().Cooldowns.ContainsKey(GetType().Name);
		}

		/// <summary>
		/// The speed of the dash per tick
		/// </summary>
		public abstract float DashSpeed { get; }


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

	public class DashPlayer : ModPlayer 
	{
		private DashSystem _dash;
		public DashSystem Dash { get => _dash; set => _dash = !dashing ? value : _dash; }
		public int DashLeft;
		public Vector2 DashVelocity;
		public bool dashing;
		public int collisions;

		public override void PreUpdate()
		{
			if (Dash is not null && dashing)
				DashLeft--;
			else 
				DashLeft = 0;
		}

		public override void ResetEffects()
		{
			if (!dashing)
				Dash = null;
		}


		public bool FirstFrame;
		/// <summary>
		/// Starts a dash, does not check for cooldowns
		/// </summary>
		/// <param name="dashToUse">The dash to use, otherwise use the already selected dash</param>
		/// <param name="direction">The direction of the normal dash, -1 or 1 for horizontal and -2 or 2 for vertical</param>
		public void StartDash(DashSystem dashToUse = null, int direction = 0)
		{
			FirstFrame = true;
			if (dashToUse is not null)
				Dash = dashToUse;

			collisions = 0;
			if (Dash.AnyDirection && direction == 0)
			{
				DashVelocity = Player.SafeDirectionTo(Main.MouseWorld) * Dash.DashSpeed;
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
				DashVelocity = standard * Dash.DashSpeed;
			}
			Dash.OnStart(Player);
			Dash.SetCooldown(Player);
			DashLeft = Dash.DashMax;
			dashing = true;
		}

		public override void PreUpdateMovement()
		{
			if (Dash is not null)
			{
				FirstFrame = Dash.DashMax < DashLeft+2;
				if (!Dash.AnyDirection)
					Player.dashType = DashID.None;
				if (!dashing && !Dash.OnCooldown(Player) && !Player.mount.Active)
				{
					if (Dash.AnyDirection && AOKeybinds.DashBind.JustPressed)
					{
						StartDash();
					}
					else if (!Dash.AnyDirection)
					{
						if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[2] < 15 && Player.doubleTapCardinalTimer[3] == 0)
						{
							StartDash(direction: 1);
						}
						else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[3] < 15 && Player.doubleTapCardinalTimer[2] == 0)
						{
							StartDash(direction: -1);
						}
					}
				}
				if (dashing && !Player.mount.Active)
				{
					Player.noFallDmg = true;
					Dash.DashEffect(Player);

					if (DashVelocity.X != 0)
						Player.direction = (DashVelocity.X > 0).ToDirectionInt();
					
					if (DashLeft <= 0 || (Player.velocity.Y < 1 && Player.velocity.Y > -1 && !FirstFrame))
					{
						Dash.OnEnd(Player);
						dashing = false;
						if (collisions == 0)
						{
							Dash.NaturalEnd(Player);
						}
					}
					else if (Dash.AnyDirection || FirstFrame)
						Player.velocity = DashVelocity; // fly
				}
			}
		}

		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
		{
			if (Dash is not null && dashing)
			{
				if (Dash.OnHit(Player, npc))
				{
					collisions++;
					Dash.OnEnd(Player);
					Dash.SetCooldown(Player);
					dashing = false;
				}

				if (Dash.Damage > 0 && npc.immune[Player.whoAmI] <= 0)
				{
					npc.SimpleStrikeNPC(Dash.Damage, Player.direction, knockBack: Dash.Knockback, damageType: Dash.DamageType);
					npc.immune[Player.whoAmI] = DashLeft;
					Player.immuneTime = Dash.DashMax;
				}

				if (Dash.Immune)
					modifiers.FinalDamage *= 0;
			}
		}
	}
}
