using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Abilities;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Weapons
{
	public class Vindicator : Weapon
	{
		public override ItemTiers WeaponTier => ItemTiers.Great;

		public override Color Motif => new(0, 30, 255);

		public override Rarities Rarity => Rarities.Rare;

		public override float Size => .9f;
		public override float Speed => 1.15f;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.DamageType = AOUtils.TrueMelee();
			Item.hammer = 90;
			Item.useStyle = ItemUseStyleID.Swing;
		}
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.greathammer[Type] = true;
		}

		public override int Value => 600;

		public override Debuff? WeaponDebuff => null;

		public override bool IsLoadingEnabled(Mod mod) => ArcaneOdysseyMod.DevMode;

		public override bool AltFunctionUse(Player player) => true;

		public override bool? UseItem(Player player)
		{
			if (player.AltUse())
			{
				var dash = new CrushingJudgement(this);
				if (!dash.OnCooldown(player))
				{
					player.ArcaneOdyssey().StartDash(dash, 0, Imbue, true);
				}
			}
			return null;
		}
	}

	public class CrushingJudgement(Vindicator vin) : ModDash(vin.Item)
	{
		public override bool Immune => false;

		public override bool ContactDamage => false;

		public override bool LocksPlayer => true;

		public override int Cooldown => 60 * 10;

		public override int DashMax => 600;

		public override bool ExtraCheck(Player player) => !player.ItemAnimationEndingOrEnded;

		public override float DashSpeed => 15;

		public override bool OnHit(Player player, NPC target) => false;

		public override int DisplayedCooldownID => ModContent.BuffType<CrushingJudgementCooldown>();

		public override void OnEnd(Player player)
		{
			player.ArcaneOdyssey().timeTillNextMove += 15;
			vin.ActivateAbility(player, false);
			if (!Main.dedServ)
			{
				if (player.whoAmI == Main.myPlayer)
				{
					var proj = AOUtils.ShootProjectile(source.GetSource_ItemUse(player), player.Center, Vector2.Zero, ModContent.ProjectileType<DevastateShockwave>(), Damage * 2, Knockback, player.whoAmI, Imbue, SecondImbue);
					proj.Bottom = player.Bottom;
				}
				SoundEngine.PlaySound(SoundID.Item14 with { Pitch = -.25f }, player.Bottom);
			}
		}

		public override bool IsLoadingEnabled(Mod mod) => ArcaneOdysseyMod.DevMode;
	}

	public class CrushingJudgementCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => AOUtils.GetTexture<Vindicator>();

		public override bool IsLoadingEnabled(Mod mod) => ArcaneOdysseyMod.DevMode;
	}
}
