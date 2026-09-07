namespace ArcaneOdyssey.Guidebook.Pages
{
	public class SpiritWeapons : GuidebookPage
	{
		public override ushort PageNum => After<StrengthWeapons>();

		public override bool MetConditions(Player player) => player.HasItemInInventory(e => ArcaneOdysseyMod.Sets.weaponType[e.type] == WeaponType.Spiritual);
	}
}
