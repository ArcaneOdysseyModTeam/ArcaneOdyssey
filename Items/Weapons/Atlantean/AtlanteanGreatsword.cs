using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Buffs.DOT;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Abilities;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Items.Weapons.Atlantean
{
	public class AtlanteanGreatsword : Weapon
	{
		public override ItemTiers WeaponTier => ItemTiers.Great;

		public override Color Motif => Color.PaleVioletRed;

		public override ItemRarities Rarity => ItemRarities.Rare;

		public override Debuff? WeaponDebuff => Debuff.Create<HeavyBleed>();

		public override float Size => 1.15f;

		public override float Speed => .9f;

		public override int Value => 80;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.atlanteanItem[Type] = true;
			ArcaneOdysseyMod.Sets.greatsword[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = Item.height = 64;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<TempestSpawner>();
			Item.DamageType = DamageClass.Melee;
			Item.shootSpeed = 1f;
		}

		public override bool CanShoot(Player player) => player.AltUse() && player.ownedProjectileCounts[Item.shoot] < 1;

		public override bool AltFunctionUse(Player player) => !(player.ArcaneOdyssey().HeavySkillActive || player.ArcaneOdyssey().OnCooldown<TempestCooldown>());

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			ActivateAbility(player, false);
			player.ArcaneOdyssey().SetCooldown<TempestCooldown>();
			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			velocity *= 0;
			position = player.Bottom + new Vector2(0, Player.defaultHeight / 2f);
			knockback *= 0;
		}
	}

	public class TempestCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<AtlanteanGreatsword>();
		public override int CooldownLength => 60 * 5;
	}
}
