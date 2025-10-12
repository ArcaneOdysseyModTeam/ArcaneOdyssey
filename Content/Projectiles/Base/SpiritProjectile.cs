using ArcaneOdyssey.Content.Items.Base;
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
		public override AOUtils.AODebuffRequirement? Debuff => null;
		public override void SetDefaults()
		{
			Projectile.DamageType = ModContent.GetInstance<SpiritDamage>();
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			base.ModifyHitNPC(target, ref modifiers);
			ManageSynergies();
		}

		public virtual void ManageSynergies() { }

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
		}
	}
}
