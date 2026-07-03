using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Rare
{
	public class SurgeSpell : RareScroll
	{
		public override bool CanHaveMagic => true;

		public override ModSkill Skill => ModContent.GetInstance<SurgeSkill>();
	}

	public class SurgeSkill : AttackSkill
	{
		public override int Damage => 15;

		public override int Shoot => ModContent.ProjectileType<Surge>();

		public override float Knockback => 0f;

		public override int Scroll => ModContent.ItemType<SurgeSpell>();

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			imbue.CreateMagicCircle(player, Projectiles.MagicCircleMode.Barrage, false, spread: imbue.ApplySpeed(MathHelper.PiOver4 / 2f));
			ActivateAbility(player, imbue);
			return true;
		}

		public override bool Channel => true;

		public override int ManaCost => 15;

		public override float Speed => 7f;

		public override int Time => 5;
	}
}
