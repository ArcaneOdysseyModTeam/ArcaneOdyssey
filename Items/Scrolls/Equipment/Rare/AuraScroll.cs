using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Items.Scrolls.Equipment.Rare
{
	public class AuraScroll : RareScroll
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
				Imbue?.LingeringEffects(player.Hitbox.Scaled(2f), player.velocity, player);
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
				Main.NewText(Mod.CustomLocalization("RandomWords.ModeCycled", Mode), Imbue.Colour);
			}

			if (Mode == AuraMode.Resistance)
			{
				player.statLifeMax2 += Imbue.AuraHP(player);
			}

			if (Mode == AuraMode.Power)
			{
				player.GetDamage(DamageClass.Generic) += .15f;
				if (Imbue is FightingStyle && player.ArcaneOdyssey().acumen)
				{
					player.GetDamage(DamageClass.Generic) += .05f;
				}
			}

			if (Mode == AuraMode.Destruction)
			{
				player.ArcaneOdyssey().StatSize += 35;
				if (Imbue is FightingStyle && player.ArcaneOdyssey().acumen)
				{
					player.ArcaneOdyssey().StatSize += 15;
				}
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
