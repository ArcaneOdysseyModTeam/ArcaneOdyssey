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
		public override void SetDefaultsSpell2()
		{
			Projectile.width = Projectile.height = 64; // placeholder sprite currently in effect
		}
	}
}
