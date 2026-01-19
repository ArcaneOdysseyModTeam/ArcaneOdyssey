using ArcaneOdyssey.Content.Items.Imbues;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Developer;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
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
	public abstract class Imbuable : AOBaseItem, IImbuable, ILocalizedModType
	{
		public override string LocalizationCategory => "Imbues";
		public Imbuable Imbue { get => Item.ArcaneOdyssey()?.Imbue; set => Item.ArcaneOdyssey().Imbue = value; }

		public virtual string ImbueUISprite => Texture;

		internal Dictionary<string, int> Skills = [];

		public override void SetStaticDefaults()
		{
			ItemID.Sets.CanGetPrefixes[Type] = false;
			if (this is AOMagic)
				ItemID.Sets.ItemNoGravity[Type] = true;
			if (ImbuableTier == AOImbuableTier.Normal)
			{
				if (this is AOMagic or BasicCombat)
				{
					BasicImbues.Add(Type);
				}
			}
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

		public virtual float AOImbueSpeed => MathF.Round(AOScrollSpeed <= 1f ? AOScrollSpeed * 1.1f : AOScrollSpeed * .85f, 3);
		public virtual float AOImbueSize => MathF.Round(AOScrollSize <= 1f ? AOScrollSize * 1.1f : AOScrollSize * .85f, 3);
		public virtual float AOImbueDamage => MathF.Round(AOScrollDamage <= 1f ? AOScrollDamage * 1.1f : AOScrollDamage * .85f, 3);
		public virtual float AOScrollSpeed => MathF.Round(AOImbueSpeed <= 1f ? AOImbueSpeed * 1.1f : AOImbueSpeed * AOImbueSpeed, 3);
		public virtual float AOScrollSize => MathF.Round(AOImbueSize <= 1f ? AOImbueSize * 1.1f : AOImbueSize * AOImbueSize, 3);
		public virtual float AOScrollDamage => MathF.Round(AOImbueDamage <= 1f ? AOImbueDamage * 1.1f : AOImbueDamage * AOImbueDamage, 3);

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
			return fallback;
		}

		public Projectile CreateChargingEffect(Item item, Player player)
		{
			if (this is AOMagic)
				return AOMagic.CreateMagicCircle(item, player, this);
			return null;
		}

		public virtual void SpawningEffects(Entity entity) { }

		public virtual void LingeringEffects(Entity entity) { }

		public virtual void KillEffects(Entity entity) { }

		/// <summary>
		/// Used for explosions and pulsar stuff ect
		/// </summary>
		public virtual void ExplosionEffects(Entity entity) { }


		private bool FirstFrame = true;

		public override bool CanUseItem(Player player)
		{
			FirstFrame = true;
			return true;
		}

		public override bool? UseItem(Player player)
		{
			if (!player.AltUse() && Main.myPlayer == player.whoAmI)
			{
				player.GetModPlayer<ThermoFallOff>().resetBar = true;
				var name = "";
				if (player.Imbue() is SteamImbue steam)
				{
					name = steam.Imbue.Name;
				}
				else if (player.Imbue() is not null)
					name = player.Imbue().Name;
				if (FirstFrame && Name != name && this is AOMagic && player == Main.LocalPlayer)
				{
					AOMagic.CreateMagicCircle(Item, player, this);
				}
				if (Name != name && FirstFrame)
				{
					FirstFrame = false;
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
				else if (FirstFrame)
				{
					FirstFrame = false;
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
			return null;
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
			if (Main.dedServ || (entity.velocity.X < 2 && entity.velocity.X > -2 && entity.velocity.Y < 2 && entity.velocity.Y > -2) || entity.velocity == entity.velocity.SafeNormalize(entity.velocity))
			{
				return false;
			}
			if (entity is Projectile projectile)
			{
				if (ImbueClassCheck(projectile))
				{
					if (projectile.ModProjectile is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
					{
						return projectile.ModProjectile is not (MagicCircle1 or ExplosionSpell or MagicCircle2);
					}
					else if (projectile.ModProjectile is AOPlayerProjectile)
					{
						return projectile.ModProjectile is not (MagicCircle1 or ExplosionSpell or MagicCircle2);
					}
				}
			}
			return false;
		}

		public virtual float ItemInvisibility => 0f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.width = Item.height = 52;
			Item.useAnimation = Item.useTime = 60;
			Item.noUseGraphic = true;
			Item.alpha = (255 * MathHelper.Clamp(ItemInvisibility, 0f, 1f)).Round();
		}

		internal static List<int> BasicImbues = [];
		internal static int? poseidonGroupNum = null;

		public override void AddRecipes()
		{
			if (ImbuableTier == AOImbuableTier.Normal)
			{
				if (this is AOMagic or BasicCombat)
				{
					if (!poseidonGroupNum.HasValue)
					{
						RecipeGroup group = new(() => ModContent.GetInstance<PoseidonSpirit>().DisplayName.Value, ModContent.ItemType<EagleLegacy>(), ModContent.ItemType<PoseidonSpirit>());
						poseidonGroupNum = RecipeGroup.RegisterGroup($"{Mod.Name}:PoseidonSpiritGroup", group);
					}
					CreateRecipe().AddRecipeGroup(poseidonGroupNum.Value).DisableDecraft().Register();
				}
			}

			if (this is EaglePatrimony)
			{
				var acrimonygroup = RecipeGroup.RegisterGroup($"{Mod.Name}:AcrimonyGroup", new(() => ModContent.GetInstance<Acrimony>().DisplayName.Value, ModContent.ItemType<Acrimony>(), ModContent.ItemType<StarterAcrimony>()));
				var anybasicgroup = RecipeGroup.RegisterGroup($"{Mod.Name}:AnyBasicImbue", new(() => Mod.CoolCustomLocalization("AnyBasicImbue", "Any Worthy Item").Value, [.. BasicImbues]));

				Recipe.Create(ModContent.ItemType<PoseidonSpirit>())
					.AddRecipeGroup(anybasicgroup)
					.AddRecipeGroup(acrimonygroup)
					.DisableDecraft()
					.Register();

				CreateRecipe()
					.AddIngredient<EagleLegacy>()
					.DisableDecraft()
					.Register();

				Recipe.Create(ModContent.ItemType<EagleLegacy>())
					.AddIngredient(Type)
					.AddRecipeGroup(acrimonygroup)
					.DisableDecraft()
					.Register();
			}
		}

		public string ModifyTooltipsPrefix
		{
			get
			{
				if (this is AOMagic) { return "Magic"; }
				if (this is FightingStyle) { return "FS"; }
				if (this is RelicImbue) { return "Relic"; }
				else { return null; }
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!Main.keyState.IsKeyDown(Keys.LeftShift))
			{
				tooltips.Add(new(Mod, "DisplayedAODamage", Mod.CustomLocalization("ImbueStuff.ScrollDamage", MathF.Round(AOScrollDamage, 3)).Value));
				tooltips.Add(new(Mod, "DisplayedAOSpeed", Mod.CustomLocalization("ImbueStuff.ScrollSpeed", MathF.Round(AOScrollSpeed, 3)).Value));
				tooltips.Add(new(Mod, "DisplayedAOSize", Mod.CustomLocalization("ImbueStuff.ScrollSize", MathF.Round(AOScrollSize, 3)).Value));
				tooltips.Add(new(Mod, "ShiftAONotice", Mod.CustomLocalization("ImbueStuff.StartShifting").Value));
			}
			else
			{
				tooltips.Add(new(Mod, "DisplayedAODamage", Mod.CustomLocalization("ImbueStuff.ImbueDamage", MathF.Round(AOImbueDamage, 3)).Value));
				tooltips.Add(new(Mod, "DisplayedAOSpeed", Mod.CustomLocalization("ImbueStuff.ImbueSpeed", MathF.Round(AOImbueSpeed, 3)).Value));
				tooltips.Add(new(Mod, "DisplayedAOSize", Mod.CustomLocalization("ImbueStuff.ImbueSize", MathF.Round(AOImbueSize, 3)).Value));
				tooltips.Add(new(Mod, "ShiftAONotice", Mod.CustomLocalization("ImbueStuff.StopShifting").Value));
			}

			if (this is not FrogMagic && ModifyTooltipsPrefix is not null)
				tooltips.Add(new TooltipLine(Mod, "ImbuableTier", Mod.CustomLocalization($"{ModifyTooltipsPrefix}TierLines.{ImbuableTier}").Value));
		}
	}
}
