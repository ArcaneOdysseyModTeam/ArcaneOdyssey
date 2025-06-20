using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using static ArcaneOdyssey.AOConversion;
using static System.Net.Mime.MediaTypeNames;

namespace ArcaneOdyssey
{
	public class ArcaneOdyssey : Mod {}
	public class VanillaSynergy : GlobalItem
	{
		public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                playerForImbue = player.GetModPlayer<AOPlayer>();
            if (player.GetModPlayer<AOPlayer>().imbue is not null && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed))
			{

			}
		}

		public override void UpdateInventory(Item item, Player player)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				playerForImbue = player.GetModPlayer<AOPlayer>();
		}

		public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
		{
			playerForImbue = null;
		}

		public static AOPlayer? playerForImbue = null;

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed)
			{
				string imbuetextthing = Mod.GetLocalization("ImbueStuff.NoneText").Value;
				if (Main.netMode != NetmodeID.SinglePlayer)
					imbuetextthing = Mod.GetLocalization("ImbueStuff.MultiplayerCannotDisplay").Value;
				if (playerForImbue is not null)
					if (playerForImbue.imbue is not null)
						imbuetextthing = playerForImbue.imbue.Item.Name;
				tooltips.Add(new TooltipLine(item.ModItem.Mod, "ImbueText", Mod.GetLocalization("ImbueStuff.ImbueTooltip").Format([imbuetextthing])));
			}
		}

		public override bool? UseItem(Item item, Player player)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                playerForImbue = player.GetModPlayer<AOPlayer>();
            if (item.ModItem is not null && item.ModItem is AOMagic magic)
			{
				player.GetModPlayer<AOPlayer>().imbue = magic;
				LocalizedText chatmessage = Mod.GetLocalization("ImbueStuff.ImbueChatMessage").WithFormatArgs([item.Name]);
				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					Main.NewText(chatmessage.Value, 13, 132, 168);
				}
				else if (Main.netMode == NetmodeID.Server) 
				{
					ChatHelper.SendChatMessageToClient(chatmessage.ToNetworkText(), new Color(13, 132, 168), Array.IndexOf(Main.player, player));
				}
			}
			return base.UseItem(item, player);
		}

		public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                playerForImbue = player.GetModPlayer<AOPlayer>();
            if (player.GetModPlayer<AOPlayer>().imbue is not null && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed))
			{
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					scale *= aoWeapon.AOSize * player.GetModPlayer<AOPlayer>().imbue.AOImbueSize;
				}
				else if (item.ModItem is null) // do not touch items from other mods
				{
					scale *= player.GetModPlayer<AOPlayer>().imbue.AOImbueSize;
                }
			}
		}

        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                playerForImbue = player.GetModPlayer<AOPlayer>();
            if (player.GetModPlayer<AOPlayer>().imbue is not null && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed))
            {
                if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
                {
                    knockback *= aoWeapon.AOSize * player.GetModPlayer<AOPlayer>().imbue.AOImbueSize;
                }
                else if (item.ModItem is null) // do not touch items from other mods
                {
                    knockback *= player.GetModPlayer<AOPlayer>().imbue.AOImbueSize;
                }
            }
        }
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                playerForImbue = player.GetModPlayer<AOPlayer>();
            if (player.GetModPlayer<AOPlayer>().imbue is not null && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed))
            {
                if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
                {
                    damage *= aoWeapon.AODamage * player.GetModPlayer<AOPlayer>().imbue.AOImbueDamage;
                }
                else if (item.ModItem is null) // do not touch items from other mods
                {
                    damage *= player.GetModPlayer<AOPlayer>().imbue.AOImbueDamage;
                }
            }
        }

        public override float UseTimeMultiplier(Item item, Player player)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                playerForImbue = player.GetModPlayer<AOPlayer>();
            if (player.GetModPlayer<AOPlayer>().imbue is not null && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed))
            {
                if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
                {
                    return FlipFloat(aoWeapon.AOSpeed * player.GetModPlayer<AOPlayer>().imbue.AOImbueSpeed);
                }
                else if (item.ModItem is null) // do not touch items from other mods
                {
                    return FlipFloat(player.GetModPlayer<AOPlayer>().imbue.AOImbueSpeed);
                }
            }
            return 1f;
        }

        public override float UseAnimationMultiplier(Item item, Player player)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
                playerForImbue = player.GetModPlayer<AOPlayer>();
            if (player.GetModPlayer<AOPlayer>().imbue is not null && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed))
            {
                if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
                {
                    return FlipFloat(aoWeapon.AOSpeed * player.GetModPlayer<AOPlayer>().imbue.AOImbueSpeed);
                }
                else if (item.ModItem is null) // do not touch items from other mods
                {
                    return FlipFloat(player.GetModPlayer<AOPlayer>().imbue.AOImbueSpeed);
                }
            }
            return 1f;
        }
    }

	public class AOPlayer : ModPlayer
	{
		public AOMagic? imbue = null;
	}
}