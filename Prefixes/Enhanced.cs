using ArcaneOdyssey.Items.Base;
using System;
using System.Collections.Generic;

namespace ArcaneOdyssey.Prefixes
{
	public class Enhanced : BasePrefix
	{
		public const int HasteBoost = 20;
		public override void ApplyAccessoryEffects(Player player)
		{
			player.ArcaneOdyssey().StatHaste += HasteBoost;
		}

		public override PrefixCategory Category => PrefixCategory.Accessory;

		public override void ModifyValue(ref float valueMult)
		{
			valueMult = 1.2f + 0.001f;
		}

		public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
		{
			yield return new(Mod, "PrefixAOSize", Mod.CustomLocalization("ArmourAutoTooltip.Haste", Math.Round(HasteBoost / BaseArmour.HasteDivision, 1)).Value)
			{
				IsModifier = true
			};
		}
	}
}
