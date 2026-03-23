using ArcaneOdyssey.Items.Base;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Prefixes
{
	public class Explosive : AOPrefix
	{
		public const int SizeBoost = 20;
		public override void ApplyAccessoryEffects(Player player)
		{
			player.ArcaneOdyssey().StatSize += SizeBoost;
		}

		public override PrefixCategory Category => PrefixCategory.Accessory;

		public override void ModifyValue(ref float valueMult)
		{
			valueMult = 1.2f + 0.001f;
		}

		public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
		{
			yield return new(Mod, "PrefixAOSize", Mod.CustomLocalization("ArmourAutoTooltip.Size", Math.Round(SizeBoost / Armour.SizeDivision, 1)).Value)
			{
				IsModifier = true
			};
		}
	}
}
