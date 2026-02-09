using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.MagicChangeOLD;

/// <summary>
/// The <see cref="UIState"/> that will appear when the player uses the item <see cref="Acrimony"/>. Made so the player can choose what magic or instead, return to monke (fighting style); however, if you are reading this in 2693 when there are more than 6 spirit weapons, the player can choose to opt for the eagle patrimony to entail their spirit focused journey
/// </summary>
public partial class MagicChoiceUIState : UIState
{
	protected readonly string TexturePath = $"{ArcaneOdysseyMod.Instance.Name}/UI/MagicChoice/Textures/",
		LocalizationPath = $"Mods.ArcaneOdyssey.UI.BaseMagicChange.";

	public enum MagicTypes
	{
		/// <summary>
		/// Note that this has a value of -1 so any for loops don't count this, since they usually start at 0
		/// </summary>
		None = -1,

		/// <summary>
		/// Fighting Style, the basic one
		/// </summary>
		ReturnToMonke,
		/// <summary>
		/// Gives spirit weapon, probably eagle patrimony
		/// </summary>
		MonkLife,

		Acid,
		Ash,

		Crystal,

		Earth,
		Explosion,

		Fire, 

		Glass,

		/// <summary>
		/// The best magic, don't @ me
		/// </summary>
		Ice,

		Light,
		Lighting,

		Magma,
		Metal,

		Plasma,
		Poison,

		Sand,
		Shadow,
		Snow,

		Water,
		Wind,
		Wood,
	}

	public static int? MagicTypeToID(MagicTypes type) => type switch
	{
		MagicTypes.ReturnToMonke => ModContent.ItemType<BasicCombat>(),
		MagicTypes.MonkLife => ModContent.ItemType<EaglePatrimony>(),

		MagicTypes.Acid => ModContent.ItemType<AcidMagic>(),
		MagicTypes.Ash => ModContent.ItemType<AshMagic>(),

		MagicTypes.Crystal => ModContent.ItemType<CrystalMagic>(),

		MagicTypes.Earth => ModContent.ItemType<EarthMagic>(),
		MagicTypes.Explosion => ModContent.ItemType<ExplosionMagic>(),

		MagicTypes.Fire => ModContent.ItemType<FireMagic>(),

		MagicTypes.Glass => ModContent.ItemType<GlassMagic>(),

		MagicTypes.Ice => ModContent.ItemType<IceMagic>(),

		MagicTypes.Light => ModContent.ItemType<LightMagic>(),
		MagicTypes.Lighting => ModContent.ItemType<LightningMagic>(),

		MagicTypes.Magma => ModContent.ItemType<MagmaMagic>(),
		MagicTypes.Metal => ModContent.ItemType<MetalMagic>(),

		MagicTypes.Plasma => ModContent.ItemType<PlasmaMagic>(),
		MagicTypes.Poison => ModContent.ItemType<PoisonMagic>(),

		MagicTypes.Sand => ModContent.ItemType<SandMagic>(),
		MagicTypes.Shadow => ModContent.ItemType<ShadowMagic>(),
		MagicTypes.Snow => ModContent.ItemType<SnowMagic>(),

		MagicTypes.Water => ModContent.ItemType<WaterMagic>(),
		MagicTypes.Wind => ModContent.ItemType<WindMagic>(),
		MagicTypes.Wood => ModContent.ItemType<WoodMagic>(),

		MagicTypes.None or _ => null,
	};
	public static Asset<Texture2D> MagicTypeToMagicTexture(MagicTypes type)
	{
		if (type is MagicTypes.None) return TextureAssets.MagicPixel;
		int? id = MagicTypeToID(type);
		if (id is not null) return TextureAssets.Item[(int)id];

		Main.NewText($"{nameof(MagicTypes)} {type} is not supported in {nameof(MagicTypeToMagicTexture)}", new Color(255, 0, 255));
		return null;
	}
	public static Item MagicTypeToItem(MagicTypes type)
	{
		int? id = MagicTypeToID(type);
		if (id is not null) return ContentSamples.ItemsByType[(int)id].Clone();

		Main.NewText($"{nameof(MagicTypes)} {type} is not supported in {nameof(MagicTypeToItem)}", new Color(255, 0, 255));
		return null;
	}

	#region UI Panels declaration but not ready for cheeseburger production
	/// <summary>
	/// The, uh, main, panel where everything will go towards to
	/// </summary>
	protected UIPanel main = new();

	// Spoky (2026 January 25): Wanted to use TexturePath but it is not static therefore no can do, and given close button won't change texture (atleast not for now)
	protected UIImageButton CloseButton = new(ButtonTextures.Neutral), ChooseButton = new(ButtonTextures.Neutral);
	protected UIText CloseText = new("Close", 1, true), ChooseText = new("Choose", 1, true);

	protected static class ButtonTextures 
	{
		public static readonly Asset<Texture2D> Neutral = ModContent.Request<Texture2D>($"{ArcaneOdysseyMod.Instance.Name}/UI/MagicChoice/Textures/Button/Neutral"),
			Good = ModContent.Request<Texture2D>($"{ArcaneOdysseyMod.Instance.Name}/UI/MagicChoice/Textures/Button/Good");
	}


	/// <summary>
	/// <para>Contains a bunch of <see cref="Product"/> in a grid, depending on how many elements this has and <see cref="ProductsPerRow"/></para>
	/// <inheritdoc cref="Product"/>
	/// </summary>
	protected List<Product> TheShop = [];

	/// <summary>
	/// Shows the icon of the <see cref="Product"/> selected
	/// </summary>
	protected DisplayProduct ProductSpotLight;

	/// <summary>
	/// The name of the selected <see cref="Product"/>
	/// </summary>
	protected UIImage ProductTitle = new(ModContent.Request<Texture2D>($"{ArcaneOdysseyMod.Instance.Name}/UI/MagicChoice/Textures/Product/Name"));

	/// <summary>
	/// Random <see cref="UIText"/>s that give stats for <see cref="ProductSpotLight"/>
	/// </summary>
	protected UIText SpotTitle = new("", 1, true), SpotStats = new("");

	/// <summary>
	/// The <see cref="UIText"/> that will state which imbuable is being swapped
	/// </summary>
	protected UIText HeFellOff = new("");
	public ModItem TheGuyThatFellOff;
	#endregion

	/// <summary>
	/// Used to easily modify the grid for <see cref="TheShop"/>
	/// </summary>
	protected const int ProductsPerRow = 11, HowManyAreWeDoing = 2 * ProductsPerRow, TotalRows = (HowManyAreWeDoing / ProductsPerRow) + (HowManyAreWeDoing % ProductsPerRow > 0 ? +1 : 0);
	public const int separation = 4;
	#region Initialize thingies to make ui panels ready for cheeseburger production
	public override void OnActivate()
	{
		HeFellOff.SetText(Language.GetTextValue($"{LocalizationPath}AnnouncingHeWhoFellOff", TheGuyThatFellOff.Item.Name));

		HeFellOff.HAlign = 0.5f;

		HeFellOff.Top.Set(-(separation * 10), 0f);

		main.Append(HeFellOff);
	}
	public override void OnInitialize()
	{
		#region Main Panel that contains the products
		main.SetPadding(0);
		main.BackgroundColor = new(73, 94, 171);

		main.Width.Set((64 + separation) * ProductsPerRow + separation, 0f);
		main.Height.Set(((64 + separation) * TotalRows) + separation, 0f);

		main.HAlign = 0.5f; main.VAlign = 0.2f;

		Append(main);

		//main.Left.Set(((64 + separation) * ProductsPerRow + separation) / -4, 0f);
		//Main.NewText($"(64 + {separation}) * {TotalRows} = {(64 + separation) * TotalRows} \n" +
		//	$"first: {HowManyAreWeDoing / ProductsPerRow}, second: {(HowManyAreWeDoing % ProductsPerRow > 0 ? +1 : 0)} ({HowManyAreWeDoing % ProductsPerRow})");
		#endregion

		#region Close Button
		CloseButton.Width.Set(256, 0f);
		CloseButton.Height.Set(64, 0f);

		CloseButton.VAlign = 0.8f;
		CloseButton.HAlign = 0.7f;

		//float offset = main.Width.Pixels / 2f;
		//CloseButton.Left.Set(offset - (CloseButton.Width.Pixels / 2), 0f);

		CloseButton.OnLeftClick += CloseButton_OnLeftClick;

		Append(CloseButton);

		CloseText.Width.Set(CloseButton.Width.Pixels - separation * 2, 0f);
		CloseText.Height.Set(CloseButton.Height.Pixels - separation * 2, 0f);

		CloseText.IgnoresMouseInteraction = true;

		CloseText.HAlign = 0.8f;

		// Spoky (2026 February 03): VAlign for close text doesn't seem to work, but this does? ? ?
		// Spoky (2026 February 03): Alright nevermind, can't be bothered to make it perfectly centered
		CloseText.Top.Set(0, 0.33f);

		CloseButton.Append(CloseText);
		#endregion

		#region Choose Button
		ChooseButton.Width.Set(256, 0f);
		ChooseButton.Height.Set(64, 0f);

		ChooseButton.VAlign = 0.8f;
		ChooseButton.HAlign = 0.7f;

		//ChooseButton.Left.Set(offset - (ChooseButton.Width.Pixels / 2), 0f);
		ChooseButton.Top.Set(-(ChooseButton.Height.Pixels + separation), 0f);

		ChooseButton.OnLeftClick += ChosenButton_OnLeftClick;

		Append(ChooseButton);

		ChooseText.Width.Set(ChooseButton.Width.Pixels - separation * 2, 0f);
		ChooseText.Height.Set(ChooseButton.Height.Pixels - separation * 2, 0f);

		ChooseText.IgnoresMouseInteraction = true;

		ChooseText.HAlign = 0.5f;

		// Spoky (2026 February 03): VAlign for close text doesn't seem to work, but this does? ? ?
		// Spoky (2026 February 03): Alright nevermind, can't be bothered to make it perfectly centered
		ChooseText.Top.Set(0, 0.33f);

		ChooseButton.Append(ChooseText);
		#endregion

		#region Getting stock for the shop
		int counting = 0, offsetY = 0;
		for (int i = 0; i < HowManyAreWeDoing; i++)
		{
			Product product = new(this, (MagicTypes)i);

			product.BackGround.Width.Set(64, 0f);
			product.BackGround.Height.Set(64, 0f);
			product.Icon.Width.Set(64 - (separation * 2), 0f);
			product.Icon.Height.Set(64 - (separation * 2), 0f);

			float left = (separation * (counting + 1)) + (counting * product.BackGround.Width.Pixels), top = (separation * (offsetY + 1)) + (offsetY * product.BackGround.Height.Pixels);

			product.BackGround.Left.Set(left, 0f);
			product.BackGround.Top.Set(top, 0f);
			product.Icon.Left.Set(left + separation, 0f);
			product.Icon.Top.Set(top + separation, 0f);

			product.BackGround.OnLeftClick += OptionSelected;
			product.Icon.IgnoresMouseInteraction = true;

			main.Append(product.BackGround);
			main.Append(product.Icon);

			counting++;
			if (counting >= ProductsPerRow)
			{
				offsetY++;
				counting = 0;
			}

			TheShop.Add(product);
		}
		#endregion

		#region Showing the coolest product
		ProductSpotLight = new(this, MagicTypes.None);
		//Append(ProductSpotLight.BackGround);
		Append(ProductSpotLight.Icon);
		#endregion

		#region Title and stats of the coolest product
		SpotTitle.HAlign = ProductSpotLight.Icon.HAlign;
		SpotTitle.VAlign = ProductSpotLight.Icon.VAlign;

		SpotTitle.Top.Set(-(ProductSpotLight.Icon.Height.Pixels), 0f);
		SpotTitle.Left.Set(-(SpotTitle.Width.Pixels) / 2, 0f);

		Append(SpotTitle);

		SpotStats.HAlign = ProductSpotLight.Icon.HAlign;
		SpotStats.VAlign = ProductSpotLight.Icon.VAlign;

		SpotStats.Top.Set((ProductSpotLight.Icon.Height.Pixels), 0f);
		SpotStats.Left.Set((SpotStats.Width.Pixels) / 2, 0f);

		Append(SpotStats);
		#endregion
	}
	#endregion

}
