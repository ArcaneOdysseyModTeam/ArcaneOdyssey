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

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
		}


		public override void AI()
		{
			if (Projectile.ai[0] == 0f)
			{
				Projectile.netUpdate = true;
				Projectile.ai[0] = 1f;
				BaseScale = Projectile.ai[2] != 2 ? 0.6f : 1.2f;
			}
			aoPlayerOwner ??= Main.player[Projectile.owner].AOPlayer();
			thisMagic ??= aoPlayerOwner.imbue;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (!thisMagic.CanBeWet && Projectile.wet)
			{
				Projectile.Kill();
				return;
			}
		}
	}
}
