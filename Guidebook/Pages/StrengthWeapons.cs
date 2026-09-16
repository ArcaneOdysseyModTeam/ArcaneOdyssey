namespace ArcaneOdyssey.Guidebook.Pages
{
	public class StrengthWeapons : GuidebookPage
	{
		public override ushort PageNum => After<AboutGodSouls>();

		public override bool MetConditions(Player player) => player.HasItemInInventory(e => ArcaneOdysseyMod.Sets.weaponType[e.type] == WeaponType.Strength);
	}
}
