using ArcaneOdyssey.Content.Buffs.Minions;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Magic.Minions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Usable.Lost
{
	public class ElementalSpell : LostScroll
	{
		public override string Texture => AOUtils.GetTexture<AnnihilationScroll>();
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.buffType = ModContent.BuffType<ElementalBuff>();
			Item.shoot = ModContent.ProjectileType<Elemental>();
			Item.damage = 25;
			Item.useTime = Item.useAnimation = 30;
			Item.mana = 50;
			Item.DamageType = DamageClass.MagicSummonHybrid;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position = Main.MouseWorld;
			player.LimitPointToPlayerReachableArea(ref position);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			player.AddBuff(Item.buffType, 2);
			return true;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Type] = true;
			ItemID.Sets.StaffMinionSlotsRequired[Type] = 1;
		}
	}
}
