using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Projectiles.Base;

namespace ArcaneOdyssey.Content.Projectiles.Berserker.Effects
{
	public class PowderExplosion : AOPlayerProjectile
	{
		public override string Texture => Mod.Name + "/Backgrounds/Blank";

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 50;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
		}

		public override void AI()
		{
			if (++Projectile.ai[0] >= 60)
			{
				SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
				for (int n = 0; n < 10; n++)
				{
					Imbue?.ExplosionEffects(Projectile.Center);
					Imbue?.ExplosionEffects(Projectile.Center);
					SecondImbue?.ExplosionEffects(Projectile.Center);
				}
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.Kill();
				}
			
			}
		}

		public override bool? CanDamage() => Projectile.ai[0] >= 60;

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
