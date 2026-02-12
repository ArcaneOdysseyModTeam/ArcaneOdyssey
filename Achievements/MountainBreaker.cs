using ArcaneOdyssey.Content.NPCS;
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
	}
}
