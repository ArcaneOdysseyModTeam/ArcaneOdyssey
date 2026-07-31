using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Skills.Base;
using Terraria.Audio;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Scrolls.Attacks.Rare
{
	public class ArrayScroll : RareScroll
	{
		public override bool CanHaveMagic => true;

		public override ModSkill Skill => ModContent.GetInstance<ArraySkill>();
	}

	public class ArraySkill : AttackSkill
	{
		public override int Damage => 190;
		public override int ManaCost => 50;

		public override int Time => 40;
		public override int Shoot => ModContent.ProjectileType<ArraySpell>();

		public override int Scroll => ModContent.ItemType<ArrayScroll>();

		public override void AttackStats(Player player, Imbuable imbue, ref Vector2 position, ref Vector2 velocity, ref int damage, ref float knockback)
		{
			velocity = velocity.Length() * -Vector2.UnitY;
		}

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			imbue.CreateMagicCircle(player, MagicCircleMode.Basic, true, Shoot, position: player.Top, rotation: -MathHelper.PiOver2);
			return false;
		}

		public override bool PreActivate(Player player, Imbuable imbue) => player.ownedProjectileCounts[Shoot] < 1;

		public override SoundStyle? ExtraSound => SoundID.DD2_GhastlyGlaiveImpactGhost;
	}
}
