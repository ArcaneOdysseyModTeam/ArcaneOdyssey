using ArcaneOdyssey.Subworlds;
using Microsoft.Xna.Framework.Input;
using SubworldLibrary;

namespace ArcaneOdyssey
{
	public class AOKeybinds : ModSystem
	{
		public static ModKeybind CycleItemImbue { get; set; }
		public static ModKeybind DashBind { get; set; }
		public static ModKeybind CycleAuraMode { get; set; }
		public static ModKeybind CycleGodSoul { get; set; }

		public static ModKeybind CycleImbueAttack { get; set; }
		public static ModKeybind AltSkillUse { get; set; }

#if VSDEBUGMODE
		public static ModKeybind GetTileCoordinate { get; set; }
#endif

		public override void Load()
		{
			CycleItemImbue = KeybindLoader.RegisterKeybind(Mod, nameof(CycleItemImbue), "G");
			DashBind = KeybindLoader.RegisterKeybind(Mod, nameof(DashBind), "F");
			CycleAuraMode = KeybindLoader.RegisterKeybind(Mod, nameof(CycleAuraMode), "J");
			CycleGodSoul = KeybindLoader.RegisterKeybind(Mod, nameof(CycleGodSoul), "K");
			CycleImbueAttack = KeybindLoader.RegisterKeybind(Mod, nameof(CycleImbueAttack), "Q");
			AltSkillUse = KeybindLoader.RegisterKeybind(Mod, nameof(AltSkillUse), Keys.LeftAlt);
#if VSDEBUGMODE
			GetTileCoordinate = KeybindLoader.RegisterKeybind(Mod, nameof(GetTileCoordinate), Keys.RightAlt);
#endif
		}

		public override void Unload()
		{
			CycleItemImbue = null;
			DashBind = null;
			CycleAuraMode = null;
			CycleGodSoul = null;
			CycleImbueAttack = null;
			AltSkillUse = null;
#if VSDEBUGMODE
			GetTileCoordinate = null;
		}
		public override void PostUpdateEverything()
		{
			if (GetTileCoordinate.Current)
			{
				Player player = Main.LocalPlayer;
				player.noThrow = 2;
				player.cursorItemIconText = Main.MouseWorld.ToTileCoordinates().ToString();
				player.cursorItemIconID = -1;
				player.cursorItemIconEnabled = true;
				Dust.DrawDebugBox(new(Main.MouseWorld.ToTileCoordinates().ToWorldCoordinates(Vector2.Zero).X.Round(), Main.MouseWorld.ToTileCoordinates().ToWorldCoordinates(Vector2.Zero).Y.Round(), 16, 16));
				if (SubworldSystem.IsActive<TalosSubworld>())
					foreach (var rect in TalosSubworld.openRooms)
					{
						if (rect.Contains(Main.MouseWorld.ToTileCoordinates()))
						{
							player.cursorItemIconText += ", in spawning room";
						}
					}
			}
#endif
		}
	}
}
