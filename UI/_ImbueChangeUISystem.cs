using ArcaneOdyssey.UI.ImbueAcquiring;
using ArcaneOdyssey.UI.ImbueChange;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI;

[Autoload(Side = ModSide.Client)]
public class ImbueAnythingUISystem : ModSystem
{
	private UserInterface _ImbueAcquire;
	internal ImbueAcquireUI imbueAcquireUI;

	private UserInterface _ImbueChange;
	internal ImbueChangeUI imbueChangeUI;

	private GameTime _prevTime;

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
		imbueChangeUI = new()
		{
			TheGuyThatFellOff = whom,
		};
		_ImbueChange = new();
		_ImbueChange?.SetState(imbueChangeUI);
		imbueChangeUI.Activate();
	}
	#endregion

	#region Hide
	public void HideTheImbueAcquire()
	{
		_ImbueAcquire?.SetState(null);
		imbueAcquireUI.Deactivate();
	}
	public void HideTheImbueChange()
	{
		_ImbueChange?.SetState(null);
		imbueChangeUI.Deactivate();
	}
	#endregion

	#region Load/Unload
	public override void Load()
	{
		// Spoky (2026 January 24): Main.gameMenu is probably unneccesary but I had some complications with TDate UI with it so I'd rather put the check before doing antyhing
		if (Main.dedServ || Main.gameMenu) return;

		imbueAcquireUI = new();
		imbueAcquireUI.Initialize();

		imbueChangeUI = new();
		imbueChangeUI.Initialize();
	}
	#endregion

	public override void UpdateUI(GameTime gameTime)
	{
		_prevTime = gameTime;
		_ImbueAcquire?.Update(gameTime);
		_ImbueChange?.Update(gameTime);
	}

	public bool CanShowImbueAcquire() => _prevTime is not null && _ImbueAcquire?.CurrentState is not null;
	public bool CanShowImbueChange() => _prevTime is not null && _ImbueChange?.CurrentState is not null;

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
			"ArcaneOdysseyMod: ImbueChangeUI",
			delegate
			{
				if (CanShowImbueChange()) _ImbueChange.Draw(Main.spriteBatch, _prevTime);
				return true;
			},
			InterfaceScaleType.UI
			));
	}
}
