using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class SunMagic : AOMagic
	{
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;
		public override float AOImbueDamage => 1f;
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1f;

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(FireMagic), typeof(PlasmaMagic));
		}
	}
}
