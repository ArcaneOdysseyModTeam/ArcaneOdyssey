using ArcaneOdyssey.Content.Items.Imbues;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	/// <summary>
	/// Imbue values are applied as multipliers to imbued projectiles,
	/// <para>Scroll values are applied as multipliers to projectiles created using spell scrolls</para>
	/// </summary>
	public abstract class Imbuable : AOBaseItem, IImbuable, ILocalizedModType
	{
		public override void UpdateEquip(Player player)
		{
			player.ArcaneOdyssey()?.AddEquippedImbue(Item);
		}

		public virtual WeaponAbility? Ability => null;

		public override string LocalizationCategory => "Imbues";
		public Imbuable Imbue { get => Item.ArcaneOdyssey()?.Imbue; set => Item.ArcaneOdyssey().Imbue = value; }

		public string ImbueUISprite => ModContent.HasAsset(Texture + "_Imbue") ? (Texture + "_Imbue") : Texture;

		internal Dictionary<string, int> Skills = [];

		public override void SetStaticDefaults()
		{
			_ = Ability?.ToolTip;
			ItemID.Sets.CanGetPrefixes[Type] = false;
		}

		/// <summary>
		/// Sets the armour stats of this magic, will be multiplied by the armour tier
		/// </summary>
		public virtual ImbueArmourStats? ArmourStats => null;

		public override AORarities AORarity
		{
			get
			{
				return ImbuableTier switch
				{
					AOImbuableTier.Normal => AORarities.Rare,
					AOImbuableTier.Lost => AORarities.Mystic,
					AOImbuableTier.Ancient => AORarities.Legendary,
					AOImbuableTier.Primordial => AORarities.Mythical,
					_ => AORarities.Special,
				};
			}
		}

		public virtual float? DashResist => null;
		public virtual float DashSpeed => 1f;

		public override bool ShowItemTypeTooltip => false;

		public virtual float AOImbueSpeed => AOScrollSpeed != 1f ? MathF.Round(AOScrollSpeed <= 1f ? AOScrollSpeed * 1.1f : AOScrollSpeed * .85f, 3) : 1f;
		public virtual float AOImbueSize => AOScrollSize != 1f ? MathF.Round(AOScrollSize <= 1f ? AOScrollSize * 1.1f : AOScrollSize * .85f, 3) : 1f;
		public virtual float AOImbueDamage => AOScrollDamage != 1f ? MathF.Round(AOScrollDamage <= 1f ? AOScrollDamage * 1.1f : AOScrollDamage * .85f, 3) : 1f;
		public virtual float AOScrollSpeed => AOImbueSpeed != 1f ? MathF.Round(AOImbueSpeed <= 1f ? AOImbueSpeed * 1.1f : AOImbueSpeed * AOImbueSpeed, 3) : 1f;
		public virtual float AOScrollSize => AOImbueSize != 1f ? MathF.Round(AOImbueSize <= 1f ? AOImbueSize * 1.1f : AOImbueSize * AOImbueSize, 3) : 1f;
		public virtual float AOScrollDamage => AOImbueDamage != 1f ? MathF.Round(AOImbueDamage <= 1f ? AOImbueDamage * 1.1f : AOImbueDamage * AOImbueDamage, 3) : 1f;

		/// <summary>
		/// For magics, you may return any value
		/// <para>For fighting stypes, Ancient is actually Lost Fighting Styles</para>
		/// </summary>
		public virtual AOImbuableTier ImbuableTier => AOImbuableTier.Normal;
		public virtual AODebuffRequirement[] ImbueDebuffs => [];
		public virtual SynergyEffects Effects => new([], []);
		public virtual Color ImbueColour => Color.Transparent;
		public virtual CombinedDebuff[] CombinedDebuffs => [];
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

		public int GetSkill(string skill, int fallback = ProjectileID.EnchantedBeam)
		{
			if (Skills.TryGetValue(skill, out var skillint))
			{
				return skillint;
			}
			else
			{
				if (Mod.TryFind<ModProjectile>(AttackPrefix + skill, out var proj))
				{
					Skills.Add(skill, proj.Type);
					return proj.Type;
				}
			}
			Main.NewText(DisplayName.Value + " is missing " + skill + " skill.", Color.Red);
			return fallback;
		}

		public Projectile CreateChargingEffect(Item item, Player player)
		{
			if (this is AOMagic)
				return AOMagic.CreateMagicCircle(item, player, this, item.damage);
			return null;
		}

		public virtual void SpawningEffects(Rectangle area, Vector2 direction) { }

		public virtual void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null) { }

		/// <summary>
		/// Called after a projectile is killed usually
		/// </summary>
		/// <param name="area">The area the dust spawns in</param>
		/// <param name="doextraeffects">Whether to do extra effects, usually spawning projectiles</param>
		public virtual void KillEffects(Rectangle area, Entity source = null) { }

		/// <summary>
		/// Used for explosions and pulsar stuff ect
		/// </summary>
		/// <param name="position">The centre of the explosion</param>
		/// <param name="intensity">The multiplier on the explosion size</param>
		public virtual void ExplosionEffects(Vector2 position, float intensity = 1f) { }

		/// <summary>
		/// Draws a solid box out of dust for walls ect
		/// <para>I am not making this lol</para>
		/// </summary>
		/// <param name="area">The box</param>
		public virtual void BoxEffects(Rectangle area, float rotation = 0f) { }


		/// <summary>
		/// For surge, ray ect
		/// </summary>
		/// <param name="origin">Where to shoot out dust from</param>
		/// <param name="rangemulti">The length of the beam</param>
		/// <param name="widthmulti">The width of the beam</param>
		public virtual void BeamEffects(Vector2 origin, float rangemulti = 1f, float widthmulti = 1f) { }

		public override void UseAnimation(Player player)
		{
			if (player.AltUse() && Main.myPlayer == player.whoAmI)
			{
				player.GetModPlayer<ThermoFallOff>().resetBar = true;
				var name = "";
				if (player.Imbue() is SteamImbue steam)
				{
					name = steam.Imbue.Name;
				}
				else if (player.Imbue() is not null)
					name = player.Imbue().Name;
				if (Name != name)
				{
					AOMagic.CreateMagicCircle(Item, player, this);
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
		}

		public Color GetColour(Color? colour = null)
		{
			if (this is FightingStyleBarred bar)
			{
				return Color.Lerp(colour.GetValueOrDefault(), ImbueColour, bar.LerpValue);
			}
			return ImbueColour;
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
				if (projectile.ModProjectile is AOPlayerProjectile proj && (!proj.CanHaveImbueVFX))
				{
					return false;
				}
				if (entity.TryGetOwner(out Player player) && player.heldProj == entity.whoAmI)
				{
					return false;
				}
				return ImbueClassCheck(projectile);
			}
			if (entity is Player)
			{
				return true;
			}
			return false;
		}

		public virtual float ItemInvisibility => 0f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.width = Item.height = 52;
			Item.useAnimation = Item.useTime = 30;
			Item.noUseGraphic = true;
			Item.alpha = (255 * MathHelper.Clamp(ItemInvisibility, 0f, 1f)).Round();
			Item.noMelee = true;
			Item.UseSound = ImbueSound;
		}

		public override bool AltFunctionUse(Player player) => true;

		internal static List<int> BasicImbues => [];

		public virtual bool Special => false;

		public string ModifyTooltipsPrefix
		{
			get
			{
				if (this is AOMagic)
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
			if (Type == ModContent.ItemType<SpiritEnergy>())
				return;
			if (this is SpiritEnergy || !Main.keyState.IsKeyDown(Keys.LeftShift))
			{
				tooltips.AddTooltip(new(Mod, "DisplayedAODamage", Mod.CustomLocalization("ImbueStuff.ScrollDamage", MathF.Round(AOScrollDamage, 3)).Value));
				tooltips.AddTooltip(new(Mod, "DisplayedAOSpeed", Mod.CustomLocalization("ImbueStuff.ScrollSpeed", MathF.Round(AOScrollSpeed, 3)).Value));
				tooltips.AddTooltip(new(Mod, "DisplayedAOSize", Mod.CustomLocalization("ImbueStuff.ScrollSize", MathF.Round(AOScrollSize, 3)).Value));
				if (this is not SpiritEnergy)
					tooltips.AddTooltip(new(Mod, "ShiftAONotice", Mod.CustomLocalization("ImbueStuff.StartShifting").Value));
			}
			else
			{
				tooltips.AddTooltip(new(Mod, "DisplayedAODamage", Mod.CustomLocalization("ImbueStuff.ImbueDamage", MathF.Round(AOImbueDamage, 3)).Value));
				tooltips.AddTooltip(new(Mod, "DisplayedAOSpeed", Mod.CustomLocalization("ImbueStuff.ImbueSpeed", MathF.Round(AOImbueSpeed, 3)).Value));
				tooltips.AddTooltip(new(Mod, "DisplayedAOSize", Mod.CustomLocalization("ImbueStuff.ImbueSize", MathF.Round(AOImbueSize, 3)).Value));
				tooltips.AddTooltip(new(Mod, "ShiftAONotice", Mod.CustomLocalization("ImbueStuff.StopShifting").Value));
			}

			if (Ability.HasValue)
			{
				tooltips.AddTooltip(Ability.Value.ToolTip);
			}

			if (ModifyTooltipsPrefix is not null)
				tooltips.AddTooltip(new TooltipLine(Mod, "ImbuableTier", Mod.CustomLocalization($"{ModifyTooltipsPrefix}TierLines.{ImbuableTier}").Value));
		}

		#region Acrimony Handling, here are the methods for right clicking in inventory (in case they are needed for something else)

		public override bool CanRightClick()
		{
			try
			{
				Player player = Main.LocalPlayer;

				//														Spoky (2026 Fev 08): in case the change should only apply to normal imbues, decomment this
				if (!(Type == ModContent.ItemType<SpiritEnergy>() || this is EaglePatrimony or AOMagic or FightingStyle /*&& ImbuableTier is AOImbuableTier.Normal*/))
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

				var instance = ModContent.GetInstance<ImbueAnythingUISystem>();

				if (!instance.CanShowImbueChange()) instance.ShowSwapUI(this);
				return false;
			}
			catch (Exception ex)
			{
				Main.NewText($"Error in {nameof(CanUseItem)}: \n{ex}", new Color(255, 0, 255));
				return false;
			}
		}
		public override bool ConsumeItem(Player player) => false;
		#endregion
	}
}
