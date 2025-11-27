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
using ArcaneOdyssey.Content.Items.Imbues;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Other;
using static ArcaneOdyssey.AOUtils;

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
				return ImbuableTier switch
				{
					AOImbuableTier.Normal => AORarities.Rare,
					AOImbuableTier.Lost => AORarities.Mystic,
					AOImbuableTier.Ancient => AORarities.Arcane,
					AOImbuableTier.Primordial => AORarities.Mythical,
					_ => AORarities.Special,
				};
			}
		}


		public virtual float DashResist => 1f;
		public virtual float DashSpeed => 1f;

		public override bool ShowItemTypeTooltip => false;

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

		public virtual float KBMulti => 1f;

		public virtual List<Type> Skills => [];

		public int GetSkill(Type skill, int fallback = ProjectileID.AmethystBolt)
		{
			var aaa = Skills.Find(e => e.IsSubclassOf(skill));
			if (Skills.Contains(aaa))
				return Mod.Find<ModProjectile>(aaa.Name).Type;
			return fallback;
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
			return null;
		}

		public Color GetColor(Color? colour = null)
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
			if ((entity.velocity.X < 2 && entity.velocity.X > -2 && entity.velocity.Y < 2 && entity.velocity.Y > -2) || entity.velocity == entity.velocity.SafeNormalize(entity.velocity))
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

		public virtual float ItemInvisibility => 0f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.noUseGraphic = true;
			Item.alpha = (255 * MathHelper.Clamp(ItemInvisibility, 0f, 1f)).Round();
		}

		internal static List<int> BasicImbues = [];

		public override void AddRecipes()
		{
			if (ImbuableTier == AOImbuableTier.Normal)
			{
				if (this is AOMagic or BasicCombat)
				{
					RecipeGroup group = new(() => ModContent.GetInstance<PoseidonSpirit>().DisplayName.Value, ModContent.ItemType<PoseidonChoice>(), ModContent.ItemType<PoseidonSpirit>());
					RecipeGroup.RegisterGroup($"{Mod.Name}:PoseidonSpirit", group);
					CreateRecipe().AddRecipeGroup(group).DisableDecraft().Register();
				}
			}

			if (this is BasicCombat)
			{
				var goru = new RecipeGroup(() => Mod.CustomLocalization("AnyBasicImbue").Value, [..BasicImbues]);
				RecipeGroup.RegisterGroup($"{Mod.Name}:AnyBasicImbue", goru);
				Recipe recipe = Recipe.Create(ModContent.ItemType<PoseidonSpirit>());
				recipe.AddRecipeGroup(goru);
				recipe.AddIngredient<Acrimony>();
				recipe.DisableDecraft().Register();
			}
		}

		public string ModifyTooltipsPrefix { get
			{
				if (this is AOMagic) { return "Magic"; }
				if (this is FightingStyle) { return "FS"; }
				else { return null; }
			} }

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (this is not FrogMagic && ModifyTooltipsPrefix is not null)
				tooltips.Add(new TooltipLine(Mod, "ImbuableTier", Mod.CustomLocalization($"{ModifyTooltipsPrefix}TierLines.{ImbuableTier}").Value));
		}

		public override void UpdateInventory(Player player)
		{
			this.ArcaneOdyssey().Imbue = this;
		}

		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			this.ArcaneOdyssey().Imbue = this;
		}
	}
}
