using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;

namespace ArcaneOdyssey.Content.Projectiles
{
	public class PowderExplosion : AOPlayerProjectile
	{
		public override string Texture => Mod.Name + "/Backgrounds/Blank";
		public bool hasExploded = false;

		public override void SetDefaults()
		{
			Projectile.height = Projectile.width = 100;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			hasExploded = false;
		}

		public override void AI()
		{
			if (++Projectile.ai[0] >= 60)
			{
				if (!hasExploded)
				{
					SoundEngine.PlaySound(SoundID.Item14, Projectile.Center, null);
					for (int n = 0; n < 10; n++)
					{
						Imbue?.ExplosionEffects(Projectile);
						Imbue?.ExplosionEffects(Projectile);
						SecondImbue?.ExplosionEffects(Projectile);
					}
				}
				hasExploded = true;
				if (Projectile.ai[0] >= 120)
				{
					Projectile.Kill();
				}
			}
		}

		public override bool? CanDamage() => Projectile.ai[0] >= 60;

		public override bool PreDraw(ref Color lightColor) => false;
	}
}
