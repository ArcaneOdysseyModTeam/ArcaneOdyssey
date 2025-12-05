using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class RelicWeapon : Imbuable
	{
		public virtual int AOValue => 0;

        public override float DashResist => 1.2f;

		public virtual bool NoUseGraphic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = ModContent.GetInstance<Oracle>();
			Item.noUseGraphic = NoUseGraphic;
			Item.noMelee = true;
			Item.value = GalleonToCopper(AOValue);
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanShoot(Player player) => player.AltUse();
	}
}
