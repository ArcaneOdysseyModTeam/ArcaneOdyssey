using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Spells.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Common
{
	public class BarrageSpell : CommonScroll
	{
		public override bool MetConditions() => NPC.downedBoss2;
		public override bool CanHaveMagic => true;
	}

	public class BarrageSkill : AttackSkill
	{
		public override int Damage => 5;


		public override void Activate(Player player, Imbuable imbue)
		{
			Imbuable.CreateMagicCircle(this, imbue, player, Projectiles.MagicCircleMode.Barrage, false, ModContent.ProjectileType<BlastSpell>(), spread: imbue.ApplySpeed(MathHelper.PiOver4 / 2f));
		}

		public override int ManaCost => 5;

		public override bool Channel => true;

		public override int Scroll => ModContent.ItemType<BarrageSpell>();
	}
}
