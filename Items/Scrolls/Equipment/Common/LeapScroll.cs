using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles;
using Terraria.Audio;

namespace ArcaneOdyssey.Items.Scrolls.Equipment.Common
{
	public class LeapScroll : CommonScroll
	{
		public override bool CanHaveRelic => true;
		public override bool CanHaveMagic => true;
		public override bool CanHaveFS => true;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (HasCorrectImbue)
			{
				player.GetJumpState<LeapAirStep>().Enable();
			}
		}
	}

	public class LeapAirStep : ExtraJump
	{
		public override Position GetDefaultPosition()
		{
			return BeforeBottleJumps;
		}
		public override void ShowVisuals(Player player)
		{
			player.Imbue()?.LingeringEffects(player.Hitbox);
			player.Imbue()?.Imbue?.LingeringEffects(player.Hitbox);
		}

		public override float GetDurationMultiplier(Player player) => player.Imbue().ScrollSize * 2;

		public override void UpdateHorizontalSpeeds(Player player)
		{
			player.runAcceleration *= (player.Imbue().ScrollSpeed + 1) * 2;
			player.maxRunSpeed *= player.Imbue().ScrollSpeed + 1;
			player.jumpSpeedBoost *= player.Imbue().ScrollSpeed;
			base.UpdateHorizontalSpeeds(player);
		}

		public override bool CanStart(Player player)
		{
			return player.Imbue() is not null;
		}

		public override void OnStarted(Player player, ref bool playSound)
		{
			//player.ChangeDir((player.oldVelocity.SafeNormalize(Vector2.UnitX * player.direction).X > 0).ToDirectionInt());
			var item = new Item(ModContent.ItemType<LeapScroll>());
			item.ArcaneOdyssey().Imbue = player.Imbue();
			if (player.whoAmI == Main.myPlayer)
			{
				var proj = Imbuable.CreateMagicCircle(item, player, MagicCircleMode.Basic, true, position: player.Bottom, rotation: -MathHelper.PiOver2).Projectile;
				for (int i = 0; i < 15; i++)
				{
					player.Imbue()?.ExplosionEffects(proj.Center);
					player.Imbue()?.Imbue?.ExplosionEffects(proj.Center);
				}
			}
			SoundEngine.PlaySound(player.Imbue().ImbueSound, player.Center);
			playSound = !player.Imbue().ImbueSound.HasValue;
			Projectile.NewProjectile(player.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<LeapFix>(), 0, 0, player.whoAmI, player.direction);
			// vfx here
		}
	}
}
