using ArcaneOdyssey.Content.Items.Equipment.MusicBoxes;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons;
using ArcaneOdyssey.Content.NPCS;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
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
		public abstract bool Immune {  get; }
		public abstract string Name { get; }

		public virtual int Damage => 0;
		public virtual DamageClass DamageType => DamageClass.Default;
		public virtual float Knockback => 0;

		public abstract bool AnyDirection { get; }

		public abstract int Cooldown { get; }

		public int DashLeft;
		public abstract int DashMax { get; }

		public void SetCooldown(Player player)
		{
			player.ArcaneOdyssey().Cooldowns[Name] = (Cooldown * (ImbueAffected ? player.Imbue().AOScrollSpeed : 1f)).Round();
		}

		public bool OnCooldown(Player player)
		{
			return player.ArcaneOdyssey().Cooldowns.ContainsKey(Name);
		}

		public abstract float DashSpeed { get; }

		public Vector2 DashVelocity;

		public bool dashing;

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
		/// Called when the dash hits a target
		/// </summary>
		/// <param name="player"></param>
		/// <param name="target"></param>
		/// <returns>Whether to end the dash</returns>
		public abstract bool OnHit(Player player, Entity target);

		public virtual void OnEnd(Player player) { }

		public virtual bool ImbueAffected => true;
	}

	public class DashPlayer : ModPlayer 
	{
		public DashSystem dash = null;

		public override void PreUpdate()
		{
			if (dash is not null && dash.dashing)
				dash.DashLeft--;
		}

		public override void ResetEffects()
		{
		}

		public override void PreUpdateMovement()
		{
			if (dash is not null && (!dash.ImbueAffected || Player.Imbue() is not null))
			{
				if (!dash.dashing && !dash.OnCooldown(Player))
				{
					if (dash.AnyDirection && AOKeybinds.DashBind.JustPressed)
					{
						dash.DashVelocity = Player.SafeDirectionTo(Main.MouseWorld) * dash.DashSpeed;
						dash.DashLeft = dash.DashMax;
						dash.dashing = true;
						dash.OnStart(Player);
					}
					else if (!dash.AnyDirection)
					{
						if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[2] < 15 && Player.doubleTapCardinalTimer[3] == 0)
						{
							dash.DashVelocity = Vector2.UnitX * dash.DashSpeed;
							dash.DashLeft = dash.DashMax;
							dash.dashing = true;
							dash.OnStart(Player);
						}
						else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[3] < 15 && Player.doubleTapCardinalTimer[2] == 0)
						{
							dash.DashVelocity = -(Vector2.UnitX * dash.DashSpeed);
							dash.DashLeft = dash.DashMax;
							dash.dashing = true;
							dash.OnStart(Player);
						}
					}
				}
				if (dash.dashing)
				{
					dash.DashEffect(Player);
					Player.velocity = dash.DashVelocity * (dash.ImbueAffected ? Player.Imbue().AOScrollSpeed : 1f);
					Player.direction = (dash.DashVelocity.X > 0).ToDirectionInt();
					if (dash.DashLeft <= 0)
					{
						dash.OnEnd(Player);
						dash.SetCooldown(Player);
						dash.dashing = false;
						dash = null;
					}
				}
			}
		}

		public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
		{
			if (npc.IsDamageDodgeable() && dash is not null && dash.dashing)
			{
				if (dash.Immune)
				{
					Player.immuneTime = 10;
					modifiers.FinalDamage *= 0;
				}
				if (dash.Damage > 0)
				{
					npc.SimpleStrikeNPC((dash.Damage * (dash.ImbueAffected ? Player.Imbue().AOScrollDamage : 1f)).Round(), Player.direction, false, dash.Knockback * (dash.ImbueAffected ? Player.Imbue().AOScrollSize : 1f), dash.DamageType, dash.DamageType != DamageClass.Default);
				}
				if (dash.OnHit(Player, npc))
				{
					dash.OnEnd(Player);
					dash.SetCooldown(Player);
					dash.dashing = false;
					dash = null;
				}
			}
		}
	}
}
