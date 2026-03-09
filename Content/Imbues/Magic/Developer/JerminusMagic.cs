using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;

namespace ArcaneOdyssey.Content.Imbues.Magic.Developer
{
	public class JerminusMagic : AOMagic
	{
		public override Color ImbueColour => new(255, 0, 0);
		public override float AOScrollSpeed => 3f;
		public override float AOScrollSize => 3.5f;
		public override float DashSpeed => 1.4f;
		public override float AOScrollDamage => .2f;
		public override AOImbuableTier ImbuableTier => AOImbuableTier.Developer;
		public override Debuff[] ImbueDebuffs => [Debuff.Create<Trauma>()];
	}
}
