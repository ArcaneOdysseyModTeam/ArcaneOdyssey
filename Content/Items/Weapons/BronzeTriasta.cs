using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using ArcaneOdyssey.PlayerClasses;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons
{
	public class BronzeTriasta : AORangedOrMeleeWeapon
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.Spears[Type] = true;
		}
		public override float AODamage => 0.9f;
		public override float AOSize => 1.1f;
		public override float AOSpeed => 1.1f;
		public override int AOValue => 350;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Good;
		public override WeaponAbility? Ability => new(this, Color.Gold);
		public override AORarities AORarity => AORarities.Rare;
		public override bool? Cold => false;
		public override AODebuffRequirement? WeaponDebuff => new(ModContent.BuffType<CharredEffect>(), 10 * 60);
		public override SoundStyle UseSound => SoundID.Item15;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.DamageType = TrueMelee();
			Item.shootSpeed = BaseSpearProjectile.Speed;
			Item.noUseGraphic = true;
			Item.width = Item.height = 52;
			Item.shoot = ModContent.ProjectileType<BronzeTriastaProjectile>();
		}

		public override bool AltFunctionUse(Player player) => true;

		public override void UseAnimation(Player player)
		{
			if (player.AltUse() && !player.ArcaneOdyssey().OnCooldown<EtherealFlashCooldown>())
			{
				player.ArcaneOdyssey().StartDash(new EtherealFlash(Item), imbue: Imbue, imbueAffectsSpeed: true);
			}
		}
	}

	public class EtherealFlash(Entity source) : DashSystem(source)
	{
		public override bool Immune => true;
		public override float DashSpeed => 120;
		public override int DashMax => 3;
		public override bool AnyDirection => true;
		public override int Cooldown => 60 * 3;
		public override bool OnHit(Player player, Entity target) => true;

		public override void OnEnd(Player player)
		{
			player.velocity *= .01f;
		}
	
		public override void OnStart(Player player)
		{
			SoundEngine.PlaySound(SoundID.Item67);
		}

		public override void DashEffect(Player player)
		{
			for (int i = 0; i < 20; i++)
			{
				Dust.NewDust(player.MountedCenter, player.width, player.height, DustID.HeatRay, player.ArcaneOdyssey().DashVelocity.X / 10f, player.ArcaneOdyssey().DashVelocity.Y / 10f, Scale: 2);
			}
		}

		public override int DisplayedCooldownID => ModContent.BuffType<EtherealFlashCooldown>();
	}

	public class EtherealFlashCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => GetTexture<BronzeTriasta>();
	}
}
