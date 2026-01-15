using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Berserker
{
	public class ShockwaveSmash : StrengthTechnique
	{
		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 100;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
			Projectile.localNPCHitCooldown = -1;
		}

		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 6;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.netUpdate = true;
				Projectile.velocity.Normalize();
				Projectile.Center = Owner.Center + (Projectile.velocity * 30);
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.ai[0] = 1;
			}

			if (++Projectile.frameCounter > 2)
			{
				Projectile.frameCounter = 0;
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Kill();
				}
			}
			BaseScale += .2f / 3;

			if (Projectile.TryGetImbue(out Imbuable imbue) && imbue is FightingStyle fs)
			{
				fs.ExplosionEffects(Projectile);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if (Imbue is not null)
			{
				lightColor = Color.Lerp(lightColor, Imbue.GetColour(lightColor), .5f);
			}
			return true;
		}
	}
}
