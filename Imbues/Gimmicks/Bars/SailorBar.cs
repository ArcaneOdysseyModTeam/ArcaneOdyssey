using ArcaneOdyssey.Imbues.Base;

namespace ArcaneOdyssey.Imbues.Gimmicks.Bars
{
	public sealed class SailorBar : BarGimmick
	{
		public override bool SaveBar => true;

		public override float BarValueMulti => 1.25f;
		public override float MaxScrollSpeed => 1f;
		public override float MaxScrollDamage => .85f;
		public override float MaxScrollSize => 1.2f;
		public override float MinScrollSpeed => 1f;
		public override float MinScrollDamage => .775f;
		public override float MinScrollSize => .8f;

		public override void UseAnimation(Item item, Player player)
		{
			if (item.Imbue() is IBarrableImbue sailor && sailor.Bar?.Type == Type)
			{
				sailor.BarValue -= FightingStyleBarred.BarMax / 100f;
			}
		}

		public override void OnConsumeItem(Item item, Player player)
		{
			if (item.potion)
			{
				if (player.Imbue() is IBarrableImbue sailor && sailor.Bar?.Type == Type)
				{
					sailor.BarValue = FightingStyleBarred.BarMax;
				}
			}
		}

		public override void UpdateInventory(Player player)
		{
			if (player.HasTypeInInventory<ModItem>(out var imbue, e => e is IBarrableImbue barrable && barrable.Bar?.Type == Type) && imbue is IBarrableImbue sailor)
			{
				if (player.wet && !player.honeyWet && !player.lavaWet)
				{
					sailor.BarValue += BarMax / (BarMax * .6f * 2.5f);
				}
			}
		}
	}
}
