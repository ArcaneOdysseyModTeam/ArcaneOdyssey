using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.UI.SubWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Creative;

namespace ArcaneOdyssey.UI._DebugUIItems;

/// <summary>
/// Testing debug items for <see cref="SubWorldUI"/>
/// </summary>
public class JohnBowMan : BaseItem
{
	// Spoky 2026 Sept 30: Won't ask why MarketableSpoky was turned to a BaseItem, but I'll do this one for it aswell
	public override ItemRarities Rarity => ItemRarities.Special;

	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 0;
	}

	public override void SetDefaults()
	{
		Item.useStyle = ItemUseStyleID.HoldUp;
		Item.width = 26;
		Item.height = 38;
		Item.useAnimation = Item.useTime = 20;
		Item.noMelee = true;
	}

	public override bool? UseItem(Player player)
	{
		try
		{
			ModUISystem instance = ModContent.GetInstance<ModUISystem>();
			instance.ShowSubWorlds();
		}
		catch (Exception ex)
		{
			Main.NewText($"Error, please tell Spoky \n{ex}", new Color(255, 0, 255));
		}
		return true;
	}
}
