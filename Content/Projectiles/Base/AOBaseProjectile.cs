using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class AOBaseProjectile : ModProjectile
	{
		public float? BaseScale;
		public Vector2? OriginalDimensions = null;
		public int FramesAlive = 0;
        public bool IsSpell => this is MagicSpell;

        /// <summary>
        /// Kills the projectile.
        /// </summary>
        public void Kill()
		{
			Projectile.Kill();
		}
	}
}
