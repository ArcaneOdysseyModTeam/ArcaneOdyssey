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
	public class ColossalGreatsword : AOWeapon
	{
		public override float AOSpeed => .65f;
		public override float AOSize => 1.2f;
		public override float AODamage => 1.15f;
		public override int AOValue => 250;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOWeaponTiers AOWeaponTier => AOWeaponTiers.Good;
        public override bool? Arcanium => false;

		public override void SetDefaultsWeapon()
		{
			Item.width = 86;
			Item.height = 86;
			Item.useStyle = ItemUseStyleID.Swing;
		}
	}
}
