using ArcaneOdyssey.Content.Buffs.Base;
using System.Collections.Generic;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
	public class Flammable : VanillaClone
	{
		public override int VanillaID => BuffID.Oiled;

		public override List<int> Counterparts => [..base.Counterparts, BuffID.Slimed, BuffID.GelBalloonBuff];

		public override string Texture => AOUtils.GetTexture<Flammable>();
	}
}
