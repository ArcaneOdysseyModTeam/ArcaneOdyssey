using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Prefixes
{
	public class Piercing : BasePrefix
	{
		public const int Bonus = 4;
		public override void ApplyAccessoryEffects(Player player)
		{
			player.GetArmorPenetration<GenericDamageClass>() += Bonus;
		}

		public override PrefixCategory Category => PrefixCategory.Accessory;

		public override void ModifyValue(ref float valueMult)
		{
			valueMult = 1.2f + 0.001f;
		}

		public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
		{
			yield return new(Mod, "PrefixAOPierce", Mod.CustomLocalization("ArmourAutoTooltip.Pierce", Bonus).Value)
			{
				IsModifier = true
			};
		}
	}
}
