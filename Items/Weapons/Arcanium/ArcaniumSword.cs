using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Weapons.Old;

namespace ArcaneOdyssey.Items.Weapons.Arcanium
{
	public class ArcaniumSword : ArcaniumWeapon
	{
		public override ItemTiers WeaponTier => ItemTiers.Good;

		public override ItemRarities Rarity => ItemRarities.Rare;

		public override string Texture => AOUtils.GetTexture<OldSword>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = Item.height = 30;
		}
	}
}
