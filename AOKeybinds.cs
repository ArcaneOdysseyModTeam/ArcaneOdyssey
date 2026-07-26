using Microsoft.Xna.Framework.Input;

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

		public override void Load()
		{
			CycleItemImbue = KeybindLoader.RegisterKeybind(Mod, nameof(CycleItemImbue), "G");
			DashBind = KeybindLoader.RegisterKeybind(Mod, nameof(DashBind), "F");
			CycleAuraMode = KeybindLoader.RegisterKeybind(Mod, nameof(CycleAuraMode), "J");
			CycleGodSoul = KeybindLoader.RegisterKeybind(Mod, nameof(CycleGodSoul), "K");
			CycleImbueAttack = KeybindLoader.RegisterKeybind(Mod, nameof(CycleImbueAttack), "Q");
			AltSkillUse = KeybindLoader.RegisterKeybind(Mod, nameof(AltSkillUse), Keys.LeftAlt);
		}

		public override void Unload()
		{
			CycleItemImbue = null;
			DashBind = null;
			CycleAuraMode = null;
			CycleGodSoul = null;
			CycleImbueAttack = null;
			AltSkillUse = null;
		}
	}
}
