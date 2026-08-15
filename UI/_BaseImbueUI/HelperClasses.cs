using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.UI.MutateThyMagic;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ArcaneOdyssey.UI._BaseImbueUI;

public abstract partial class BaseImbueUI : UIState
{

	/// <summary>
	/// Shrimple thingy that contains a simple <see cref="UIImage"/> for a background an Icon
	/// </summary>
	protected abstract class BaseProduct
	{
		protected BaseImbueUI MainUI;

		protected readonly string TexturePath;
		public MagicTypes CurrentType;

		public UIImage BackGround;
		public UIImage Icon;

		public BaseProduct(BaseImbueUI mainUI, Asset<Texture2D> texture)
		{
			MainUI = mainUI;
			TexturePath = $"{mainUI.TexturePath}Product/";

			BackGround = new(ModContent.Request<Texture2D>($"{TexturePath}Neutral"));
			Icon = new(texture) { ScaleToFit = true };
		}

		/// <summary>
		/// <b>Set Icon to something in this constructor</b>
		/// </summary>
		/// <param name="mainUI"></param>
		public BaseProduct(BaseImbueUI mainUI)
		{
			MainUI = mainUI;
			TexturePath = $"{mainUI.TexturePath}Product/";
			BackGround = new(ModContent.Request<Texture2D>($"{TexturePath}Neutral"));
		}

		protected int EdgeCounter = 0;
		public virtual void Update()
		{
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

	/// <summary>
	/// A <see cref="MagicProduct"/> is a bunch of ui stuff to represent an option for the player to select, kind of like a product, hmm. <b>Uses <see cref="MagicTypes"/></b>
	/// </summary>
	protected class MagicProduct : BaseProduct
	{
		public MagicProduct(BaseImbueUI mainUI, MagicTypes type) : base(mainUI)
		{
			CurrentType = type;

			Asset<Texture2D> texture = MagicTypeToMagicTexture(CurrentType);
			if (texture is null)
			{
				Main.NewText($"{nameof(MagicTypes)} {CurrentType} is not supported in {nameof(MagicTypeToMagicTexture)}", new Color(255, 0, 255));
				texture = ModContent.Request<Texture2D>($"{TexturePath}Neutral");
			}



			Icon = new(texture)
			{
				ScaleToFit = true,
			};
		}

		public override void Update()
		{
			MagicTypes magicType = MainUI is MutateThyMagicUI mui ? mui.WhoWeMutating : MainUI.ProductSpotLight.CurrentType;

			#region BackGround Logic
			BackGroundLogic(); void BackGroundLogic()
			{
				if (CurrentType == magicType)
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
			#endregion

			#region Edge Case
			EdgeCounter++;
			if (EdgeCounter >= 5)
			{
				EdgeCounter = 0;
				EdgeCase();
			}

			void EdgeCase()
			{
				if (CurrentType is not MagicTypes.HeHasAcceptedChristInHisHeart) return;
				Icon.Color = SpiritEnergy.Instance.SpiritColour;
			}
			#endregion
		}
	}

	protected class CustomProduct : BaseProduct
	{
		public ModItem Item { get; protected set; }
		public CustomProduct(BaseImbueUI mainUI, ModItem item) : base(mainUI)
		{
			Item = item;
			Icon = new(TextureAssets.Item[Item.Type]) { ScaleToFit = true };
		}

		public override void Update()
		{
			if (MainUI is MutateThyMagicUI mui)
			{
				if (mui.ProductSpotLight.Mutation is not null)
				{
					if (Item.Type == mui.ProductSpotLight.Mutation.Type)
					{
						BackGround.Color = Color.White;
						return;
					}
				}
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
	}

	protected class DisplayProduct
	{
		public BaseImbueUI MainUI;
		protected readonly string TexturePath;
		public MagicTypes CurrentType { protected set; get; }
		public ModItem Mutation { set; get; }

		public UIImage Icon;

		protected void SetIconSizes()
		{
			//Main.NewText($"Hmming, {Icon.Width.Pixels}, {Icon.Height.Pixels}, left {Icon.Left.Pixels}, {Icon.Top.Pixels}");

			#region Warning! Math!
			float maxLength = 128;
			float ratio = Icon.Width.Pixels / Icon.Height.Pixels;

			if (ratio >= 1f) // Fat and Short; Tragedy
			{
				float height = (int)(maxLength / ratio), topReal = (maxLength - height) / 2;
				//Main.NewText($"On est gros; height: {height}, expected: {topReal}");
				Icon.Width.Set(maxLength, 0f);
				Icon.Left.Set(0, 0f);

				Icon.Height.Set(height, 0f);
				Icon.Top.Set(topReal, 0f);
			}
			else // Paper Straw build
			{
				float width = (int)(maxLength * ratio), leftReal = (maxLength - width) / 2;
				//Main.NewText($"On est grand; width: {width}, expected: {leftReal}");

				Icon.Height.Set(maxLength, 0f);
				Icon.Top.Set(0, 0f);

				Icon.Width.Set((int)(maxLength * ratio), 0f);
				Icon.Left.Set(leftReal, 0f);
			}

			#endregion

			Icon.IgnoresMouseInteraction = true;

			Icon.VAlign = MainUI is MutateThyMagicUI ? 0.65f : 0.5f;
			Icon.HAlign = 0.3f;

			//float offset = MainUI.main.Width.Pixels / 2f;
			//Icon.Left.Set(-offset + separation, 0f);

			Icon.ScaleToFit = true;
			Icon.IgnoresMouseInteraction = true;
		}

		public DisplayProduct(BaseImbueUI mainUI, MagicTypes type)
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
		}


		public void ChangeType(MagicTypes type)
		{
			CurrentType = type;
			Icon.SetImage(MagicTypeToMagicTexture(type));
			if (CurrentType is MagicTypes.None) Icon.Color = Color.Transparent;
			else Icon.Color = Color.White;
			SetIconSizes();
			Mutation = null;
		}

		public void ChangeType(ModItem item)
		{
			//Main.NewText($"item? {item is null}");
			CurrentType = MagicTypes.None;
			if (item is not null)
			{
				Icon.Remove();
				Icon = new(TextureAssets.Item[item.Type]) { ScaleToFit = true };
				MainUI.Append(Icon);
			}
			else ChangeType(MagicTypes.None);
			SetIconSizes();
			Mutation = item;
		}

		protected int EdgeCounter = 5;
		public void Update()
		{
			#region Edge Case
			EdgeCounter++;
			if (EdgeCounter >= 5)
			{
				EdgeCounter = 0;
				EdgeCase();
			}

			void EdgeCase()
			{
				if (CurrentType is not MagicTypes.HeHasAcceptedChristInHisHeart) return;
				Icon.Color = SpiritEnergy.Instance.SpiritColour;
			}
			#endregion
		}
	}
}
