using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Projectiles.Relics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Common
{
	public class ExplosionScroll : CommonScroll
	{
		public override bool CanHaveMagic => true;
		public override bool CanHaveRelic => true;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 50;
			Item.reuseDelay = 60;
			Item.InterruptChannelOnHurt = true;
			Item.channel = true;
			Item.DamageType = DamageClass.MagicSummonHybrid;
			Item.UseSound = SoundID.Item84;
			Item.mana = 100;
			Item.shoot = ModContent.ProjectileType<ExplosionSpell>();
			Item.useAnimation = Item.useTime = 40;
		}

		public override void UpdateInventory(Player player)
		{
			base.UpdateInventory(player);
			if (Imbue is SpiritEnergy)
			{
				Item.DamageType = DamageClass.Summon;
			}
			else if (Imbue is AOMagic)
			{
				Item.DamageType = DamageClass.Magic;
			}
			else
			{
				Item.DamageType = DamageClass.MagicSummonHybrid;
			}
		}

		public override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			if (Imbue is SpiritEnergy)
				mult *= 0;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			if (Imbue is SpiritEnergy)
			{
				type = ModContent.ProjectileType<SpiritExplosion>();
			}
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool CanUseItem(Player player) => base.CanUseItem(player) && player.ownedProjectileCounts[Item.shoot] < 1 && player.ArcaneOdyssey().myCircle == null;
		
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			AOMagic.CreateMagicCircle(Item, player, Imbue, damage);
			return true;
		}
	}
}
