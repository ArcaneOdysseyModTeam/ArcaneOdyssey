using ArcaneOdyssey.UI.ImbueAcquiring;
using ArcaneOdyssey.UI.MagicChangeOLD;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using static ArcaneOdyssey.UI.MagicChangeOLD.MagicChoiceUIState;

namespace ArcaneOdyssey.UI;

[Autoload(Side = ModSide.Client)]
public class ImbueChangeUISystem : ModSystem
{
	private UserInterface _ImbueAcquire;
	internal ImbueAcquireUI imbueAcquireUI;

	private UserInterface _MagicChoice;
	internal MagicChoiceUIState magicChoice;
	private GameTime _prevTime;

	#region Show/Hide
	#region Show
	public void ShowAcquireUI()
	{
		imbueAcquireUI = new();
		_ImbueAcquire = new();
		_ImbueAcquire?.SetState(imbueAcquireUI);
		imbueAcquireUI.Activate();
	}
	public void ShowSwapUI(ModItem whom)
	{
		magicChoice = new()
		{
			TheGuyThatFellOff = whom,
		};
		_MagicChoice = new();
		_MagicChoice?.SetState(magicChoice);
		magicChoice.Activate();
	}
	#endregion

	#region Hide
	public void HideTheUI()
	{
		_MagicChoice?.SetState(null);
		magicChoice.Deactivate();
	}
	public void HideTheImbueAcquire()
	{
		_ImbueAcquire?.SetState(null);
		imbueAcquireUI.Deactivate();
	}
	#endregion
	#endregion

	#region Load/Unload
	public override void Load()
	{
		// Spoky (2026 January 24): Main.gameMenu is probably unneccesary but I had some complications with TDate UI with it so I'd rather put the check before doing antyhing
		if (Main.dedServ || Main.gameMenu) return;

		imbueAcquireUI = new();
		imbueAcquireUI.Initialize();

		magicChoice = new();
		magicChoice.Initialize();
	}
	#endregion

	public override void UpdateUI(GameTime gameTime)
	{
		_prevTime = gameTime;
		_ImbueAcquire?.Update(gameTime);
		_MagicChoice?.Update(gameTime);
	}

	public bool CanShowImbueAcquire() => _prevTime is not null && _ImbueAcquire?.CurrentState is not null;
	public bool CanShowUI() => _prevTime is not null && _MagicChoice?.CurrentState is not null;

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

		if (index is -1) return;

		layers.Insert(index, new LegacyGameInterfaceLayer(
			"ArcaneOdysseyMod: ImbueAcquireUI",
			delegate
			{
				if (CanShowImbueAcquire()) _ImbueAcquire.Draw(Main.spriteBatch, _prevTime);
				return true;
			},
			InterfaceScaleType.UI
			));

		layers.Insert(index, new LegacyGameInterfaceLayer(
			"ArcaneOdysseyMod: MagicChoiceUIState",
			delegate
			{
				if (CanShowUI()) _MagicChoice.Draw(Main.spriteBatch, _prevTime);
				return true;
			},
			InterfaceScaleType.UI)
			);
	}
}
