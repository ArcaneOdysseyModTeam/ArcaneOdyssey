using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI.MagicChoice
{
	[Autoload(Side = ModSide.Client)]
	public class MagicChoiceUISystem : ModSystem
	{
		private UserInterface _MagicChoice;
		internal MagicChoiceUIState magicChoice;
		private GameTime _prevTime;

		#region Show/Hide
		public void ShowUI()
		{
			magicChoice = new();
			_MagicChoice = new();
			_MagicChoice?.SetState(magicChoice);
			magicChoice.Activate();
		}

		public void HideTheUI()
		{
			_MagicChoice?.SetState(null);
			magicChoice.Deactivate();
		}
		#endregion

		#region Load/Unload
		public override void Load()
		{
			// Spoky (2026 January 24): Main.gameMenu is probably unneccesary but I had some complications with TDate UI with it so I'd rather put the check before doing antyhing
			if (Main.dedServ || Main.gameMenu) return;

			magicChoice = new();
			magicChoice.Initialize();
		}
		#endregion

		public override void UpdateUI(GameTime gameTime)
		{
			_prevTime = gameTime;
			_MagicChoice?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

			if (index is -1) return;

			layers.Insert(index, new LegacyGameInterfaceLayer(
				"ArcaneOdysseyMod: MagicChoiceUIState",
				delegate
				{
					if (_prevTime is not null && _MagicChoice?.CurrentState is not null)
						_MagicChoice.Draw(Main.spriteBatch, _prevTime);
					return true;
				},
				InterfaceScaleType.UI)
				);
		}
	}
}
