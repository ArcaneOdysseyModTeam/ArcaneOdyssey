using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
namespace ArcaneOdyssey.UI.ReadingSimulator.DevItem;

public class MarketableSpoky : ModItem
{
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 0;
    }

    public override void SetDefaults()
    {
        Item.damage = 0;
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.width = 26;
        Item.height = 38;
        Item.useAnimation = 20;
        Item.useTime = 20;
        Item.rare = ItemRarityID.Gray;
        Item.noMelee = true;
    }

    public override bool? UseItem(Player player)
    {
        try
        {
			ModUISystem instance = ModContent.GetInstance<ModUISystem>();
			instance.ShowReadingSimulator();
		}
        catch (Exception ex)
        {
            Main.NewText($"Error, please tell Spoky \n{ex}", new Color(255, 0, 255));
        }
        return true;
    }
}
