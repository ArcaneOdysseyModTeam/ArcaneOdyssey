using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.NPCS;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Consumables
{
	public class EvanderPoster : AOBaseItem
	{
		public override AORarities AORarity => AORarities.Uncommon;

		public override void SetStaticDefaults()
		{
            ItemID.Sets.SortingPriorityBossSpawns[Type] = ItemID.Sets.SortingPriorityBossSpawns[ItemID.QueenSlimeCrystal] - 1;
		}

        public override void SetDefaults()
        {
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = true;
        }

        public override bool? UseItem(Player player)
        {
            NPC.SpawnBoss(player.position.X.Round(), player.position.Y.Round() - Main.screenHeight, ModContent.NPCType<Evander>(), player.whoAmI);
            return true;
        }
	}
}
