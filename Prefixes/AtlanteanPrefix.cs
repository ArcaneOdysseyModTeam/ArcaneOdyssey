using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Prefixes
{
	public class AtlanteanPrefix : BasePrefix
	{
		public override PrefixCategory Category => PrefixCategory.Accessory;

		public override void ModifyValue(ref float valueMult)
		{
			valueMult = 1.2f + 0.001f;
		}

		public override void ApplyAccessoryEffects(Player player)
		{
			player.ArcaneOdyssey().Insanity++;
		}

		public override IEnumerable<TooltipLine> GetTooltipLines(Item item)
		{
			yield return new TooltipLine(Mod, "RandomStat", "Empty!") { IsModifier = true };
			yield return new TooltipLine(Mod, "Insanity", Mod.CustomLocalization("ArmourAutoTooltip.Insanity", 1).Value) { IsModifier = true, IsModifierBad = true };
		}

		public override bool CanRoll(Item item) => true;

		public override float RollChance(Item item) => 0f;
	}
}
