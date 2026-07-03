using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Projectiles.Relics;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Common
{
	public class ExplosionScroll : CommonScroll
	{
		public override bool CanHaveMagic => true;
		public override bool CanHaveRelic => true;

		public override ModSkill Skill => ModContent.GetInstance<ExplosionSkill>();
	}

	public class ExplosionSkill : AttackSkill
	{
		public override int Damage => 60;

		public override int Shoot => ModContent.ProjectileType<ExplosionSpell>();

		public override int Scroll => ModContent.ItemType<ExplosionScroll>();

		public override int ManaCost => 25;

		public override bool Channel => true;

		public override int Time => 40;

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			var type = Shoot;
			if (imbue is SpiritEnergy)
			{
				type = ModContent.ProjectileType<SpiritExplosion>();
			}
			imbue.CreateMagicCircle(player, MagicCircleMode.Rotating, false, type, AltUsing);
			return false;
		}

		public override bool PreActivate(Player player, Imbuable imbue)
		{
			var type = Shoot;
			if (imbue is SpiritEnergy)
			{
				type = ModContent.ProjectileType<SpiritExplosion>();
			}
			return player.ownedProjectileCounts[type] < 1 && player.ArcaneOdyssey().myCircle == null;
		}
	}
}
