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
		public override float AOSize => .5f;
		public override float AOSpeed => 0.1f;

		public const int TimeLeftMax = 60 * 3;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.timeLeft = TimeLeftMax;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = Projectile.timeLeft / 2;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
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
				for (int i = 0; i < 30; i++)
					Imbue?.ExplosionEffects(Entity);
			Projectile.Opacity = Projectile.timeLeft / (float)TimeLeftMax;
			if (Projectile.timeLeft % 10 == 0)
				Imbue?.ExplosionEffects(Projectile);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			AOUtils.SimulateAOE(Projectile.width * 1.25f, Projectile.damage, Projectile.Center, Projectile.knockBack, Projectile, Projectile.DamageType);
			Imbue?.ExplosionEffects(Projectile);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = (Imbue?.ImbueColour ?? Color.LightBlue);
			return base.PreDraw(ref lightColor);
		}
	}
}
