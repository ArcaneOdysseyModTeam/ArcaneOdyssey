using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public class AOKeybinds : ModSystem
	{
		public static ModKeybind CycleItemImbue { get; set; }
		public static ModKeybind DashBind { get; set; }
		public static ModKeybind CycleAuraMode { get; set; }
		public static ModKeybind CycleGodSoul { get; set; }

		public static ModKeybind CycleImbueAttack { get; set; }
		public static ModKeybind ActivateImbuePassive { get; set; }

		public override void Load()
		{
			CycleItemImbue = KeybindLoader.RegisterKeybind(Mod, nameof(CycleItemImbue), "G");
			DashBind = KeybindLoader.RegisterKeybind(Mod, nameof(DashBind), "F");
			CycleAuraMode = KeybindLoader.RegisterKeybind(Mod, nameof(CycleAuraMode), "J");
			CycleGodSoul = KeybindLoader.RegisterKeybind(Mod, nameof(CycleGodSoul), "K");
			CycleImbueAttack = KeybindLoader.RegisterKeybind(Mod, nameof(CycleImbueAttack), "Q");
			ActivateImbuePassive = KeybindLoader.RegisterKeybind(Mod, nameof(ActivateImbuePassive), "L");
		}

		public override void Unload()
		{
			CycleItemImbue = null;
			DashBind = null;
			CycleAuraMode = null;
			CycleGodSoul = null;
		}
	}
}
