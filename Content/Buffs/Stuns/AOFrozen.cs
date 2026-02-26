using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.Localization;

namespace ArcaneOdyssey.Content.Buffs.Stuns
{
	/// <summary>
	/// nobody will tell its a custom debuff thats the point lol
	/// </summary>
	public class AOFrozen : Stun
	{
		public const int VanillaID = BuffID.Frozen;

		public override string Texture => $"Terraria/Images/Buff_{VanillaID}";

		public override List<int> Counterparts => [VanillaID];

		public override LocalizedText Description => Language.GetText($"BuffDescription.{BuffID.Search.GetName(VanillaID)}");

		public override LocalizedText DisplayName => Language.GetText($"BuffName.{BuffID.Search.GetName(VanillaID)}");
	}
}
