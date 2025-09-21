using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Items.Materials;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey
{
	public class ItemManager : GlobalItem
	{
		public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (item.ModItem is AOWeapon weap)
			{
				if (weap.WeaponDebuff is not null && (weap.WeaponDebuff.DebuffPercent is null or 0 || modifiers.GetDamage(item.damage, true) > (target.lifeMax / weap.WeaponDebuff.DebuffPercent)))
				{
					target.AddBuff(weap.WeaponDebuff.debuffID, weap.WeaponDebuff.debuffDuration);
				}
			}

			if (item.TryGetImbue(out AOMagic imbue))
			{
				if (imbue is CrystalMagic && target.HasBuff<Crystallized>() && Crystallized.GetCrystalStack(target, target.FindBuffIndex(ModContent.BuffType<Crystallized>())) == 4)
				{
					modifiers.FinalDamage += .3f;
				}

				if ((imbue.MagicDebuff is not null) && (imbue.MagicDebuff.DebuffPercent != 0f))
				{
					if (imbue.MagicDebuff.DebuffPercent is null || modifiers.GetDamage(item.damage, true) > (target.lifeMax / imbue.MagicDebuff.DebuffPercent))
					{
						target.AddBuff(imbue.MagicDebuff.debuffID, imbue.MagicDebuff.debuffDuration);
					}
				}
				if ((imbue.MagicDebuff2 is not null) && (imbue.MagicDebuff2.DebuffPercent != 0f))
				{
					if (imbue.MagicDebuff2.DebuffPercent is null || modifiers.GetDamage(item.damage, true) > (target.lifeMax / imbue.MagicDebuff2.DebuffPercent))
					{
						target.AddBuff(imbue.MagicDebuff2.debuffID, imbue.MagicDebuff2.debuffDuration);
					}
				}

				if (imbue.CombinedDebuffs is not null)
				{
					foreach (CombinedDebuff buffkeys in imbue.CombinedDebuffs)
					{
						if (target.HasBuff(buffkeys.requirement) || (buffkeys.requirement == BuffID.Wet && target.wet))
						{
							target.AddBuff(buffkeys.result, buffkeys.duration);
						}
					}
				}

				foreach (MagicBuffMultiplier multiplier in imbue.Effects.magicBuffMultipliers)
				{
					if (target.HasBuff(multiplier.buffID) || (multiplier.buffID == BuffID.Wet && target.wet))
					{
						modifiers.FinalDamage += multiplier.multiplier.MultiToPercent();
					}
				}

				if (Main.netMode == NetmodeID.SinglePlayer) // things would get chaotic in multiplayer if everyone kept clearing eachothers debuffs
				{
					foreach (int buffid in imbue.Effects.clearBuffs)
					{
						if (target.HasBuff(buffid))
						{
							target.DelBuff(target.FindBuffIndex(buffid));
						}

					}
				}
			}
		}

		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			if (item.type == ItemID.OceanCrateHard)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ArcaniumScrap>(), 15));
			}
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Acrimony>(), 6000));
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.ArcaneOdyssey().owner is not null)
				if (ImbueClassCheck(item))
				{
					string imbuetextthing = Mod.CustomLocalization("ImbueStuff.NoneText").Value;
					if (item.TryGetImbue(out AOMagic imbue) && imbue is not SteamImbue)
					{
						imbuetextthing = imbue.DisplayName.Value;
					}
					else if (item.TryGetImbue(out AOMagic imbue1) && imbue1 is SteamImbue)
					{
						imbuetextthing = Language.GetTextValue("RandomWorldName_Adjective.Steaming");
					}
					tooltips.Add(new TooltipLine(Mod, "ImbueText", Mod.CustomLocalization("ImbueStuff.ImbueTooltip", [imbuetextthing]).Value));
				}
		}

		public override void ModifyItemScale(Item item, Player player, ref float scale)
		{
			if (item.TryGetImbue(out AOMagic imbue))
			{
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					scale += aoWeapon.AOSize.MultiToPercent() + imbue.AOImbueSize.MultiToPercent() + player.ArcaneOdyssey().GetSizeMulti(item).MultiToPercent();
				}
				else if (item.ModItem is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					scale += imbue.AOImbueSize.MultiToPercent() + player.ArcaneOdyssey().GetSizeMulti(item).MultiToPercent();
				}
			}
		}

		public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
		{
			if (item.TryGetImbue(out AOMagic imbue))
			{
				float extrakbmulti = 1f;
				if (imbue is WindMagic)
				{
					extrakbmulti = 3f;
                }

                if (item.ModItem is not null && item.ModItem.GetType().IsSubclassOf(typeof(DefaultScroll)))
                {
                    knockback += imbue.AOMagicSize.MultiToPercent() + extrakbmulti;
					return;
                }

                if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					knockback += aoWeapon.AOSize.MultiToPercent() + imbue.AOImbueSize.MultiToPercent() + extrakbmulti + player.ArcaneOdyssey().GetSizeMulti(item).MultiToPercent();
				}
				else if (item.ModItem is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					knockback += imbue.AOImbueSize.MultiToPercent() + extrakbmulti.MultiToPercent();
				}
			}
		}

		public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
		{
			if (item.ModItem is not null && item.ModItem.GetType().IsSubclassOf(typeof(DefaultScroll)))
			{
				damage += ((item.damage+(BossesKilled * 2f)) / item.damage)-1; // now it actually shows up on the scrolls damage, although it means nothing to a scroll
			}
			if (item.TryGetImbue(out AOMagic imbue))
            {
                if (item.ModItem is not null && item.ModItem.GetType().IsSubclassOf(typeof(DefaultScroll)))
                {
                    damage += imbue.AOMagicDamage.MultiToPercent();
					return;
                }

                if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					damage += aoWeapon.AODamage.MultiToPercent() + imbue.AOImbueDamage.MultiToPercent();
				}
				else if (item.ModItem is null) // do not touch items from other mods
				{
					damage += imbue.AOImbueDamage.MultiToPercent();
				}
			}
		}
		public override float UseSpeedMultiplier(Item item, Player player)
		{
			if (item.TryGetImbue(out AOMagic imbue) && item.DamageType != DamageClass.MeleeNoSpeed)
            {
                if (item.ModItem is not null && item.ModItem.GetType().IsSubclassOf(typeof(DefaultScroll)))
                {
                    return imbue.AOMagicSpeed;
                }

                if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					return aoWeapon.AOSpeed + imbue.AOImbueSpeed.MultiToPercent();
				}
				else if (item.ModItem is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
				{
					return imbue.AOImbueSpeed;
				}
			}
			return 1f;
		}
	}

	public class AOItem : GlobalItem
	{
		public override bool InstancePerEntity => true;

		public Player owner;
		public AOMagic imbue;

		public override void UpdateInventory(Item item, Player player)
		{
			owner = player;
			if (ImbueClassCheck(item))
			{
				imbue = player.ArcaneOdyssey().imbue;
				if ((item.ModItem is AOWeapon weapon && imbue is not null) && (weapon.ColdWeapon.HasValue && imbue.ColdMagic.HasValue) && (weapon.ColdWeapon.Value != imbue.ColdMagic.Value))
				{
					imbue = new SteamImbue() { originalImbue = imbue };
				}
			}
		}

		public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
		{
			owner = null;
			imbue = null;
		}
	}
}
