using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Imbues.Magic.Mythical;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace ArcaneOdyssey.Imbues.FightingStyles.Mythical
{
	public sealed class GodFist : FightingStyle, IBarrableImbue
	{
		public override string Texture => AOUtils.GetTexture<SailorStyle>();
		public override bool ImmuneDash => true;
		public override float ScrollDamage => MatrixMagic.MatrixDamage;
		public override float ScrollSize => MatrixMagic.MatrixSize;
		public override float ScrollSpeed => MatrixMagic.MatrixSpeed;
		public override ImbuableTiers ImbuableTier => ImbuableTiers.Mythical;

		public override Debuff[] ImbueDebuffs => MatrixMagic.MatrixDebuffs;
		public override Combo[] CombinedDebuffs => MatrixMagic.MatrixCombos;
		public override SynergyEffects Effects => MatrixMagic.MatrixEffects;
		public override Color ImbueColour => new(255, 224, 228);
		private float barValue;

		public const float BarMax = BarGimmick.BarMax;
		public const float BarMin = BarGimmick.BarMin;
		public float BarValue { get => barValue.Clamp(BarMin, BarMax); set => barValue = value.Clamp(BarMin, BarMax); }
		public override ImbueGimmick Gimmick => Bar ?? MatrixMagic.MatrixGimmick;
		public BarGimmick Bar => MatrixMagic.MatrixGimmick is BarGimmick gimmick ? gimmick : null;
		public float LerpValue => Bar is null ? 0f : MathHelper.Clamp(BarValue * Bar.BarValueMulti / BarMax, 0f, 1f);

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (Bar is not null)
				spriteBatch.DrawString(FontAssets.ItemStack.Value, $"{BarValue.Round()}%", position, Color.Lerp(ImbueColour2, ImbueColour, LerpValue), 0f, FontAssets.ItemStack.Value.MeasureString($"{BarValue.Round()}%") / 2f, Main.inventoryScale, SpriteEffects.None, 1f);
		}
	}
}
