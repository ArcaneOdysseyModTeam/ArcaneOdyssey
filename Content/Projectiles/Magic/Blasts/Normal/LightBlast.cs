using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts.Normal
{
	public class LightBlast : BlastSpell
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 3;
		}
	}
}
