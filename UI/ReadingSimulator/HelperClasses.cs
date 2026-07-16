using ArcaneOdyssey.Guidebook;
using CalamityMod.Tiles.DraedonStructures.CagedLights;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace ArcaneOdyssey.UI.ReadingSimulator;

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

	public class ImageButtonButWithAFewExtraThingsForVerySpecificPurposesInTheGuideBookThatShouldNotBeUsedForAnythingElseJeezThisIsALongNameWonderHowMuchCanIPadTheLengthOfThisClass: UIPanel
	{
		public readonly ReadingSimulatorUI main;
		/// <summary>
		/// This is a number, incredible
		/// </summary>
		public int Number { get; protected set; }

		public GuidebookPage Page;
		protected UIText Label = new("") { IgnoresMouseInteraction = true };

		public ImageButtonButWithAFewExtraThingsForVerySpecificPurposesInTheGuideBookThatShouldNotBeUsedForAnythingElseJeezThisIsALongNameWonderHowMuchCanIPadTheLengthOfThisClass(
			ReadingSimulatorUI main, int number, GuidebookPage page, Vector2 size) : base()
		{
			this.main = main;
			main.PageButtons.Add(this);
			Number = number;

			Page = page;
			if (Page is null)
			{
				Main.NewText($"{nameof(Page)} is null for Page n°{Number}; Big sad", new Color(255, 0, 255));
				return;
			}

			Width.Set(size.X, 0f);
			Height.Set(size.Y, 0f);

			Append(Label);

			// Spoky 2026 Jul 15: It seems that maybe panels have a base padding, which breaks all of the operations below, thus I set them all to 0
			PaddingBottom = 0;
			PaddingLeft = 0;
			PaddingRight = 0;
			PaddingTop = 0;

			Left.Set(separation, 0f);
			Top.Set(separation + main.CloseButton.Height.Pixels + (separation * 2) + ((Height.Pixels + separation) * number), 0f);


			Label.Left.Set(separation * 3f, 0f);
			Label.Top.Set(separation * 2, 0f);

			Label.VAlign = 0;
			Label.HAlign = 0;

			//Label.IsWrapped = true;
			Label.DynamicallyScaleDownToWidth = true;

			Label.SetText(Page.DisplayName.Value);

			Label.Recalculate();
			Height.Set(Label.MinHeight.Pixels + (separation * 4), 0f);

			// Spoky [2026 Jul 15]: Had the left of Label be multiplied by 2 (to account for the padding from both sides), changed it to 3 to be a bit more strict
			float testLines = Label.MinWidth.Pixels / (Width.Pixels - (Label.Left.Pixels * 2));
			double expectedLines = Math.Ceiling(testLines);
			int linesReal = expectedLines < 1 ? 1 : (int)expectedLines;

			//Main.NewText($"Checking for {Page.DisplayName.Value}\n" +
			//	$"\tHmm left {Left.Pixels} top {Top.Pixels}; size {Width.Pixels}, {Height.Pixels} \n" +
			//	$"\tLabel left {Label.Left.Pixels} top {Label.Top.Pixels}; size {Label.Width.Pixels}, {Label.Height.Pixels} \n" +
			//	$"\tHeight? {Label.MinHeight.Pixels}, Width? {Label.MinWidth.Pixels}; LinesReal: {linesReal}; Expected: {expectedLines}; test {testLines}");

			Label.IsWrapped = true;
			Height.Set(size.Y * linesReal, 0f);

			Label.Width.Set(Width.Pixels - (separation * 4f), 0f);
			Label.Height.Set(Height.Pixels - (separation * 2), 0f);



			Recalculate();
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
