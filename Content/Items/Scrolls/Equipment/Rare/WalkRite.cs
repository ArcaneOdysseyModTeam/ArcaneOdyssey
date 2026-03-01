using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.PlayerClasses;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Content.Items.Scrolls.Equipment.Rare
{
	public class WalkRite : RareScroll
	{
		public override bool CanHaveRelic => true;
		public const int Cooldown = 60 * 5;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (HasCorrectImbue)
			{
				player.ArcaneOdyssey()?.SetDash(new Walk1(Item), 3 * Math.Sign(player.velocity.X));
			}
		}
	}

	public class Walk1(Entity source) : DashSystem(source)
	{
		public override bool ContactDamage => false;
		public override int Cooldown => WalkRite.Cooldown;

		public override bool LocksPlayer => true;

		public override bool OnHit(Player player, Entity target) => false;

		public override void OnEnd(Player player)
		{
			var dash = new Walk2(source);
			player.ArcaneOdyssey().StartDash(dash, 4 * player.direction, Imbue, true);
		}

		public override void OnStart(Player player)
		{
			SoundEngine.PlaySound(Imbue?.ImbueSound, player.Center);
		}

		public override float DashSpeed => 15;

		public override int DashMax => 15;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<WalkCooldown>();
	}

	public class Walk2(Entity source) : DashSystem(source)
	{
		public override bool ContactDamage => false;
		public override int Cooldown => WalkRite.Cooldown;

		public override bool LocksPlayer => true;

		public override bool OnHit(Player player, Entity target) => false;

		public override void OnEnd(Player player)
		{
			var dash = new Walk3(source);
			player.ArcaneOdyssey().StartDash(dash, 3 * player.direction, Imbue, true);
		}

		public override void OnStart(Player player)
		{
			SoundEngine.PlaySound(Imbue?.ImbueSound, player.Center);
		}

		public override float DashSpeed => 15;

		public override int DashMax => 15;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<WalkCooldown>();
	}

	public class Walk3(Entity source) : DashSystem(source)
	{
		public override bool ContactDamage => false;
		public override int Cooldown => WalkRite.Cooldown;

		public override bool LocksPlayer => true;

		public override bool OnHit(Player player, Entity target) => false;

		public override void OnEnd(Player player)
		{
			player.velocity *= .25f;
		}

		public override void OnStart(Player player)
		{
			SoundEngine.PlaySound(Imbue?.ImbueSound, player.Center);
		}

		public override float DashSpeed => 15;

		public override int DashMax => 15;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<WalkCooldown>();
	}

	public class WalkCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => AOUtils.GetTexture<WalkRite>();
	}
}
