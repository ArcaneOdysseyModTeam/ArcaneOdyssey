using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class BlastSpell : MagicSpell
	{
		public virtual void SetDefaultsSpell2()
		{

		}

        public override void SetDefaultsSpell()
        {
            Projectile.scale = .6f;
			Projectile.timeLeft = 5 * 60;
			SetDefaultsSpell2();
        }

		public override void AI()
		{
			aoPlayerOwner ??= Main.player[Projectile.owner].GetModPlayer<AOPlayer>();
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.spriteDirection = (Projectile.velocity.X > 0).ToDirectionInt();
			if (Projectile.wet)
			{
				Projectile.Kill();
				return;
			}
			if (aoPlayerOwner is not null)
			{
				thisMagic ??= aoPlayerOwner.imbue;
				if (thisMagic is not null)
				{
					Projectile.scale = thisMagic.AOMagicSize * 0.6f;
					if (Projectile.localAI[0] == 0)
					{
						Projectile.localAI[0] = 1;
						thisMagic.SpawningDust(Projectile.Center, Projectile.scale);
					}
				}
			}
		}
	}
}
