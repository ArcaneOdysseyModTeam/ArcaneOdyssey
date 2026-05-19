using ArcaneOdyssey.NPCs.Minibosses;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Achievements
{
	public class PettyTheft : ModAchievement
	{
		public override void SetStaticDefaults()
		{
			Achievement.SetCategory(AchievementCategory.Slayer);
			AddNPCKilledCondition(ModContent.NPCType<Laelus>());
		}

		public override Position GetDefaultPosition() => new After("HOLD_ON_TIGHT");

		public override Position GetAdvisorPosition() => new After("STAR_POWER");
	}
}
