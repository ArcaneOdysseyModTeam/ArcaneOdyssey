using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Cannons.Lost
{
	public class DarknessCannon : CannonSpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 7;
		}

		public override void PostDraw(Color lightColor)
		{
			// pulse effect goes here, add to blast and pulsar too
		}
	}
}
