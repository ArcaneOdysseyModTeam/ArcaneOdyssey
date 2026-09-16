using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Projectiles.Base;

namespace ArcaneOdyssey.Projectiles.Magic.Effects
{
	public class PrismLinger : PlayerProjectile
	{
		public override bool PreDraw(ref Color lightColor) => false;
		public override string Texture => AOUtils.BlankTexture;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 200;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.tileCollide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.timeLeft = 120;
			Projectile.localNPCHitCooldown = (Projectile.timeLeft / 3) - 1;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.noEnchantmentVisuals = true;
		}

		public override void AI()
		{
			if (Main.dedServ)
				return;
			var dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.AncientLight, newColor: PrismMagic.rainbowColors[Main.GameUpdateCount % 3], Scale: 1.25f);
			dust.noGravity = true;
		}

		public override void SetStaticDefaults()
		{
			ArcaneOdysseyMod.Sets.imbueEffect[Type] = true;
		}
	}
}
