#if VSDEBUGMODE
using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.GlobalTypes;
#endif
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Shaders;
using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.NPCs.Town;

namespace ArcaneOdyssey
{
	public class ArcaneOdysseyMod : Mod
	{
		/// <summary>
		/// disable all cooldowns and stuff lmao
		/// </summary>
		public static bool DevMode => ArcaneOdyssey.DevMode.devMode;
		public const string InternalName = "ArcaneOdyssey";

		public static Asset<Texture2D> MagicCircleSprite;

		internal static List<string> NoticeQueue = [];

		public static ArcaneOdysseyMod Instance => ModContent.GetInstance<ArcaneOdysseyMod>();

		internal static Dictionary<string, LocalizedText> staticLocalizer = [];

		internal static List<int> excludedItems = [];

		internal static List<int> excludedProjectiles = [];

		/// <param name="args">
		/// BlacklistProjectile/ExcludeProjectile (<seealso cref="int"/>)
		/// <para>BlacklistItem/ExcludeItem (<seealso cref="int"/>)</para>
		/// <para>AddMordenDialogue (<seealso cref="string"/>, <seealso cref="Func{bool}"/>)</para>
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
				case "AddMordenDialogue":
					Edgelord.AddHelpOption((string)args[1], (Func<bool>)args[2]);
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
		}

		public override void PostSetupContent()
		{
			this.CoolCustomLocalization("RandomWords.Default");
			this.CoolCustomLocalization("RandomWords.Unbound");
			this.CoolCustomLocalization("RandomWords.None");
			this.CoolCustomLocalization("RandomWords.AnyMaterial");
			this.CoolCustomLocalization("RandomWords.Help");
			this.CoolCustomLocalization("RandomWords.Press");
		}

		public string BTitlesHook_BiomeChecker(Player player)
		{
			if (player.InModBiome<EliusArena>())
				return "EliusArena";

			return "";
		}

		public IEnumerable<dynamic> BTitlesHook_GetBiomes()
		{
			var icon = Assets.Request<Texture2D>("icon_small", AssetRequestMode.ImmediateLoad);
			yield return new
			{
				Key = "EliusArena",
				Title = "Djin Ruins",
				SubTitle = DisplayNameClean,
				TitleColor = Color.Purple,
				TitleStroke = Color.MediumPurple,
				Icon = icon.Value,
			};
		}
	}

	public class DevMode : ModSystem 
	{
		#if VSDEBUGMODE
		public static bool devMode = true;
		#else
		public static bool devMode = false;
		#endif
	}

	public class AODebuffManager : GlobalBuff
	{
		public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
		{
			buffName = buffName.Replace("Imbue", "Gel");
		}
	}

	public class DownedBosses : ModSystem
	{
		public static bool downedEvander;
		public static bool downedDusk;
		public static bool downedLaelus;
		public static bool downedCrone;
		public static bool downedDelamere;

		public static bool downedElius;

		public static bool downedEnragedEmpress;
		public static bool downedWorldEater;
		public static bool downedBrain;

		public static void ResetDefaults()
		{
			downedEvander = false;
			downedEnragedEmpress = false;
			downedDusk = false;
			downedLaelus = false;
			downedCrone = false;
			downedDelamere = false;
		}

		public override void OnWorldLoad() => ResetDefaults();

		public override void OnWorldUnload() => ResetDefaults();

		public override void SaveWorldData(TagCompound tag)
		{
			List<string> downed = [];
			if (downedEvander)
				downed.Add("Evander");
			if (downedEnragedEmpress)
				downed.Add("EnragedEoL");
			if (downedDelamere)
				downed.Add("Delamere");
			if (downedDusk)
				downed.Add("Dusk");
			if (downedCrone)
				downed.Add("Crone");
			if (downedLaelus)
				downed.Add("Laelus");
			if (downedElius)
				downed.Add("Elius");

			tag["downed"] = downed;
		}

		public override void LoadWorldData(TagCompound tag)
		{
			var downed = tag.GetList<string>("downed");
			downedEvander = downed.Contains("Evander");
			downedDusk = downed.Contains("Dusk");
			downedCrone = downed.Contains("Crone");
			downedLaelus = downed.Contains("Laelus");
			downedDelamere = downed.Contains("Delamere");
			downedEnragedEmpress = downed.Contains("EnragedEoL");
			downedElius = downed.Contains("Elius");
		}
	}

	[ReinitializeDuringResizeArrays]
	public static class ArrayCollections
	{
		public static List<int>[] Mutations = ItemID.Sets.Factory.CreateCustomSet<List<int>>(null);

		public static int[] SizeStats = ItemID.Sets.Factory.CreateIntSet([
			ItemID.MoltenBreastplate, 7,
			ItemID.MoltenGreaves, 5,
			ItemID.MoltenHelmet, 3,
		]);

		public static int[] HasteStats = ItemID.Sets.Factory.CreateIntSet();

		public static bool[] phoenixAffected = NPCID.Sets.Factory.CreateBoolSet();
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
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOPlayer.Grounded) + " " + Main.LocalPlayer.ArcaneOdyssey().Grounded);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOPlayer.StatSize) + " " + Main.LocalPlayer.ArcaneOdyssey().StatSize);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOPlayer.Insanity) + " " + Main.LocalPlayer.ArcaneOdyssey().Insanity);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(AOPlayer.StatHaste) + " " + Main.LocalPlayer.ArcaneOdyssey().StatHaste);
				ArcaneOdysseyMod.NoticeQueue.Add(nameof(ArcaneOdysseyMod.DevMode) + " " + ArcaneOdysseyMod.DevMode);
			}
		}
	}
	#endif
}
