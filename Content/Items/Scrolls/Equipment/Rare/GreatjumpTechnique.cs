using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Berserker;
using ArcaneOdyssey.AOPlayers;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Scrolls.Equipment.Rare
{
	public class GreatjumpTechnique : RareScroll
	{
		public override bool CanHaveFS => true;
		public const int Cooldown = 60 * 10;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
			Item.damage = 40;
			Item.DamageType = AOUtils.TrueMelee();
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
				var dash = new Greatjump(Item);
				player.ArcaneOdyssey().OmniDash = dash;
				player.ArcaneOdyssey().OmniDashDir = -2;
			}
		}
	}

	public class Greatjump(Scroll scroll) : DashSystem(scroll.Item)
	{
		public override bool ContactDamage => false;
		public override float DashSpeed => 30;
		public override int DashMax => 60;
		public override bool LocksPlayer => false;
		public override bool Immune => false;
		public override int Cooldown => 60 * 3;
		public override bool OnHit(Player player, Entity target) => false;

		public override void OnStart(Player player)
		{
			scroll.ActivateAbility(player, ModContent.ProjectileType<GreatjumpShockwave>());
			if (player.whoAmI == Main.myPlayer)
			{
				var proj = AOUtils.ShootProjectile(source.GetSource_ItemUse(player), player.Center, Vector2.Zero, ModContent.ProjectileType<GreatjumpShockwave>(), Damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
				proj.Bottom = player.Bottom;
			}
		}

		public override int DisplayedCooldownID => ModContent.BuffType<GreatjumpCooldown>();
	}

	public class GreatjumpCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => AOUtils.GetTexture<GreatjumpTechnique>();
	}
}
