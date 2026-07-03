using ArcaneOdyssey.Buffs.Minions;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic.Minions;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Rare
{
	public class ElementalSpell : RareScroll
	{
		public override bool CanHaveMagic => true;
		public override ModSkill Skill => ModContent.GetInstance<ElementalSkill>();
	}

	public class ElementalSkill : AttackSkill
	{
		public override int Damage => 25;

		public override int Shoot => ModContent.ProjectileType<Elemental>();

		public override int Scroll => ModContent.ItemType<ElementalSpell>();

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			player.AddBuff(ModContent.BuffType<ElementalBuff>(), 2);
			ActivateAbility(player, imbue);
			return true;
		}

		public override void AttackStats(Player player, Imbuable imbue, ref Vector2 position, ref Vector2 velocity, ref int damage, ref float knockback)
		{
			position = Main.MouseWorld;
			player.LimitPointToPlayerReachableArea(ref position);
		}

		public override int ManaCost => 50;
	}
}
