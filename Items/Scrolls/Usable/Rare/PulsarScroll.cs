using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Magic;
using ArcaneOdyssey.Skills.Base;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Usable.Rare
{
	public class PulsarScroll : RareScroll
	{
		public override bool CanHaveMagic => true;

		public override ModSkill Skill => ModContent.GetInstance<PulsarSkill>();
	}

	public class PulsarSkill : AttackSkill
	{
		public override int Damage => 70;

		public override int Shoot => ModContent.ProjectileType<PulsarSpell>();

		public override int ManaCost => 50;

		public override int Scroll => ModContent.ItemType<PulsarScroll>();

		public override SoundStyle? ExtraSound => SoundID.Item84;

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			imbue.CreateMagicCircle(player, Projectiles.MagicCircleMode.Basic, true, Shoot, AltUsing);
			return false;
		}
	}
}
