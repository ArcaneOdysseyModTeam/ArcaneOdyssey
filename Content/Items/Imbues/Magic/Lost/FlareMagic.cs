using ArcaneOdyssey.Content.Buffs.DOT;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Imbues.Magic.Lost
{
	public class FlareMagic : AOMagic
	{
		public override float DashSpeed => 1.2f; // burst
		public override Color ImbueColour => new(255, 0, 0);
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override float AOScrollSpeed => 1f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollDamage => .925f;

		public override AODebuffRequirement[] ImbueDebuffs => [new(ModContent.BuffType<Singed>(), 60 * 5)];
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