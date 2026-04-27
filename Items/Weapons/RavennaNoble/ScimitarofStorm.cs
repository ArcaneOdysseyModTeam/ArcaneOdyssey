using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Abilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Weapons.RavennaNoble
{
	public class ScimitarofStorm : Weapon
	{
		public override int Value => 210;

		public override ItemTiers WeaponTier => ItemTiers.Average;

		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override float Speed => 1.15f;
		public override float Damage => 1.05f;
		public override float Size => .85f;

		public int noUseCounter = 0;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<StormCaller>();
			ArcaneOdysseyMod.Sets.dualbladed[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useTime = (Item.useAnimation / 2) + 1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.DamageType = DamageClass.Melee;
			Item.shoot = ModContent.ProjectileType<TwinCrescent>();
			Item.shootSpeed = 7f;
		}

		public override bool CanShoot(Player player) => swings == 1;

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			damage /= 2;
			knockback /= 2;
		}

		public override Color Motif => Color.MediumPurple;

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.Draw(Sprite, position, frame, drawColor, 0, origin, scale, SpriteEffects.FlipHorizontally, 0f);
		}

		public int swings = 0;

		public override void UseAnimation(Player player)
		{
			noUseCounter = 0;
			if (swings == 0)
				ActivateAbility(player, true);
			if (++swings > 2)
			{
				swings = 0;
			}
		}

		public override void UpdateInventory(Player player)
		{
			if (!Main.mouseLeft && noUseCounter < 100)
			{
				noUseCounter++;
			}

			if (noUseCounter > 60 || player.PlayerItem().type != Type)
			{
				swings = 0;
			}
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Main.GetItemDrawFrame(Type, out var itemTexture, out var itemFrame);
			Vector2 drawOrigin = itemFrame.Size() / 2f;
			Vector2 drawPosition = Item.Bottom - Main.screenPosition - new Vector2(0, drawOrigin.Y);
			spriteBatch.Draw(itemTexture, drawPosition, itemFrame, lightColor, rotation, drawOrigin, scale, SpriteEffects.FlipHorizontally, 0f);
		}
	}

	public class TwinCrecsentsCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<ScimitarofStorm>();

		public override int CooldownLength => 60;
	}
}
