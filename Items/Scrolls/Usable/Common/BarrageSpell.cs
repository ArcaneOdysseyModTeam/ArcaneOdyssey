using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Common
{
	public class BarrageSpell : CommonScroll
	{
		public override bool MetConditions() => NPC.downedBoss2;
		public override bool CanHaveMagic => true;

		public override ModSkill Skill => ModContent.GetInstance<BarrageSkill>();
	}

	public class BarrageSkill : AttackSkill
	{
		public override int Damage => 5;

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			imbue.CreateMagicCircle(player, MagicCircleMode.Barrage, false, Shoot, spread: imbue.ApplySpeed(MathHelper.PiOver4 / 2f));
			return false;
		}

		public override int ManaCost => 5;

		public override bool Channel => true;

		public override int Scroll => ModContent.ItemType<BarrageSpell>();

		public override int Shoot => ModContent.ProjectileType<BlastSpell>();
	}
}
