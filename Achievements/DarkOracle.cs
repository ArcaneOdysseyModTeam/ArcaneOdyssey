using ArcaneOdyssey.NPCs.Minibosses;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Achievements
{
	public class DarkOracle : ModAchievement
	{
		public override void SetStaticDefaults()
		{
			Achievement.SetCategory(AchievementCategory.Slayer);
			AddNPCKilledCondition(ModContent.NPCType<Dusk>());
		}

		public override Position GetDefaultPosition() => new After("MASTERMIND");
	}
}
