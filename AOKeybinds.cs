using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public class AOKeybinds : ModSystem
	{
		public static ModKeybind CycleItemImbue { get; set; }
		public static ModKeybind DashBind { get; set; }
		public static ModKeybind Vanish { get; set; }

		public override void Load()
		{    
			CycleItemImbue = KeybindLoader.RegisterKeybind(Mod, "CycleItemImbue", "G");
			DashBind = KeybindLoader.RegisterKeybind(Mod, "DashBind", "F");
			Vanish = KeybindLoader.RegisterKeybind(Mod, "Vanish", "V");
		}

		public override void Unload()
		{
			CycleItemImbue = null;
			DashBind = null;
			Vanish = null;
		}
	}
}
