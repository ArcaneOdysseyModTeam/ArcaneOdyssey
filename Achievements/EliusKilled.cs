using ArcaneOdyssey.NPCs.Bosses;
using Terraria.Achievements;

namespace ArcaneOdyssey.Achievements
{
	public class EliusKilled : ModAchievement
	{
		public override void SetStaticDefaults()
		{
			Achievement.SetCategory(AchievementCategory.Slayer);
			AddNPCKilledCondition(ModContent.NPCType<LordElius>());
		}

		public override Position GetDefaultPosition() => new After("EYE_ON_YOU");

		public override Position GetAdvisorPosition() => new After("SMASHING_POPPET");
	}
}
