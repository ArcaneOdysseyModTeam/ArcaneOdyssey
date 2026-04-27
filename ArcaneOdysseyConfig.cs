using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.ComponentModel;
using Terraria;
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
		public bool VanillaItemTemperatures { get; set; }

		[DefaultValue(true)]
		public bool GroundReflexes { get; set; }

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

		[DefaultValue(MagicCircleTypes.Familiar)]
		public MagicCircleTypes MagicCircleType { get; set; }

		public static ArcaneOdysseyClientConfig Instance;

		public override void OnChanged()
		{
			if (!Main.dedServ)
			{
				ArcaneOdysseyMod.MagicCircleSprite = Mod.Assets?.Request<Texture2D>($"Effects/MagicCircles/{MagicCircleType}", AssetRequestMode.ImmediateLoad);
			}
		}
	}

	public enum MagicCircleTypes
	{
		Familiar,
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
