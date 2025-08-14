import os

magics = "Acid Ash Crystal Earth Explosion Fire Glass Light Lightning Magma Metal Plasma Poison Sand Shadow Snow Water Wind Wood".split()

basespellthing = """using ArcaneOdyssey.Content.Items.Base;
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
	public class {magicname}Blast : BlastSpell
	{
		public override void SetDefaultsSpell2()
		{
			Projectile.width = Projectile.height = 64; // placeholder sprite currently in effect
		}
	}
}
"""

path = "Content/Projectiles/Magic/Blasts/"

for magic in magics:
	text = basespellthing.format(magicname=magic)
	with open(path + f"{magic}Blast.cs", "w") as w:
		w.write(text)
	with open(path + "IceBlast.png", "b+r") as w:
		img = w.read()
		with open(path + f"{magic}Blast.png", "b+w") as w2:
			w2.write(img)
