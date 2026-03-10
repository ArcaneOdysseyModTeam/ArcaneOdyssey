using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Magic.Minions
{
	public class MinionMinionBeam : MagicSpell
	{
		public override string Texture => AOUtils.BlankTexture;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 4; // hitscan
			Projectile.extraUpdates = 100;
			Projectile.DamageType = DamageClass.MagicSummonHybrid;
			Projectile.timeLeft = 400;
			Projectile.tileCollide = false;
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
