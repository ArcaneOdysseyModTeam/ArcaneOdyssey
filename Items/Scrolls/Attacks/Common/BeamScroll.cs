using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Attacks.Common
{
	public class BeamScroll : CommonScroll
	{
		public override bool MetConditions() => NPC.downedBoss2;
		public override bool CanHaveMagic => true;

		public override ModSkill Skill => ModContent.GetInstance<BeamSkill>();
	}

	public class BeamSkill : AttackSkill
	{
		public override int Damage => 100;

		public override int Scroll => ModContent.ItemType<BeamScroll>();

		public override float Knockback => 0f;

		public override bool Channel => true;

		public override int ManaCost => 30;

		public override int Time => 40;

		public override int Shoot => ModContent.ProjectileType<BeamSpell>();

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			imbue.CreateMagicCircle(player, Projectiles.MagicCircleMode.Basic, false, Shoot);
			return false;
		}
	}
}
