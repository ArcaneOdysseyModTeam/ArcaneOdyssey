using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Berserker;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Usable.Lost
{
	public class CrescendoTechnique : LostScroll
	{
		public override bool CanHaveFS => true;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = 60;
			Item.damage = 70;
			Item.shoot = ModContent.ProjectileType<Crescendo>();
			Item.shootSpeed = 7.5f;
			Item.DamageType = DamageClass.Melee;
			Item.UseSound = SoundID.DD2_ExplosiveTrapExplode;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, type);
			return true;
		}

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[Item.shoot] < 1;
	}
}
