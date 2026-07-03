using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Skills.Base;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Equipment.Common
{
	public class AuraScroll : CommonScroll
	{
		//public override bool CanHaveRelic => true;
		public override bool CanHaveFS => true;
		public override bool CanHaveMagic => true;

		public override ModSkill Skill => ModContent.GetInstance<AuraSkill>();

		public override bool MetConditions() => NPC.downedBoss3;
	}

	public class AuraSkill : PassiveSkill
	{
		public override int Scroll => ModContent.ItemType<AuraScroll>();

		public AuraMode Mode = AuraMode.Resistance;

		public override int Length => 60 * 60;

		public override void Activate(Player player, Imbuable Imbue)
		{
			if (Main.GameUpdateCount % 2 == 0)
			{
				Imbue?.LingeringEffects(player.Hitbox.Scaled(2f), player.velocity, player);
			}

			if (Main.myPlayer == player.whoAmI && AOKeybinds.CycleAuraMode.JustPressed)
			{
				if (Mode == AuraMode.Resistance)
				{
					Mode = AuraMode.Power;
				}
				else if (Mode == AuraMode.Power)
				{
					Mode = AuraMode.Destruction;
				}
				else if (Mode == AuraMode.Destruction)
				{
					Mode = AuraMode.Resistance;
				}
				Main.NewText(Mod.CustomLocalization("RandomWords.ModeCycled", Mode), Imbue.Colour);
			}

			if (Mode == AuraMode.Resistance)
			{
				player.statLifeMax2 += Imbue.AuraHP(player);
			}

			if (Mode == AuraMode.Power)
			{
				player.GetDamage(DamageClass.Generic) += .15f;
				if (Imbue is FightingStyle && player.ArcaneOdyssey().acumen)
				{
					player.GetDamage(DamageClass.Generic) += .05f;
				}
			}

			if (Mode == AuraMode.Destruction)
			{
				player.ArcaneOdyssey().StatSize += 35;
				if (Imbue is FightingStyle && player.ArcaneOdyssey().acumen)
				{
					player.ArcaneOdyssey().StatSize += 15;
				}
			}
		}
	}

	public enum AuraMode
	{
		Resistance,
		Power,
		Destruction
	}
}
