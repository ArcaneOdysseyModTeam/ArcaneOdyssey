using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Rare
{
	public class MeteorScroll : RareScroll
	{
		public override bool MetConditions() => NPC.downedPlantBoss;
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = 15;
			Item.damage = 300;
			Item.mana = 100;
			Item.UseSound = SoundID.Item82;
			Item.DamageType = DamageClass.Magic;
			Item.shootSpeed = 6f;
			Item.shoot = ModContent.ProjectileType<MeteorSpell>(); // does not need magic circle since it spawns offscreen
		}

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[Item.shoot] < 1;


		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position = new Vector2(Main.MouseWorld.X, Main.screenPosition.Y);
			player.LimitPointToPlayerReachableArea(ref position);
			position.Y -= Main.maxScreenH * .15f;
			velocity = Vector2.UnitY * velocity.Length();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player);
			return true;
		}
	}
}
