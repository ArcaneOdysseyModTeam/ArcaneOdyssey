using ArcaneOdyssey.Content.Projectiles.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class OldTridentProjectile : BaseSpearProjectile
	{
		public override AOWeaponTiers AOWeaponTier => AOWeaponTiers.Poor;
	}
}
