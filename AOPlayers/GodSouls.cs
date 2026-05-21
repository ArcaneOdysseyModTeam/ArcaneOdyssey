using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.AOPlayers
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		public List<GodSoulID> Souls = [GodSoulID.None];

		public void AddSoul(GodSoulID id)
		{
			if (Main.myPlayer != Player.whoAmI || Souls.Contains(id))
				return;

			Souls.Add(id);
			Main.NewText(Mod.CustomLocalization("GodSouls.Obtained", Mod.CustomLocalization($"GodSouls.Soul{(int)id}").Value).Value);
		}
	}

	public class GodSoul // unused
	{
		public GodSoulID ID;
		public GodSoul(GodSoulID id)
		{
			ID = id;
			switch (id)
			{
				case GodSoulID.Poseidon:

					break;
				case GodSoulID.Athena:

					break;
			}
		}
	}

	public enum GodSoulID : byte
	{
		None,
		Poseidon,
		Athena
	}
}
