using ArcaneOdyssey.Content.NPCS;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public class MountainBreaker : ModAchievement
    {
        public override string TextureName => Mod.Name + "/Assets/" + Name;
        public override void SetStaticDefaults()
		{
			Achievement.SetCategory(AchievementCategory.Slayer);
            AddNPCKilledCondition(ModContent.NPCType<Evander>());
		}

		public override Position GetDefaultPosition() => new After("STILL_HUNGRY");
	}
}
