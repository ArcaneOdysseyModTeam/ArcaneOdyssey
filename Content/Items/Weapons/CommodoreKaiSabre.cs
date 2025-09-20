using ArcaneOdyssey.Content.Buffs;
using ArcaneOdyssey.Content.Items.Base;
using Steamworks;
using System.Linq.Expressions;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class CommodoreKaiSabre : AOWeapon
	{
		public override float AOSpeed => 1.1f;
		public override float AOSize => 1.1f;
		public override float AODamage => .925f;
		public override int AOValue => 900;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOWeaponTiers AOWeaponTier => AOWeaponTiers.Good;

		public override void SetDefaultsWeapon()
		{
			Item.width = 52;
			Item.height = 54;
			Item.useStyle = ItemUseStyleID.Swing;
		}
	}
}
