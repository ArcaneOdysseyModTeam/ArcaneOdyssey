using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class PiercingGale : AOPlayerProjectile
	{
		public override string Texture => AOUtils.BlankTexture;
		public override AODebuffRequirement? Debuff => null;
		public const int DustCount = 30;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 64;
			Projectile.friendly = true;
			Projectile.timeLeft = 60 * (Projectile.extraUpdates + 1);
			Projectile.DamageType = DamageClass.Melee;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
			}
		}

		public override bool PreKill(int timeLeft)
		{
			if (!Main.dedServ)
			{
				for (float i = 0; i < DustCount; i++)
				{
					var centre2 = (MathHelper.TwoPi / DustCount * i).ToRotationVector2() * (Projectile.width * 2);
					var dust2 = AOUtils.NewDustImperfect(centre2 + Projectile.Center, DustID.BubbleBurst_White, (-centre2) / 5, 0, Imbue?.GetColour() ?? Color.White, 1.5f);
					dust2.noLight = true;
					dust2.noGravity = true;
					Imbue?.ExplosionEffects(Projectile.Center);
				}
			}
			return base.PreKill(timeLeft);
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 4;
			height /= 4;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
