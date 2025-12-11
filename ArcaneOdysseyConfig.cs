using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ArcaneOdyssey
{
	public class ArcaneOdysseyConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[DefaultValue(true)]
		[ReloadRequired]
		public bool AffectsOtherMods { get; set; }

		[DefaultValue(true)]
		public bool EnableMorden { get; set; }

		[DefaultValue(true)]
		public bool VanillaItemTemperatures { get; set; }

		[DefaultValue(true)]
		[ReloadRequired]
		public bool ProjectileSizes { get; set; }

		public static ArcaneOdysseyConfig Instance;
	}

	public class ArcaneOdysseyClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[DefaultValue(true)]
		public bool GenerateTucker { get; set; }

		[DefaultValue(true)]
		public bool ElfPetSoundEffects { get; set; }

		[DefaultValue(false)]
		public bool AlternatePhoenixEffectVFX { get; set; }

		public static ArcaneOdysseyClientConfig Instance;
	}
}
