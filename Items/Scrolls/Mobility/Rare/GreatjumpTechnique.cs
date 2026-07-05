using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Berserker;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Mobility.Rare
{
	public class GreatjumpTechnique : RareScroll
	{
		public override bool CanHaveFS => true;
		public const int Cooldown = 60 * 10;
		public override ModSkill Skill => ModContent.GetInstance<GreatjumpSkill>();
	}

	public class GreatjumpSkill : DashSkill
	{
		public override int Scroll => ModContent.ItemType<GreatjumpTechnique>();

		public override int Damage => 40;

		public override void Activate(Player player, Imbuable imbue)
		{
			var dash = new Greatjump(imbue);
			player.ArcaneOdyssey().OmniDash = dash;
			player.ArcaneOdyssey().OmniDashDir = -2;
		}
	}

	public class Greatjump(Imbuable scroll) : ModDash(scroll.Item)
	{
		public override DamageClass DamageType => AOUtils.TrueMelee();
		public override bool ContactDamage => false;
		public override float DashSpeed => 30;
		public override int DashMax => 60;
		public override bool LocksPlayer => false;
		public override bool Immune => false;
		public override int Cooldown => 60 * 3;
		public override bool OnHit(Player player, NPC target) => false;

		public override void OnStart(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				var proj = AOUtils.ShootProjectile(Source.GetSource_ItemUse(player), player.Center, Vector2.Zero, ModContent.ProjectileType<GreatjumpShockwave>(), Damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
				proj.Bottom = player.Bottom;
			}
		}

		public override int DisplayedCooldownID => ModContent.BuffType<GreatjumpCooldown>();
	}

	public class GreatjumpCooldown : DisplayedCooldown
	{
		public override string Texture => AOUtils.GetTexture<GreatjumpTechnique>();
	}
}
