using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class RelicWeapon : AOBaseItem
	{
		public abstract int AOValue { get; }
		public override ItemType ItemType => ItemType.Relic;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = DamageClass.MagicSummonHybrid;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.value = GalleonToCopper(AOValue);
		}
	}
}
