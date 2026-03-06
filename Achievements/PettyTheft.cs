using ArcaneOdyssey.Content.NPCS.Minibosses;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Achievements
{
	public class PettyTheft : ModAchievement
	{
		public override string TextureName => AOUtils.GetTexture<MountainBreaker>();

		public override void SetStaticDefaults()
		{
			Achievement.SetCategory(AchievementCategory.Slayer);
			AddNPCKilledCondition(ModContent.NPCType<Laelus>());
		}

		public override Position GetDefaultPosition() => new Before("EYE_ON_YOU");
	}
}
