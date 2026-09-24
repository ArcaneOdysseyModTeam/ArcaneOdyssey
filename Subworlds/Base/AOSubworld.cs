using ArcaneOdyssey.MainMenu;
using ReLogic.Graphics;
using SubworldLibrary;
using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent;
using Terraria.GameInput;

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

		public static string message = string.Empty;

		public override void OnEnter()
		{
			ArcaneOdysseyMainMenu.RandomSelect();
			SubworldSystem.noReturn = true;
			SubworldSystem.hideUnderworld = true;
			message = $"Traveling to {DisplayName}...";
		}

		public override void OnExit()
		{
			message = "Returning...";
		}

		public override void Load()
		{
			ModTypeLookup<AOSubworld>.Register(this);
			AOSubworldSystem.Count++;
			ModContent.RequestIfExists(Texture, out Image);
		}

		public abstract bool MetConditions();

		public static AOSubworld[] UnlockedSubworlds => [.. ModContent.GetContent<AOSubworld>().Where(e => e.MetConditions())];

		/// <summary>
		/// The file name of this type's texture file in the mod loader's file space.
		/// </summary>
		public virtual string Texture => (GetType().Namespace + "." + Name).Replace('.', '/');

		/// <summary>
		/// The icon of this subworld
		/// </summary>
		public Asset<Texture2D> Image;

		/// <summary>
		/// use <seealso cref="Before{T}"/> or <seealso cref="After{T}"/>
		/// </summary>
		public abstract ushort OrderNum { get; }

		public override void SetStaticDefaults()
		{
			AOSubworldSystem.AllPages[OrderNum] = this;
			_ = DisplayName;
			_ = Description; // forces this to generate if they don't exist
		}

		public virtual LocalizedText Description => this.GetLocalization("Description");

		/// <summary>
		/// Gets a guidebook page by page number
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public static AOSubworld Get(int page)
		{
			var pages = ModContent.GetContent<AOSubworld>();
			return pages.ToList().Find(e => e.OrderNum == page);
		}

		/// <summary>
		/// Gets a guidebook page by internal name
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public static AOSubworld Get(string page)
		{
			var pages = ModContent.GetContent<AOSubworld>();
			return pages.ToList().Find(e => e.Name == page);
		}

		internal static Dictionary<string, ushort> Ordered = [];

		/// <summary>
		/// Gets the page number before the provided page
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public ushort Before<T>() where T : AOSubworld
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
		public ushort After<T>() where T : AOSubworld
		{
			if (!Ordered.ContainsKey(Name))
			{
				var inst = ModContent.GetInstance<T>();
				if (Ordered.TryGetValue(inst.Name, out var value))
				{
					Ordered[Name] = (ushort)(value + 1);
				}
				else
				{
					Ordered[Name] = (ushort)(inst.OrderNum + 1);
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

		public static AOSubworld[] AllPages = [];

		public override void PostSetupContent()
		{
			foreach (var page in AOSubworld.Ordered.Keys)
			{
				AllPages[AOSubworld.Get(page).OrderNum] = AOSubworld.Get(page);
			}
		}

		public override void ResizeArrays()
		{
			AllPages = Sets.Factory.CreateCustomSet<AOSubworld>(null);
		}

		[ReinitializeDuringResizeArrays]
		public static class Sets
		{
			public static SetFactory Factory = new (Count, nameof(AOSubworld), i => AOSubworld.Get(i).Name);
		}
	}
}
