using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Berserker;
using ArcaneOdyssey.PlayerClasses;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Equipment.Rare
{
	public class SelinoTechnique : RareScroll
	{
		public const int Cooldown = 60 * 10;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
			Item.damage = 50;
			Item.DamageType = AOUtils.TrueMeleeNoSpeed();
			Item.knockBack = 8f;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			tooltips.RemoveAll((TooltipLine line) => line.Name == "Speed");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (HasCorrectImbue)
			{
				player.ArcaneOdyssey()?.SetDash(new Selino1(Item));
			}
		}

		public override bool CanHaveFS => true;
	}

	public class Selino1(Item item) : DashSystem(item)
	{
		public override int Damage => 0;
		public override int Cooldown => SelinoTechnique.Cooldown;

		public override bool AnyDirection => true;

		public override bool OnHit(Player player, Entity target) => false;

		public override void OnEnd(Player player)
		{
			var dash = new Selino2(item);
			player.ArcaneOdyssey().StartDash(dash, 0, Imbue, true);
			AOUtils.ShootProjectile(item.GetSource_ItemUse(player), player.Center, player.SafeDirectionTo(Main.MouseWorld, Vector2.UnitX), ModContent.ProjectileType<ShockwaveSmash>(), item.damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
		}

		public override float DashSpeed => 8;

		public override int DashMax => 15;

		public override float Knockback => 2f;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<SelinoCooldown>();
	}

	public class Selino2(Item item) : DashSystem(item)
	{
		public override int Damage => 0;
		public override int Cooldown => SelinoTechnique.Cooldown;

		public override bool AnyDirection => true;

		public override bool OnHit(Player player, Entity target) => false;

		public override void OnEnd(Player player)
		{
			var dash = new Selino3((Item)source);
			player.ArcaneOdyssey().StartDash(dash, 0, Imbue, true);
			AOUtils.ShootProjectile(item.GetSource_ItemUse(player), player.Center, player.SafeDirectionTo(Main.MouseWorld, Vector2.UnitX), ModContent.ProjectileType<ShockwaveSmash>(), item.damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
		}

		public override float DashSpeed => 8;

		public override int DashMax => 15;

		public override float Knockback => 2f;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<SelinoCooldown>();
	}

	public class Selino3(Item item) : DashSystem(item)
	{
		public override int Damage => 0;
		public override int Cooldown => SelinoTechnique.Cooldown;

		public override bool AnyDirection => true;

		public override bool OnHit(Player player, Entity target) => false;

		public override void OnEnd(Player player)
		{
			player.velocity *= .25f;
			AOUtils.ShootProjectile(item.GetSource_ItemUse(player), player.Center, player.SafeDirectionTo(Main.MouseWorld, Vector2.UnitX), ModContent.ProjectileType<Selino>(), item.damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
		}

		public override float DashSpeed => 8;

		public override int DashMax => 15;

		public override float Knockback => 10f;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<SelinoCooldown>();
	}

	public class SelinoCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => AOUtils.GetTexture<SelinoTechnique>();
	}
}
