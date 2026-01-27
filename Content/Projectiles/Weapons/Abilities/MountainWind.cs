using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Weapons.Abilities
{
	public class MountainWind : AOPlayerProjectile
	{
		public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.WeatherPainShot}";
		public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = Main.projFrames[ProjectileID.WeatherPainShot];
		}
		public override float AOSize => 1.05f;
		public override float AOSpeed => .9f;

		public override void SetDefaults()
		{
			Projectile.width = Projectile.height = 60;
			Projectile.friendly = true;
			Projectile.timeLeft = 120;
			Projectile.DamageType = DamageClass.Melee;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();

			if (++Projectile.frameCounter > 10)
			{
				if (++Projectile.frame >= Main.projFrames[Type])
				{
					Projectile.frame = 0;
				}
				SoundEngine.PlaySound(SoundID.Item1 with { Pitch = -.25f }, Projectile.Center);
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width /= 2;
			height /= 2;
			fallThrough = true;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
	}
}
