using ArcaneOdyssey.Imbues.Relics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Projectiles.Base
{
	public abstract class SpiritProjectile : AOPlayerProjectile, IImbuable
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
				Kill();
				return false;
			}
			return true;
		}
	}
}
