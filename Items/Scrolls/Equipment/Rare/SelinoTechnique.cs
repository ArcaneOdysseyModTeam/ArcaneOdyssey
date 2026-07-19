using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Berserker;
using System.Collections.Generic;

namespace ArcaneOdyssey.Items.Scrolls.Equipment.Rare
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
				player.ArcaneOdyssey()?.SetDash(new Selino1(this));
			}
		}

		public override bool CanHaveFS => true;
	}

	public class Selino1(Scroll scroll) : ModDash(scroll.Item)
	{
		public override bool ContactDamage => false;
		public override int Cooldown => SelinoTechnique.Cooldown;

		public override bool LocksPlayer => true;

		public override bool OnHit(Player player, NPC target) => false;

		public override void OnEnd(Player player)
		{
			scroll.ActivateAbility(player);
			var dash = new Selino2(Source);
			player.ArcaneOdyssey().StartDash(dash, 0, Imbue, true);
			AOUtils.ShootProjectile(Source.GetSource_ItemUse(player), player.Center, player.SafeDirectionTo(Main.MouseWorld, Vector2.UnitX), ModContent.ProjectileType<ShockwaveSmash>(), Damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
		}

		public override float DashSpeed => 8;

		public override int DashMax => 15;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<SelinoCooldown>();
	}

	public class Selino2(Entity source) : ModDash(source)
	{
		public override bool ContactDamage => false;
		public override int Cooldown => SelinoTechnique.Cooldown;

		public override bool LocksPlayer => true;

		public override bool OnHit(Player player, NPC target) => false;

		public override void OnEnd(Player player)
		{
			var dash = new Selino3(Source);
			player.ArcaneOdyssey().StartDash(dash, 0, Imbue, true);
			AOUtils.ShootProjectile(Source.GetSource_ItemUse(player), player.Center, player.SafeDirectionTo(Main.MouseWorld, Vector2.UnitX), ModContent.ProjectileType<ShockwaveSmash>(), Damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
		}

		public override float DashSpeed => 8;

		public override int DashMax => 15;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<SelinoCooldown>();
	}

	public class Selino3(Entity source) : ModDash(source)
	{
		public override bool ContactDamage => false;
		public override int Cooldown => SelinoTechnique.Cooldown;

		public override bool LocksPlayer => true;

		public override bool OnHit(Player player, NPC target) => false;

		public override void OnEnd(Player player)
		{
			player.velocity *= .25f;
			AOUtils.ShootProjectile(Source.GetSource_ItemUse(player), player.Center, player.SafeDirectionTo(Main.MouseWorld, Vector2.UnitX), ModContent.ProjectileType<Selino>(), Damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
		}

		public override float DashSpeed => 8;

		public override int DashMax => 15;

		public override float Knockback => base.Knockback * 4f;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<SelinoCooldown>();
	}

	public class SelinoCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<SelinoTechnique>();
	}
}
