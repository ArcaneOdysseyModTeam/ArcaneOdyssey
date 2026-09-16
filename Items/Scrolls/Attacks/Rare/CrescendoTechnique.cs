using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Berserker;
using ArcaneOdyssey.Skills.Base;
using Terraria.Audio;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Scrolls.Attacks.Rare
{
	public class CrescendoTechnique : RareScroll
	{
		public override bool MetConditions() => NPC.downedMechBossAny;
		public override bool CanHaveFS => true;
		public override ModSkill Skill => ModContent.GetInstance<CrescendoSkill>();
	}

	public class CrescendoSkill : AttackSkill
	{
		public override int Damage => 70;

		public override int Shoot => ModContent.ProjectileType<Crescendo>();

		public override int Scroll => ModContent.ItemType<CrescendoTechnique>();

		public override float Speed => 7.5f;

		public override int Time => 60;

		public override SoundStyle? ExtraSound => SoundID.DD2_ExplosiveTrapExplode;

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			ActivateAbility(player, imbue);
			return true;
		}

		public override bool PreActivate(Player player, Imbuable imbue) => player.ownedProjectileCounts[Shoot] < 1;
	}
}
