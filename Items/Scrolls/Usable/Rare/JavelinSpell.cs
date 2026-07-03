using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Rare
{
	public class JavelinSpell : RareScroll
	{
		public override bool CanHaveMagic => true;

		public override ModSkill Skill => ModContent.GetInstance<JavelinSkill>();
	}
	public class JavelinSkill : AttackSkill
	{
		public override int Damage => 55;

		public override int Time => 20;

		public override int ManaCost => 45;

		public override bool Channel => true;

		public override int Shoot => ModContent.ProjectileType<Javelin>();

		public override int Scroll => ModContent.ItemType<JavelinSpell>();

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			imbue.CreateMagicCircle(player, Projectiles.MagicCircleMode.Rotating, false);
			return true;
		}
		public override bool PreActivate(Player player, Imbuable imbue) => player.ownedProjectileCounts[Shoot] < 1;

		public override int UseStyleID => ItemUseStyleID.Swing;

	}
}
