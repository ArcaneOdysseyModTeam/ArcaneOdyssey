using ArcaneOdyssey.Guidebook;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace ArcaneOdyssey.UI.ReadingSimulator;

// Spoky (2026 Apr 08): If this isn't deleted after the UI is done, then I forgot to delete this
public partial class ReadingSimulatorUI : UIState
{
	/// <summary>
	/// <para>Straight up just a <see cref="UIImageButton"></see> with one change.
	/// Reason Drag Button needs a specific class for itself, is because when dragging the UI, the sound effect of mouseOver can sound multiple times</para>
	/// <para>It seems to have slightly fixed the issue, the extra sounds only occurring when dragging the UI too quickly</para>
	/// </summary>
	/// <param name="main"></param>
	/// <param name="texture"></param>
	public class DragButtonHelper(ReadingSimulatorUI main, Asset<Texture2D> texture) : UIElement()
	{
		private Asset<Texture2D> _texture = texture;
		private float _visibilityActive = 1f;
		private float _visibilityInactive = 0.4f;
		private Asset<Texture2D> _borderTexture;

		protected ReadingSimulatorUI main = main;

		public void SetHoverImage(Asset<Texture2D> texture)
		{
			_borderTexture = texture;
		}

		public void SetImage(Asset<Texture2D> texture)
		{
			_texture = texture;
			Width.Set(_texture.Width(), 0f);
			Height.Set(_texture.Height(), 0f);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			spriteBatch.Draw(_texture.Value, dimensions.Position(), Color.White * (base.IsMouseHovering ? _visibilityActive : _visibilityInactive));
			if (_borderTexture != null && base.IsMouseHovering)
				spriteBatch.Draw(_borderTexture.Value, dimensions.Position(), Color.White);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			if (!main.Dragging) SoundEngine.PlaySound(SoundID.MenuTick);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
		}

		public void SetVisibility(float whenActive, float whenInactive)
		{
			_visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
			_visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
		}
	}

	public class ImageButtonButWithAFewExtraThingsForVerySpecificPurposesInTheGuideBookThatShouldNotBeUsedForAnythingElseJeezThisIsALongNameWonderHowMuchCanIPadTheLengthOfThisClass : UIImageButton
	{
		public readonly ReadingSimulatorUI main;
		/// <summary>
		/// This is a number, incredible
		/// </summary>
		public int Number { get; protected set; }

		public GuidebookPage Page;
		public UIText Label = new("") { IgnoresMouseInteraction = true };

		public ImageButtonButWithAFewExtraThingsForVerySpecificPurposesInTheGuideBookThatShouldNotBeUsedForAnythingElseJeezThisIsALongNameWonderHowMuchCanIPadTheLengthOfThisClass(
			ReadingSimulatorUI main, int number, Asset<Texture2D> texture) : base(texture)
		{
			this.main = main;
			main.PageButtons.Add(this);
			Number = number;

			Width.Set(144f, 0f);
			Height.Set(32f, 0f);

			Left.Set(separation, 0f);
			Top.Set(separation + main.CloseButton.Height.Pixels + (separation * 2) + ((Height.Pixels + separation) * number), 0f);

			Label.Width.Set(Width.Pixels - (separation * 2), 0f);
			Label.Height.Set(Height.Pixels - (separation * 2), 0f);

			Label.Left.Set(separation, 0f);
			Label.Top.Set(separation + 2, 0f);

			Append(Label);
		}

		public void NewPage(GuidebookPage page)
		{
			Page = page;
			if (Page is null)
			{
				Main.NewText($"{nameof(Page)} is null for Page n°{Number}");
				return;
			}

			Label.SetText(Page.DisplayName);
		}

		public override void LeftClick(UIMouseEvent evt)
		{
			if (Page is null)
			{
				SoundEngine.PlaySound(SoundID.Tink);
				return;
			}
			if (main.ChosenPage == Number)
			{
				SoundEngine.PlaySound(SoundID.MenuClose);
				main.ChosenPage = -1;
				main.BigTextLmao.SetText($"");
			}
			else
			{
				SoundEngine.PlaySound(SoundID.MenuOpen);
				main.ChosenPage = Number;
				if (Page is not null) main.BigTextLmao.SetText(Page.Description);
				else
				{
					Main.NewText($"{nameof(Page)} is null for Option number {Number}, this is bad", new Color(255, 0, 255));
					main.CommitSudoku();
				}
			}

			//Main.NewText($"Yo we numbering {Number}");
			main.RebootPages();
		}
	}
}
