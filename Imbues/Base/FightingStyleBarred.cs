using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Imbues.Base
{
	public abstract class FightingStyleBarred : FightingStyle
	{
		public override void Load()
		{
			base.Load();
			ModTypeLookup<FightingStyleBarred>.Register(this);
		}

		public const float BarMax = 100f;
		public const float BarMin = 0f;

		public float BarValue { get; set; } = BarMin;

		public abstract Color DisplayColor { get; }

		public abstract float MaxImbueSpeed { get; }
		public abstract float MaxImbueDamage { get; }
		public abstract float MaxImbueSize { get; }
		public abstract float MinImbueSpeed { get; }
		public abstract float MinImbueDamage { get; }
		public abstract float MinImbueSize { get; }
		public abstract float MaxScrollSpeed { get; }
		public abstract float MaxScrollDamage { get; }
		public abstract float MaxScrollSize { get; }
		public abstract float MinScrollSpeed { get; }
		public abstract float MinScrollDamage { get; }
		public abstract float MinScrollSize { get; }

		public abstract float BarValueMulti { get; }

		public virtual bool SaveBar => false;

		public float LerpValue => MathHelper.Clamp(BarValue * BarValueMulti / BarMax, 0f, 1f);

		public sealed override float ImbueDamage { get => MathHelper.Lerp(MinImbueDamage, MaxImbueDamage, LerpValue); }
		public sealed override float ScrollDamage { get => MathHelper.Lerp(MinScrollDamage, MaxScrollDamage, LerpValue); }
		public sealed override float ImbueSpeed { get => MathHelper.Lerp(MinImbueSpeed, MaxImbueSpeed, LerpValue); }
		public sealed override float ScrollSpeed { get => MathHelper.Lerp(MinScrollSpeed, MaxScrollSpeed, LerpValue); }
		public sealed override float ImbueSize { get => MathHelper.Lerp(MinImbueSize, MaxImbueSize, LerpValue); }
		public sealed override float ScrollSize { get => MathHelper.Lerp(MinScrollSize, MaxScrollSize, LerpValue); }

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.DrawString(FontAssets.ItemStack.Value, $"{BarValue.Round()}%", position, Color.Lerp(DisplayColor, ImbueColour, LerpValue), 0f, FontAssets.ItemStack.Value.MeasureString($"{BarValue.Round()}%") / 2f, Main.inventoryScale, SpriteEffects.None, 1f);
		}

		public override void SaveData(TagCompound tag)
		{
			base.SaveData(tag);
			if (SaveBar && BarValue > 1)
				tag.Add("bar", (byte)BarValue);
		}

		public override void LoadData(TagCompound tag)
		{
			base.LoadData(tag);
			BarValue = tag.GetByte("bar");
		}
	}
}
