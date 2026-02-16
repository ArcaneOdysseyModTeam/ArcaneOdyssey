using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.PlayerClasses;
using ArcaneOdyssey.VFX.Gores;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	public class CrashScroll : Scroll
	{
		public override bool CanHaveFS => true;
		public const int Cooldown = 60 * 5;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
			Item.damage = 50;
			Item.DamageType = TrueMeleeNoSpeed();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			tooltips.RemoveAll((TooltipLine line) => line.Name == "Speed");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			if (HasCorrectImbue)
			{
				player.ArcaneOdyssey()?.SetDash(new Crash(Item));
			}
		}
	}

	public class Crash(Entity source) : DashSystem(source)
	{
		public override DamageClass DamageType => TrueMeleeNoSpeed();
		public override int Cooldown => CrashScroll.Cooldown;

		public override bool AnyDirection => true;

		public override bool OnHit(Player player, Entity target)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("OmniDash"), target.Center, Vector2.Zero, ModContent.GoreType<Impact>());
			gore.Centre(target.Center);
			return true;
		}

		public override void OnEnd(Player player)
		{
			player.velocity = Vector2.Zero;
			SoundEngine.PlaySound(SoundID.Item14 with { Pitch = -.25f }, player.MountedCenter + player.velocity);
		}

		public override float DashSpeed => 15;

		public override int DashMax => 15;

		public override float Knockback => 2f;

		public override bool Immune => true;

		public override void NaturalEnd(Player player)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("OmniDash"), player.velocity + player.MountedCenter, Vector2.Zero, ModContent.GoreType<Impact>());
			gore.Centre(player.MountedCenter + player.velocity);
			player.ArcaneOdyssey()?.StartDash(new Smash(source) { Imbue = Imbue, SecondImbue = SecondImbue }, 2);
		}

		public override void OnStart(Player player)
		{
			if (Imbue is not null)
			{
				if (Imbue is ThermoFist thermo)
				{
					thermo.BarValue += FightingStyleBarred.BarMax / 20f;
				}
				if (Imbue is SailorStyle sailor)
				{
					sailor.BarValue -= FightingStyleBarred.BarMax / 10f;
				}
			}
		}

		public override int DisplayedCooldownID => ModContent.BuffType<CrashCooldown>();
	}

	public class Smash(Entity source) : DashSystem(source)
	{
		public override DamageClass DamageType => TrueMeleeNoSpeed();

		public override bool AnyDirection => true;
		public override int Cooldown => 0;

		public override float DashSpeed => 10;

		public override int DashMax => 600;
		public override float Knockback => 0;
		public override bool Immune => true;

		public override bool ExtraCheck(Player player)
		{
			return !player.wet;
		}

		public override void OnStart(Player player)
		{
			if (player.TryGetImbue(out Imbuable imbue))
			{
				if (imbue is ThermoFist thermo)
				{
					thermo.BarValue += FightingStyleBarred.BarMax / 20f;
				}
				if (imbue is SailorStyle sailor)
				{
					sailor.BarValue -= FightingStyleBarred.BarMax / 10f;
				}
			}
		}
		public override bool OnHit(Player player, Entity target)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("OmniDash"), player.velocity + player.Center, Vector2.Zero, ModContent.GoreType<Impact>(), player.Imbue().AOImbueSize);
			gore.Centre(target.Center);
			return false;
		}

		public override void OnEnd(Player player)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("OmniDash"), player.velocity + player.MountedCenter, Vector2.Zero, ModContent.GoreType<Impact>(), player.Imbue().AOImbueSize);
			gore.Centre(player.Bottom);

			SimulateAOE(Player.defaultHeight * 3, Damage, player.Bottom, Knockback, player, DamageType);
			player.ArcaneOdyssey().timeTillNextMove += 15;
			SoundEngine.PlaySound(SoundID.Item14 with { Pitch = -.25f }, player.MountedCenter + player.velocity);
		}
	}

	public class CrashCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => GetTexture<CrashScroll>();
	}
}
