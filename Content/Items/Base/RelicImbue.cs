using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class RelicImbue : Imbuable
	{
		public virtual int AOValue => 0;

		public override float DashResist => 1.2f;

		public virtual bool NoUseGraphic => true;

		public override string AttackPrefix => "Spirit";

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = ModContent.GetInstance<Oracle>();
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
