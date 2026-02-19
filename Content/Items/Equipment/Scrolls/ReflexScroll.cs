using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using ArcaneOdyssey.PlayerClasses;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	public class ReflexScroll : RareScroll
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
			base.UpdateAccessory(player, hideVisual);
			if (HasCorrectImbue)
				player.ArcaneOdyssey()?.SetDash(new Reflex(Item));
		}
	}

	public class Reflex(Entity source) : DashSystem(source)
	{
		private float invisbase;
		private bool ground;

		public override int Damage => 0;

		public override int Cooldown => 30;

		public override bool AnyDirection => false;

		public override void OnStart(Player player)
		{
			ground = player.ArcaneOdyssey().Grounded;
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

		public override bool OnHit(Player player, Entity target) => true;

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

		public override bool Immune => false;
	}
}
