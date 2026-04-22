using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Abilities;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Weapons.RavennaLion
{
	public class LionsHalberd : Weapon
	{
		public override float Speed => .5f;
		public override float Size => 1.35f;
		public override float Damage => 1.15f;
		public override int Value => 250;
		public override Rarities Rarity => Rarities.Rare;
		public override ItemTiers WeaponTier => ItemTiers.Good;
		public override WeaponType WeaponsType => WeaponType.Strength;
		public override Color Motif => Color.Gold;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<LanceofLoyalty>();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 70;
			Item.height = 68;
			Item.axe = 105 / 5;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.autoReuse = true;
		}

		public override bool AltFunctionUse(Player player) => Imbue is not null;

		public override bool? UseItem(Player player)
		{
			if (player.AltUse())
			{
				var dash = new SeismicSlash(this);
				if (!dash.OnCooldown(player))
				{
					player.ArcaneOdyssey().StartDash(dash, 2, Imbue);
				}
			}
			return null;
		}
	}

	public class SeismicSlash(Weapon hal) : ModDash(hal.Item)
	{
		public override bool FallThrough => false;
		public override bool LocksPlayer => true;
		public override int Cooldown => 300;
		public override float DashSpeed => 20;
		public override int DashMax => 600;
		public override DamageClass DamageType => DamageClass.Melee;
		public override bool Immune => true;

		public override bool OnHit(Player player, NPC target) => true;

		public SlotId? sound = null;

		public override bool ContactDamage => false;

		public override void DashEffect(Player player)
		{
			if (player.ItemAnimationEndingOrEnded)
				player.itemAnimation = player.itemTime = 2;

			if (player.ArcaneOdyssey().DashLeft < (DashMax - 2))
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
			if (player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(new EntitySource_ItemUse(player, player.PlayerItem()), player.Bottom with { X = player.Bottom.X + (30 * player.direction)}, player.SafeDirectionTo(Main.MouseWorld.Y < player.MountedCenter.Y ? Main.MouseWorld : player.MountedCenter + (new Vector2(16 * player.direction, -4) * 5)) * 12f * (player.Imbue()?.ImbueSpeed ?? 1f), ModContent.ProjectileType<SeismicSlashRock>(), Damage, Knockback, player.whoAmI);
			}
			hal.ActivateAbility(player, false);
		}

		public override int DisplayedCooldownID => ModContent.BuffType<SeismicSlashCooldown>();
	}

	public class SeismicSlashCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<LionsHalberd>();
	}
}
