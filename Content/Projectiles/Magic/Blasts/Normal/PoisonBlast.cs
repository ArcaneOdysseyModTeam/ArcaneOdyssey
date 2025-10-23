using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Normal
{
	public class PoisonBlast : BlastSpell
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 7;
		}
	}
}
