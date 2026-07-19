using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Items.Base;
using System.Collections.Generic;
using Terraria.Audio;

namespace ArcaneOdyssey.Items.Scrolls.Equipment.Common
{
	public class ReflexScroll : CommonScroll
	{
		public override bool MetConditions() => (NPC.downedBoss1 && Main.expertMode) || NPC.downedBoss3;
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

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			var tool = tooltips.Find(e => e.Mod == "Terraria" && e.Name == "Tooltip1"); // second line of tooltip
			if (tool != null && HasCorrectImbue)
			{
				tool.OverrideColor = Imbue.Colour;

				if (Imbue.DashSpeed > 1f)
				{
					tool.Text = this.GetLocalizedValue("Special.Fast");
				}

				if (Imbue.DashResist.HasValue)
				{
					tool.Text = this.GetLocalizedValue("Special.Resist");
				}

				if (Imbue.ImmuneDash)
				{
					tool.Text = this.GetLocalizedValue("Special.Instant");
				}

				if (Imbue is VanishingStyle)
				{
					tool.Text = this.GetLocalizedValue("Special.Vanish");
				}

				if (Imbue is ThermoFist)
				{
					tool.Text = this.GetLocalizedValue("Special.Thermo");
				}

				if (Imbue is SailorStyle)
				{
					tool.Text = this.GetLocalizedValue("Special.Sailor");
				}
			}
		}
	}

	public class Reflex(Entity source) : ModDash(source)
	{
		private float invisbase;

		public override bool ContactDamage => false;

		public override int Cooldown => 30;

		public override bool LocksPlayer => false;

		public override void OnStart(Player player)
		{
			if (Imbue is not null)
			{
				player.ArcaneOdyssey().DashVelocity *= Imbue.DashSpeed;
				SoundEngine.PlaySound(Imbue.ImbueSound, player.MountedCenter);
				if (Imbue is VanishingStyle)
				{
					invisbase = player.opacityForAnimation;
				}
			}
		}

		public override bool OnHit(Player player, NPC target) => !Immune;

		public override void DashEffect(Player player)
		{
			if (Imbue?.DashResist.HasValue == true)
				player.statDefense *= Imbue.DashResist.Value;

			if (Imbue is VanishingStyle)
				player.opacityForAnimation = MathHelper.Lerp(invisbase, 0f, player.ArcaneOdyssey().DashLerp);
		}

		public override void OnEnd(Player player)
		{
			SoundEngine.PlaySound(Imbue?.ImbueSound, player.MountedCenter);
			player.opacityForAnimation = 1f;
		}

		public override float DashSpeed => 15;

		public override int DashMax => 30;

		public override bool Immune => Imbue is not null && Imbue.ImmuneDash;
	}
}
