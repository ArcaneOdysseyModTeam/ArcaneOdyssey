using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.VFX.Gores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Berserker
{
	public class BasicStrike : StrengthTechnique
	{
		public override string Texture => Mod.Name + "/Backgrounds/Blank";

		public override bool CanHaveImbueVFX => false;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = Projectile.height = 60;
			Projectile.timeLeft = 10;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.MeleeNoSpeed;
			Projectile.localNPCHitCooldown = -1;
			Projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{
			if (Projectile.ai[0] == 0)
			{
				Projectile.ai[0] = 1;
				Imbue?.SpawningEffects(Projectile.Hitbox, Projectile.velocity);
				Projectile.Center = Owner.Center + (Projectile.velocity * 10);
				Projectile.rotation = Projectile.velocity.ToRotation();
				AOUtils.SpawnGore(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.GoreType<Impact>(), Imbue.AOScrollSize * (SecondImbue?.AOScrollSize ?? 1f));
			}
		}
	}
}
