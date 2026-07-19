global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using ReLogic.Content;
global using Terraria;
global using Terraria.ID;
global using Terraria.Localization;
global using Terraria.ModLoader;

using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Buffs;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Scrolls.Usable.Rare;
using ArcaneOdyssey.NPCs.Bosses;
using ArcaneOdyssey.NPCs.Town;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.Chat;
using Terraria.Graphics.Shaders;

namespace ArcaneOdyssey
{
	public class ArcaneOdysseyMod : Mod
	{
		/// <summary>
		/// misc dev stuff
		/// </summary>
#if VSDEBUGMODE
		public const bool DevMode = true;
#else
		public const bool DevMode = false;
#endif
		public const string InternalName = "ArcaneOdyssey";

		internal static List<string> NoticeQueue = [];

		public static ArcaneOdysseyMod Instance;

		internal static Dictionary<string, LocalizedText> staticLocalizer = [];

		internal static bool finishedLoading = false;

		/// <param name="args">
		/// ExcludeProjectile (<seealso cref="int"/>)
		/// <para/>ExcludeItem (<seealso cref="int"/>)
		/// <para/>AddSizeStat (<seealso cref="int"/>, <seealso cref="int"/>)
		/// <para/>AddHasteStat (<seealso cref="int"/>, <seealso cref="int"/>)
		/// <para/>SetItemTemperature (<seealso cref="int"/>, <seealso cref="Nullable"/>{<seealso cref="bool"/>}))
		/// <para/>SetWeaponType (<seealso cref="int"/>, <seealso cref="int"/> (<seealso cref="WeaponType"/>))
		/// <para/>RegisterWoodWand (<seealso cref="int"/>, <seealso cref="int"/>)
		/// </param>
		public override object Call(params object[] args)
		{
			switch (args[0])
			{
				case "BlacklistProjectile":
				case "ExcludeProjectile":
					Sets.excludedProjectile[(int)args[1]] = true;
					break;
				case "BlacklistItem":
				case "ExcludeItem":
					Sets.excludedItem[(int)args[1]] = true;
					break;
				case "AddSizeStat":
				case "SetSizeStat":
					Sets.SizeStats[(int)args[1]] = (int)args[2];
					break;
				case "AddHasteStat":
				case "SetHasteStat":
					Sets.HasteStats[(int)args[1]] = (int)args[2];
					break;
				case "SetItemTemperature":
					Sets.cold[(int)args[1]] = (bool?)args[2];
					break;
				case "SetWeaponType":
					Sets.weaponType[(int)args[1]] = (WeaponType)(int)args[2];
					break;
				case "RegisterWoodWand":
					Sets.woodWand[(int)args[1]] = true;
					Sets.wandWoodType[(int)args[1]] = (int)args[2];
					break;
			}
			return null;
		}

		public override void Load()
		{
			Instance = this;
			finishedLoading = false;
			staticLocalizer.Clear();
			NoticeQueue.Clear();

			if (!Main.dedServ)
			{
				Asset<Effect> MagicCircleShaderBase = Assets.Request<Effect>("Effects/MagicCircleShaderBase", AssetRequestMode.ImmediateLoad);

				GameShaders.Misc[InternalName + ":MagicCircleBase"] = new MiscShaderData(MagicCircleShaderBase, "MagicCircleShaderBase");
			}
		}

		public override void Unload()
		{
			Instance = null;
			finishedLoading = false;
			staticLocalizer.Clear();
			NoticeQueue.Clear();
			GameShaders.Misc[InternalName + ":MagicCircleBase"] = null;
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
			/// <para/> Requires two imbue item ids and a rectangle
			/// </summary>
			public const byte LingeringVisuals = 0;

			/// <summary>
			/// Create explosion visuals on all clients
			/// <para/> Requires two imbue item ids, a vector2, a float, and the amount of explosions
			/// </summary>
			public const byte ExplosionVisuals = 1;

			/// <summary>
			/// Enchants all players
			/// </summary>
			public const byte Enchantment = 2;
			/// <summary>
			/// Marks elius as killed or spared, and spawns vfx on all clients
			/// <para/> Requires a spared bool, and a rectangle
			/// </summary>
			public const byte EliusSpare = 3;

			/// <summary>
			/// Spawns Elius's arena
			/// </summary>
			public const byte SpawnEliusArena = 4;
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			var command = reader.ReadByte();
			if (command == PacketID.LingeringVisuals)
			{
				if (Main.dedServ) // forward to clients
				{
					var packet = GetPacket();
					packet.Write(command);
					packet.Write(reader.ReadInt32()); // imbue 1
					packet.Write(reader.ReadInt32()); // imbue 2, if applicable
					packet.Write(reader.ReadRectangle()); // area
					packet.Send(ignoreClient: whoAmI);
				}
				else
				{
					var imbue = AOUtils.Safe<Imbuable>(ModContent.GetModItem(reader.ReadInt32()));
					var imbue2 = AOUtils.Safe<Imbuable>(ModContent.GetModItem(reader.ReadInt32()));
					var area = reader.ReadRectangle();

					imbue?.LingeringEffects(area);
					imbue2?.LingeringEffects(area);
				}
			}
			else if (command == PacketID.ExplosionVisuals)
			{
				if (Main.dedServ) // forward to clients
				{
					var packet = GetPacket();
					packet.Write(command);
					packet.Write(reader.ReadInt32()); // imbue 1
					packet.Write(reader.ReadInt32()); // imbue 2, if applicable
					packet.Write(reader.ReadVector2()); // area
					packet.Write(reader.ReadSingle()); // intensity
					packet.Write(reader.ReadByte()); // explosion amount, to avoid spamming the network
					packet.Send(ignoreClient: whoAmI);
				}
				else
				{
					var imbue = AOUtils.Safe<Imbuable>(ModContent.GetModItem(reader.ReadInt32()));
					var imbue2 = AOUtils.Safe<Imbuable>(ModContent.GetModItem(reader.ReadInt32()));
					var area = reader.ReadVector2();
					var intensity = reader.ReadSingle();
					var max = reader.ReadByte();

					for (var i = 0; i < max; i++)
					{
						imbue?.ExplosionEffects(area, intensity);
						imbue2?.ExplosionEffects(area, intensity);
					}
				}
			}
			else if (command == PacketID.Enchantment)
			{
				if (Main.dedServ)
				{
					ChatHelper.BroadcastChatMessage(ModContent.GetInstance<EnchantmentSpell>().GetLocalization("Message").ToNetworkText(Main.player[whoAmI].name), Color.AliceBlue);
					var packet = GetPacket();
					packet.Write(command);
					packet.Send();
				}
				else
				{
					foreach (var player in Main.ActivePlayers)
					{
						player.AddBuff(ModContent.BuffType<Enchanted>(), 60 * 60 * 5); // 5 mins
					}
				}
			}
			else if (command == PacketID.EliusSpare)
			{
				var spared = reader.ReadBoolean();
				EliusSpareSystem.spared = spared;
				var npc = Main.npc[reader.ReadInt32()];
				if (Main.dedServ)
				{
					var elius = npc.ModNPC as LordElius;
					var player = Main.player[whoAmI];
					if (!spared) // kill
					{
						ChatHelper.BroadcastChatMessage(this.CustomLocalization($"{elius.LocalizationCategory}.{elius.Name}.MPMessage", player.name, this.CustomLocalization("RandomWords.Kill").Value.ToLower()).ToNetworkText(), Color.Purple);
						ChatHelper.BroadcastChatMessage(elius.GetLocalization("Killed").ToNetworkText(), Color.Purple);
					}
					else
					{
						ChatHelper.BroadcastChatMessage(this.CustomLocalization($"{elius.LocalizationCategory}.{elius.Name}.MPMessage", player.name, this.CustomLocalization("RandomWords.Spare").Value.ToLower()).ToNetworkText(), new(0, 183, 255));
						ChatHelper.BroadcastChatMessage(elius.GetLocalization("Spared").ToNetworkText(), new(0, 183, 255));
					}

					var packet = GetPacket();
					packet.Write(PacketID.EliusSpare);
					packet.Write(spared);
					packet.Write(npc.whoAmI);
					packet.Send();

					npc.NPCLoot();
				}
				else
				{
					var hitbox = npc.Hitbox;
					if (!spared) // kill
					{
						// gore goes here
						for (int n = 0; n < 17; n++)
						{
							Dust.NewDust(hitbox.Center(), 0, 0, DustID.Blood, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f);
						}
					}
					else
					{
						for (int n = 0; n < 17; n++)
						{
							Dust.NewDust(hitbox.Center(), 0, 0, DustID.Smoke, (Main.rand.NextFloat() - 0.5f) * 3f, (Main.rand.NextFloat() - 0.5f) * 8f, 255 / 2);
						}
					}
				}
			}
			else if (command == PacketID.SpawnEliusArena)
			{
				if (Main.dedServ)
				{
					WorldGenStuff.SpawnEliusArena();
					NetMessage.SendData(MessageID.WorldData);
				}
			}
		}


		[ReinitializeDuringResizeArrays]
		public class Sets : ModSystem
		{
			public static bool[] excludedItem = ItemID.Sets.Factory.CreateBoolSet();

			public static bool[] excludedProjectile = ProjectileID.Sets.Factory.CreateBoolSet();

			public static bool[] OldWeapon = ItemID.Sets.Factory.CreateBoolSet();

			public static List<int>[] Mutations = ItemID.Sets.Factory.CreateCustomSet<List<int>>(null);
			public override void ResizeArrays()
			{
				// manually change default value
				for (int i = 0; i < Mutations.Length; i++)
				{
					Mutations[i] = [];
				}
			}

			public static int[] SizeStats = ItemID.Sets.Factory.CreateIntSet(0,
				ItemID.MoltenBreastplate, 7,
				ItemID.MoltenGreaves, 5,
				ItemID.MoltenHelmet, 3
			);

			public static bool[] toggleablePulse = ItemID.Sets.Factory.CreateBoolSet();

			public static int[] HasteStats = ItemID.Sets.Factory.CreateIntSet(0,
				ItemID.NecroBreastplate, 7,
				ItemID.NecroGreaves, 5,
				ItemID.NecroHelmet, 3,
				ItemID.AncientNecroHelmet, 3
			);

			/// <summary>
			/// Leave null for neutral, true for cold, false for hot
			/// </summary>
			public static bool?[] cold = ItemID.Sets.Factory.CreateCustomSet<bool?>(null,
				ItemID.IceSickle, true,
				ItemID.IceBlade, true,
				ItemID.Frostbrand, true,
				ItemID.ChristmasTreeSword, true,
				ItemID.NorthPole, true,
				ItemID.Snowball, true,
				ItemID.SnowballCannon, true,
				ItemID.FrostDaggerfish, true,
				ItemID.IceBow, true,
				ItemID.IceBoomerang, true,
				ItemID.Flairon, true,
				ItemID.ElfMelter, true,
				ItemID.Tsunami, true,

				ItemID.DD2SquireBetsySword, false,
				ItemID.DD2SquireDemonSword, false,
				ItemID.ShadowFlameKnife, false,
				ItemID.FieryGreatsword, false,
				ItemID.Flamarang, false,
				ItemID.Sunfury, false,
				ItemID.FlamingMace, false,
				ItemID.DayBreak, false,
				ItemID.MoltenFury, false,
				ItemID.HellwingBow, false,
				ItemID.ShadowFlameBow, false,
				ItemID.SolarEruption, false,
				ItemID.MolotovCocktail, false,
				ItemID.PhoenixBlaster, false,
				ItemID.Flamethrower, false,
				ItemID.BluePhaseblade, false,
				ItemID.DD2BetsyBow, false,
				ItemID.GreenPhaseblade, false,
				ItemID.OrangePhaseblade, false,
				ItemID.DD2PhoenixBow, false,
				ItemID.PurplePhaseblade, false,
				ItemID.RedPhaseblade, false,
				ItemID.WhitePhaseblade, false,
				ItemID.YellowPhaseblade, false,
				ItemID.GreenPhasesaber, false,
				ItemID.OrangePhasesaber, false,
				ItemID.PurplePhasesaber, false,
				ItemID.WhitePhasesaber, false,
				ItemID.YellowPhasesaber, false,
				ItemID.RedPhasesaber, false,
				ItemID.BluePhasesaber, false,
				ItemID.HelFire, false,
				ItemID.Amarok, false,
				ItemID.Cascade, false,
				ItemID.MoltenPickaxe, false,
				ItemID.SolarFlareDrill, false,
				ItemID.SolarFlarePickaxe, false,
				ItemID.MeteorHamaxe, false,
				ItemID.MoltenHamaxe, false,
				ItemID.LunarHamaxeSolar, false
			);

			public static WeaponType[] weaponType = ItemID.Sets.Factory.CreateCustomSet(WeaponType.Normal,
				ItemID.BreakerBlade, WeaponType.Strength,
				ItemID.Anchor, WeaponType.Strength,
				ItemID.Zenith, WeaponType.Artisinal
			);

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

			public static bool[] flail = ItemID.Sets.Factory.CreateBoolSet(ItemID.DripplerFlail, ItemID.Mace, ItemID.FlamingMace, ItemID.Flairon, ItemID.BallOHurt, ItemID.BlueMoon, ItemID.DaoofPow, ItemID.FlowerPow, ItemID.Sunfury, ItemID.TheMeatball); // PORT add other flairon

			public static int?[] baseImbues = ItemID.Sets.Factory.CreateCustomSet<int?>(null);

			public static bool[] atlanteanItem = ItemID.Sets.Factory.CreateBoolSet();

			public static bool[] shield = ItemID.Sets.Factory.CreateBoolSet();

			public static bool[] showItemTypeTooltip = ItemID.Sets.Factory.CreateBoolSet(true);

			public static bool[] imbueEffect = ProjectileID.Sets.Factory.CreateBoolSet();

			public static bool[] woodWand = ItemID.Sets.Factory.CreateBoolSet(ItemID.LivingWoodWand, ItemID.LeafWand, ItemID.LivingMahoganyLeafWand, ItemID.LivingMahoganyWand);

			public static int[] wandWoodType = ItemID.Sets.Factory.CreateIntSet(-1, ItemID.LivingWoodWand, ItemID.Wood, ItemID.LeafWand, ItemID.Wood, ItemID.LivingMahoganyLeafWand, ItemID.RichMahogany, ItemID.LivingMahoganyWand, ItemID.RichMahogany);

			[ReinitializeDuringResizeArrays]
			public static class Assets
			{

				public static Asset<Texture2D>[] annihilationSprites = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

				public static Asset<Texture2D>[] raySprites = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

				public static Asset<Texture2D>[] rayEndSprites = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

				public static Asset<Texture2D>[] rayStartSprites = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

				public static Asset<Texture2D>[] blasts = ItemID.Sets.Factory.CreateCustomSet<Asset<Texture2D>>(null);

				public static Dictionary<string, Asset<Texture2D>> MagicCircles = [];
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
		public override void PostUpdateEverything()
		{
			foreach (string message in ArcaneOdysseyMod.NoticeQueue)
			{
				Main.NewText(message, Color.Yellow);
			}
			ArcaneOdysseyMod.NoticeQueue = [];
		}
	}

	public class WeaponsLoader : ModSystem
	{
		internal static bool InArray(int i)
		{
			return ItemID.Sets.Deprecated[i] || ArcaneOdysseyMod.Sets.claw[i] || ArcaneOdysseyMod.Sets.spear[i] || ArcaneOdysseyMod.Sets.dualbladed[i] || ArcaneOdysseyMod.Sets.greatsword[i] || ArcaneOdysseyMod.Sets.dagger[i] || ArcaneOdysseyMod.Sets.staff[i] || ArcaneOdysseyMod.Sets.rapier[i] || ArcaneOdysseyMod.Sets.greathammer[i] || ItemID.Sets.Yoyo[i] || ArcaneOdysseyMod.Sets.greataxe[i] || ArcaneOdysseyMod.Sets.flail[i];
		}

		public override void SetStaticDefaults()
		{
			ExternalModSupport.SetItemAttributes();
		}

		public override void PostSetupRecipes()
		{
			ArcaneOdysseyMod.finishedLoading = true;

			for (int i = 0; i < ItemLoader.ItemCount; i++)
			{
				if (!InArray(i))
				{
					var item = new Item(i);

					if (item.shieldSlot != -1)
					{
						ArcaneOdysseyMod.Sets.shield[i] = true;
					}

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
}
