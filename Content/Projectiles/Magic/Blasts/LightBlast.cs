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
	public class LightBlast : BlastSpell
	{
		public override void AI()
		{
			if (Projectile.ai[0] == 0f)
			{
				Projectile.ai[0] = 1f;
				BaseScale = Projectile.ai[2] != 2 ? 0.6f : 1.2f;
				Projectile.netUpdate = true;
			}
			aoPlayerOwner ??= Main.player[Projectile.owner].ArcaneOdyssey();
			Projectile.rotation += 1f;
			if (Projectile.TryGetImbue(out Imbuable imbue) && !imbue.CanBeWet && Projectile.wet)
			{
				Kill();
				return;
			}
		}
	}
}
