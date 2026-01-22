using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.PlayerClasses
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		public int? gel = null;

		public int? bloodDisease = null;

		public int pheonixHealing;

		public override void NaturalLifeRegen(ref float regen)
		{
			regen *= 1f + (pheonixHealing / 5f);
		}

		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
		{
			base.ModifyMaxStats(out health, out mana);
			if (bloodDisease.HasValue)
				health *= 2f / 3f;
		}

		public override void LoadData(TagCompound tag)
		{
			if (tag.TryGet("aodisease", out int disease))
			{
				bloodDisease = disease;
			}
		}

		public override void SaveData(TagCompound tag)
		{
			if (bloodDisease.HasValue)
				tag.Add("aodisease", bloodDisease.Value);
		}
	}
}
