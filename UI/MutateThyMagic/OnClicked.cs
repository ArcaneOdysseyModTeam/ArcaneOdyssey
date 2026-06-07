using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Consumable;
using ArcaneOdyssey.UI._BaseImbueUI;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.MutateThyMagic;

public partial class MutateThyMagicUI : BaseImbueUI
{
	protected override void ChosenButton_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (WhoWeMutating is not MagicTypes.None)
		{
			Player player = Main.LocalPlayer;
			int normieIndex = player.FindItem((int)MagicTypeToID(WhoWeMutating)), hecateIndex = player.FindItem(ModContent.ItemType<HecateShard>());

			if (normieIndex >= 0 && hecateIndex >= 0 && ProductSpotLight.Mutation is not null)
			{
				player.inventory[normieIndex].SetDefaults(ProductSpotLight.Mutation.Type);
				player.inventory[hecateIndex].TurnToAir();

				var newItem = player.inventory[normieIndex];

				//if (player.GetItem(player.whoAmI, ContentSamples.ItemsByType[ProductSpotLight.Mutation.Type].Clone(), GetItemSettings.InventoryEntityToPlayerInventorySettings) is Item)
				//{
				//	if (newItem.netID != ItemID.None)
				//	{
				//		newItem = player.QuickSpawnItemDirect(player.GetSource_FromThis(), newItem, newItem.stack);
				//	}
				//}

				if (newItem.ModItem is MagicType magic)
				{
					var og = AOUtils.Safe<Imbuable>(ModContent.GetModItem((int)MagicTypeToID(WhoWeMutating)));
					magic.OriginalImbue = og;
				}
				SoundEngine.PlaySound(SoundID.Unlock);
				YoungMan_KillYourself();
			}
			else if (normieIndex < 0)
			{
				SoundEngine.PlaySound(SoundID.Tink);
				Main.NewText($"Have you managed to lose your [i:{(int)MagicTypeToID(WhoWeMutating)}]? What a fool.");
			}
			else if (hecateIndex < 0)
			{
				SoundEngine.PlaySound(SoundID.Tink);
				Main.NewText($"Were you really thinking I wouldn't check if you dropped your [i:{ModContent.ItemType<HecateShard>()}]!?");
			}
			else if (ProductSpotLight.Mutation is null)
			{
				SoundEngine.PlaySound(SoundID.Tink);
				Main.NewText($"Did you even have the decency of clicking a mutation to choose from?");
			}
		}
	}

	public MagicTypes WhoWeMutating { get; protected set; } = MagicTypes.None;
	protected override void OptionChosen(MagicProduct p)
	{
		SoundEngine.PlaySound(SoundID.MenuOpen, Main.LocalPlayer.position);
		Item item = MagicTypeToItem(p.CurrentType).Clone();
		if (item.ModItem is not MagicType magic)
		{
			Main.NewText($"Item {item.Name}([i:{item.type}]) is not a magic? ? ?");
			return;
		}
		WhoWeMutating = p.CurrentType;
		ProductSpotLight.ChangeType(null);
		SpotTitle.SetText($"");
		SpotStats.SetText($"");

		bool doingASilly = Main.rand.NextBool(100);

		#region Title Handling
		string suffix;
		if (doingASilly)
		{
			suffix = p.CurrentType switch
			{
				MagicTypes.Acid or MagicTypes.Sand or MagicTypes.Sand or MagicTypes.Shadow => Main.rand.Next(["AndJustGoingToTheToilet", "ANDORDERING54NUGGETS"]),

				MagicTypes.Ash or MagicTypes.Crystal or MagicTypes.Magma or MagicTypes.Glass => Main.rand.Next(["AndScanning500Coupons", "AndJustBuyingABigMac", "AndJustGoingToTheToilet"]),

				MagicTypes.Earth or MagicTypes.Explosion or MagicTypes.Fire or MagicTypes.Lightning or MagicTypes.Earth => Main.rand.Next(["AndOrderingSomethingActuallyInteresting", "ANDORDERING54NUGGETS"]),

				MagicTypes.Light or MagicTypes.Metal or MagicTypes.Plasma or MagicTypes.Poison => Main.rand.Next(["AndJustBuyingABigMac", "ANDORDERING54NUGGETS", "AndJustGoingToTheToilet"]),

				MagicTypes.Ice or MagicTypes.Snow or MagicTypes.Water or MagicTypes.Wind or MagicTypes.Wood => Main.rand.Next(["AndOrderingSodaWITHEXTRAICE"]),

				MagicTypes.None or MagicTypes.ReturnToMonke or MagicTypes.MonkLife or MagicTypes.HeHasAcceptedChristInHisHeart or _ => "AndWaitWaitWhat",
			};
		}
		else suffix = "AndEatingLikeANormalPerson";
		TitleText.SetText(Language.GetTextValue($"{LocalizationPath}Titles.{suffix}", item.Name));
		#endregion

		AuxTitle.SetText(Language.GetTextValue($"{LocalizationPath}Titles.HowLongIsThisGuyGoingToTakeToMakeASimpleOrderForChristsSake"));

		#region Clearing Previous Offers
		// Spoky (2026 March 07): Hope removing them fixes problems
		foreach (var i in TodaysOffers)
		{
			i.BackGround.Remove();
			i.Icon.Remove();
		}
		// Spoky (2026 March 07): Should use Clear(), but last time I used it deleted things outside the list itself; so call it PTSD
		TodaysOffers = [];
		#endregion

		List<int> mutations = ArcaneOdysseyMod.Sets.Mutations[magic.Type];

		int total = mutations.Count, totalRows = (total / ProductsPerRow) + (total % ProductsPerRow > 0 ? +1 : 0);
		AuxPanel.Height.Set(((64 + Separation) * totalRows) + Separation, 0f);

		Append(AuxTitle);
		Append(AuxPanel);

		#region Getting Todays Offers, better buy now! Or you'll miss out!
		int counting = 0, offsetY = 0;
		for (int i = 0; i < mutations.Count; i++)
		{
			int id = mutations[i];
			ModItem mutation = ContentSamples.ItemsByType[id].Clone().ModItem;
			CustomProduct product = new(this, mutation);

			product.BackGround.Width.Set(64, 0f);
			product.BackGround.Height.Set(64, 0f);

			float left = (Separation * (counting + 1)) + (counting * product.BackGround.Width.Pixels),
				top = (Separation * (offsetY + 1)) + (offsetY * product.BackGround.Height.Pixels);

			product.BackGround.Left.Set(left, 0f);
			product.BackGround.Top.Set(top, 0f);

			#region Mathing math to make the images not squished whilst being increased in size if needed
			float maxLength = (64 - (Separation * 2));
			float ratio = product.Icon.Width.Pixels / product.Icon.Height.Pixels;

			//Main.NewText($"Product: {mType}, ratio: {ratio}, width{product.Icon.Width.Pixels}, height: {product.Icon.Height.Pixels}");

			if (ratio >= 1f) // Fat and Short; Tragedy
			{
				float height = (int)(maxLength / ratio), topReal = (maxLength - height) / 2;
				//Main.NewText($"On est gros; height: {height}, expected: {topReal}");
				product.Icon.Width.Set(maxLength, 0f);
				product.Icon.Left.Set(left + Separation, 0f);

				product.Icon.Height.Set(height, 0f);
				product.Icon.Top.Set(top + topReal + Separation, 0f);
			}
			else // Paper Straw build
			{
				float width = (int)(maxLength * ratio), leftReal = (maxLength - width) / 2;
				//Main.NewText($"On est grand; width: {width}, expected: {leftReal}");

				product.Icon.Height.Set(maxLength, 0f);
				product.Icon.Top.Set(top + Separation, 0f);

				product.Icon.Width.Set((int)(maxLength * ratio), 0f);
				product.Icon.Left.Set(left + leftReal + Separation, 0f);
			}

			//Main.NewText($"\tNew, width{product.Icon.Width.Pixels}, height: {product.Icon.Height.Pixels}");
			//product.Icon.Left.Set(left + Separation, 0f);
			//product.Icon.Top.Set(top + Separation, 0f);
			#endregion

			product.BackGround.OnLeftClick += MutationSelected;
			product.Icon.IgnoresMouseInteraction = true;

			AuxPanel.Append(product.BackGround);
			AuxPanel.Append(product.Icon);

			counting++;
			if (counting >= ProductsPerRow)
			{
				offsetY++;
				counting = 0;
			}

			TodaysOffers.Add(product);
		}
		#endregion

		//string text = "";
		//foreach (var s in mutations) text += $"[i:{s}], ";
		//Main.NewText($"Hmming {text}, {mutations.Count}, {magic.Name}");
	}

	protected void MutationChosen(CustomProduct product)
	{
		SoundEngine.PlaySound(SoundID.MenuOpen, Main.LocalPlayer.position);
		ProductSpotLight.ChangeType(product.Item);

		SpotTitle.SetText(product.Item.Item.Name);
		if (product.Item is MagicType magic)
		{
			// Spoky (2026 Feb 05): Doesn't work? Maybe it does?
			string prefix = magic.ImbueDebuffs.Length switch
			{
				> 1 => "Status Effects:",
				1 => "Status Effect:",
				_ => "",
			},
			text = "";
			if (magic.ImbueDebuffs.Length > 1)
			{
				for (int i = 0; i < magic.ImbueDebuffs.Length; i++)
				{
					string imbue = Lang.GetBuffName(magic.ImbueDebuffs[i].debuffID);
					text += i < magic.ImbueDebuffs.Length - 1 ? $"{imbue}, " : $"{imbue}";
				}
			}
			else if (magic.ImbueDebuffs.Length == 1) text = $"{Lang.GetBuffName(magic.ImbueDebuffs[0].debuffID)}";

			var abiliytext = magic.Property.HasValue ? magic.Property.Value.Name + "\n" : "";

			SpotStats.SetText(abiliytext +
				$"Size: {magic.ScrollSize} \n" +
				$"Speed: {magic.ScrollSpeed} \n" +
				$"Damage: {magic.ScrollDamage} \n" +
				$"{prefix} {text}");
		}
	}

	protected void MutationSelected(UIMouseEvent evt, UIElement listeningElement)
	{
		bool changed = false;
		foreach (CustomProduct p in TodaysOffers) if (p.BackGround.IsMouseHovering || p.Icon.IsMouseHovering)
		{
			MutationChosen(p);

			changed = true;
			break;
		}
		if (!changed && ProductSpotLight.Mutation is not null)
		{
			ProductSpotLight.ChangeType(null);
			SpotTitle.SetText("");
			SpotStats.SetText("");
		}
	}
}
