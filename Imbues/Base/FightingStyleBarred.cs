using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Imbues.Base
{
	public abstract class FightingStyleBarred : FightingStyle, IBarrableImbue
	{
		public override void Load()
		{
			base.Load();
			ModTypeLookup<FightingStyleBarred>.Register(this);
		}
		private float barValue = BarMin;

		public const float BarMax = BarGimmick.BarMax;
		public const float BarMin = BarGimmick.BarMin;
		public float BarValue { get => barValue.Clamp(BarMin, BarMax); set => barValue = value.Clamp(BarMin, BarMax); }
		public abstract Color DisplayColor { get; }

		public sealed override ImbueGimmick Gimmick => Bar;
		public abstract BarGimmick Bar { get; }

		public float LerpValue => MathHelper.Clamp(BarValue * Bar.BarValueMulti / BarMax, 0f, 1f);

		public sealed override float ImbueDamage { get => MathHelper.Lerp(Bar.MinImbueDamage, Bar.MaxImbueDamage, LerpValue); }
		public sealed override float ScrollDamage { get => MathHelper.Lerp(Bar.MinScrollDamage, Bar.MaxScrollDamage, LerpValue); }
		public sealed override float ImbueSpeed { get => MathHelper.Lerp(Bar.MinImbueSpeed, Bar.MaxImbueSpeed, LerpValue); }
		public sealed override float ScrollSpeed { get => MathHelper.Lerp(Bar.MinScrollSpeed, Bar.MaxScrollSpeed, LerpValue); }
		public sealed override float ImbueSize { get => MathHelper.Lerp(Bar.MinImbueSize, Bar.MaxImbueSize, LerpValue); }
		public sealed override float ScrollSize { get => MathHelper.Lerp(Bar.MinScrollSize, Bar.MaxScrollSize, LerpValue); }

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.DrawString(FontAssets.ItemStack.Value, $"{BarValue.Round()}%", position, Color.Lerp(DisplayColor, ImbueColour, LerpValue), 0f, FontAssets.ItemStack.Value.MeasureString($"{BarValue.Round()}%") / 2f, Main.inventoryScale, SpriteEffects.None, 1f);
		}

		public override void SaveData(TagCompound tag)
		{
			base.SaveData(tag);
			if (Bar.SaveBar && BarValue > 1)
				tag.Add("bar", (byte)BarValue);
		}

		public override void LoadData(TagCompound tag)
		{
			base.LoadData(tag);
			BarValue = tag.GetByte("bar");
		}
	}
}
