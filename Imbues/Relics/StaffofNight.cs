using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Relics;
using ArcaneOdyssey.Skills.Base;
using Terraria.Audio;
using Terraria.DataStructures;


namespace ArcaneOdyssey.Imbues.Relics
{
	[LegacyName("NyxStaff")]
	public class StaffofNight : SpiritEnergy
	{
		public override int Value => 700;
		public override SoundStyle? ImbueSound => SoundID.Item8;
		public override Color ImbueColour => Color.Purple;
		public override float ImbueDamage => .9f;
		public override float ImbueSize => 1.1f;
		public override float ImbueSpeed => 1.1f;

		public override AttackSkill DefaultAttack => ModContent.GetInstance<SpiritBeamSkill>();

		public override SynergyEffects Effects => AOUtils.CopyDamageSynergiesFromImbue<ShadowMagic>();

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Item.staff[Type] = true;
		}
	}

	public class SpiritBeamSkill : AttackSkill
	{
		public override int Damage => 30;

		public override int Shoot => ModContent.ProjectileType<Nichtetheis>();

		public override int Scroll => 0;

		public override float Speed => 7f;

		public override int UseStyleID => ItemUseStyleID.Shoot;

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			ActivateAbility(player, imbue);
			return true;
		}
	}
}
