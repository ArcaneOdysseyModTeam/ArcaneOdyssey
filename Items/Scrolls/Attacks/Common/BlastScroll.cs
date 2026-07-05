using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Relics;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Attacks.Common
{
	public class BlastScroll : CommonScroll
	{
		public override bool CanHaveRelic => true;

		public override ModSkill Skill => ModContent.GetInstance<BlastSkill>();
	}

	public class BlastSkill : AttackSkill
	{
		public override int Time => 67;

		public override int Damage => 20;

		public override int Scroll => ModContent.ItemType<BlastScroll>();

		public override int Shoot => ModContent.ProjectileType<SpiritBlast>();

		public override float Speed => 7f;

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			ActivateAbility(player, imbue);
			return true;
		}
	}
}
