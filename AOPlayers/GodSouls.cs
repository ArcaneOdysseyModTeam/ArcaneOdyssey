using ArcaneOdyssey.GodSouls;
using System.Collections.Generic;
using System.Linq;

namespace ArcaneOdyssey.AOPlayers
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		public List<GodSoul> Souls = [];
		private static List<string> cachedUnloadedSouls = [];

		public void AddSoul(GodSoul soul)
		{
			if (Main.myPlayer != Player.whoAmI || soul is null)
				return;

			if (!Souls.Select(e => e.Type).Contains(soul.Type))
			{
				Souls.Add(soul);
				Main.NewText(Mod.CustomLocalization("GodSouls.Obtained", soul.DisplayName.Value).Value);
			}
		}
	}
}
