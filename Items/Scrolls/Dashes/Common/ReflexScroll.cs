using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Skills.Base;
using Terraria.Audio;

namespace ArcaneOdyssey.Items.Scrolls.Dashes.Common
{
	public class ReflexScroll : CommonScroll
	{
		public override bool MetConditions() => (NPC.downedBoss1 && Main.expertMode) || NPC.downedBoss3;
		public override bool CanHaveRelic => true;
		public override bool CanHaveFS => true;
		public override bool CanHaveMagic => true;

		public override ModSkill Skill => ModContent.GetInstance<ReflexSkill>();
	}

	public class ReflexSkill : DashSkill
	{
		public override void Activate(Player player, Imbuable imbue)
		{
			player.ArcaneOdyssey()?.SetDash(new Reflex(imbue.Item));
		}

		public override int Scroll => ModContent.ItemType<ReflexScroll>();
	}

	public class Reflex(Entity source) : ModDash(source)
	{
		private float invisbase;

		public override bool ContactDamage => false;

		public override int Cooldown => 30;

		public override bool LocksPlayer => false;

		public override void OnStart(Player player)
		{
			if (Imbue is not null)
			{
				player.ArcaneOdyssey().DashVelocity *= Imbue.DashSpeed;
				SoundEngine.PlaySound(Imbue.ImbueSound, player.MountedCenter);
				if (Imbue is VanishingStyle)
				{
					invisbase = player.opacityForAnimation;
				}
			}
		}

		public override bool OnHit(Player player, NPC target) => !Immune;

		public override void DashEffect(Player player)
		{
			if (Imbue?.DashResist.HasValue == true)
				player.statDefense *= Imbue.DashResist.Value;

			if (Imbue is VanishingStyle)
				player.opacityForAnimation = MathHelper.Lerp(invisbase, 0f, player.ArcaneOdyssey().DashLerp);
		}

		public override void OnEnd(Player player)
		{
			SoundEngine.PlaySound(Imbue?.ImbueSound, player.MountedCenter);
			player.opacityForAnimation = 1f;
		}

		public override float DashSpeed => 15;

		public override int DashMax => 30;

		public override bool Immune => Imbue is not null && Imbue.ImmuneDash;
	}
}
