using ArcaneOdyssey.MainMenu;
using ArcaneOdyssey.UI.SubWorld;
using ReLogic.Graphics;
using SubworldLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Subworlds.Base
{
	public abstract class AOSubworld : Subworld
	{
		public const int MaxSeed = 2147483647;
		public override void DrawMenu(GameTime gameTime)
		{
			Main.spriteBatch.DrawString(FontAssets.DeathText.Value, message, new Vector2(Main.screenWidth, Main.screenHeight) / 2f - FontAssets.DeathText.Value.MeasureString(message) / 2f, Color.Black, 0, default, 1f, SpriteEffects.None, 0);
			Main.spriteBatch.DrawString(FontAssets.DeathText.Value, message, new Vector2(Main.screenWidth, Main.screenHeight) / 2f - FontAssets.DeathText.Value.MeasureString(message) / 2f, Color.White, 0, default, 1f, SpriteEffects.None, 0);
		}

		public abstract ModBiome Biome { get; }

		public override void DrawSetup(GameTime gameTime)
		{
			PlayerInput.SetZoom_UI();
			Main.instance.GraphicsDevice.Clear(Color.Black);
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.UIScaleMatrix);
			ArcaneOdysseyMainMenu.DrawAOBackground(Main.spriteBatch);
			DrawMenu(gameTime);
			//Main.DrawCursor(Main.DrawThickCursor());
			Main.spriteBatch.End();
		}

		public virtual ArcaneOdysseyMainMenu.MainMenuStyle? MenuStyle => null;

		public static string message = string.Empty;

		public override void OnEnter()
		{
			ArcaneOdysseyMainMenu.RandomSelect();
			SubworldSystem.noReturn = true;
			SubworldSystem.hideUnderworld = true;
			message = ArcaneOdysseyMod.Instance.CustomLocalization($"{nameof(AOSubworld)}.EnterMessage", DisplayName.Value).Value;
		}

		public override void OnExit()
		{
			message = ArcaneOdysseyMod.Instance.CustomLocalization($"{nameof(AOSubworld)}.ExitMessage").Value;
		}

		public override void CopyMainWorldData()
		{
			TagCompound downed = [];
			if (DownedBossesInfo is not null)
			{
				for (int i = 0; i < DownedBossesInfo.Length; i++)
				{
					downed.Add(DownedBossesInfo[i].Name, DownedBossesInfo[i].GetGetMethod()?.Invoke(null, []) is bool down && down);
				}
				SubworldSystem.CopyWorldData("DownedAOBosses", downed);
			}
		}

		public override void ReadCopiedMainWorldData()
		{
			if (DownedBossesInfo is not null)
			{
				var downed = SubworldSystem.ReadCopiedWorldData<TagCompound>("DownedAOBosses");
				if (downed != null && downed.Count != 0)
				{
					for (int i = 0; i < DownedBossesInfo.Length; i++)
					{
						string Key = DownedBossesInfo[i].Name;
						if (downed.TryGet(Key, out bool down))
						{
							DownedBossesInfo[i].GetSetMethod()?.Invoke(null, [down]);
						}
					}
				}
			}
		}

		public override void Load()
		{
			ModTypeLookup<AOSubworld>.Register(this);
			AOSubworldSystem.Count++;
		}

		public override void Unload()
		{
			DownedBossesInfo = null;
		}

		public abstract bool MetConditions { get; }

		public static AOSubworld[] UnlockedSubworlds => [.. AOSubworldSystem.AllSubworlds.Where(e => e.MetConditions)];

		/// <summary>
		/// Small thingy, mainly used by <see cref="SubWorldUI"/>
		/// </summary>
		public static AOSubworld[] ALLSUBWORLDS => AOSubworldSystem.AllSubworlds;

		/// <summary>
		/// use <seealso cref="Before{T}"/> or <seealso cref="After{T}"/>
		/// </summary>
		public abstract byte OrderNum { get; }

		public static PropertyInfo[] DownedBossesInfo;

		public override void SetStaticDefaults()
		{
			AOSubworldSystem.AllSubworlds[OrderNum] = this;
			DownedBossesInfo = typeof(DownedBosses).GetProperties(BindingFlags.Static | BindingFlags.Public).Where(e => e.GetValue(null) is bool).ToArray();
			_ = DisplayName; // forces these to generate if they don't exist
			_ = Description;
			_ = UnlockCondition;
			ArcaneOdysseyMod.Instance.CoolCustomLocalization($"{nameof(AOSubworld)}.EnterMessage", "Traveling to {0}...");
			ArcaneOdysseyMod.Instance.CoolCustomLocalization($"{nameof(AOSubworld)}.ExitMessage", "Returning...");
		}

		public virtual LocalizedText Description => this.GetLocalization("Description", ToString);
		public virtual LocalizedText UnlockCondition => this.GetLocalization("UnlockCondition", () => "No Conditions.");

		public virtual bool ShowInUI => true;

		public override string ToString()
		{
			return $"{{ {nameof(Name)}: {Name}, {nameof(Mod)}: {Mod.Name}, {nameof(Biome)}: {Biome.Name}, {nameof(ShowInUI)}: {ShowInUI}, {nameof(MenuStyle)}: {MenuStyle?.Name} }}";
		}

		/// <summary>
		/// Gets a subworld by number
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public static AOSubworld Get(byte page)
		{
			var pages = ModContent.GetContent<AOSubworld>();
			return pages.ToList().Find(e => e.OrderNum == page);
		}

		/// <summary>
		/// Gets a subworld by internal name
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public static AOSubworld Get(string page)
		{
			var pages = ModContent.GetContent<AOSubworld>();
			return pages.ToList().Find(e => e.FullName == page);
		}

		internal static Dictionary<string, byte> Ordered = [];

		/// <summary>
		/// Gets the page number before the provided page
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public byte Before<T>() where T : AOSubworld
		{
			var inst = ModContent.GetInstance<T>();
			if (!Ordered.ContainsKey(Name))
			{
				if (Ordered.TryGetValue(inst.Name, out var value))
				{
					Ordered[Name] = value;
				}
				else
				{
					Ordered[Name] = inst.OrderNum;
				}
				var keys = Ordered.Keys;
				foreach (var val in keys)
				{
					if (Ordered[val] >= Ordered[Name] && val != Name)
					{
						Ordered[val]++;
					}
				}
			}
			return Ordered[Name];
		}

		/// <summary>
		/// Gets the page number after the provided page
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public byte After<T>() where T : AOSubworld
		{
			if (!Ordered.ContainsKey(Name))
			{
				var inst = ModContent.GetInstance<T>();
				if (Ordered.TryGetValue(inst.Name, out var value))
				{
					Ordered[Name] = (byte)(value + 1);
				}
				else
				{
					Ordered[Name] = (byte)(inst.OrderNum + 1);
				}
				var keys = Ordered.Keys;
				foreach (var val in keys)
				{
					if (Ordered[val] >= Ordered[Name] && val != Name)
					{
						Ordered[val]++;
					}
				}
			}
			return Ordered[Name];
		}
	}

	public class AOSubworldSystem : ModSystem
	{
		public static int Count = 0;

		internal static AOSubworld[] AllSubworlds = [];

		public override void PostSetupContent()
		{
			foreach (var page in AOSubworld.Ordered.Keys)
			{
				AllSubworlds[AOSubworld.Get(page).OrderNum] = AOSubworld.Get(page);
			}

			foreach (var subworld in AllSubworlds)
			{
				if (subworld.MenuStyle.HasValue)
				{
					ArcaneOdysseyMainMenu.Titles.Add(subworld.MenuStyle.Value);
				}
			}
		}

		public override void ResizeArrays()
		{
			AllSubworlds = Sets.Factory.CreateCustomSet<AOSubworld>(null);
		}

		[ReinitializeDuringResizeArrays]
		public static class Sets
		{
			public static SetFactory Factory = new(Count, nameof(AOSubworld), i => AOSubworld.Get((byte)i).Name);
		}
	}
}
