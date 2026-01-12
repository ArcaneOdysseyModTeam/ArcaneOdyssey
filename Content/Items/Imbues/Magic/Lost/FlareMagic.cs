using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class FlareMagic : AOMagic
	{
		public override float DashSpeed => 1.2f; // burst
		public override Color ImbueColour => new(255, 0, 0);
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override float AOImbueSpeed => 1f;
		public override float AOImbueSize => 1.1f;
		public override float AOImbueDamage => .925f;

		public override AOImbuableTier ImbuableTier => AOImbuableTier.Lost;

		public override SynergyEffects Effects => new(
			[ // these are debuffs cleared on hit
				
			],
			[

			]
			);

		public override void AddRecipes()
		{
			CreateLostRecipe(typeof(FireMagic));
		}
	}
}