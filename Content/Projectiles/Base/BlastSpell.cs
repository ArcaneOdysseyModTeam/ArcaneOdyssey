using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class BlastSpell : MagicSpell
	{
		public virtual void SetDefaultsSpell2() {}
        public override void SetDefaultsSpell()
        {
			Projectile.timeLeft = 5 * 60;
			SetDefaultsSpell2(); 
			BaseScale = Projectile.ai[2] != 2 ? 0.6f : 1.2f;
        }

		public override void AI()
		{
			aoPlayerOwner ??= Main.player[Projectile.owner].GetModPlayer<AOPlayer>();
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.wet && !thisMagic.CanBeWet)
			{
				Projectile.Kill();
				return;
			}
		}
    }
}
