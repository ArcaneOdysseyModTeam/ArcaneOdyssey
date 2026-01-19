using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class RelicImbue : Imbuable, ILocalizedModType
	{
		public override string LocalizationCategory => base.LocalizationCategory + ".Relics";
		public virtual int AOValue => 0;

		public override float AOImbueDamage => AOScrollDamage;
		public override float AOImbueSize => AOScrollSize;
		public override float AOImbueSpeed => AOScrollSpeed;
		public override float AOScrollDamage => 1f;
		public override float AOScrollSize => 1f;
		public override float AOScrollSpeed => 1f;

		public override float? DashResist => 1.2f;

		public virtual bool NoUseGraphic => true;

		public override string AttackPrefix => "Spirit";

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = ModContent.GetInstance<OracleDamage>();
			Item.noUseGraphic = NoUseGraphic;
			Item.noMelee = true;
			Item.value = GalleonToCopper(AOValue);
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			if (Ability.HasValue)
				Ability.Value.GenerateTooltip();
		}

		public virtual WeaponAbility? Ability => null;

		public override bool AltFunctionUse(Player player) => player.ownedProjectileCounts[Item.shoot] < 1;

		public override bool CanShoot(Player player) => player.AltUse();
	}
}
