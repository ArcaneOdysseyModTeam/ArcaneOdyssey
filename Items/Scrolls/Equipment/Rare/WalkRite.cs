using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Skills.Base;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Items.Scrolls.Equipment.Rare
{
	public class WalkRite : RareScroll
	{
		public override bool CanHaveRelic => true;

		public override ModSkill Skill => ModContent.GetInstance<WalkSkill>();

		public const int Cooldown = 60 * 5;
	}

	public class WalkSkill : DashSkill
	{
		public override int Scroll => ModContent.ItemType<WalkRite>();

		public override void Activate(Player player, Imbuable imbue)
		{
			player.ArcaneOdyssey()?.SetDash(new Walk1(imbue), 3 * Math.Sign(player.velocity.X));
		}
	}

	public class Walk1(Imbuable imbue) : ModDash(imbue.Item)
	{
		public override bool ContactDamage => false;
		public override int Cooldown => WalkRite.Cooldown;

		public override bool LocksPlayer => true;

		public override bool OnHit(Player player, NPC target) => false;

		public override void OnEnd(Player player)
		{
			var dash = new Walk2(Source);
			player.ArcaneOdyssey().StartDash(dash, 4 * player.direction, Imbue, true);
		}

		public override void OnStart(Player player)
		{
			SoundEngine.PlaySound(Imbue?.ImbueSound, player.Center);
			imbue.Dash.ActivateAbility(player, imbue);
		}

		public override float DashSpeed => 15;

		public override int DashMax => 15;

		public override bool Immune => true;

		public override int DisplayedCooldownID => ModContent.BuffType<WalkCooldown>();
	}

	public class Walk2(Entity source) : ModDash(source)
	{
		public override bool ContactDamage => false;
		public override int Cooldown => WalkRite.Cooldown;

		public override bool LocksPlayer => true;

		public override bool OnHit(Player player, NPC target) => false;

		public override void OnEnd(Player player)
		{
			var dash = new Walk3(Source);
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

	public class Walk3(Entity source) : ModDash(source)
	{
		public override bool ContactDamage => false;
		public override int Cooldown => WalkRite.Cooldown;

		public override bool LocksPlayer => true;

		public override bool OnHit(Player player, NPC target) => false;

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
		public override string Texture => AOUtils.GetTexture<WalkRite>();
	}
}
