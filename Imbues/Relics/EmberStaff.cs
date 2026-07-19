using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Buffs.Stuns;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Relics;
using ArcaneOdyssey.Skills.Base;
using Terraria.Audio;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Imbues.Relics
{
	public class EmberStaff : SpiritEnergy
	{
		public override int Value => 700;
		public override bool CanBeWet => false;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Color ImbueColour => new(252, 107, 3);
		public override float ImbueDamage => .95f;
		public override float ImbueSize => 1.1f;
		public override float ImbueSpeed => 1f;
		public override Combo[] CombinedDebuffs => [Combo.Create<CharredEffect, Petrified>()];
		public override SynergyEffects Effects => AOUtils.CopyDamageSynergiesFromImbue<FireMagic>();

		public override AttackSkill DefaultAttack => ModContent.GetInstance<FlogSkill>();

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Item.staff[Type] = true;
			ArcaneOdysseyMod.Sets.cold[Type] = false;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.noUseGraphic = false;
		}
	}

	public class FlogSkill : AttackSkill
	{
		public override int Damage => 120;

		public override int Shoot => ModContent.ProjectileType<Floganymai>();

		public override int Scroll => 0;

		public override int UseStyleID => ItemUseStyleID.Shoot;

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			ActivateAbility(player, imbue);
			return true;
		}

		public override void AttackStats(Player player, Imbuable imbue, ref Vector2 position, ref Vector2 velocity, ref int damage, ref float knockback)
		{
			position = Main.MouseWorld;
			player.LimitPointToPlayerReachableArea(ref position);
		}

		public override bool PreActivate(Player player, Imbuable imbue) => player.ownedProjectileCounts[Shoot] < 1;
	}
}
