using ArcaneOdyssey.Items.Accessories;
using ArcaneOdyssey.Items.Armour.RavennaNoble;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Weapons.RavennaNoble;
using ArcaneOdyssey.NPCs.Bosses;
using Terraria.GameContent.ItemDropRules;

namespace ArcaneOdyssey.Items.BossBags
{
	public class EliusBossBag : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Uncommon;

		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 3;
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.expert = true;
			Item.consumable = true;
			Item.maxStack = Item.CommonMaxStack;
		}

		public override bool CanRightClick() => true;

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(
				new AnyDropHelper([
					ModContent.ItemType<EliusBoots>(),
					ModContent.ItemType<EliusChest>(),
					ModContent.ItemType<EliusHelm>(),
					ModContent.ItemType<NobleThunderspear>(),
					ModContent.ItemType<ScimitarofStorm>(),
					ModContent.ItemType<StormCaller>()
					], rolls: 2)
				);

			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ThunderingCape>()));
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<LordElius>()));
		}
	}
}
