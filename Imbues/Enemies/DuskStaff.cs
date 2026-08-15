using ArcaneOdyssey.Imbues.Relics;

namespace ArcaneOdyssey.Imbues.Enemies
{
	public class DuskStaff : StaffofNight
	{
		public override string Texture => AOUtils.GetTexture<StaffofNight>();
		protected override Color? SpiritColourOverride => EvilColour;

		public override void UpdateInventory(Player player)
		{
			Item.TurnToAir(true);
		}
	}
}
