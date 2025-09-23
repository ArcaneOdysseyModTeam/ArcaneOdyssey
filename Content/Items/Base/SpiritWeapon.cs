using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class SpiritWeapon : ModItem
	{
		public override void SetDefaults() 
		{
			Item.DamageType = DamageClass.MagicSummonHybrid;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.useTime = Item.useAnimation = 100;
		}
	}
}
