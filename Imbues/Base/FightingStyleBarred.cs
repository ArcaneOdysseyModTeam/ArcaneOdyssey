using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;

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

		public float LerpValue => MathHelper.Clamp(BarValue * BarValueMulti / BarMax, 0f, 1f);


		public override float AOImbueDamage { get => MathHelper.Lerp(MinImbueDamage, MaxImbueDamage, LerpValue); }
		public override float AOScrollDamage { get => MathHelper.Lerp(MinScrollDamage, MaxScrollDamage, LerpValue); }
		public override float AOImbueSpeed { get => MathHelper.Lerp(MinImbueSpeed, MaxImbueSpeed, LerpValue); }
		public override float AOScrollSpeed { get => MathHelper.Lerp(MinScrollSpeed, MaxScrollSpeed, LerpValue); }
		public override float AOImbueSize { get => MathHelper.Lerp(MinImbueSize, MaxImbueSize, LerpValue); }
		public override float AOScrollSize { get => MathHelper.Lerp(MinScrollSize, MaxScrollSize, LerpValue); }

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.DrawString(FontAssets.ItemStack.Value, $"{BarValue.Round()}%", position - (FontAssets.ItemStack.Value.MeasureString($"{BarValue.Round()}%") / 2), DisplayColor);
		}

		public override void UpdateInventory(Player player)
		{
			if (player.Imbue() is FightingStyleBarred fs1 && fs1.Name == Name)
			{
				BarValue = fs1.BarValue;
			}
			base.UpdateInventory(player);
		}
	}
}
