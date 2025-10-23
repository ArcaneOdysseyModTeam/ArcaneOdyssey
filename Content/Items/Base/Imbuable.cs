using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles;
using ArcaneOdyssey.Content.Items.Imbues;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class Imbuable : AOBaseItem
	{
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
				switch (ImbuableTier)
				{
					case AOImbuableTier.Normal:
						return AORarities.Rare;
						break;
					case AOImbuableTier.Lost:
						return AORarities.Mystic;
						break;
					case AOImbuableTier.Ancient:
						return AORarities.Arcane;
						break;
					case AOImbuableTier.Custom:
						return AORarities.Zenith;
						break;
					default:
						return AORarities.Special;
						break;
				}
			}
		}

		public override ItemType ItemType => ItemType.None;

		public abstract float AOImbueSpeed { get; }
		public abstract float AOImbueSize { get; }
		public abstract float AOImbueDamage { get; }
		public virtual float AOScrollSpeed => AOImbueSpeed;
		public virtual float AOScrollSize => AOImbueSize;
		public virtual float AOScrollDamage => AOImbueDamage;
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

		public virtual Dictionary<Type, int> Skills => [];

		public virtual void SpawningEffects(Entity entity) { }
		public virtual void LingeringEffects(Entity entity) { }
		public virtual void KillEffects(Entity entity) { }
		/// <summary>
		/// used for explosions and pulsar type stuff ect
		/// </summary>
		public virtual void ExplosionEffects(Entity entity) { }


		public bool FirstFrame = true;

		public override bool CanUseItem(Player player)
		{
			FirstFrame = true;
			return true;
		}

		public override bool? UseItem(Player player)
		{
			player.GetModPlayer<ThermoFallOff>().resetBar = true;
			var name = "";
			if (player.Imbue() is SteamImbue steam)
			{
				name = steam.originalImbue.Name;
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
				player.ArcaneOdyssey().imbue = this;
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
				player.ArcaneOdyssey().imbue = null;
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
			return null;
		}

		/// <summary>
		/// Return false to cancel VFX
		/// </summary>
		/// <param name="entity">The entity to check</param>
		/// <returns></returns>
		public virtual bool PreEffects(Entity entity)
		{
			if (entity.velocity == entity.velocity.SafeNormalize(Vector2.Zero))
			{
				return false;
			}
			if (entity is Projectile projectile)
			{
				if (ImbueClassCheck(projectile))
				{
					if (projectile.ModProjectile is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
					{
						return !Main.dedServ && projectile.ModProjectile is not (MagicCircle1 or ExplosionTracker or MagicCircle2);
					}
					else if (projectile.ModProjectile is AOPlayerProjectile)
					{
						return !Main.dedServ && projectile.ModProjectile is not (MagicCircle1 or ExplosionTracker or MagicCircle2);
					}
				}
			}
			return false;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.noUseGraphic = true;
			if (this is GlassMagic)
			{
				Item.alpha = (255 * .5f).Round(); // glass gets 50% less visible
			}
		}

		private static List<int> BasicImbues = [];

		public override void AddRecipes()
		{
			if (ImbuableTier == AOImbuableTier.Normal)
			{
				if (this is AOMagic or BasicCombat)
				{
					CreateRecipe().AddIngredient<PoseidonChoice>().DisableDecraft().Register();
				}
			}

			if (this is BasicCombat)
			{
				var goru = new RecipeGroup(() => Mod.CustomLocalization("AnyBasicImbue").Value, [..BasicImbues]);
				RecipeGroup.RegisterGroup("ArcaneOdyssey:AOMagic", goru);
				Recipe recipe = Recipe.Create(ModContent.ItemType<PoseidonChoice>());
				recipe.AddRecipeGroup(goru);
				recipe.AddIngredient<Acrimony>();
				recipe.DisableDecraft().Register();
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "ImbuableTier", Mod.CustomLocalization($"{(this is AOMagic ? "Magic" : "FS")}TierLines.{ImbuableTier}").Value));
		}
	}
}
