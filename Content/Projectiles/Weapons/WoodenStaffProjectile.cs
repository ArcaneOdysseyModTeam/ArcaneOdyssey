using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Projectiles.Weapons
{
	public class WoodenStaffProjectile : BaseStaffProjectile
	{
		public override float AOSpeed => 1.05f;
		public override float AOSize => .9f;
		public override float AODamage => 1f;
		public override AOWeaponTiers AOWeaponTier => AOWeaponTiers.Poor;
		public override AODebuffRequirement? Debuff => null;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.height = Projectile.width = 60;
			BaseScale = 2f;
		}
	}
}
