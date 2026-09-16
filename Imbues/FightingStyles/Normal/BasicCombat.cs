using ArcaneOdyssey.Imbues.Base;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.GameContent;

namespace ArcaneOdyssey.Imbues.FightingStyles.Normal
{
	public sealed class BasicCombat : FightingStyle
	{
		public override float Aura => 1f;
		public override Color ImbueColour => Color.White;
		public override SoundStyle? ImbueSound => SoundID.Item39;


		public override float ImbueSize => 1.06f;
		public override float ScrollDamage => .925f;
		public override float ScrollSize => 1f;
		public override float ScrollSpeed => 1f;

		public override void SpawningEffects(Rectangle area, Vector2 direction)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, direction.X * 2f, direction.Y * 2f, Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
		}

		public override void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null)
		{
			Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, Scale: area.RelativeScale())];
			spawnedDust.noGravity = true;
		}

		public override void ExplosionEffects(Vector2 position, float intensity = 1f)
		{
			for (int n = 0; n < 3; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(position, 0, 0, DustID.BubbleBurst_White, (Main.rand.NextFloat() - 0.5f) * (15f * intensity), (Main.rand.NextFloat() - 0.5f) * (15f * intensity), Scale: 3f * intensity)];
				spawnedDust.noGravity = true;
			}
		}

		public override void KillEffects(Rectangle area, Entity source = null)
		{
			for (int n = 0; n < 10; n++)
			{
				Dust spawnedDust = Main.dust[Dust.NewDust(area.TopLeft(), area.Width, area.Height, DustID.BubbleBurst_White, 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), 8f * area.RelativeScale() * (Main.rand.NextFloat() - 0.5f), Scale: 3f * area.RelativeScale())];
				spawnedDust.noGravity = true;
			}
			SoundEngine.PlaySound(ImbueSound, area.Center());
		}

		public static void ReuseSkills(Recipe recipe, Item item, List<Item> consumedItems, Item destinationStack)
		{
			if (item.ModItem is Imbuable imbue)
			{
				foreach (var usedItem in consumedItems)
				{
					if (usedItem.ModItem is BasicCombat combat)
					{
						for (var i = 0; i < combat.Skills.Length; i++)
						{
							var skill = combat.Skills[i];
							if (skill?.Scroll != 0)
							{
								imbue.Skills[i] = skill;
							}
						}
						imbue.cachedSpells = combat.cachedSpells;
						imbue.selectedIndex = 80;
						imbue.CycleAttack();
						break;
					}
				}
			}
		}

		public override void Load()
		{
			base.Load();
			On_Item.GetShimmered += ShimmerSkills;
		}

		private static void ShimmerSkills(On_Item.orig_GetShimmered orig, Item self)
		{
			if (self.ModItem is Imbuable imbue)
			{
				var shimmerEquivalentType = self.GetShimmerEquivalentType();
				var decraftingRecipeIndex = ShimmerTransforms.GetDecraftingRecipeIndex(shimmerEquivalentType);
				if (ItemID.Sets.ShimmerTransformToItem[shimmerEquivalentType] > 0)
				{
					int itemCounter = 1;
					var enumerable = AddSkillsToShimmerOutput(imbue);
					foreach (Item item in enumerable)
					{
						if (item.type <= 0)
						{
							break;
						}
						itemCounter++;
						int tempItemID = Item.NewItem(self.GetItemSource_Misc(8), (int)self.position.X, (int)self.position.Y, self.width, self.height, item.type);
						Item tempItem = Main.item[tempItemID];
						tempItem.shimmerTime = 1f;
						tempItem.shimmered = true;
						tempItem.shimmerWet = true;
						tempItem.wet = true;
						tempItem.velocity *= 0.1f;
						tempItem.playerIndexTheItemIsReservedFor = Main.myPlayer;
						tempItem.velocity.X = 1f * itemCounter;
						tempItem.velocity.X *= 1f + itemCounter * 0.05f;
						if (itemCounter % 2 == 0)
						{
							tempItem.velocity.X *= -1f;
						}
						NetMessage.SendData(MessageID.SyncItemsWithShimmer, -1, -1, null, tempItemID, 1f);
					}
				}
				else if (decraftingRecipeIndex >= 0)
				{
					Recipe recipe = Main.recipe[decraftingRecipeIndex];
					IEnumerable<Item> enumerable = recipe.requiredItem;
					if (recipe.customShimmerResults != null)
					{
						enumerable = recipe.customShimmerResults;
					}
					int itemCounter = 0;
					foreach (Item item in enumerable)
					{
						if (item.type <= 0)
						{
							break;
						}
						itemCounter++;
					}
					enumerable = AddSkillsToShimmerOutput(imbue);
					foreach (Item item in enumerable)
					{
						if (item.type <= 0)
						{
							break;
						}
						itemCounter++;
						int tempItemID = Item.NewItem(self.GetItemSource_Misc(8), (int)self.position.X, (int)self.position.Y, self.width, self.height, item.type);
						Item tempItem = Main.item[tempItemID];
						tempItem.shimmerTime = 1f;
						tempItem.shimmered = true;
						tempItem.shimmerWet = true;
						tempItem.wet = true;
						tempItem.velocity *= 0.1f;
						tempItem.playerIndexTheItemIsReservedFor = Main.myPlayer;
						tempItem.velocity.X = 1f * itemCounter;
						tempItem.velocity.X *= 1f + itemCounter * 0.05f;
						if (itemCounter % 2 == 0)
						{
							tempItem.velocity.X *= -1f;
						}
						NetMessage.SendData(MessageID.SyncItemsWithShimmer, -1, -1, null, tempItemID, 1f);
					}
				}
			}
			orig(self);
		}

		private static IEnumerable<Item> AddSkillsToShimmerOutput(Imbuable imbue)
		{
			foreach (var skill in imbue.Skills)
			{
				if (skill is not null)
				{
					if (skill.Scroll != 0)
					{
						yield return new Item(skill.Scroll);
					}
				}
			}
		}
		

		public override void Unload()
		{
			On_Item.GetShimmered -= ShimmerSkills;
		}
	}
}
