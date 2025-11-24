using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class RelicWeapon : AOBaseItem
	{
		public abstract int AOValue { get; }

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = ModContent.GetInstance<Oracle>();
			Item.noUseGraphic = true; // could add a virtual bool to toggle this later
			Item.noMelee = true;
			Item.value = GalleonToCopper(AOValue);
		}
	}
}
