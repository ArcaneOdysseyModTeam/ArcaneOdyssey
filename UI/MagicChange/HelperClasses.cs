using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.MagicChangeOLD;

public partial class MagicChoiceUIState : UIState
{
	/// <summary>
	/// A <see cref="Product"/> is a bunch of ui stuff to represent an option for the player to select, kind of like a product, hmm.
	/// </summary>
	protected class Product
	{
		protected MagicChoiceUIState MainUI;

		protected readonly string TexturePath;
		public MagicTypes CurrentType;

		public UIImage BackGround;
		public UIImage Icon;

		public Product(MagicChoiceUIState mainUI, MagicTypes type)
		{
			MainUI = mainUI;
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

		public void Update()
		{
			if (CurrentType == MainUI.ProductSpotLight.CurrentType)
			{
				BackGround.Color = Color.White;
				return;
			}

			Color color = new(80, 80, 80, 80);

			if (BackGround.IsMouseHovering)
			{
				color = new(160, 160, 160, 160);
				if (!HasPlayedSound)
				{
					SoundEngine.PlaySound(SoundID.MenuTick, Main.LocalPlayer.position);
					HasPlayedSound = true;
				}
			}
			else HasPlayedSound = false;

			BackGround.Color = color;
		}
		public bool HasPlayedSound = false;
	}

	protected class DisplayProduct
	{
		public MagicChoiceUIState MainUI;
		protected readonly string TexturePath;
		public MagicTypes CurrentType { protected set; get; }

		public UIImage Icon;

		protected void SetIconSizes()
		{
			
			Icon.Width.Set(128, 0f);
			Icon.Height.Set(128, 0f);

			Icon.IgnoresMouseInteraction = true;

			Icon.VAlign = 0.5f;
			Icon.HAlign = 0.3f;

			//float offset = MainUI.main.Width.Pixels / 2f;
			//Icon.Left.Set(-offset + separation, 0f);

			Icon.ScaleToFit = true;
			Icon.IgnoresMouseInteraction = true;
		}

		public DisplayProduct(MagicChoiceUIState mainUI, MagicTypes type)
		{
			TexturePath = $"{mainUI.TexturePath}Product/";
			MainUI = mainUI;

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
			SetIconSizes();
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
