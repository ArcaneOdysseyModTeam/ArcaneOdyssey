using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;

namespace ArcaneOdyssey.Items.Scrolls.Equipment.Common
{
	public class ReflexScroll : CommonScroll
	{
		public override bool CanHaveRelic => true;
		public override bool CanHaveFS => true;
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (HasCorrectImbue)
				player.ArcaneOdyssey()?.SetDash(new Reflex(Item));
		}
	}

	public class Reflex(Entity source) : ModDash(source)
	{
		private float invisbase;
		private bool ground;

		public override bool ContactDamage => false;

		public override int Cooldown => 30;

		public override bool LocksPlayer => false;

		public override void OnStart(Player player)
		{
			ground = player.ArcaneOdyssey().grounded;
			if (!ground && Imbue is not null)
			{
				player.ArcaneOdyssey().DashVelocity *= Imbue.DashSpeed;
				SoundEngine.PlaySound(Imbue.ImbueSound, player.MountedCenter);
				if (Imbue is VanishingStyle)
				{
					invisbase = player.opacityForAnimation;
				}
				if (Imbue is SailorStyle sailor)
					sailor.BarValue -= FightingStyleBarred.BarMax / 20f;
			}
		}

		public override bool OnHit(Player player, NPC target) => true;

		public override void DashEffect(Player player)
		{
			if (ground)
				player.statDefense *= 1.75f;
			else
			{
				if (Imbue?.DashResist.HasValue == true)
					player.statDefense *= Imbue.DashResist.Value;

				if (Imbue is VanishingStyle)
					player.opacityForAnimation = MathHelper.Lerp(invisbase, 0f, player.ArcaneOdyssey().DashLerp);
			}
		}

		public override void OnEnd(Player player)
		{
			if (!ground)
			{
				SoundEngine.PlaySound(Imbue?.ImbueSound, player.MountedCenter);
				player.opacityForAnimation = 1f;
			}
		}

		public override float DashSpeed => 15;

		public override int DashMax => 30;

		public override bool Immune => Imbue is not null && (Imbue.DashSpeed >= 1.4f);
	}
}
