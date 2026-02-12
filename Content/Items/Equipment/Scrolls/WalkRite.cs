using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.PlayerClasses;
using System;
using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	public class WalkRite : Scroll
	{
		public override ScrollTier Tier => ScrollTier.Rare;
		public override bool CanHaveRelic => true;
		public const int Cooldown = 60 * 5;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			if (HasCorrectImbue)
			{
				player.ArcaneOdyssey()?.SetDash(new Walk1(Item), 3 * Math.Sign(player.velocity.X));
			}
		}
	}

	public class Walk1(Entity source) : DashSystem(source)
	{
		public override int Damage => 0;
		public override int Cooldown => WalkRite.Cooldown;

		public override bool AnyDirection => true;

		public override bool OnHit(Player player, Entity target) => false;

		public override void OnEnd(Player player)
		{
			var dash = new Walk2(source);
			player.ArcaneOdyssey().StartDash(dash, 4 * player.direction, Imbue, true);
		}

		public override float DashSpeed => 15;

		public override int DashMax => 15;

		public override float Knockback => 2f;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<WalkCooldown>();
	}

	public class Walk2(Entity source) : DashSystem(source)
	{
		public override int Damage => 0;
		public override int Cooldown => WalkRite.Cooldown;

		public override bool AnyDirection => true;

		public override bool OnHit(Player player, Entity target) => false;

		public override void OnEnd(Player player)
		{
			var dash = new Walk3(source);
			player.ArcaneOdyssey().StartDash(dash, 3 * player.direction, Imbue, true);
		}

		public override float DashSpeed => 15;

		public override int DashMax => 15;

		public override float Knockback => 2f;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<WalkCooldown>();
	}

	public class Walk3(Entity source) : DashSystem(source)
	{
		public override int Damage => 0;
		public override int Cooldown => WalkRite.Cooldown;

		public override bool AnyDirection => true;

		public override bool OnHit(Player player, Entity target) => false;

		public override void OnEnd(Player player)
		{
			player.velocity *= .25f;
		}

		public override float DashSpeed => 15;

		public override int DashMax => 15;

		public override float Knockback => 2f;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<WalkCooldown>();
	}

	public class WalkCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => GetTexture<WalkRite>();
	}
}
