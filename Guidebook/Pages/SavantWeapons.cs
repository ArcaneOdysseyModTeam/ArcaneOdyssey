namespace ArcaneOdyssey.Guidebook.Pages
{
	public class SavantWeapons : GuidebookPage
	{
		public override ushort PageNum => After<SpiritWeapons>();

		public override bool MetConditions(Player player) => player.HasItemInInventory(e => ArcaneOdysseyMod.Sets.weaponType[e.type] == WeaponType.Savant);
	}
}
