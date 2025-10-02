using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Old;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using ArcaneOdyssey.VFX.Gores;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		public override AORarities AORarity => AORarities.Common;
		public override AOWeaponTiers AOWeaponTier => AOWeaponTiers.Average;
		public override WeaponAbility Ability => new(Mod, "Devastate", "Use the weight of your weapon to slam downwards", Color.Orange);

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.height = 40;
			Item.height = 40;
			Item.useTurn = true;
			Item.axe = 90 / 5;
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override void AddRecipes()
		{
			CreateRecipe().AddIngredient<BronzeBar>(12).AddIngredient<OldSword>().AddTile(TileID.Anvils).Register();
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
					player.DashPlayer().StartDash(dash, 2);
				}
			}
			return null;
		}
	}

	public class Devastate : DashSystem
	{
		public override bool AnyDirection => true;
		public override int Damage => 50;
		public override int Cooldown => 600;
		public override float DashSpeed => 15;
		public override int DashMax => 99999;
		public override DamageClass DamageType => DamageClass.Melee;
		public override float Knockback => 5;
		public override bool Immune => true;
		public override bool OnHit(Player player, Entity target)
		{
			return false;
		}

		public override void DashEffect(Player player)
		{
			if (player.itemAnimation < 8 || player.itemTime < 8)
				player.itemAnimation = player.itemTime = 7;
		}

		public override void OnEnd(Player player)
		{
			player.ArcaneOdyssey().timeTillNextMove += 15;
			foreach (NPC npc in Main.ActiveNPCs)
			{
				if (npc.Center.Distance(player.MountedCenter) < 40f * 1.025f * 2f)
				{
					npc.SimpleStrikeNPC(Damage, (player.MountedCenter.X - npc.Center.X > 0).ToDirectionInt(), knockBack: Knockback, damageType: DamageType);
				}
			}
		}
	}
}
