using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Prefixes
{
	public class Healing : BasePrefix
	{
		public const int Bonus = 2;
		public override void ApplyAccessoryEffects(Player player)
		{
			player.lifeRegen += Bonus;
		}

		public override PrefixCategory Category => PrefixCategory.Accessory;

		public override void ModifyValue(ref float valueMult)
		{
			valueMult = 1.2f + 0.001f;
		}

		public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
		{
			yield return new(Mod, "PrefixAORegen", Mod.CustomLocalization("ArmourAutoTooltip.Regen").Value)
			{
				IsModifier = true
			};
		}
	}
}
