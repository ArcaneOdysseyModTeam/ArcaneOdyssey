using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Relics
{
	public class Nichtetheis : SpiritProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override Debuff? ProjectileDebuff => new(ModContent.BuffType<DrainedEffect>(), 60 * 5);
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 40; // hitscan
			Projectile.extraUpdates = 100;
			Projectile.timeLeft = 60;
		}

		public override bool PreDraw(ref Color lightColor) => false;

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
	}
}
