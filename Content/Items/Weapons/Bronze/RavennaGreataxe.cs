using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Old;
using ArcaneOdyssey.VFX.Gores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Weapons.Bronze
{
	public class RavennaGreataxe : AORangedOrMeleeWeapon
	{
		public override int AOValue => 100;
		public override float AOSize => 1.025f;
		public override float AOSpeed => .925f;
		public override float AODamage => 1.025f;
		public override AORarities AORarity => AORarities.Uncommon;
		public override AOItemTiers AOWeaponTier => AOItemTiers.Average;
		public override WeaponAbility? Ability => new(Mod, "Devastate", "Use the weight of your weapon to slam downwards", Color.Orange);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = 40;
			Item.height = 40;
			Item.useTurn = true;
			Item.DamageType = TrueMelee();
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
				var dash = new Devastate();
				if (!dash.OnCooldown(player))
				{
					player.ArcaneOdyssey().StartDash(dash, 2, Imbue);
				}
			}
			return null;
		}
	}

	public class Devastate : DashSystem
	{
		public override bool? UseScrollImbueStats => false;
		public override bool AnyDirection => true;
		public override int Damage => 50;
		public override int Cooldown => 300;
		public override float DashSpeed => 15;
		public override int DashMax => 600;
		public override DamageClass DamageType => TrueMelee();
		public override float Knockback => 5;
		public override bool Immune => true;
		public override bool OnHit(Player player, Entity target) => false;

		public override int DisplayedCooldownID => ModContent.BuffType<DevastateCooldown>();

		public override void DashEffect(Player player)
		{
			if (player.itemAnimation < 8 || player.itemTime < 8)
				player.itemAnimation = player.itemTime = 7;
		}

		public override bool ExtraCheck(Player player) => !player.wet;

		public override void OnEnd(Player player)
		{
			player.ArcaneOdyssey().timeTillNextMove += 15;
			SimulateAOE(100, Damage, player.itemLocation, Knockback, player.PlayerItem(), DamageType);
			if (!Main.dedServ)
			{
				var gore1 = Gore.NewGorePerfect(player.GetSource_ItemUse(player.PlayerItem()), player.Top, Vector2.Zero, ModContent.GoreType<DevastateEffect>());
				gore1.Centre(player.Top);
			}
			// Vfx
		}
	}

	public class DevastateCooldown : DisplayedCooldown
	{
		public override string ExtraIconTexture => GetType().Namespace.Replace('.', '/') + '/' + nameof(RavennaGreataxe);
	}
}
