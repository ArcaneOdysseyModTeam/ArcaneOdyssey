using ArcaneOdyssey.NPCs.Bosses;
using Terraria.Achievements;
using Terraria.ModLoader;

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
	}
}
