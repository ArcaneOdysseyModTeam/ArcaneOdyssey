using System;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader.Config;
using System.Collections.Generic;

namespace ArcaneOdyssey
{
	public class ArcaneOdysseyConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[DefaultValue(true)]
		public bool AffectsOtherMods { get; set; }


		[DefaultValue(true)]
		public bool GenerateTucker { get; set; }


		[DefaultValue(true)]
		public bool EnableMorden { get; set; }

		[DefaultValue(true)]
		public bool ElfPetSoundEffects { get; set; }


		public static ArcaneOdysseyConfig Instance;
	}
}
