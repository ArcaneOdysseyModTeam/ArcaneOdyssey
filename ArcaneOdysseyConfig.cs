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
		public bool PredictiveArray { get; set; }

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
		public bool GenerateTucker { get; set; }

		[DefaultValue(false)]
		public bool AlternatePhoenixEffectVFX { get; set; }

		[DefaultValue(true)]
		public bool ItemTypeTooltips { get; set; }

		[DefaultValue(true)]
		public bool AbilityText { get; set; }

		[DefaultValue(MagicCircleTypes.Ancient)]
		[ReloadRequired]
		public MagicCircleTypes MagicCircleType { get; set; }

		public static ArcaneOdysseyClientConfig Instance;
	}

	public enum MagicCircleTypes
	{
		Ancient,
		Collision,
		Ornamental,
		Penta,
		Reminiscent,
		Segmented,
		Singularity,
		Solar
	}
}
