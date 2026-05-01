#if VSDEBUGMODE
using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.GlobalTypes;
#endif
using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Items.Weapons.Old;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using System.IO;

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

		public static bool finishedLoading = false;

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
			finishedLoading = false;
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
			finishedLoading = false;
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

		public class PacketID
		{
			/// <summary>
			/// Create lingering visuals on all clients, best used for item swing visuals
			/// <para/> Requires two item ids and a rectangle
			/// </summary>
			public const byte LingeringVisuals = 0;
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			var command = reader.ReadByte();
			if (command == PacketID.LingeringVisuals)
			{
				if (Main.dedServ) // forward to clients
				{
					var packet = GetPacket();
					packet.Write(PacketID.LingeringVisuals);
					packet.Write(reader.ReadInt32()); // imbue 1
					packet.Write(reader.ReadInt32()); // imbue 2, if applicable
					packet.Write(reader.ReadRectangle()); // area
					packet.Send(ignoreClient: whoAmI);
				}
				else
				{
					var imbue = AOUtils.SafeImbuable(ModContent.GetModItem(reader.ReadInt32()));
					var imbue2 = AOUtils.SafeImbuable(ModContent.GetModItem(reader.ReadInt32()));
					var area = reader.ReadRectangle();

					imbue?.LingeringEffects(area);
					imbue2?.LingeringEffects(area);
				}
			}
		}


		[ReinitializeDuringResizeArrays]
		public static class Sets
		{
			public static int[] OldWeapons = [ModContent.ItemType<OldRapier>(), ModContent.ItemType<OldSword>(), ModContent.ItemType<OldGreataxe>(), ModContent.ItemType<OldGreatsword>(), ModContent.ItemType<WoodenStaff>()];

			public static List<int>[] Mutations = ItemID.Sets.Factory.CreateCustomSet<List<int>>(null);

			public static int[] SizeStats = ItemID.Sets.Factory.CreateIntSet(0,
				ItemID.MoltenBreastplate, 7,
				ItemID.MoltenGreaves, 5,
				ItemID.MoltenHelmet, 3
			);

			public static int[] HasteStats = ItemID.Sets.Factory.CreateIntSet(0);

			public static bool[] phoenixAffected = NPCID.Sets.Factory.CreateBoolSet();

			public static int[] BlastMaxFrames = ItemID.Sets.Factory.CreateIntSet(1);

			public static bool[] staff = ItemID.Sets.Factory.CreateBoolSet(ItemID.MonkStaffT1, ItemID.MonkStaffT3);

			public static bool[] claw = ItemID.Sets.Factory.CreateBoolSet(ItemID.FetidBaghnakhs);

			public static bool[] bow = ItemID.Sets.Factory.CreateBoolSet();

			public static bool[] spear = ItemID.Sets.Factory.CreateBoolSet();

			public static bool[] greatsword = ItemID.Sets.Factory.CreateBoolSet(ItemID.FieryGreatsword, ItemID.BreakerBlade, ItemID.AdamantiteSword, ItemID.TitaniumSword, ItemID.ChlorophyteClaymore, ItemID.StarWrath, ItemID.Seedler, ItemID.TerraBlade);

			public static bool[] sword = ItemID.Sets.Factory.CreateBoolSet();

			public static bool[] greataxe = ItemID.Sets.Factory.CreateBoolSet(ItemID.ChlorophyteGreataxe, ItemID.TitaniumWaraxe, ItemID.WarAxeoftheNight, ItemID.AdamantiteWaraxe);

			public static bool[] rapier = ItemID.Sets.Factory.CreateBoolSet();

			public static bool[] dualbladed = ItemID.Sets.Factory.CreateBoolSet();

			public static bool[] dagger = ItemID.Sets.Factory.CreateBoolSet(ItemID.ThrowingKnife, ItemID.VampireKnives, ItemID.PoisonedKnife, ItemID.FrostDaggerfish, ItemID.BoneDagger, ItemID.FlyingKnife, ItemID.ShadowFlameKnife, ItemID.PsychoKnife);

			public static bool[] gun = ItemID.Sets.Factory.CreateBoolSet();

			public static bool[] greathammer = ItemID.Sets.Factory.CreateBoolSet(ItemID.ChlorophyteWarhammer, ItemID.PaladinsHammer);

			public static bool[] flail = ItemID.Sets.Factory.CreateBoolSet(ItemID.DripplerFlail, ItemID.Mace, ItemID.FlamingMace, ItemID.Flairon, ItemID.BallOHurt, ItemID.BlueMoon, ItemID.DaoofPow, ItemID.FlowerPow, ItemID.Sunfury, ItemID.TheMeatball);

			public static int[] baseImbues = ItemID.Sets.Factory.CreateIntSet();

			public static bool[] tombstone = ProjectileID.Sets.Factory.CreateBoolSet();

			public static bool[] atlanteanItem = ItemID.Sets.Factory.CreateBoolSet();

			[ReinitializeDuringResizeArrays]
			public static class Assets
			{

				public static Asset<Texture2D>[] annihilationSprites = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

				public static Asset<Texture2D>[] raySprites = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

				public static Asset<Texture2D>[] rayEndSprites = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

				public static Asset<Texture2D>[] rayStartSprites = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

				public static Asset<Texture2D>[] blasts = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);
			}
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

		internal static bool InArray(int i)
		{
			return ItemID.Sets.Deprecated[i] || ArcaneOdysseyMod.Sets.claw[i] || ArcaneOdysseyMod.Sets.spear[i] || ArcaneOdysseyMod.Sets.dualbladed[i] || ArcaneOdysseyMod.Sets.greatsword[i] || ArcaneOdysseyMod.Sets.dagger[i] || ArcaneOdysseyMod.Sets.staff[i] || ArcaneOdysseyMod.Sets.rapier[i] || ArcaneOdysseyMod.Sets.greathammer[i] || ItemID.Sets.Yoyo[i] || ArcaneOdysseyMod.Sets.greataxe[i] || ArcaneOdysseyMod.Sets.flail[i];
		}

		public override void PostSetupRecipes()
		{
			ArcaneOdysseyMod.finishedLoading = true;

			for (int i = 0; i < ProjectileLoader.ProjectileCount; i++)
			{
				if (ProjectileID.Sets.IsAGravestone[i])
				{
					ArcaneOdysseyMod.Sets.tombstone[i] = true; 
				}
			}

			for (int i = 0; i < ItemLoader.ItemCount; i++)
			{
				if (!InArray(i))
				{
					var item = new Item(i);

					if (item.ModItem is not null)
					{
						if (AOUtils.ImbueClassCheck(item) || item.ArcaneOdyssey().WeaponsType is WeaponType.Arcanium)
						{
							ExternalModSupport.CheckWeapon(item.ModItem);
						}
					}

					if (!InArray(i))
					{
						if (Item.claw[i])
						{
							ArcaneOdysseyMod.Sets.claw[i] = true;
						}

						else if (ItemID.Sets.Spears[i])
						{
							ArcaneOdysseyMod.Sets.spear[i] = true;
						}

						if (!InArray(i))
						{
							if (item.DamageType.CountsAsClass(DamageClass.Melee) && item.axe == 0 && item.hammer == 0 && item.pick == 0 && item.ModItem is not (Imbuable or Scroll) && !item.accessory)
							{
								ArcaneOdysseyMod.Sets.sword[i] = true;
							}

							else if (item.useAmmo == AmmoID.Arrow)
							{
								ArcaneOdysseyMod.Sets.bow[i] = true;
							}

							else if (item.useAmmo == AmmoID.Bullet)
							{
								ArcaneOdysseyMod.Sets.gun[i] = true;
							}
						}
					}
				}
			}
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
