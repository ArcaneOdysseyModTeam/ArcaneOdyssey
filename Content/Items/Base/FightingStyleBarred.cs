using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Base
{
    public abstract class FightingStyleBarred : FightingStyle
    {
		public const int BarMax = 100;
		public const int BarMin = 0;

		private float _barValue = BarMin;
		public float BarValue { get => _barValue; set => _barValue = MathHelper.Clamp(value, BarMin, BarMax); }

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

		public override float AOImbueDamage { get => MathHelper.Lerp(MinImbueDamage, MaxImbueDamage, BarValue / 100f); }
		public override float AOScrollDamage { get => MathHelper.Lerp(MinScrollDamage, MaxScrollDamage, BarValue / 100f); }
		public override float AOImbueSpeed { get => MathHelper.Lerp(MinImbueSpeed, MaxImbueSpeed, BarValue / 100f); }
		public override float AOScrollSpeed { get => MathHelper.Lerp(MinScrollSpeed, MaxScrollSpeed, BarValue / 100f); }
		public override float AOImbueSize { get => MathHelper.Lerp(MinImbueSize, MaxImbueSize, BarValue / 100f); }
		public override float AOScrollSize { get => MathHelper.Lerp(MinScrollSize, MaxScrollSize, BarValue / 100f); }

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			spriteBatch.DrawString(FontAssets.ItemStack.Value, $"{BarValue.Round()}%", position - (FontAssets.ItemStack.Value.MeasureString($"{BarValue.Round()}%") / 2), DisplayColor);
		}

		public override void UpdateInventory(Player player)
		{
			if (player.Imbue() is FightingStyleBarred fs && player.Imbue().Name == Name)
				BarValue = fs.BarValue;
			base.UpdateInventory(player);
		}
	}

	public class ImbueBarDisplay : GlobalItem
	{
		public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (item.ArcaneOdyssey().imbue is FightingStyleBarred fs && ImbueClassCheck(item) && !item.ArcaneOdyssey().Arcanium.GetValueOrDefault(false) && item.ModItem is not MagicScroll)
				spriteBatch.DrawString(FontAssets.ItemStack.Value, $"{fs.BarValue.Round()}%", position - (FontAssets.ItemStack.Value.MeasureString($"{fs.BarValue.Round()}%") / 2), fs.DisplayColor);
		}
	}
}
