using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Berserker;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Weapons.Lost
{
	public class CresendoTechnique : LostScroll
	{
		public override string Texture => AOUtils.GetTexture<AnnihilationScroll>();
		public override bool CanHaveFS => true;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = 60;
			Item.damage = 70;
			Item.shoot = ModContent.ProjectileType<Cresendo>();
			Item.shootSpeed = 7.5f;
			Item.DamageType = DamageClass.Melee;
			Item.UseSound = SoundID.DD2_ExplosiveTrapExplode;
		}

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[Item.shoot] < 1;
	}
}
