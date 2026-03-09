using ArcaneOdyssey.Content.Projectiles.Abilities;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;

namespace ArcaneOdyssey.Content.Projectiles.Berserker
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
					Kill();
					return;
				}
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue?.GetColour(Color.White) ?? Color.White;
			return base.PreDraw(ref lightColor);
		}
	}
}
