using ArcaneOdyssey.AOPlayers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Weapons;
using ArcaneOdyssey.Buffs.MagicMarks;


namespace ArcaneOdyssey.Items.Weapons
{
	public class BronzeTriasta : AOWeapon
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
		public override Color Colour => Color.Gold;
		public override AORarities AORarity => AORarities.Rare;
		public override bool? Cold => false;
		public override Debuff? WeaponDebuff => Debuff.Create<CharredEffect>();
		public override SoundStyle UseSound => SoundID.Item15;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.DamageType = AOUtils.TrueMelee();
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
				player.ArcaneOdyssey().StartDash(new EtherealFlash(this), imbue: Imbue, imbueAffectsSpeed: true);
			}
		}
	}

	public class EtherealFlash(AOWeapon tri) : DashSystem(tri.Item)
	{
		public override bool Immune => true;
		public override float DashSpeed => 120;
		public override int DashMax => 3;
		public override bool LocksPlayer => true;
		public override int Cooldown => 60 * 3;
		public override bool OnHit(Player player, Entity target) => true;

		public override void OnEnd(Player player)
		{
			player.velocity *= .01f;
			tri.ActivateAbility(player, false);
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
		public override string ExtraIconTexture => AOUtils.GetTexture<BronzeTriasta>();
	}
}
