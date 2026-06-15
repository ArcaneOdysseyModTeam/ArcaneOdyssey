using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Gores;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Items.Base;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Items.Scrolls.Equipment.Common
{
	public class CrashScroll : CommonScroll
	{
		public override bool CanHaveFS => true;
		public const int Cooldown = 60 * 5;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
			Item.damage = 50;
			Item.DamageType = AOUtils.TrueMeleeNoSpeed();
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
				player.ArcaneOdyssey()?.SetDash(new Crash(this));
			}
		}
	}

	public class Crash(Scroll scroll) : ModDash(scroll.Item)
	{
		public override DamageClass DamageType => AOUtils.TrueMeleeNoSpeed();
		public override int Cooldown => CrashScroll.Cooldown;

		public override bool LocksPlayer => true;

		public override bool OnHit(Player player, NPC target)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("OmniDash"), target.Center, Vector2.Zero, ModContent.GoreType<Impact>());
			gore.Centre(target.Center);
			return true;
		}

		public override void OnEnd(Player player)
		{
			player.velocity = Vector2.Zero;
			scroll.ActivateAbility(player);
			SoundEngine.PlaySound(SoundID.Item14 with { Pitch = -.25f }, player.MountedCenter + player.velocity);
		}

		public override float DashSpeed => 15;

		public override int DashMax => 15;

		public override bool Immune => true;

		public override void NaturalEnd(Player player)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("OmniDash"), player.velocity + player.MountedCenter, Vector2.Zero, ModContent.GoreType<Impact>());
			gore.Centre(player.MountedCenter + player.velocity);
			player.ArcaneOdyssey()?.StartDash(new Smash(Source) { Imbue = Imbue, SecondImbue = SecondImbue }, 2);
		}

		public override int DisplayedCooldownID => ModContent.BuffType<CrashCooldown>();
	}

	public class Smash(Entity source) : ModDash(source)
	{
		public override bool FallThrough => false;
		public override DamageClass DamageType => AOUtils.TrueMeleeNoSpeed();

		public override bool LocksPlayer => true;
		public override int Cooldown => 0;

		public override float DashSpeed => 10;

		public override int DashMax => 600;
		public override bool Immune => true;

		public override bool ExtraCheck(Player player) => !player.wet;
		public override bool OnHit(Player player, NPC target)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("OmniDash"), player.velocity + player.Center, Vector2.Zero, ModContent.GoreType<Impact>(), player.Imbue().ImbueSize);
			gore.Centre(target.Center);
			return false;
		}

		public override void OnEnd(Player player)
		{
			var gore = Gore.NewGorePerfect(player.GetSource_Misc("OmniDash"), player.velocity + player.MountedCenter, Vector2.Zero, ModContent.GoreType<Impact>(), player.Imbue().ImbueSize);
			gore.Centre(player.Bottom);

			AOUtils.SimulateAOE(Player.defaultHeight * 3, Damage, player.Bottom, Knockback, player, DamageType);
			player.ArcaneOdyssey().timeTillNextMove += 15;
			SoundEngine.PlaySound(SoundID.Item14 with { Pitch = -.25f }, player.MountedCenter + player.velocity);
			if (sound.HasValue)
			{
				if (SoundEngine.TryGetActiveSound(sound.Value, out var activeSound))
				{
					activeSound.Stop();
				}
			}
		}

		public SlotId? sound = null;

		public override void DashEffect(Player player)
		{
			if (player.ArcaneOdyssey().DashLeft < (DashMax - 30))
			{
				if (!Main.dedServ)
				{
					if (!sound.HasValue || !SoundEngine.TryGetActiveSound(sound.Value, out var activeSound))
					{
						sound = SoundEngine.PlaySound(SoundID.DD2_BookStaffTwisterLoop with { Pitch = .25f, IsLooped = true }, player.Center);
					}
					else
					{
						activeSound.Position = player.Center;
					}
				}
			}
		}
	}

	public class CrashCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<CrashScroll>();
	}
}
