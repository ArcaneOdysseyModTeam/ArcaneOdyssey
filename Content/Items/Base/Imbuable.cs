using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.FightingStyles;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Scrolls;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using ArcaneOdyssey.Content.Items.Weapons.Scrolls;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class Imbuable : AOBaseItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.CanGetPrefixes[Type] = false;
			if (this is BasicCombat or AOMagic)
				ItemID.Sets.ShimmerTransformToItem[Type] = Type;
			ItemID.Sets.ItemNoGravity[Type] = this is AOMagic;
		}

		public override AORarities AORarity => ImbuableTier == AOImbuableTier.Normal ? AORarities.Common : AORarities.Exotic;

		public override ItemType ItemType => ItemType.None;

		public abstract float AOImbueSpeed { get; }
		public abstract float AOImbueSize { get; }
		public abstract float AOImbueDamage { get; }
		public virtual float AOScrollSpeed => AOImbueSpeed;
		public virtual float AOScrollSize => AOImbueSize;
		public virtual float AOScrollDamage => AOImbueDamage;
		public virtual AOImbuableTier ImbuableTier => AOImbuableTier.Normal;
		public virtual AODebuffRequirement ImbueDebuff => null;

		/// <summary>
		/// used for having freezing and frozen on a single magic ect
		/// </summary>
		public virtual AODebuffRequirement ImbueDebuff2 => null;
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

		public virtual void SpawningEffects(Projectile projectile) { }
		public virtual void LingeringEffects(Projectile projectile) { }
		public virtual void KillEffects(Projectile projectile) { }

		/// <summary>
		/// used for explosions and pulsar type stuff ect
		/// </summary>
		/// <param name="projectile">THE PROJECTILE DUMBASS</param>
		public virtual void ExplosionEffects(Projectile projectile) { }


		public bool FirstFrame = true;

		public override bool CanUseItem(Player player)
		{
			FirstFrame = true;
			return true;
		}

		public override bool? UseItem(Player player)
		{
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

		public virtual bool PreEffects(Projectile projectile)
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
			return false;
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.noUseGraphic = true;
			if (this is GlassMagic)
			{
				Item.alpha = (255 * .5f).Round(); // glass gets 50% less visible
			}
		}

		public override void AddRecipes()
		{
			if (ImbuableTier == AOImbuableTier.Normal)
			{
				if (this is AOMagic or BasicCombat)
					CreateRecipe().AddIngredient<PoseidonChoice>().Register();
				Recipe.Create(ModContent.ItemType<PoseidonChoice>()).AddIngredient(Type).AddIngredient<Acrimony>().Register(); // replace with something better later
			}
			else CustomRecipe();
		}

		public virtual void CustomRecipe() { }

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(Mod, "ImbuableTier", Mod.CustomLocalization($"{(this is AOMagic ? "Magic" : "FS")}TierLines.{ImbuableTier}").Value));
		}
	}
}
