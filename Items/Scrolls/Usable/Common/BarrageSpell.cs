using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Spells.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Common
{
	public class BarrageSpell : CommonScroll
	{
		public override bool MetConditions() => NPC.downedBoss2;
		public override bool CanHaveMagic => true;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.damage = 5;
			Item.mana = 5;
			Item.DamageType = DamageClass.Magic;
			Item.shootSpeed = 7;
			Item.channel = true;
			Item.useTime = Item.useAnimation = 10;
			Item.shoot = ProjectileID.WoodenArrowFriendly; // does not actually shoot
		}
	}

	public class BarrageSkill : AttackSkill
	{
		public override int Damage => 5;


		public override void Activate(Player player, Imbuable imbue)
		{
			Imbuable.CreateMagicCircle(this, imbue, player, Projectiles.MagicCircleMode.Barrage, false, ModContent.ProjectileType<BlastSpell>(), spread: imbue.ApplySpeed(MathHelper.PiOver4 / 2f));
		}

		public override int ManaCost => base.ManaCost;
	}
}
