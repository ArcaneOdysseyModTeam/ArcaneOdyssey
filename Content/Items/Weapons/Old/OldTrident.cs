using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons.Old
{
	public class OldTrident : AORangedOrMeleeWeapon
	{
		public override AOWeaponTiers AOWeaponTier => AOWeaponTiers.Poor;
		public override AORarities AORarity => AORarities.Common;
		public override float AODamage => 1.05f;
		public override float AOSize => 1f;
		public override float AOSpeed => .925f;
		public override int AOValue => 25;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.shoot = ModContent.ProjectileType<OldTridentProjectile>();
			Item.shootSpeed = BaseSpearProjectile.Speed;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.autoReuse = true;
			Item.width = Item.height = 60;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
	}
}
