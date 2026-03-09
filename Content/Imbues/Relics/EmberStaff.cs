using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Buffs.Stuns;
using ArcaneOdyssey.Content.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Projectiles.Relics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Imbues.Relics
{
	public class EmberStaff : SpiritEnergy
	{
		public override int AOValue => 700;
		public override bool? Cold => false;
		public override bool CanBeWet => false;
		public override SoundStyle? ImbueSound => SoundID.Item20;
		public override Color ImbueColour => new(252, 107, 3);
		public override float AOScrollDamage => .95f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollSpeed => 1f;
		public override Combo[] CombinedDebuffs => [Combo.Create<CharredEffect, Petrified>()];
		public override SynergyEffects Effects => AOUtils.CopyDamageSynergiesFromImbue<FireMagic>();

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Item.staff[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 56;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<Floganymai>();
			Item.damage = (120 * AOScrollDamage).Round();
			Item.shootSpeed = 1f;
			Item.noUseGraphic = false;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position = Main.MouseWorld;
			player.LimitPointToPlayerReachableArea(ref position);
		}
	}
}
