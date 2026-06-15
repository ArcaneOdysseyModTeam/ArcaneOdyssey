using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Imbues.Base;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Gimmicks.Bars
{
	public sealed class ThermoBar : BarGimmick
	{
		public override float BarValueMulti => 1f;

		public override float MaxImbueSpeed => 1.3f;
		public override float MaxImbueDamage => .85f;
		public override float MaxImbueSize => .833f;
		public override float MinImbueSpeed => 1f;
		public override float MinImbueDamage => .85f;
		public override float MinImbueSize => .833f;
		public override float MaxScrollSpeed => 1.3f;
		public override float MaxScrollDamage => .75f;
		public override float MaxScrollSize => .8f;
		public override float MinScrollSpeed => 1f;
		public override float MinScrollDamage => .75f;
		public override float MinScrollSize => .8f;

		public override void Update(Item item)
		{
			(item.ModItem as IBarrableImbue).BarValue = BarMin;
		}

		public override void UpdateInventory(Player player)
		{
			if (player.HasTypeInInventory<ModItem>(out var imbue, e => e is IBarrableImbue barrable && barrable.Bar?.Type == Type) && imbue is IBarrableImbue thermo)
			{
				if (!player.ArcaneOdyssey().OnCooldown(thermo.Name))
					thermo.BarValue -= BarMax / (BarMax * .6f * (BarMax / 10f));
			}
		}

		public override void UseAnimation(Item item, Player player)
		{
			if (item.Imbue() is IBarrableImbue thermo && thermo.Bar?.Type == Type)
			{
				thermo.BarValue += BarMax / 20f;
				player.ArcaneOdyssey().SetCooldown(new Cooldown(thermo.Name, thermo.Mod, item.Name, 60));
			}
		}
	}
}
