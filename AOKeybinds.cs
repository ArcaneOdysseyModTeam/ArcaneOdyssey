using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public class AOKeybinds : ModSystem
	{
		public static ModKeybind CycleItemImbue { get; set; }
		public static ModKeybind DashBind { get; set; }

		public override void Load()
		{
			CycleItemImbue = KeybindLoader.RegisterKeybind(Mod, "CycleItemImbue", "G");
			DashBind = KeybindLoader.RegisterKeybind(Mod, "DashBind", "F");
		}

		public override void Unload()
		{
			CycleItemImbue = null;
			DashBind = null;
		}
	}
}
