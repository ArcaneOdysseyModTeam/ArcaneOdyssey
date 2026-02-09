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

		[DefaultValue(true)]
		public bool PredictiveArray { get; set; }

		public override void OnLoaded()
		{
			if (!AffectsOtherMods)
			{
				ArcaneOdysseyMod.NoticeQueue.Add("\"[i:Cog] Affect other mods\" config is disabled, items from other mods will not be affected by this mod.");
			}
			if (!ProjectileSizes)
			{
				ArcaneOdysseyMod.NoticeQueue.Add("\"[i:ArcaneOdyssey/ColossalGreatsword] Projectile Sizes\" config is disabled, projectiles will not have their size affected by imbues or your attack size stat.");
			}
		}

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

		[DefaultValue(true)]
		public bool ItemTypeTooltips { get; set; }

		[DefaultValue(true)]
		[ReloadRequired]
		public bool MissingDebuffSprites { get; set; }

		public static ArcaneOdysseyClientConfig Instance;
	}
}
