using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Consumable;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Base
{
	/// <summary>
	/// Imbue values are applied as multipliers to imbued projectiles,
	/// <para>Scroll values are applied as multipliers to projectiles created using spell scrolls</para>
	/// </summary>
	public abstract class Imbuable : BaseItem, IImbuable
	{
		public virtual float Aura => .7f;

		public virtual int Drawback => 0;

		public int AuraHP(Player player)
		{
			float aura;
			if (this is not FightingStyle || player.ArcaneOdyssey().acumen)
			{
				aura = player.statLifeMax * (.225f * Aura);
			}
			else
			{
				aura = player.statLifeMax * (.18f * Aura);
			}
			return (int)Math.Round(aura / 5f, MidpointRounding.AwayFromZero) * 5;
		}

		public virtual void UpdateProjectile(Projectile Projectile)
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public void ActivateAbility(Player player, bool passive)
		{
			if (Ability.HasValue)
			{
				if (ArcaneOdysseyClientConfig.Instance.AbilityText && player is not null && player.active && !player.DeadOrGhost && Main.myPlayer == player.whoAmI)
				{
					CombatText.NewText(player.Hitbox, Ability.Value.Colour, (Ability.Value.Name + "!").Trim(), !passive);
				}
			}
		}

		public static bool IsValidEnemyImbue(Imbuable imbue)
		{
			if (imbue is MagicType or FightingStyle)
			{
				if (NPC.downedMoonlord || DownedBosses.DownedEnragedEmpress)
				{
					if ((imbue.ImbuableTier == ImbuableTiers.Normal) || ((imbue.ImbuableTier == ImbuableTiers.Lost) && (!imbue.Special)) || ((imbue.ImbuableTier == ImbuableTiers.Ancient) && (imbue is MagicType) && (!imbue.Special)))
					{
						return true;
					}
				}

				else if (NPC.downedMechBossAny)
				{
					if ((imbue.ImbuableTier == ImbuableTiers.Normal) || ((imbue.ImbuableTier == ImbuableTiers.Lost) && (imbue is MagicType) && (!imbue.Special)))
					{
						return true;
					}
				}

				else if (Main.hardMode)
				{
					if (imbue.ImbuableTier == ImbuableTiers.Normal)
					{
						return true;
					}
				}

				else
				{
					if (imbue.ImbuableTier == ImbuableTiers.Normal)
					{
						if (imbue is FightingStyle)
						{
							return imbue.Type == ModContent.ItemType<BasicCombat>();
						}
						return true;
					}
				}
			}

			return false;
		}

		public static Imbuable[] AllValidEnemyImbues 
		{ 
			get
			{
				var imbues = ModContent.GetContent<Imbuable>().ToList();
				imbues.RemoveAll(e => !IsValidEnemyImbue(e));
				return [.. imbues];
			}
		}

		public WeaponAbility? Ability
		{
			get
			{
				var ab = new WeaponAbility { Colour = Colour };
				if (Imbue is not null)
				{
					ab.Colour = Imbue.ImbueColour;
				}
				if (Language.Exists($"Mods.{Mod.Name}.{LocalizationCategory}.{Name}.Ability.DisplayName") && Language.Exists($"Mods.{Mod.Name}.{LocalizationCategory}.{Name}.Ability.Description"))
				{
					ab.Name = Mod.CustomLocalization($"{LocalizationCategory}.{Name}.Ability.DisplayName").Value;
					ab.Description = Mod.CustomLocalization($"{LocalizationCategory}.{Name}.Ability.Description").Value;
					if (Imbue is not null)
					{
						ab.Name = (Imbue.PrettyAttackPrefix + " " + ab.Name).Trim();
					}
					return ab;
				}
				else if (Language.Exists($"Mods.{Mod.Name}.{LocalizationCategory}.{Name}.Ability"))
				{
					ab.Name = Mod.CustomLocalization($"{LocalizationCategory}.{Name}.Ability").Value;
					ab.Description = null;
					return ab;
				}
				return null;
			}
		}

		public override void UpdateEquip(Player player)
		{
			player.ArcaneOdyssey()?.AddEquippedImbue(this);
		}

		/// <summary>
		/// The second imbue
		/// </summary>
		public Imbuable Imbue { get => Item.ArcaneOdyssey()?.Imbue; set => Item.ArcaneOdyssey().Imbue = value; }

		public virtual string ImbueUISprite => ModContent.HasAsset(Texture + "_Imbue") ? (Texture + "_Imbue") : Texture;

		internal Dictionary<string, int> Skills = [];

		public override void SetStaticDefaults()
		{
			ItemID.Sets.CanGetPrefixes[Type] = false;
			_ = PrettyAttackPrefix;
			_ = PrettySpellPrefix;
		}

		/// <summary>
		/// Sets the armour stats of this magic, will be multiplied by the armour tier
		/// </summary>
		public virtual ImbueArmourStats? ArmourStats => null;

		public override Rarities Rarity
		{
			get
			{
				return ImbuableTier switch
				{
					ImbuableTiers.Normal => Rarities.Rare,
					ImbuableTiers.Lost => Rarities.Mystic,
					ImbuableTiers.Ancient => Rarities.Legendary,
					_ => Rarities.Special,
				};
			}
		}

		public virtual float? DashResist => null;
		public virtual float DashSpeed => 1f;

		public override bool ShowItemTypeTooltip => false;

		public virtual float ImbueSpeed => ScrollSpeed != 1f ? MathF.Round(ScrollSpeed <= 1f ? ScrollSpeed * 1.1f : ScrollSpeed * .85f, 3) : 1f;
		public virtual float ImbueSize => ScrollSize != 1f ? MathF.Round(ScrollSize <= 1f ? ScrollSize * 1.1f : ScrollSize * .85f, 3) : 1f;
		public virtual float ImbueDamage => ScrollDamage != 1f ? MathF.Round(ScrollDamage <= 1f ? ScrollDamage * 1.1f : ScrollDamage * .85f, 3) : 1f;
		public virtual float ScrollSpeed => ImbueSpeed != 1f ? MathF.Round(ImbueSpeed <= 1f ? ImbueSpeed * 1.1f : ImbueSpeed * ImbueSpeed, 3) : 1f;
		public virtual float ScrollSize => ImbueSize != 1f ? MathF.Round(ImbueSize <= 1f ? ImbueSize * 1.1f : ImbueSize * ImbueSize, 3) : 1f;
		public virtual float ScrollDamage => ImbueDamage != 1f ? MathF.Round(ImbueDamage <= 1f ? ImbueDamage * 1.1f : ImbueDamage * ImbueDamage, 3) : 1f;

		/// <summary>
		/// For magics or fighing styles, you may return any value
		/// <para>Relics are always Normal for now</para>
		/// </summary>
		public virtual ImbuableTiers ImbuableTier => ImbuableTiers.Normal;
		public virtual Debuff[] ImbueDebuffs => [];
		public virtual SynergyEffects Effects => new([], []);
		public abstract Color ImbueColour { get; }
		public virtual Color ImbueColour2 => Color.White;
		public virtual ColourTransitionStyle TransitionStyle => ColourTransitionStyle.None;
		public virtual Combo[] CombinedDebuffs => [];
		public virtual SoundStyle? ImbueSound => null;

		/// <summary>
		/// Leave null for neutral, true for cold, false for hot
		/// </summary>
		public virtual bool? Cold => null;

		/// <summary>
		/// magic/fs works underwater
		/// </summary>
		public virtual bool CanBeWet => true;

		public virtual float KBMulti => 1f;

		public virtual string AttackPrefix => Name.Replace("Magic");

		public LocalizedText PrettyAttackPrefix => Language.GetOrRegister(this.GetLocalizationKey("AttackPrefix"), () => Regex.Replace(AttackPrefix, "([A-Z])", " $1").Trim());

		public LocalizedText PrettySpellPrefix => Language.GetOrRegister(this.GetLocalizationKey("SpellPrefix"), () => Regex.Replace(AttackPrefix, "([A-Z])", " $1").Trim());

		public virtual void SpawningEffects(Rectangle area, Vector2 direction) { }

		public virtual void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null) { }

		public virtual int[] Dusts => [];

		/// <summary>
		/// Called after a projectile is killed usually
		/// </summary>
		/// <param name="area">The area the dust spawns in</param>
		public virtual void KillEffects(Rectangle area, Entity source = null) { }

		/// <summary>
		/// Used for explosions and pulsar stuff ect
		/// </summary>
		/// <param name="position">The centre of the explosion</param>
		/// <param name="intensity">The multiplier on the explosion size</param>
		public virtual void ExplosionEffects(Vector2 position, float intensity = 1f) { }

		/// <summary>
		/// Draws a solid box out of dust for walls ect
		/// </summary>
		/// <param name="area">The box</param>
		public virtual void BoxEffects(Rectangle area) 
		{
			for (int X = 0; X <= area.Width; X += 4)
			{
				Dust.NewDustPerfect(area.TopLeft() with { X = area.X + X }, DustID.ShimmerSpark, Vector2.Zero, newColor: Colour);
				Dust.NewDustPerfect(area.BottomLeft() with { X = area.X + X }, DustID.ShimmerSpark, Vector2.Zero, newColor: Colour);
			}
			for (int Y = 0; Y <= area.Height; Y += 4)
			{
				Dust.NewDustPerfect(area.TopLeft() with { Y = area.Y + Y }, DustID.ShimmerSpark, Vector2.Zero, newColor: Colour);
				Dust.NewDustPerfect(area.TopRight() with { X = area.Y + Y }, DustID.ShimmerSpark, Vector2.Zero, newColor: Colour);
			}

			LingeringEffects(area, Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2());
		}


		/// <summary>
		/// For surge, ect
		/// </summary>
		public virtual void ConeEffects(Vector2 coneCenter, float coneLength, float coneRotation, float maximumAngle = 0f)
		{
			for (int i = 0; i < 2; i++)
			{
				AOUtils.NewDustImperfect(coneCenter, DustID.ShimmerSpark, (coneRotation + Main.rand.NextFloat(-maximumAngle, maximumAngle)).ToRotationVector2() * (coneLength / 45f), newColor: Colour, Scale: .2f * (coneLength / 25f));
			}
		}

		public override void UseAnimation(Player player)
		{
			if (player.AltUse() && Main.myPlayer == player.whoAmI)
			{
				var name = "";
				if (player.Imbue() is SteamImbue steam)
				{
					steam.Imbue ??= ModContent.GetInstance<WindMagic>();
					name = steam.Imbue.Name;
				}
				else if (player.Imbue() is not null)
					name = player.Imbue().Name;
				if (Name != name)
				{
					CreateMagicCircle(Item, player, MagicCircleMode.Rotating, true);
				}
				if (Name != name)
				{
					player.ArcaneOdyssey().Imbue = this;
					LocalizedText chatmessage = Mod.CustomLocalization("ImbueStuff.ImbueChatMessage", [Item.Name]);
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						Main.NewText(chatmessage.Value, 13, 132, 168);
					}
					else if (Main.dedServ)
					{
						ChatHelper.SendChatMessageToClient(chatmessage.ToNetworkText(), new Color(13, 132, 168), player.whoAmI);
					}
				}
				else
				{
					player.ArcaneOdyssey().Imbue = null;
					LocalizedText chatmessage = Mod.CustomLocalization("ImbueStuff.UnimbueText");
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						Main.NewText(chatmessage.Value, 13, 132, 168);
					}
					else if (Main.dedServ)
					{
						ChatHelper.SendChatMessageToClient(chatmessage.ToNetworkText(), new Color(13, 132, 168), player.whoAmI);
					}
				}
			}
			else if (Drawback > 0)
			{
				if ((!player.AltUse()) && Main.myPlayer == player.whoAmI)
				{
					player.Hurt(PlayerDeathReason.ByCustomReason(Mod.CustomLocalization($"Drawback.Death{Main.rand.Next(4)}", player.name).ToNetworkText()), player.statLifeMax / 100 * Drawback, Main.rand.NextBool().ToDirectionInt(), dodgeable: false, knockback: 0f, scalingArmorPenetration: player.statDefense.Positive);
				}
			}
		}

		public Color Colour
		{
			get
			{
				var colour = ImbueColour2;
				if (this is FightingStyleBarred bar)
				{
					if (Imbue is not null)
					{
						colour = Imbue.ImbueColour;
					}
					return Color.Lerp(colour, ImbueColour, bar.LerpValue);
				}
				if (TransitionStyle == ColourTransitionStyle.Smooth)
				{
					return Color.Lerp(ImbueColour, colour, Math.Abs(MathF.Sin(AOUtils.UpdateCount)));
				}
				else if (TransitionStyle == ColourTransitionStyle.Tangent)
				{
					return Color.Lerp(colour, ImbueColour, Math.Abs(MathF.Tan(AOUtils.UpdateCount)));
				}
				else if (TransitionStyle == ColourTransitionStyle.Linear)
				{
					return Color.Lerp(ImbueColour, colour, Math.Abs((AOUtils.UpdateCount % 2f) - 1f));
				}
				return ImbueColour;
			}
		}

		/// <summary>
		/// Return false to cancel VFX
		/// </summary>
		/// <param name="entity">The entity to check</param>
		/// <returns></returns>
		public virtual bool PreEffects(Entity entity)
		{
			if (Main.dedServ || entity.velocity.Length() == 1)
			{
				return false;
			}
			if (entity is Projectile projectile)
			{
				if (projectile.ModProjectile is PlayerProjectile proj && (!proj.CanHaveImbueVFX))
				{
					return false;
				}
				if (entity.TryGetOwner(out Player player) && player.heldProj == entity.whoAmI)
				{
					return false;
				}
			}
			return true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.width = Item.height = 52;
			Item.useAnimation = Item.useTime = 30;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.UseSound = ImbueSound;
		}

		public override bool AltFunctionUse(Player player) => true;

		public virtual bool Special => false;

		public string TooltipsPrefix
		{
			get
			{
				if (this is MagicType)
				{
					if (Special && ArcaneOdysseyMod.DevMode) // eventually i want to add some way to get lore, like athenas wisdom. if we have the knowledge it will be added here...
					{
						return "Special";
					}
					return "Magic";
				}

				if (this is FightingStyle)
				{
					return "FS";
				}

				if (this is SpiritEnergy)
				{
					return "Relic";
				}

				return null;
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!tooltips.Contains(tooltips.Find(e => e.Name == "Social" && e.Mod == "Terraria")))
			{
				TooltipLine tip = new(Mod, "Drawback", Mod.CustomLocalization("ImbueStuff.Drawback", Drawback).Value);
				if (Drawback < 1)
				{
					tip.Hide();
				}
				tooltips.AddTooltip(tip, Color.Red);

				if (!Main.keyState.IsKeyDown(Keys.LeftShift))
				{
					tooltips.AddTooltip(new(Mod, "DisplayedAODamage", Mod.CustomLocalization("ImbueStuff.ScrollDamage", MathF.Round(ScrollDamage, 3)).Value));
					tooltips.AddTooltip(new(Mod, "DisplayedAOSpeed", Mod.CustomLocalization("ImbueStuff.ScrollSpeed", MathF.Round(ScrollSpeed, 3)).Value));
					tooltips.AddTooltip(new(Mod, "DisplayedAOSize", Mod.CustomLocalization("ImbueStuff.ScrollSize", MathF.Round(ScrollSize, 3)).Value));

					if (ImbueDebuffs.Length > 0)
					{
						string req = "";
						if (ImbueDebuffs[0].debuffPercent > 0)
						{
							req = Mod.CustomLocalization("ImbueStuff.Requirement", (ImbueDebuffs[0].debuffPercent * 100f).Round()).Value;
						}
						var debufftext = Mod.CustomLocalization("ImbueStuff.Debuffs", Lang.GetBuffName(ImbueDebuffs[0].debuffID) + req).Value;
						foreach (var debuff in ImbueDebuffs)
						{
							req = "";
							if (debuff.debuffID != ImbueDebuffs[0].debuffID)
							{
								if (debuff.debuffPercent > 0)
								{
									req = Mod.CustomLocalization("ImbueStuff.Requirement", (debuff.debuffPercent * 100f).Round()).Value;
								}
								debufftext = Mod.CustomLocalization("ImbueStuff.Conjoined", debufftext, Lang.GetBuffName(debuff.debuffID) + req).Value;
							}
						}
						tooltips.AddTooltip(new(Mod, "DebuffInfo", debufftext));
					}
					else
					{
						tooltips.AddTooltip(new(Mod, "DebuffInfo", Mod.CustomLocalization("ImbueStuff.NoDebuffs").Value));
					}
				}
				else
				{
					tooltips.AddTooltip(new(Mod, "DisplayedAODamage", Mod.CustomLocalization("ImbueStuff.ImbueDamage", MathF.Round(ImbueDamage, 3)).Value));
					tooltips.AddTooltip(new(Mod, "DisplayedAOSpeed", Mod.CustomLocalization("ImbueStuff.ImbueSpeed", MathF.Round(ImbueSpeed, 3)).Value));
					tooltips.AddTooltip(new(Mod, "DisplayedAOSize", Mod.CustomLocalization("ImbueStuff.ImbueSize", MathF.Round(ImbueSize, 3)).Value));
					
					if (CombinedDebuffs.Length > 0)
					{
						var aaaaa = Mod.CustomLocalization("ImbueStuff.Result", Lang.GetBuffName(CombinedDebuffs[0].requirement), Lang.GetBuffName(CombinedDebuffs[0].result));
						var debufftext = Mod.CustomLocalization("ImbueStuff.Combined", aaaaa).Value;
						foreach (var debuff in CombinedDebuffs)
						{
							if (debuff.requirement != CombinedDebuffs[0].requirement)
							{
								aaaaa = Mod.CustomLocalization("ImbueStuff.Result", Lang.GetBuffName(debuff.requirement), Lang.GetBuffName(debuff.result));
								debufftext = Mod.CustomLocalization("ImbueStuff.Conjoined", debufftext, aaaaa).Value;
							}
						}
						tooltips.AddTooltip(new(Mod, "DebuffInfo", debufftext));
					}
					else
					{
						tooltips.AddTooltip(new(Mod, "DebuffInfo", Mod.CustomLocalization("ImbueStuff.NoCombinedDebuffs").Value));
					}
				}
				tooltips.AddTooltip(new(Mod, "ShiftNotice", Mod.CustomLocalization("ImbueStuff.ShiftNotice").Value));

				if (Ability.HasValue)
				{
					string text = Ability?.Name;
					if (Ability?.Description is not null)
					{
						text += $": {Ability?.Description}";
					}
					tooltips.AddTooltip(new TooltipLine(Mod, "AOAbility", text), Ability?.Colour);
				}
			}

			if (TooltipsPrefix is not null)
				tooltips.AddTooltip(new TooltipLine(Mod, "ImbuableTier", Mod.CustomLocalization($"{TooltipsPrefix}TierLines.{ImbuableTier}").Value));
		}

		private static int SortMultipliers(Synergy x, Synergy y)
		{
			if (x.multiplier > y.multiplier)
			{
				return 1;
			}
			if (x.multiplier < y.multiplier)
			{
				return -1;
			}
			return 0;
		}

		public string SynergiesText()
		{
			var text = Mod.CustomLocalization("ImbueStuff.NoSynergies").Value;

			var syns = Effects.magicBuffMultipliers.Sorted(new Comparison<Synergy>(SortMultipliers));
			if (syns.Count > 0)
			{
				text = Mod.CustomLocalization("ImbueStuff.SynergiesInfo", DisplayName.Value, Lang.GetBuffName(syns[0].buffID) + Mod.CustomLocalization("ImbueStuff.SynergyMulti", syns[0].multiplier).Value).Value;

				foreach (var effect in syns)
				{
					if (effect.buffID != syns[0].buffID)
					{
						text = Mod.CustomLocalization("ImbueStuff.Conjoined", text, Lang.GetBuffName(effect.buffID) + Mod.CustomLocalization("ImbueStuff.SynergyMulti", effect.multiplier).Value).Value;
					}
				}
			}

			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				var clear = Effects.clearBuffs.OrderBy(e => Lang.GetBuffName(e.id)).ToArray();
				if (clear.Length > 0)
				{
					text = Mod.CustomLocalization("ImbueStuff.ClearsInfo", text, Lang.GetBuffName(clear[0].id)).Value;

					foreach (var buff in clear)
					{
						if (buff.id != clear[0].id)
						{
							text = Mod.CustomLocalization("ImbueStuff.Conjoined", text, Lang.GetBuffName(buff.id)).Value;
						}
					}
				}
			}

			text = Mod.CustomLocalization("ImbueStuff.SentenceEnd", text).Value;

			return text;
		}

		// spread 
		public static Circle CreateMagicCircle(Item item, Player player, MagicCircleMode mode, bool markedfordeath, int chargingProjectile = 0, bool altfire = false, float spread = 0f, Vector2? position = null, float? rotation = null)
		{
			if (mode != MagicCircleMode.Rotating)
			{
				position ??= player.RotatedRelativePoint(player.MountedCenter) + (player.SafeDirectionTo(Main.MouseWorld) * 30f);
				rotation ??= player.AngleTo(Main.MouseWorld);
			}
			else
			{
				position ??= player.RotatedRelativePoint(player.MountedCenter);
				rotation ??= 0;
			}
			Circle circle = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), position.Value, Vector2.Zero, ModContent.ProjectileType<Circle>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI, ai2: (int)mode).ModProjectile as Circle;
			circle.ProjectileSpread = spread;
			circle.MarkedForDeath = markedfordeath;
			circle.originallyAltFire = altfire;
			circle.ChargingProjectile = chargingProjectile;
			circle.Projectile.rotation = rotation.Value;
			if ((chargingProjectile != 0) || (!markedfordeath))
			{
				player.ArcaneOdyssey().myCircle = circle;
			}
			return circle;
		}

		#region Acrimony Handling, here are the methods for right clicking in inventory (in case they are needed for something else)
		public override void RightClick(Player player)
		{
			Main.playerInventory = false;
			var instance = ModContent.GetInstance<ModUISystem>();
			if (!instance.CanShowImbueChange())
				instance.ShowSwapUI(this);
		}

		public override bool CanRightClick()
		{
			try
			{
				Player player = Main.LocalPlayer;

				if (!(Type == ModContent.ItemType<SpiritEnergy>() || this is EaglePatrimony or MagicType or FightingStyle && ImbuableTier == ImbuableTiers.Normal))
				{
					//Main.NewText($"Item is not swappable");
					return false;
				}

				if (!player.HasTypeInInventory<Acrimony>())
				{
					//Main.NewText($"Doesn't have acrimony");
					return false;
				}

				//Main.NewText($"Can use item {!ModContent.GetInstance<MagicChoiceUISystem>().CanShowUI()}");

				return true;
			}
			catch (Exception ex)
			{
				Main.NewText($"Error in {nameof(CanRightClick)}: \n{ex}", new Color(255, 0, 255));
				return false;
			}
		}
		public override bool ConsumeItem(Player player) => false;
		#endregion
	}

	public enum ColourTransitionStyle
	{
		None,
		Smooth,
		Tangent,
		Linear
	}
}
