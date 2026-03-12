using ArcaneOdyssey.Projectiles.Base;
using System;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Magic.Arrays.Lost
{
	public class PhoenixArray : ArraySpell
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 4;
		}

		public override void Rotate()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.spriteDirection = Math.Sign(Projectile.velocity.X);
		}
	}
}
