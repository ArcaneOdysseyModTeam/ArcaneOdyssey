using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Usable.Lost
{
	public class AcumenTechnique : LostScroll
	{
		public override bool ExtraConditionsForImbue(Imbuable imbue) => false;
		public override bool CanHaveFS => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.consumable = true;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.noUseGraphic = false;
			Item.useTime = Item.useAnimation = 30;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ArcaneOdyssey().acumen)
			{
				return false;
			}
			return true;
		}

		public override bool? UseItem(Player player)
		{
			if (player.itemAnimation > 0 && player.itemTime == 0)
			{
				player.itemTime = Item.useTime;
				if (player.ArcaneOdyssey().acumen)
				{
					return null;
				}
				player.ArcaneOdyssey().acumen = true;
				Main.NewText(Mod.CustomLocalization("RandomWords.Acumen"), Color.MediumVioletRed);
			}
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> list)
		{
			if (Main.LocalPlayer.ArcaneOdyssey().acumen)
				list.AddTooltip(new(Mod, "AlreadyConsumed", Mod.CustomLocalization("RandomWords.Acumen").Value), Color.MediumVioletRed);
		}
	}
}
