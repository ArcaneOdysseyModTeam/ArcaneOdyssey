using ArcaneOdyssey.Content.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Projectiles.Relics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Content.Imbues.Relics
{
	public class NyxStaff : SpiritEnergy
	{
		public override int AOValue => 700;
		public override SoundStyle? ImbueSound => SoundID.Item8;
		public override Color ImbueColour => Color.Purple;
		public override float AOScrollDamage => .9f;
		public override float AOScrollSize => 1.1f;
		public override float AOScrollSpeed => 1.1f;

		public override SynergyEffects Effects => AOUtils.CopyDamageSynergiesFromImbue<ShadowMagic>();

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Item.staff[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.autoReuse = true;
			Item.width = Item.height = 46;
			Item.shoot = ModContent.ProjectileType<Nichtetheis>();
			Item.noUseGraphic = false;
			Item.damage = (30 * AOScrollDamage).Round();
			Item.shootSpeed = 7f * AOScrollSpeed;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, true);
			return true;
		}
	}
}
