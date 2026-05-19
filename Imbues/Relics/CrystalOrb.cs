using ArcaneOdyssey.Buffs.Minions;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Projectiles.Relics.Minions;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Relics
{
	public class CrystalOrb : SpiritEnergy
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.GamepadWholeScreenUseRange[Type] = true;
			ItemID.Sets.LockOnIgnoresCollision[Type] = true;
			ItemID.Sets.StaffMinionSlotsRequired[Type] = 1;
		}

		public override Color ImbueColour => new(255, 255, 0, 255);
		public override SoundStyle? ImbueSound => SoundID.Item9;
		public override float ImbueSpeed => 1.2f;
		public override float ImbueSize => 1f;
		public override float ImbueDamage => 0.9f;
		public override int Value => 700;

		public override SynergyEffects Effects => AOUtils.CopyDamageSynergiesFromImbue<LightMagic>();

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 32;
			Item.noUseGraphic = false;
			Item.holdStyle = ItemHoldStyleID.HoldGolfClub;
			Item.scale = .5f;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.buffType = ModContent.BuffType<SpiritMinionBuff>();
			Item.shoot = ModContent.ProjectileType<SpiritMinion>();
			Item.damage = 30;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position = Main.MouseWorld;
			player.LimitPointToPlayerReachableArea(ref position);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, false);
			player.AddBuff(Item.buffType, 2);
			return true;
		}

		public override bool CanShoot(Player player) => !player.AltUse();
	}
}
