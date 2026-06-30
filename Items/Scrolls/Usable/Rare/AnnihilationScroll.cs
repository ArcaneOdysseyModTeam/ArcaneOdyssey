using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Rare
{
	public class AnnihilationScroll : RareScroll
	{
		public override bool MetConditions() => NPC.downedMechBossAny;
		public override bool CanHaveMagic => true;
		public override ModSkill Skill => ModContent.GetInstance<AnnihilationSkill>();
	}

	public class AnnihilationSkill : AttackSkill
	{
		public override int Damage => 60;
		public override int ManaCost => 200;
		public override float Knockback => 0;
		public override int Scroll => ModContent.ItemType<AnnihilationScroll>();

		public override void Activate(Player player, Imbuable imbue)
		{
			player.ArcaneOdyssey()?.StartDash(new Annihilation(imbue), -2, imbue, false);
			imbue.CreateMagicCircle(this, player, MagicCircleMode.Basic, true, position: player.Bottom, rotation: -MathHelper.PiOver2);
		}

		public override bool PreActivate(Player player, Imbuable imbue) => player.ownedProjectileCounts[ModContent.ProjectileType<AnnihilationSpell>()] < 1;

	}

	public class Annihilation(Imbuable scroll) : ModDash(scroll.Item)
	{
		public override bool Immune => false;

		public override bool LocksPlayer => true;

		public override float DashSpeed => 23;

		public override int Cooldown => 0;

		public override int DashMax => 10;

		public override bool ContactDamage => false;

		public override bool OnHit(Player player, NPC target) => false;

		public override void OnEnd(Player player)
		{
			AOUtils.ShootProjectile(Source.GetSource_ItemUse(player), player.Center, player.SafeDirectionTo(Main.MouseWorld) * 10, ModContent.ProjectileType<AnnihilationSpell>(), Damage, Knockback, player.whoAmI, Imbue, SecondImbue, true);
		}
	}
}
