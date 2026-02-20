using ArcaneOdyssey.UI.ImbueAcquiring;
using ArcaneOdyssey.UI.ImbueAcquiringSequel;
using ArcaneOdyssey.UI.ImbueChange;

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace ArcaneOdyssey.UI;

/// <summary>
/// The <see cref="ModSystem"/> 
/// </summary>
[Autoload(Side = ModSide.Client)]
public class ImbueAnythingUISystem : ModSystem
{
	private UserInterface _ImbueAcquire;
	internal ImbueAcquireUI imbueAcquireUI;

	private UserInterface _ImbueChange;
	internal ImbueChangeUI imbueChangeUI;

	private UserInterface _ImbueAcquireSequel;
	internal ImbueAcquireSequelUI imbueAcquireSequelUI;

	private GameTime _prevTime;

	#region Show
	public void ShowAcquireUI()
	{
		imbueAcquireUI = new();
		imbueAcquireUI.Initialize();
		_ImbueAcquire = new();
		_ImbueAcquire?.SetState(imbueAcquireUI);
		imbueAcquireUI.Activate();
	}
	public void ShowAcquireSequelUI()
	{
		imbueAcquireSequelUI = new();
		imbueAcquireSequelUI.Initialize();
		_ImbueAcquireSequel = new();
		_ImbueAcquireSequel?.SetState(imbueAcquireSequelUI);
		imbueAcquireSequelUI.Activate();
	}
	public void ShowSwapUI(ModItem whom)
	{
		imbueChangeUI = new()
		{
			TheGuyThatFellOff = whom,
		};
		imbueChangeUI.Initialize();
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
	public void HideTheImbueSequelAcquire()
	{
		_ImbueAcquireSequel?.SetState(null);
		imbueAcquireSequelUI.Deactivate();
	}
	public void HideTheImbueChange()
	{
		_ImbueChange?.SetState(null);
		imbueChangeUI.Deactivate();
	}
	#endregion

	// Spoky (2026 February 20): Turns out load method is unnecessary, cool? If something breaks maybe load method was needed
	#region Load/Unload
	public override void Load()
	{
		// Spoky (2026 January 24): Main.gameMenu is probably unneccesary but I had some complications with TDate UI with it so I'd rather put the check before doing antyhing
		if (Main.dedServ || Main.gameMenu) return;

		//imbueAcquireUI = new();
		//imbueAcquireUI.Initialize();

		//imbueAcquireSequelUI = new();
		//imbueAcquireSequelUI.Initialize();

		//imbueChangeUI = new();
		//imbueChangeUI.Initialize();

	}
	#endregion

	public override void UpdateUI(GameTime gameTime)
	{
		_prevTime = gameTime;
		_ImbueAcquire?.Update(gameTime);
		_ImbueAcquireSequel?.Update(gameTime);
		_ImbueChange?.Update(gameTime);
	}

	public bool CanShowImbueAcquire() => _prevTime is not null && _ImbueAcquire?.CurrentState is not null;
	public bool CanShowImbueSequelAcquire() => _prevTime is not null && _ImbueAcquireSequel?.CurrentState is not null;

	public bool CanShowImbueChange() => _prevTime is not null && _ImbueChange?.CurrentState is not null;

	public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		int index = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

		if (index is -1) return;

		string[] names = ["ImbueAcquireUI", "ImbueAcquireSequelUI", "ImbueChangeUI"];
		bool[] canShows = [CanShowImbueAcquire(), CanShowImbueSequelAcquire(), CanShowImbueChange()];
		UserInterface[] uis = [_ImbueAcquire, _ImbueAcquireSequel, _ImbueChange];

		if (names.Length != canShows.Length || canShows.Length != uis.Length)
		{
			Main.NewText($"Lengh of {nameof(names)}, {nameof(canShows)} and/or {nameof(uis)} is inconsistent!", new Color(255, 0, 255));
			return;
		}

		// Spoky (2026 Feb 14): Reason I'm doin this part this way is because the 2nd Acquire UI wasn't opening for me because I forgot to change its condition
		for (int i = 0; i < names.Length; i++)
		{
			string name = names[i];
			bool canShow = canShows[i];
			UserInterface ui = uis[i];

			layers.Insert(index, new LegacyGameInterfaceLayer(
				$"ArcaneOdysseyMod: {name}",
				delegate
				{
					if (canShow) ui.Draw(Main.spriteBatch, _prevTime);
					return true;
				},
				InterfaceScaleType.UI
				));
		}

	}
}
