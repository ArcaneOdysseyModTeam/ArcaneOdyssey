using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Rare
{
	public class AcumenTechnique : RareScroll
	{
		public override bool MetConditions()
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				return !Main.LocalPlayer.ArcaneOdyssey().acumen;
			}
			else
			{
				return true;
			}
		}

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
				if (Main.myPlayer == player.whoAmI)
				{
					if (ArcaneOdysseyClientConfig.Instance.AbilityText)
						CombatText.NewText(player.Hitbox, Color.White, SkillName + "!", true); // manually do since its never imbued
					Main.NewText(Mod.CustomLocalization("RandomWords.Acumen"), Color.MediumVioletRed);
				}
			}
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			if (Main.LocalPlayer.ArcaneOdyssey().acumen)
				tooltips.AddTooltip(new(Mod, "AlreadyConsumed", Mod.CustomLocalization("RandomWords.Acumen").Value), Color.MediumVioletRed);
		}
	}
}
