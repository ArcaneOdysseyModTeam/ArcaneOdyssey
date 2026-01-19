using Terraria.ModLoader;

namespace ArcaneOdyssey.PlayerClasses
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		public int? gel = null;

		public int pheonixHealing;

		public override void NaturalLifeRegen(ref float regen)
		{
			regen *= 1f + (pheonixHealing / 5f);
		}
	}
}
