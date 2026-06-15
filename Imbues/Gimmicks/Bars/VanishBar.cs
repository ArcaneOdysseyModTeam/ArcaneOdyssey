using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Imbues.Gimmicks.Bars
{
	public sealed class VanishBar : BarGimmick
	{
		public override float BarValueMulti => 1.111f;
		public override float MinScrollSize => !VanishingStyle.HasYou ? 1.0f : 1.125f;
		public override float MinScrollSpeed => MinScrollSpeed;
		public override float MinScrollDamage => MaxScrollDamage;
		public override float MaxScrollSpeed => !VanishingStyle.HasYou ? 1.1f : 1.5f;
		public override float MaxScrollDamage => !VanishingStyle.HasYou ? .85f : 1f;
		public override float MaxScrollSize => !VanishingStyle.HasYou ? 1.056f : 1.2f;

		public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
		{
			projectile.CritChance = projectile.OriginalCritChance;
			if (projectile.TryGetOwner(out var player) && player.HasTypeInInventory<ModItem>(out var imbue, e => e is IBarrableImbue barrable && barrable.Bar?.Type == Type) && imbue is IBarrableImbue vanish)
			{
				if (target.boss || !AOUtils.BossAlive)
				{
					player.ArcaneOdyssey()?.SetCooldown(new Cooldown(vanish.Name, imbue.DisplayName, 60));
					if (target.boss)
						vanish.BarValue += damageDone / (target.lifeMax / 10f) * FightingStyleBarred.BarMax;
					else
						vanish.BarValue += damageDone / (target.lifeMax * 2f) * FightingStyleBarred.BarMax;
				}
			}
		}

		public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
		{
			if (!player.ArcaneOdyssey().OnCooldown(nameof(VanishingStyle)))
				crit = 100;
		}

		public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (player.HasTypeInInventory<ModItem>(out var imbue, e => e is IBarrableImbue barrable && barrable.Bar?.Type == Type) && imbue is IBarrableImbue vanish)
			{
				if (target.boss || !AOUtils.BossAlive)
				{
					player.ArcaneOdyssey()?.SetCooldown(new Cooldown(vanish.Name, imbue.DisplayName, 60));
					if (target.boss)
						vanish.BarValue += damageDone / (target.lifeMax / 10f) * FightingStyleBarred.BarMax;
					else
						vanish.BarValue += damageDone / (target.lifeMax * 2f) * FightingStyleBarred.BarMax;
				}
			}
		}

		public override void NoInventoryEffects(Item item, Player player)
		{
			foreach (var floatingItem in Main.ActiveItems)
			{
				if (floatingItem.ModItem is IBarrableImbue vanish && vanish.Bar?.Type == Type)
				{
					vanish.BarValue = BarMin;
				}
			}
		}

		public override void InventoryEffects(Item item, Player player)
		{
			if (item.ModItem is IBarrableImbue vanish && vanish.Bar?.Type == Type)
			{
				if (!player.ArcaneOdyssey().OnCooldown(Name))
					vanish.BarValue -= BarMax / (BarMax * .6f * (BarMax / 10f));
				if (vanish.BarValue <= BarMin)
				{
					player.ArcaneOdyssey().SetCooldown(new(vanish.Name, vanish.Mod, item.Name, 2));
				}
			}
		}
	}
}
