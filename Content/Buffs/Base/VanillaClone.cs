using Terraria;
using Terraria.ID;
using Terraria.Localization;

namespace ArcaneOdyssey.Content.Buffs.Base
{
	public abstract class VanillaClone : AODebuff
	{
		public abstract int VanillaID { get; }

		public override string Texture => $"Terraria/Images/Buff_{VanillaID}";

		public override int[] Counterparts => [VanillaID];

		public override LocalizedText Description => Language.GetText($"BuffDescription.{BuffID.Search.GetName(VanillaID)}");

		public override LocalizedText DisplayName => Language.GetText($"BuffName.{BuffID.Search.GetName(VanillaID)}");
	}
}
