using ArcaneOdyssey.Content.Items.FightingStyles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class Cannonball : ModProjectile // not imbuable, already for imbue
	{
		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 18;
			Projectile.friendly = true;
		}

		public override void AI()
		{
			Projectile.rotation += MathHelper.Pi / 30 * Projectile.direction;
			Projectile.velocity += Vector2.UnitY / 10;
		}

		public override void OnKill(int timeLeft)
		{
			var fist = (CannonFist)new Item(ModContent.ItemType<CannonFist>()).ModItem;
			fist.KillEffects(Projectile);
		}
	}
}
