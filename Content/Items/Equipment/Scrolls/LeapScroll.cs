using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Projectiles;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	public class LeapScroll : Scroll
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
			base.UpdateAccessory(player, hideVisual);
			if (HasCorrectImbue)
			{
				player.GetJumpState<LeapAirStep>().Enable();
			}
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.ShinyRedBalloon).Register();
			CreateRecipe().AddIngredient<EmptyScroll>().AddIngredient(ItemID.PinkGel, 5).Register();
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
			player.Imbue()?.LingeringEffects(player);
			player.Imbue()?.Imbue?.LingeringEffects(player);
		}

		public override float GetDurationMultiplier(Player player) => player.Imbue().AOScrollSize * 2;

		public override void UpdateHorizontalSpeeds(Player player)
		{
			player.runAcceleration *= (player.Imbue().AOScrollSpeed + 1) * 2;
			player.maxRunSpeed *= player.Imbue().AOScrollSpeed + 1;
			player.jumpSpeedBoost *= player.Imbue().AOScrollSpeed;
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
				var proj = AOMagic.CreateMagicCircle(item, player, player.Imbue());
				for (int i = 0; i < 5; i++)
				{
					player.Imbue()?.ExplosionEffects(proj);
					player.Imbue()?.Imbue?.ExplosionEffects(proj);
				}


				SoundEngine.PlaySound(player.Imbue().ImbueSound, proj.Center);
			}
			playSound = !player.Imbue().ImbueSound.HasValue;
			Projectile.NewProjectile(player.GetSource_FromThis(), player.position, Vector2.Zero, ModContent.ProjectileType<LeapFix>(), 0, 0, player.whoAmI, player.direction);
			// vfx here
		}
	}
}
