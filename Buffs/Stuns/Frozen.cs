using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;

namespace ArcaneOdyssey.Buffs.Stuns
{
	/// <summary>
	/// nobody will tell its a custom debuff thats the point lol
	/// </summary>
	public class Frozen : Stun
	{
		public const int VanillaID = BuffID.Frozen;

		public override string Texture => $"Terraria/Images/Buff_{VanillaID}";

		public override List<int> Counterparts => [VanillaID];

		public override LocalizedText Description => Language.GetText($"BuffDescription.{BuffID.Search.GetName(VanillaID)}");

		public override LocalizedText DisplayName => Language.GetText($"BuffName.{BuffID.Search.GetName(VanillaID)}");

		public override void Update(Player player, ref int buffIndex)
		{
			player.frozen = true;
		}
	}
}
