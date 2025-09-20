using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Projectiles.Base
{
	public abstract class StrengthTechnique : AOPlayerProjectile
	{
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;
            SetDefaultsSkill();
		}

		public virtual void SetDefaultsSkill() { }
	}
}
