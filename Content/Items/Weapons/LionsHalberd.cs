using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Weapons.Bronze;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class LionsHalberd : AORangedOrMeleeWeapon
	{
		public override float AOSpeed => .5f;
		public override float AOSize => 1.35f;
		public override float AODamage => 1.15f;
		public override int AOValue => 250;
		public override AORarities AORarity => AORarities.Rare;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
		public override bool? Arcanium => false;
		public override WeaponAbility? Ability => new(Mod, "Seismic Slash", "Slam into the ground, then upearth and launch a rock towards your cursor", Color.Gold);

		public override void SetStaticDefaults()
		{
			ItemID.Sets.UsesBetterMeleeItemLocation[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 70;
			Item.height = 68;
			Item.axe = 105 / 5;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return CanUseItem(player);
		}

		public override bool? UseItem(Player player)
		{
			if (player.AltUse())
			{
				var dash = new SeismicSlash();
				if (!dash.OnCooldown(player))
				{
					player.ArcaneOdyssey().StartDash(dash, 2);
				}
			}
			return null;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<RavennaGreataxe>().AddIngredient(ItemID.Anchor).AddTile(TileID.MythrilAnvil).Register(); // placeholder
		}
	}

	public class SeismicSlash : DashSystem
	{
		public override bool AnyDirection => true;
		public override int Damage => 50;
		public override int Cooldown => 300;
		public override float DashSpeed => 15;
		public override int DashMax => 99999;
		public override DamageClass DamageType => DamageClass.Melee;
		public override float Knockback => 5;
		public override bool Immune => true;

		public override bool OnHit(Player player, Entity target) => true;

		public override void DashEffect(Player player)
		{
			if (player.itemAnimation < 8 || player.itemTime < 8)
				player.itemAnimation = player.itemTime = 7;

			if (player.TryGetImbue(out var imbue))
			{
				imbue.LingeringEffects(player);
			}
		}

		public override void OnEnd(Player player)
		{
			player.ArcaneOdyssey().timeTillNextMove += 15;
			if (player.TryGetImbue(out var imbue))
			{
				for (int i = 0; i < 15; i++)
					imbue.ExplosionEffects(player);
			}
			if (player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(new EntitySource_ItemUse(player, player.PlayerItem()), player.itemLocation, player.itemLocation.DirectionTo(Main.MouseWorld.Y < player.MountedCenter.Y ? Main.MouseWorld : player.MountedCenter + (new Vector2(16 * player.direction, -4) * 5)) * 12f * (imbue?.AOImbueSpeed ?? 1f), ModContent.ProjectileType<SeismicSlashRock>(), Damage, Knockback, player.whoAmI);
			}
		}

		public override int DisplayedCooldownID => ModContent.BuffType<SeismicSlashCooldown>();
	}

	public class SeismicSlashCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => GetType().Namespace.Replace('.', '/') + '/' + nameof(LionsHalberd);
	}
}
