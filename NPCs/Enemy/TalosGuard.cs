using ArcaneOdyssey.Items.Armour.Centurion;
using ArcaneOdyssey.Items.Pickups;
using ArcaneOdyssey.NPCs.Base;
using ArcaneOdyssey.NPCs.Bosses;
using Terraria.GameContent.ItemDropRules;

namespace ArcaneOdyssey.NPCs.Enemy
{
	public class TalosGuard : BaseNPC
	{
		public override string Texture => AOUtils.GetTexture<GeneralArgos>();
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			NPC.aiStyle = NPCAIStyleID.Fighter;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(AOUtils.Common<RavennaBoots>(5)); // change to AnyDropHelper
			LeadingConditionRule keycondition = new(new ArgosKeyDrop());
			keycondition.OnSuccess(AOUtils.Common<ArgosKey>(6));
			npcLoot.Add(keycondition);
		}
	}
}
