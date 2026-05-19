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
		[ReloadRequired]
		public bool VanillaItemTemperatures { get; set; }

		[DefaultValue(true)]
		public bool GroundReflexes { get; set; }

		[DefaultValue(true)]
		[ReloadRequired]
		public bool SyncProjectileSizes { get; set; }

		public override void OnLoaded()
		{
			if (!AffectsOtherMods)
			{
				ArcaneOdysseyMod.NoticeQueue.Add("\"[i:Cog] Affect other mods\" config is disabled, items from other mods will not be affected by this mod.");
			}
		}

		public static ArcaneOdysseyConfig Instance;
	}

	public class ArcaneOdysseyClientConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[DefaultValue(true)]
		public bool ItemTypeTooltips { get; set; }

		[DefaultValue(true)]
		public bool AbilityText { get; set; }

		[DefaultValue(true)]
		public bool UniqueMagicCircles { get; set; }

		[DefaultValue(true)]
		[ReloadRequired]
		public bool PulsingImbueIcons { get; set; }


		public static ArcaneOdysseyClientConfig Instance;
	}
}
