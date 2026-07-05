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

namespace ArcaneOdyssey.Items.Scrolls.Attacks.Rare
{
	public class MeteorScroll : RareScroll
	{
		public override bool MetConditions() => NPC.downedPlantBoss;

		public override bool CanHaveMagic => true;

		public override ModSkill Skill => ModContent.GetInstance<MeteorSkill>();
	}

	public class MeteorSkill : AttackSkill
	{
		public override int Damage => 300;

		public override int Time => 15;

		public override int Shoot => ModContent.ProjectileType<MeteorSpell>();

		public override int Scroll => ModContent.ItemType<MeteorScroll>();

		public override float Speed => 6f;

		public override SoundStyle? ExtraSound => SoundID.Item82;

		public override int ManaCost => 100;

		public override void AttackStats(Player player, Imbuable imbue, ref Vector2 position, ref Vector2 velocity, ref int damage, ref float knockback)
		{
			position = new Vector2(Main.MouseWorld.X, Main.screenPosition.Y);
			player.LimitPointToPlayerReachableArea(ref position);
			position.Y -= Main.maxScreenH * .15f;
			velocity = Vector2.UnitY * velocity.Length();
		}

		public override bool Attack(Player player, Imbuable imbue, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int damage, float knockback)
		{
			ActivateAbility(player, imbue);
			return true;
		}

		public override bool PreActivate(Player player, Imbuable imbue) => player.ownedProjectileCounts[Shoot] < 1;
	}
}
