using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.MagicChoice;

/// <summary>
/// The <see cref="UIState"/> that will appear when the player uses the item <see cref="Acrimony"/>. Made so the player can choose what magic or instead, return to monke (fighting style); however, if you are reading this in 2693 when there are more than 6 spirit weapons, the player can choose to opt for the eagle patrimony to entail their spirit focused journey
/// </summary>
public partial class MagicChoiceUIState : UIState
{
	protected readonly string TexturePath = $"{ArcaneOdysseyMod.Instance.Name}/UI/MagicChoice/Textures/";

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
	public static Asset<Texture2D> MagicTypeToMagicTexture(MagicTypes type)
	{
		if (type is MagicTypes.None) return TextureAssets.MagicPixel;
		int? id = type switch
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
		if (id is not null) return Terraria.GameContent.TextureAssets.Item[(int)id];

		Main.NewText($"{nameof(MagicTypes)} {type} is not supported in {nameof(MagicTypeToMagicTexture)}", new Color(255, 0, 255));
		return null;
	}

	#region UI Panels declaration but not ready for cheeseburger production
	/// <summary>
	/// The, uh, main, panel where everything will go towards to
	/// </summary>
	public UIPanel main = new();

	// Spoky (2026 January 25): Wanted to use TexturePath but it is not static therefore no can do, and given close button won't change texture (atleast not for now)
	public UIImageButton CloseButton = new(ModContent.Request<Texture2D>($"{ArcaneOdysseyMod.Instance.Name}/UI/MagicChoice/Textures/CloseButton"));

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
	protected UIText TitleText = new("No magic selected");
	#endregion

	/// <summary>
	/// Used to easily modify the grid for <see cref="TheShop"/>
	/// </summary>
	public const int ProductsPerRow = 5, HowManyAreWeDoing = 4 * ProductsPerRow + 2, TotalRows = (HowManyAreWeDoing / ProductsPerRow) + (ProductsPerRow % HowManyAreWeDoing > 0 ? +1 : 0);
	public const int separation = 4;
	#region Initialize thingies to make ui panels ready for cheeseburger production
	public override void OnInitialize()
	{
		#region Main Panel
		main.SetPadding(0);
		main.BackgroundColor = new(73, 94, 171);
		main.HAlign = 0.5f; main.VAlign = 0.5f;
		main.Width.Set(400, 0f);
		main.Height.Set(((64 + separation) * TotalRows) + 32 + 4 + 4, 0f);
		Main.NewText($"(64 + {separation}) * {TotalRows} = {(64 + separation) * TotalRows} \n" +
			$"first: {HowManyAreWeDoing / ProductsPerRow}, second: {(ProductsPerRow % HowManyAreWeDoing > 0 ? +1 : 0)}");

		Append(main);
		#endregion

		#region Close Button
		CloseButton.Width.Set(32f, 0f);
		CloseButton.Height.Set(32f, 0f);
		CloseButton.Left.Set(4f, 0f);
		CloseButton.Top.Set(main.Height.Pixels - CloseButton.Height.Pixels - 4f, 0f);
		CloseButton.OnLeftClick += CloseButton_OnLeftClick;

		main.Append(CloseButton);
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
		main.Append(ProductSpotLight.BackGround);
		main.Append(ProductSpotLight.Icon);
		#endregion

		#region
		ProductTitle.Width.Set(ProductSpotLight.BackGround.Width.Pixels, 0f);
		ProductTitle.Height.Set(ProductSpotLight.BackGround.Height.Pixels, 0f);

		ProductTitle.Left.Set(TheShop[ProductsPerRow - 1].BackGround.Left.Pixels + TheShop[ProductsPerRow - 1].BackGround.Width.Pixels + separation * 2, 0f);
		ProductTitle.Top.Set(TheShop[ProductsPerRow - 1].BackGround.Top.Pixels, 0f);

		main.Append(ProductTitle);

		TitleText.Width.Set(ProductSpotLight.Icon.Width.Pixels, 0f);
		TitleText.Height.Set(TheShop[0].BackGround.Height.Pixels, 0f);

		TitleText.Left.Set(separation * 2, 0f);

		ProductTitle.Append(TitleText);
		#endregion
	}
	#endregion

}
