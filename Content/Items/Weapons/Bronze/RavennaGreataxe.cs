using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Old;
using ArcaneOdyssey.Content.Projectiles.Abilities;
using ArcaneOdyssey.AOPlayers;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;


namespace ArcaneOdyssey.Content.Items.Weapons.Bronze
{
	public class RavennaGreataxe : AOWeapon
	{
		public override int AOValue => 100;
		public override float AOSize => 1.025f;
		public override float AOSpeed => .925f;
		public override float AODamage => 1.025f;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
		public override Color Colour => Color.Orange;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = 40;
			Item.height = 40;
			Item.useTurn = true;
			Item.DamageType = AOUtils.TrueMelee();
			Item.axe = 90 / 5;
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(10).AddIngredient<OldGreataxe>().AddTile(TileID.Anvils).Register();
		}

		public override bool AltFunctionUse(Player player)
		{
			return CanUseItem(player);
		}

		public override bool? UseItem(Player player)
		{
			if (player.AltUse())
			{
				var dash = new Devastate(this);
				if (!dash.OnCooldown(player))
				{
					player.ArcaneOdyssey().StartDash(dash, 2, Imbue);
				}
			}
			return null;
		}
	}

	public class Devastate(AOWeapon axe) : DashSystem(axe.Item)
	{
		public override bool FallThrough => false;
		public override bool LocksPlayer => true;
		public override int Cooldown => 300;
		public override float DashSpeed => 15;
		public override int DashMax => 600;
		public override bool Immune => true;
		public override bool ContactDamage => false;
		public override bool OnHit(Player player, Entity target) => false;

		public override int DisplayedCooldownID => ModContent.BuffType<DevastateCooldown>();

		public override void DashEffect(Player player)
		{
			if (player.ItemAnimationEndingOrEnded)
				player.itemAnimation = player.itemTime = 2;

			if (player.ArcaneOdyssey().DashLeft < (DashMax - 30))
			{
				if (!Main.dedServ)
				{
					if (!sound.HasValue || !SoundEngine.TryGetActiveSound(sound.Value, out var activeSound))
					{
						sound = SoundEngine.PlaySound(SoundID.DD2_BookStaffTwisterLoop with { Pitch = .25f }, player.Center);
					}
					else
					{
						activeSound.Position = player.Center;
					}
				}
			}
		}

		public SlotId? sound = null;

		public override bool ExtraCheck(Player player) => !player.wet;

		public override void OnEnd(Player player)
		{
			player.ArcaneOdyssey().timeTillNextMove += 15;
			axe.ActivateAbility(player, false);
			if (!Main.dedServ)
			{
				if (player.whoAmI == Main.myPlayer)
				{
					var proj = AOUtils.ShootProjectile(source.GetSource_ItemUse(player), player.Center, Vector2.Zero, ModContent.ProjectileType<DevastateShockwave>(), Damage * 2, Knockback, player.whoAmI, Imbue, SecondImbue);
					proj.Bottom = player.Bottom;
				}
				SoundEngine.PlaySound(SoundID.Item14 with { Pitch = -.25f }, player.Bottom);
			}
			if (sound.HasValue && SoundEngine.TryGetActiveSound(sound.Value, out var activeSound))
			{
				activeSound.Stop();	
			}
			// Vfx
		}
	}

	public class DevastateCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => AOUtils.GetTexture<RavennaGreataxe>();
	}
}
