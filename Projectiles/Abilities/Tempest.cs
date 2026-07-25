using ArcaneOdyssey.Projectiles.Base;
using System.Collections.Generic;

namespace ArcaneOdyssey.Projectiles.Abilities
{
	public class Tempest : PlayerProjectile
	{
		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0]++;
				NetUpdate();
				Projectile.spriteDirection *= Main.rand.Next([-1, 1]);
			}
			Projectile.velocity = -(Vector2.UnitY * ApplySize(3f));
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (Projectile.frameCounter++ >= 5)
			{
				Projectile.frameCounter = 0;
				if (Projectile.frame++ >= (Main.projFrames[Type] + 1))
				{
					Kill();
				}
			}
			Projectile.Opacity = .5f + (Projectile.frame / (float)Main.projFrames[Type]);
		}

		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
		{
			overPlayers.Add(index);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.projFrames[Type] = 10;
		}

		public override bool CanHaveImbueVFX => false;

		public override float Size => 2.5f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = 50;
			Projectile.width = 150;
			Projectile.DamageType = AOUtils.TrueMelee();
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.timeLeft = 60;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 25;
			Projectile.ownerHitCheck = true;
			Projectile.penetrate = -1;
			Projectile.hide = true;
		}

		public override SpriteEffects FlippedMode => SpriteEffects.FlipHorizontally;

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue?.Colour.MultiplyRGB(lightColor) ?? lightColor;
			return base.PreDraw(ref lightColor);
		}
	}
}
