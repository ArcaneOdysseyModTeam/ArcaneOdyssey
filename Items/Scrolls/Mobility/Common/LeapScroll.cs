using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Skills.Base;
using Terraria.Audio;

namespace ArcaneOdyssey.Items.Scrolls.Mobility.Common
{
	public class LeapScroll : CommonScroll
	{
		public override bool CanHaveRelic => true;
		public override bool CanHaveMagic => true;
		public override bool CanHaveFS => true;

		public override ModSkill Skill => ModContent.GetInstance<LeapSkill>();
	}

	public class LeapSkill : ModSkill
	{
		public override SkillType SkillSlot => SkillType.Mobility;

		public override void Activate(Player player, Imbuable imbue)
		{
			player.GetJumpState<LeapAirStep>().Enable();
		}

		public override int Scroll => ModContent.ItemType<LeapScroll>();
	}

	public class LeapAirStep : ExtraJump
	{
		public override Position GetDefaultPosition() => BeforeBottleJumps;

		public override void ShowVisuals(Player player)
		{
			player.Imbue()?.LingeringEffects(player.Hitbox);
			player.Imbue()?.Imbue?.LingeringEffects(player.Hitbox);
		}

		public override float GetDurationMultiplier(Player player) => player.Imbue().ScrollSize * 2f;

		public override void UpdateHorizontalSpeeds(Player player)
		{
			base.UpdateHorizontalSpeeds(player);
			player.runAcceleration *= player.Imbue().ScrollSpeed;
			player.maxRunSpeed *= player.Imbue().ScrollSpeed;
			player.jumpSpeedBoost *= player.Imbue().ScrollSpeed * 2f;
		}

		public override bool CanStart(Player player) => player.Imbue() is not null;

		public override void OnStarted(Player player, ref bool playSound)
		{
			//player.ChangeDir((player.oldVelocity.SafeNormalize(Vector2.UnitX * player.direction).X > 0).ToDirectionInt());
			var item = new Item(ModContent.ItemType<LeapScroll>());
			item.ArcaneOdyssey().Imbue = player.Imbue();
			if (player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<LeapFix>(), 0, 0, player.whoAmI, player.direction);
				var proj = Imbuable.CreateMagicCircle(item, player, MagicCircleMode.Basic, true, position: player.Bottom, rotation: -MathHelper.PiOver2).Projectile;
				for (int i = 0; i < 15; i++)
				{
					player.Imbue()?.ExplosionEffects(proj.Center);
					player.Imbue()?.Imbue?.ExplosionEffects(proj.Center);
				}
			}
			else
			{
				for (int i = 0; i < 15; i++)
				{
					player.Imbue()?.ExplosionEffects(player.Bottom);
					player.Imbue()?.Imbue?.ExplosionEffects(player.Bottom);
				}
			}
			SoundEngine.PlaySound(player.Imbue().ImbueSound, player.Center);
			playSound = !player.Imbue().ImbueSound.HasValue;
		}
	}
}
