using ArcaneOdyssey.Content.Items.Base;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	public class AuraScroll : CommonScroll
	{
		//public override bool CanHaveRelic => true;
		public override bool CanHaveFS => true;
		public override bool CanHaveMagic => true;

		public AuraMode Mode = AuraMode.Resistance;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (!HasCorrectImbue)
				return;

			if (!hideVisual && Main.GameUpdateCount % 2 == 0)
			{
				Imbue?.LingeringEffects(AOUtils.ScaleRectangleNotRef(player.Hitbox, 2f), player.velocity, player);
			}

			if (Main.myPlayer == player.whoAmI && AOKeybinds.CycleAuraMode.JustPressed)
			{
				if (Mode == AuraMode.Resistance)
				{
					Mode = AuraMode.Power;
				}
				else if (Mode == AuraMode.Power)
				{
					Mode = AuraMode.Destruction;
				}
				else if (Mode == AuraMode.Destruction)
				{
					Mode = AuraMode.Resistance;
				}
				Main.NewText(Mod.CustomLocalization("RandomWords.ModeCycled", Mode), Imbue.GetColour());
			}

			if (Mode == AuraMode.Resistance)
			{
				player.statLifeMax2 += Imbue.AuraHP(player);
			}

			if (Mode == AuraMode.Power)
			{
				player.GetDamage(DamageClass.Generic) += .15f;
			}

			if (Mode == AuraMode.Destruction)
			{
				player.ArcaneOdyssey().AOSizeStat += 50;
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.AddTooltip(new(Mod, "CycleKeybind", Mod.CustomLocalization("RandomWords.AuraMode", Mode, AOKeybinds.CycleAuraMode.GetAssignedKeys().FirstOrDefault(Mod.CustomLocalization("RandomWords.Unbound").Value)).Value));
			base.ModifyTooltips(tooltips);
		}

		public override void SaveData(TagCompound tag)
		{
			if (Mode != AuraMode.Resistance)
				tag.Add("AuraMode", (int)Mode);
		}

		public override void LoadData(TagCompound tag)
		{
			Mode = (AuraMode)tag.GetInt("AuraMode");
		}
	}

	public enum AuraMode
	{
		Resistance,
		Power,
		Destruction
	}
}
