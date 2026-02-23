using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Enemies
{
	public class LaelusExplosion : AOBaseProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override void SetDefaults()
		{
			Projectile.DamageType = OracleDamage.Instance;
			Projectile.timeLeft = 25;
			Projectile.hostile = true;
			Projectile.height = Projectile.width = 170;
			Projectile.tileCollide = false;
		}
		public Imbuable Imbue = ModContent.GetInstance<NyxStaff>();

		public override void AI()
		{
			Imbue?.ExplosionEffects(Projectile.Center);
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
