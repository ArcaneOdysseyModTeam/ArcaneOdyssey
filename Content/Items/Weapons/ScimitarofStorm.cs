using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class ScimitarofStorm : AORangedOrMeleeWeapon
	{
		public override int AOValue => 210;

		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;

		public override AORarities AORarity => AORarities.Rare;

		public override float AOSpeed => 1.15f;
		public override float AODamage => 1.05f;
		public override float AOSize => .85f;


		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = (Item.useAnimation / 2) + 1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ModContent.ProjectileType<TwinCrescent>();
			Item.shootSpeed = 7f;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			damage /= 2;
			knockback /= 2;
		}

		public override WeaponAbility? Ability => new(Mod, "Twin Crescents", "Slash both blades one after the other, sending two flying slashes towards the target", Color.Gold);

		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.Draw(Sprite, position, frame, drawColor, 0, origin, scale, SpriteEffects.FlipHorizontally, 0f);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Main.GetItemDrawFrame(Type, out var itemTexture, out var itemFrame);
			Vector2 drawOrigin = itemFrame.Size() / 2f;
			Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, drawOrigin.Y);
			spriteBatch.Draw(itemTexture, drawPosition, itemFrame, lightColor, rotation, drawOrigin, scale, SpriteEffects.FlipHorizontally, 0f);
		}
	}
}
