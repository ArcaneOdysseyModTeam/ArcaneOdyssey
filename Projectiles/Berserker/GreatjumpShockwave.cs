using ArcaneOdyssey.Projectiles.Abilities;
using ArcaneOdyssey.Projectiles.Base;
using Terraria;

namespace ArcaneOdyssey.Projectiles.Berserker
{
	public class GreatjumpShockwave : StrengthTechnique
	{
		public override string Texture => AOUtils.GetTexture<DevastateShockwave>();
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 12;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 594;
			Projectile.height = 108;
			Projectile.ownerHitCheck = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.friendly = true;
			Projectile.DamageType = AOUtils.TrueMelee();
		}

		public override void AI()
		{
			if (++Projectile.frameCounter >= 7)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					if (Projectile.owner == Main.myPlayer)
					{
						Kill();
						return;
					}
				}
			}
		}
	}
}
