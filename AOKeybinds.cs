using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public class AOKeybinds : ModSystem
	{
		public static ModKeybind CycleItemImbue { get; set; }
		public static ModKeybind DashBind { get; set; }
		public static ModKeybind CycleAuraMode { get; set; }

		public override void Load()
		{
			CycleItemImbue = KeybindLoader.RegisterKeybind(Mod, "CycleItemImbue", "G");
			DashBind = KeybindLoader.RegisterKeybind(Mod, "DashBind", "F");
			CycleAuraMode = KeybindLoader.RegisterKeybind(Mod, "CycleAuraMode", "C");
		}

		public override void Unload()
		{
			CycleItemImbue = null;
			DashBind = null;
			CycleAuraMode = null;
		}
	}
}
