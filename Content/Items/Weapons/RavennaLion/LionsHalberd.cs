using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using ArcaneOdyssey.PlayerClasses;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Weapons.RavennaLion
{
	public class LionsHalberd : AORangedOrMeleeWeapon
	{
		public override float AOSpeed => .5f;
		public override float AOSize => 1.35f;
		public override float AODamage => 1.15f;
		public override int AOValue => 250;
		public override AORarities AORarity => AORarities.Rare;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
		public override WeaponType WeaponsType => WeaponType.Strength;
		public override WeaponAbility? Ability => new(this, Color.Gold);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 70;
			Item.height = 68;
			Item.axe = 105 / 5;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
		}

		public override bool AltFunctionUse(Player player) => Imbue is not null;

		public override bool? UseItem(Player player)
		{
			if (player.AltUse())
			{
				var dash = new SeismicSlash(Item);
				if (!dash.OnCooldown(player))
				{
					player.ArcaneOdyssey().StartDash(dash, 2, Imbue);
				}
			}
			return null;
		}
	}

	public class SeismicSlash(Entity source) : DashSystem(source)
	{

		public override bool AnyDirection => true;
		public override int Cooldown => 300;
		public override float DashSpeed => 20;
		public override int DashMax => 600;
		public override DamageClass DamageType => DamageClass.Melee;
		public override float Knockback => 5;
		public override bool Immune => true;

		public override bool OnHit(Player player, Entity target) => true;

		public SlotId? sound = null;

		public override void DashEffect(Player player)
		{
			if (player.itemAnimation < 8 || player.itemTime < 8)
				player.itemAnimation = player.itemTime = 7;

			if (player.ArcaneOdyssey().DashLeft < (DashMax - 30))
			{
				if (!Main.dedServ)
				{
					if (!sound.HasValue)
					{
						sound = SoundEngine.PlaySound(SoundID.DD2_BookStaffTwisterLoop with { Pitch = -.25f }, player.Center);
					}
					else
					{
						if (SoundEngine.TryGetActiveSound(sound.Value, out var activeSound))
						{
							activeSound.Position = player.Center;
						}
					}
				}
			}
		}

		public override bool ExtraCheck(Player player) => !player.wet;

		public override void OnEnd(Player player)
		{
			player.ArcaneOdyssey().timeTillNextMove += 15;
			if (!Main.dedServ)
				SoundEngine.PlaySound(SoundID.Item14 with { Pitch = -.25f }, player.MountedCenter + player.velocity);

			if (sound.HasValue)
			{
				if (SoundEngine.TryGetActiveSound(sound.Value, out var activeSound))
				{
					activeSound.Stop();
				}
			}
		}

		public override void NaturalEnd(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(new EntitySource_ItemUse(player, player.PlayerItem()), player.itemLocation, player.itemLocation.DirectionTo(Main.MouseWorld.Y < player.MountedCenter.Y ? Main.MouseWorld : player.MountedCenter + (new Vector2(16 * player.direction, -4) * 5)) * 12f * (player.Imbue()?.AOImbueSpeed ?? 1f), ModContent.ProjectileType<SeismicSlashRock>(), Damage, Knockback, player.whoAmI);
			}
		}

		public override int DisplayedCooldownID => ModContent.BuffType<SeismicSlashCooldown>();
	}

	public class SeismicSlashCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => AOUtils.GetTexture<LionsHalberd>();
	}
}
