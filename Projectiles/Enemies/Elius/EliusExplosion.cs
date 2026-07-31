using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Enemies.Elius
{
	public class EliusExplosion : BaseProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 30;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.height = Projectile.width = 200;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}
		public override void AI()
		{
			for (int n = 0; n < 3; n++)
			{
				Dust.NewDustDirect(Projectile.Center, 0, 0, DustID.WitherLightning, (Main.rand.NextFloat() - 0.5f) * 15f, (Main.rand.NextFloat() - 0.5f) * 15f, Scale: 1.2f).noGravity = true;
			}
		}

		public Imbuable Imbue => ModContent.GetInstance<LightningMagic>();

		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
		{
			modifiers = AOUtils.CalculateImbueDamage(Imbue, target, modifiers);
		}
	}
}
