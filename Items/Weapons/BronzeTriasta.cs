using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Projectiles.Weapons;
using Terraria.Audio;


namespace ArcaneOdyssey.Items.Weapons
{
	public class BronzeTriasta : Weapon
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.Spears[Type] = true;
			ArcaneOdysseyMod.Sets.cold[Type] = false;
		}
		public override float Damage => 0.9f;
		public override float Size => 1.1f;
		public override float Speed => 1.1f;
		public override int Value => 350;
		public override ItemTiers WeaponTier => ItemTiers.Good;
		public override Color Motif => Color.Gold;
		public override ItemRarities Rarity => ItemRarities.Rare;
		public override Debuff? WeaponDebuff => Debuff.Create<CharredEffect>();
		public override SoundStyle UseSound => SoundID.Item15;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.noMelee = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.DamageType = AOUtils.TrueMelee();
			Item.shootSpeed = BaseSpearProjectile.SpearSpeed;
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

	public class EtherealFlash(Weapon tri) : ModDash(tri.Item)
	{
		public override bool Immune => true;
		public override float DashSpeed => 120;
		public override int DashMax => 3;
		public override bool LocksPlayer => true;
		public override int Cooldown => 60 * 3;
		public override bool OnHit(Player player, NPC target) => true;

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
		public override string Texture => AOUtils.GetTexture<BronzeTriasta>();
	}
}
