using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.MagicChoice;

public partial class MagicChoiceUIState : UIState
{
	/// <summary>
	/// A <see cref="Product"/> is a bunch of ui stuff to represent an option for the player to select, kind of like a product, hmm.
	/// </summary>
	protected class Product
	{
		protected readonly string TexturePath;
		public MagicTypes CurrentType;

		public UIImage BackGround;
		public UIImage Icon;

		public Product(MagicChoiceUIState mainUI, MagicTypes type)
		{
			TexturePath = $"{mainUI.TexturePath}Product/";
			CurrentType = type;


			BackGround = new(ModContent.Request<Texture2D>($"{TexturePath}Neutral"));

			Asset<Texture2D> texture = MagicTypeToMagicTexture(CurrentType);
			if (texture is null)
			{
				Main.NewText($"{nameof(MagicTypes)} {CurrentType} is not supported in {nameof(MagicTypeToMagicTexture)}", new Color(255, 0, 255));
				texture = ModContent.Request<Texture2D>($"{TexturePath}Neutral");
			}

			Icon = new(texture)
			{
				ScaleToFit = true
			};
		}
	}

	protected class DisplayProduct
	{
		public MagicChoiceUIState MainUI;
		protected readonly string TexturePath;
		public MagicTypes CurrentType { protected set; get; }

		public UIImage BackGround;
		public UIImage Icon;

		protected void SetSizes()
		{
			BackGround.Width.Set(264, 0f);
			BackGround.Height.Set(264, 0f);

			if (MainUI.TheShop is not null && MainUI.TheShop.Count > 0)
			{
				Product quoi = MainUI.TheShop[^1];

				BackGround.Left.Set(((separation + quoi.BackGround.Width.Pixels) * ProductsPerRow) + separation * 2, 0f);
				//Main.NewText($"sep: {separation}; width: {quoi.BackGround.Width.Pixels}; row: {ProductsPerRow}\n" +
				//	$"Hmm: {(separation + quoi.BackGround.Width.Pixels) * ProductsPerRow}");
				BackGround.Top.Set(quoi.BackGround.Top.Pixels - (separation + quoi.BackGround.Height.Pixels) * 3, 0f);
			}
			else Main.NewText($"Last Thingie is null");

			SetIconSizes();
		}
		protected void SetIconSizes()
		{
			
			Icon.Width.Set(264 - (separation * 4), 0f);
			Icon.Height.Set(264 - (separation * 4), 0f);

			if (MainUI.TheShop is not null && MainUI.TheShop.Count > 0)
			{
				Icon.Left.Set(BackGround.Left.Pixels + separation, 0f);
				Icon.Top.Set(BackGround.Top.Pixels + separation, 0f);
			}

			Icon.ScaleToFit = true;
			Icon.IgnoresMouseInteraction = true;
		}

		public DisplayProduct(MagicChoiceUIState mainUI, MagicTypes type)
		{
			TexturePath = $"{mainUI.TexturePath}Product/";
			MainUI = mainUI;


			BackGround = new(ModContent.Request<Texture2D>($"{TexturePath}ThickBoi"));

			Asset<Texture2D> texture = MagicTypeToMagicTexture(CurrentType);
			if (texture is null)
			{
				Main.NewText($"{nameof(MagicTypes)} {CurrentType} is not supported in {nameof(MagicTypeToMagicTexture)}", new Color(255, 0, 255));
				texture = ModContent.Request<Texture2D>($"{TexturePath}ThickBoi");
			}

			Icon = new(texture)
			{
				ScaleToFit = true
			};
			SetSizes();
			ChangeType(type);

			// Spoky (2026 January 28): Made it so it turns invisible if it is indeed nothing (so it doesn't show up for one frame)
		}


		public void ChangeType(MagicTypes type)
		{
			CurrentType = type;
			Icon.SetImage(MagicTypeToMagicTexture(type));
			if (CurrentType is MagicTypes.None) Icon.Color = Color.Transparent;
			else Icon.Color = Color.White;
			SetIconSizes();
		}
	}

}
