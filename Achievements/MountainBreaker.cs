using ArcaneOdyssey.NPCs.Minibosses;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Achievements
{
	public class MountainBreaker : ModAchievement
	{
		public override void SetStaticDefaults()
		{
			Achievement.SetCategory(AchievementCategory.Slayer);
			AddNPCKilledCondition(ModContent.NPCType<Evander>());
		}

		public override Position GetDefaultPosition() => new After("STILL_HUNGRY");

		public override Position GetAdvisorPosition() => new After("HEAD_IN_THE_CLOUDS");
	}
}
