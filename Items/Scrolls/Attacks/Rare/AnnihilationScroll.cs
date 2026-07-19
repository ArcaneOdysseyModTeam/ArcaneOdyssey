using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Skills.Base;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Scrolls.Attacks.Rare
{
	public class AnnihilationScroll : RareScroll
	{
		public override bool MetConditions() => NPC.downedMechBossAny;
		public override bool CanHaveMagic => true;
		public override ModSkill Skill => ModContent.GetInstance<AnnihilationSkill>();
	}

	public class AnnihilationSkill : AttackSkill
	{
		public override int Damage => 60;
		public override int ManaCost => 200;
		public override float Knockback => 0f;
		public override int Scroll => ModContent.ItemType<AnnihilationScroll>();

		public override int Shoot => ModContent.ProjectileType<AnnihilationSpell>();

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			player.ArcaneOdyssey()?.StartDash(new Annihilation(imbue), -2, imbue, false);
			ActivateAbility(player, imbue);
			imbue.CreateMagicCircle(player, MagicCircleMode.Basic, true, position: player.Bottom, rotation: -MathHelper.PiOver2);
			return false;
		}

		public override bool PreActivate(Player player, Imbuable imbue) => player.ownedProjectileCounts[Shoot] < 1;

		public override int UseStyleID => ItemUseStyleID.HiddenAnimation;

	}

	public class Annihilation(Imbuable imbue) : ModDash(imbue.Item)
	{
		public override bool Immune => false;

		public override bool LocksPlayer => true;

		public override float DashSpeed => 23;

		public override int Cooldown => 0;

		public override int DashMax => 10;

		public override bool ContactDamage => false;

		public override bool OnHit(Player player, NPC target) => false;

		public override void OnEnd(Player player)
		{
			AOUtils.ShootProjectile(Source.GetSource_ItemUse(player), player.Center, player.SafeDirectionTo(Main.MouseWorld) * 10, ModContent.ProjectileType<AnnihilationSpell>(), (int)imbue.Item.ArcaneOdyssey().owner.GetTotalDamage(imbue.Item.DamageType).ApplyTo(imbue.Item.damage), 0f, player.whoAmI, imbue, imbue.Imbue, true);
		}
	}
}
