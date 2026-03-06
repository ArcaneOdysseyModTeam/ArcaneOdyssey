using ArcaneOdyssey.Content.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Arrays.Normal
{
	public class LightArray : ArraySpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 6;
		}
		public override void Rotate()
		{
			Projectile.rotation += 0.1f * Projectile.direction;
		}
	}
}
