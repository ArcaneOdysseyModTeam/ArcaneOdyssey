using ArcaneOdyssey.Subworlds.Base;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ArcaneOdyssey.UI.SubWorld;

public partial class SubWorldUI : UIState
{ 
	protected class SubworldPicker : UIPanel
	{

		public override void LeftClick(UIMouseEvent evt)
		{
			Main.NewText($"Testing. . .");
			if (main is null)
			{
				Main.NewText($"{nameof(main)} is somehow null for ${Subworld.DisplayName}!?");
				return;
			}

			main.AtEaseWithTheHarmonyIAm(Subworld, Subworld.DisplayName, ModContent.Request<Texture2D>($"{Subworld.Biome.BackgroundPath}", AssetRequestMode.ImmediateLoad), Subworld.Description.Value);
	}

		public SubWorldUI main { get; protected set; }

		public AOSubworld Subworld { get; protected set; }

		protected UIText Label = new("") { IgnoresMouseInteraction = true },
			UnlockHint;

		public SubworldPicker(SubWorldUI main, AOSubworld subworld, Vector2 size, float top, int number) : base()
		{
			this.main = main;
			Subworld = subworld;

			Width.Set(size.X, 0f);
			Height.Set(size.Y, 0f);

			Append(Label);

			//OnLeftClick += SubworldPicker_OnLeftClick;

			// Spoky 2026 Jul 15: It seems that maybe panels have a base padding, which breaks all of the operations below, thus I set them all to 0
			PaddingBottom = 0;
			PaddingLeft = 0;
			PaddingRight = 0;
			PaddingTop = 0;

			Left.Set(0, 0f);
			Top.Set(separation + top + ((Height.Pixels + separation) * number), 0f);

			Label.IgnoresMouseInteraction = true;
			Label.Left.Set(separation * 3f, 0f);
			Label.Top.Set(separation * 2, 0f);

			Label.VAlign = 0;
			Label.HAlign = 0;

			//Label.IsWrapped = true;
			Label.DynamicallyScaleDownToWidth = true;

			Label.SetText(Subworld.MetConditions ? $"{Subworld.DisplayName}" : $"? ? ?");

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

			if (!Subworld.MetConditions)
			{
				UnlockHint = new("") { IgnoresMouseInteraction = true };
				UnlockHint.Width.Set(32, 0f);
				UnlockHint.Height.Set(32, 0f);

				Append(UnlockHint);
			}

			Recalculate();
		}

		public override void Update(GameTime gameTime)
		{
			if (!Subworld.MetConditions)
			{
				Main.NewText($"Didn't met conditions");
				if (!IsMouseHovering)
				{
					Main.NewText($"Not hovering");
					UnlockHint.SetText($"");
				}
				else
				{
					UnlockHint.SetText($"{Subworld.UnlockCondition}");
					UnlockHint.Left.Set(Main.mouseX - 32, 0);
					UnlockHint.Top.Set(Main.mouseY + 32, 0);
					Recalculate();
				}
			}
			else Main.NewText($"Met conditions");
		}
	}
}
