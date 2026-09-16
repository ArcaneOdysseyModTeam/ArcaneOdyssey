using ArcaneOdyssey.Imbues.Relics;

namespace ArcaneOdyssey.Projectiles.Base
{
	public abstract class SpiritProjectile : PlayerProjectile, IImbuable
	{
		public override Debuff? ProjectileDebuff => null;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.DamageType = DamageClass.Summon;
			Projectile.friendly = true;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Owner.MinionAttackTargetNPC = target.whoAmI;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			lightColor = Imbue?.Colour ?? Color.White;
			return base.PreDraw(ref lightColor);
		}

		public override bool PreAI()
		{
			Imbue ??= ModContent.GetInstance<SpiritEnergy>();
			if (Main.myPlayer == Projectile.owner && Imbue?.CanBeWet == false && Projectile.wet)
			{
				return TouchingWater();
			}
			return true;
		}


		/// <summary>
		/// Override for custom behaviour on touching water
		/// <para/>By default, cancels ai and kills the projectile
		/// </summary>
		/// <returns></returns>
		public virtual bool TouchingWater()
		{
			Kill();
			return false;
		}
	}
}
