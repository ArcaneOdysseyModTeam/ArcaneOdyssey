using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Imbues.Base
{
	public abstract class FightingStyleBarred : FightingStyle
	{
		public const float BarMax = 100f;
		public const float BarMin = 0f;

		protected float _barValue = BarMin;
		public float BarValue { get => UpdateBar(); set => UpdateBar(value); }

		/// <summary>
		/// Allows extra stuff to happen when the bar value changes or is requested
		/// </summary>
		/// <param name="value">The new bar value, if any</param>
		/// <returns>The bar, after any changes</returns>
		public virtual float UpdateBar(float? value = null)
		{
			if (value.HasValue)
				_barValue = MathHelper.Clamp(value.Value, BarMin, BarMax);
			return _barValue;
		}

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

		public virtual bool SaveBar => true;

		public float LerpValue => MathHelper.Clamp(BarValue * BarValueMulti / BarMax, 0f, 1f);


		public override float ImbueDamage { get => MathHelper.Lerp(MinImbueDamage, MaxImbueDamage, LerpValue); }
		public override float ScrollDamage { get => MathHelper.Lerp(MinScrollDamage, MaxScrollDamage, LerpValue); }
		public override float ImbueSpeed { get => MathHelper.Lerp(MinImbueSpeed, MaxImbueSpeed, LerpValue); }
		public override float ScrollSpeed { get => MathHelper.Lerp(MinScrollSpeed, MaxScrollSpeed, LerpValue); }
		public override float ImbueSize { get => MathHelper.Lerp(MinImbueSize, MaxImbueSize, LerpValue); }
		public override float ScrollSize { get => MathHelper.Lerp(MinScrollSize, MaxScrollSize, LerpValue); }

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.DrawString(FontAssets.ItemStack.Value, $"{BarValue.Round()}%", position - (FontAssets.ItemStack.Value.MeasureString($"{BarValue.Round()}%") * Main.inventoryScale / 2f), Color.Lerp(DisplayColor, ImbueColour, LerpValue));
		}

		public override void UpdateInventory(Player player)
		{
			base.UpdateInventory(player);
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
