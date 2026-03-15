using ArcaneOdyssey.Projectiles.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;

namespace ArcaneOdyssey.Projectiles.Relics
{
	public class Astrapikis : SpiritProjectile
	{
		public override string Texture => AOUtils.SlashTexture;
		public override float AOSize => 1.5f;

		public const int TimeLeftMax = 90;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ProjectileID.Sets.TrailingMode[Type] = 0;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = TimeLeftMax;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = TimeLeftMax / 2;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.height = Projectile.width = 80;
		}

		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
			SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center);
			Projectile.position += Projectile.velocity * 50;
		}

		public override void AI()
		{
			Projectile.rotation = Projectile.velocity.ToRotation();
			if (Projectile.timeLeft == TimeLeftMax)
			{
				if (Projectile.owner == Main.myPlayer)
				{
					Projectile.netUpdate = true;
					Projectile.netSpam = 0;
				}
				for (int i = 0; i < 30; i++)
				{
					Imbue?.ExplosionEffects(Projectile.Center);
					SecondImbue?.ExplosionEffects(Projectile.Center);
				}
			}

			Projectile.Opacity = Projectile.timeLeft / (float)TimeLeftMax;
			if (Projectile.timeLeft % 10 == 0)
			{
				Imbue?.ExplosionEffects(Projectile.Center);
				SecondImbue?.ExplosionEffects(Projectile.Center);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			for (int k = Projectile.oldPos.Length - 1; k > -1; k--)
			{
				Vector2 drawPos = Projectile.oldPos[k] + (Projectile.Size / 2f) + new Vector2(0f, Projectile.gfxOffY);
				var colour2 = Projectile.GetAlpha(Imbue?.GetColour() ?? Color.LightBlue) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(Sprite, drawPos - Main.screenPosition, null, colour2, Projectile.rotation, Sprite.Size() / 2, Projectile.scale - (.05f * k), SpriteEffects.None, 0);
			}
			return false;
		}
	}
}
