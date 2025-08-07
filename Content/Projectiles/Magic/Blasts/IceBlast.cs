using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Blasts
{
	public class IceBlast : BlastSpell
	{
		public override void SetDefaultsSpell()
		{
			Projectile.width = Projectile.height = 64; // placeholder sprite currently in effect
			Projectile.scale = .6f;
		}

		public override void AI()
        {
            aoPlayerOwner ??= Main.player[Projectile.owner].GetModPlayer<AOPlayer>();
            Projectile.position += Projectile.velocity;
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
				}
			}
		}
	}
}
