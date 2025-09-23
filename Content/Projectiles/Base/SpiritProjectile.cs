using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class SpiritProjectile : AOPlayerProjectile
	{
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.MagicSummonHybrid;
			SetSpiritDefaults();
		}

		public abstract void SetSpiritDefaults();

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
		}
	}
}
