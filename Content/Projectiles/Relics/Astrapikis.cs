using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Content.Projectiles.Relics
{
	public class Astrapikis : SpiritProjectile
	{
		public override string Texture => typeof(ColossalCleave).FullName.Replace('.', '/');
		public override float AOSize => .75f;
		public override float AOSpeed => 0.1f;

		public const int TimeLeftMax = 60;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = TimeLeftMax;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = TimeLeftMax / 2;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.height = 234;
			Projectile.width = 74;
			Projectile.AverageDimensions();
			Projectile.knockBack = 4.5f;
			Projectile.alpha = 255 / 2;
		}

		public override void OnSpawn(IEntitySource source)
		{
			base.OnSpawn(source);
			SoundEngine.PlaySound(Imbue?.ImbueSound, Projectile.Center, null);
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.position += Projectile.velocity * 30;
			Projectile.velocity = Vector2.Zero;
		}

		public override void AI()
		{
			if (Projectile.timeLeft == TimeLeftMax)
			{
				Projectile.netUpdate = true;
				for (int i = 0; i < 30; i++)
				{
					Imbue?.ExplosionEffects(Entity);
					Imbue?.Imbue?.ExplosionEffects(Projectile);
				}
			}

			Projectile.Opacity = Projectile.timeLeft / (float)TimeLeftMax;
			if (Projectile.timeLeft % 10 == 0)
			{
				Imbue?.ExplosionEffects(Projectile);
				Imbue?.Imbue?.ExplosionEffects(Projectile);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue.GetColor();
			return base.PreDraw(ref lightColor);
		}
	}
}
