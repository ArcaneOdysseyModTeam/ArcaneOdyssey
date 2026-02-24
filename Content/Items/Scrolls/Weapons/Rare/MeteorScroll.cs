using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Magic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Weapons.Rare
{
	public class MeteorScroll : RareScroll
	{
		public override string Texture => AOUtils.GetTexture<CannonScroll>();

		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = Item.useAnimation = 15;
			Item.damage = 150;
			Item.mana = 100;
			Item.UseSound = SoundID.Item82;
			Item.DamageType = DamageClass.Magic;
			Item.shootSpeed = 8f;
			Item.shoot = ModContent.ProjectileType<MeteorSpell>(); // does not need magic circle since it spawns offscreen
		}

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[Item.shoot] < 1;
		

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			var offsetX = Main.MouseWorld.X;
			var offsetY = Main.screenPosition.Y - (Main.screenHeight * .15f);
			position = new Vector2(offsetX, offsetY);
			velocity = Vector2.UnitY * velocity.Length();
		}
	}
}
