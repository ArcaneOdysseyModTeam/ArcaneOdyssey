using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Relics;
using Terraria.Audio;

namespace ArcaneOdyssey.Projectiles.Enemies
{
	public class EliusSlash : BaseProjectile
	{
		public override string Texture => AOUtils.SlashTexture;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 500;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.height = 200;
			Projectile.width = 70;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.scale = 0.6f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Color.Gold;
			return base.PreDraw(ref lightColor);
		}
		public override void AI()
		{
			base.AI();
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
	}
}
