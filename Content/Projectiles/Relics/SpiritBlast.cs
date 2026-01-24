using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Projectiles.Relics
{
	public class SpiritBlast : SpiritProjectile
	{
		public override string Texture => Mod.Name + "/Backgrounds/Blank";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 64;
			Projectile.timeLeft = 2 * 60;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
				Projectile.netUpdate = true;
			}

			if (!Main.dedServ)
			{
				for (float i = 0; i < 5; i++)
				{
					Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IcyMerman, Projectile.velocity.X / 2, Projectile.velocity.Y / 2).noGravity = true;
				}
			}
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = Projectile.width / 4;
			height = Projectile.height / 4;
			fallThrough = true;
			return true;
		}

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
