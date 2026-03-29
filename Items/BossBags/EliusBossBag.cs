using ArcaneOdyssey.Items.Accessories;
using ArcaneOdyssey.Items.Armour.RavennaNoble;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Weapons.RavennaNoble;
using ArcaneOdyssey.NPCs.Bosses;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.BossBags
{
	public class EliusBossBag : BaseItem
	{
		public override Rarities Rarity => Rarities.Uncommon;

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

		public override string Texture => AOUtils.GelTexture;

		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(
				AnyDropHelper.Create(
					ModContent.ItemType<EliusBoots>(),
					ModContent.ItemType<EliusChest>(),
					ModContent.ItemType<EliusHelm>(),
					ModContent.ItemType<NobleThunderspear>(),
					ModContent.ItemType<ScimitarofStorm>(),
					ModContent.ItemType<StormCaller>()
					)
				);

			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ThunderingCape>()));
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<LordElius>()));
		}
	}
}
