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

		public override void Load()
		{    
			CycleItemImbue = KeybindLoader.RegisterKeybind(Mod, "CycleItemImbue", "H");
		}

		public override void Unload()
		{
			CycleItemImbue = null;
		}
	}
}
