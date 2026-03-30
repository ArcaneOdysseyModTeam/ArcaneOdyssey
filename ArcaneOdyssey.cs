#if VSDEBUGMODE
using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.GlobalTypes;
#endif
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using ArcaneOdyssey.Biomes;

namespace ArcaneOdyssey
{
	public class ArcaneOdysseyMod : Mod
	{
		/// <summary>
		/// disable all cooldowns and stuff lmao
		/// </summary>
#if VSDEBUGMODE
		public const bool DevMode = true;
#else
		public const bool DevMode = false;
#endif
		public const string InternalName = "ArcaneOdyssey";

		public static Asset<Texture2D> MagicCircleSprite;

		internal static List<string> NoticeQueue = [];

		public static ArcaneOdysseyMod Instance => ModContent.GetInstance<ArcaneOdysseyMod>();

		internal static Dictionary<string, LocalizedText> staticLocalizer = [];

		internal static List<int> excludedItems = [];

		internal static List<int> excludedProjectiles = [];

		/// <param name="args">
		/// BlacklistProjectile/ExcludeProjectile (<seealso cref="int"/>)
		/// <para/>BlacklistItem/ExcludeItem (<seealso cref="int"/>)
		/// <para/>AddSizeStat (<seealso cref="int"/>, <seealso cref="int"/>)
		/// <para/>AddHasteStat (<seealso cref="int"/>, <seealso cref="int"/>)
		/// </param>
		public override object Call(params object[] args)
		{
			switch (args[0])
			{
				case "BlacklistProjectile":
				case "ExcludeProjectile":
					excludedProjectiles.Add((int)args[1]);
					break;
				case "BlacklistItem":
				case "ExcludeItem":
					excludedItems.Add((int)args[1]);
					break;
				case "AddSizeStat":
				case "SetSizeStat":
				case "SizeStat":
					Sets.SizeStats[(int)args[1]] = (int)args[2];
					break;
				case "AddHasteStat":
				case "SetHasteStat":
				case "HasteStat":
					Sets.HasteStats[(int)args[1]] = (int)args[2];
					break;
			}
			return null;
		}

		public override void Load()
		{
			excludedItems.Clear();
			excludedProjectiles.Clear();
			staticLocalizer.Clear();
			NoticeQueue.Clear();

			if (!Main.dedServ)
			{
				MagicCircleSprite = Assets.Request<Texture2D>($"Effects/MagicCircles/{ArcaneOdysseyClientConfig.Instance.MagicCircleType}", AssetRequestMode.ImmediateLoad);

				Asset<Effect> MagicCircleShaderBase = Assets.Request<Effect>("Effects/MagicCircleShaderBase", AssetRequestMode.ImmediateLoad);

				GameShaders.Misc[InternalName + ":MagicCircleBase"] = new MiscShaderData(MagicCircleShaderBase, "MagicCircleShaderBase");
			}
		}

		public override void Unload()
		{
			excludedItems.Clear();
			excludedProjectiles.Clear();
			staticLocalizer.Clear();
			NoticeQueue.Clear();
			GameShaders.Misc[InternalName + ":MagicCircleBase"] = null;
			MagicCircleSprite = null;
		}

		public override void PostSetupContent()
		{
			this.CoolCustomLocalization("RandomWords.Default");
			this.CoolCustomLocalization("RandomWords.Unbound");
			this.CoolCustomLocalization("RandomWords.None");
			this.CoolCustomLocalization("RandomWords.AnyMaterial");
			this.CoolCustomLocalization("RandomWords.Help");
			this.CoolCustomLocalization("RandomWords.Guide");
			this.CoolCustomLocalization("RandomWords.Press");
			this.CoolCustomLocalization("RandomWords.Kill");
			this.CoolCustomLocalization("RandomWords.Spare");
		}

		public string BTitlesHook_BiomeChecker(Player player)
		{
			if (player.InModBiome<EliusArena>())
				return "EliusArena";

			return "";
		}

		public IEnumerable<dynamic> BTitlesHook_GetBiomes()
		{
			var icon = ModContent.Request<Texture2D>(AOUtils.GetTexture<EliusArena>() + "_Icon", AssetRequestMode.ImmediateLoad);
			yield return new
			{
				Key = "EliusArena",
				Title = "Djin Ruins",
				SubTitle = DisplayNameClean,
				TitleColor = Color.LightGray,
				TitleStroke = Color.MediumPurple,
				Icon = icon.Value,
			};
		}


		[ReinitializeDuringResizeArrays]
		public static class Sets
		{
			public static List<int>[] Mutations = ItemID.Sets.Factory.CreateCustomSet<List<int>>(null);

			public static int[] SizeStats = ItemID.Sets.Factory.CreateIntSet(0,
				ItemID.MoltenBreastplate, 7,
				ItemID.MoltenGreaves, 5,
				ItemID.MoltenHelmet, 3
			);

			public static int[] HasteStats = ItemID.Sets.Factory.CreateIntSet(0);

			public static bool[] phoenixAffected = NPCID.Sets.Factory.CreateBoolSet();

			public static Asset<Texture2D>[] annihilationSprites = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

			public static Asset<Texture2D>[] raySprites = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

			public static Asset<Texture2D>[] rayEndSprites = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

			public static Asset<Texture2D>[] rayStartSprites = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

			public static Asset<Texture2D>[] blasts = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

			public static int[] BlastMaxFrames = ItemID.Sets.Factory.CreateIntSet(1);
		}
	}

	public class AODebuffManager : GlobalBuff
	{
		public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
		{
			buffName = buffName.Replace("Imbue", "Gel");
		}
	}

	public class MessageHelper : ModSystem
	{
		public override void PostUpdateWorld()
		{
			foreach (string message in ArcaneOdysseyMod.NoticeQueue)
			{
				Main.NewText(message, Color.Yellow);
			}
			ArcaneOdysseyMod.NoticeQueue = [];
		}
	}

	#if VSDEBUGMODE
	public class DebugStuff : ModSystem
	{
		public static ModKeybind PrintInfo { get; set; }

		public override void Load()
		{
			PrintInfo = KeybindLoader.RegisterKeybind(Mod, "PrintInfo", "P");
		}

		public override void Unload()
		{
			PrintInfo = null;
		}

		public override void PostUpdateItems()
		{
			if (PrintInfo.JustPressed) 
			{
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOUtils.BossesKilled) + " " + AOUtils.BossesKilled);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOTile.commonpity) + " " + AOTile.commonpity);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOTile.rarepity) + " " + AOTile.rarepity);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOTile.lostpity) + " " + AOTile.lostpity);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOPlayer.acumen) + " " + Main.LocalPlayer.ArcaneOdyssey().acumen);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOPlayer.BronzeSealed) + " " + Main.LocalPlayer.ArcaneOdyssey().BronzeSealed);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOPlayer.NimbusSealed) + " " + Main.LocalPlayer.ArcaneOdyssey().NimbusSealed);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOPlayer.DarkSealed) + " " + Main.LocalPlayer.ArcaneOdyssey().DarkSealed);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOPlayer.grounded) + " " + Main.LocalPlayer.ArcaneOdyssey().grounded);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOPlayer.StatSize) + " " + Main.LocalPlayer.ArcaneOdyssey().StatSize);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOPlayer.Insanity) + " " + Main.LocalPlayer.ArcaneOdyssey().Insanity);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOPlayer.StatHaste) + " " + Main.LocalPlayer.ArcaneOdyssey().StatHaste);
			}
		}
	}
	#endif
}
