using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Items.Materials;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;
using static System.Net.Mime.MediaTypeNames;

namespace ArcaneOdyssey
{
	public class ItemManager : GlobalItem
	{
		public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
			AOPlayer playah = player.AOPlayer();
			if (item.ModItem is AOWeapon weap)
			{
				if (weap.WeaponDebuff is not null && (weap.WeaponDebuff.DebuffPercent is null or 0 || modifiers.GetDamage(item.damage, true) > (target.lifeMax / weap.WeaponDebuff.DebuffPercent)))
				{
					target.AddBuff(weap.WeaponDebuff.debuffID, weap.WeaponDebuff.debuffDuration);
				}
			}

			if (playah.imbue is not null)
			{
				if (playah.imbue is CrystalMagic && target.HasBuff<Crystallized>() && Crystallized.GetCrystalStack(target, target.FindBuffIndex(ModContent.BuffType<Crystallized>())) == 4)
				{
					modifiers.FinalDamage += .3f;
				}

				if ((playah.imbue.MagicDebuff is not null) && (playah.imbue.MagicDebuff.DebuffPercent != 0f))
				{
					if (playah.imbue.MagicDebuff.DebuffPercent is null || modifiers.GetDamage(item.damage, true) > (target.lifeMax / playah.imbue.MagicDebuff.DebuffPercent))
					{
						target.AddBuff(playah.imbue.MagicDebuff.debuffID, playah.imbue.MagicDebuff.debuffDuration);
					}
				}
				if ((playah.imbue.MagicDebuff2 is not null) && (playah.imbue.MagicDebuff2.DebuffPercent != 0f))
				{
					if (playah.imbue.MagicDebuff2.DebuffPercent is null || modifiers.GetDamage(item.damage, true) > (target.lifeMax / playah.imbue.MagicDebuff2.DebuffPercent))
					{
						target.AddBuff(playah.imbue.MagicDebuff2.debuffID, playah.imbue.MagicDebuff2.debuffDuration);
					}
				}

				if (playah.imbue.CombinedDebuffs is not null)
				{
					foreach (CombinedDebuff buffkeys in playah.imbue.CombinedDebuffs)
					{
						if (target.HasBuff(buffkeys.requirement) || (buffkeys.requirement == BuffID.Wet && target.wet))
						{
							target.AddBuff(buffkeys.result, buffkeys.duration);
						}
					}
				}

				foreach (MagicBuffMultiplier multiplier in playah.imbue.Effects.magicBuffMultipliers)
				{
					if (target.HasBuff(multiplier.buffID) || (multiplier.buffID == BuffID.Wet && target.wet))
					{
						modifiers.FinalDamage += multiplier.multiplier.MultiToPercent();
					}
				}

				if (Main.netMode == NetmodeID.SinglePlayer) // things would get chaotic in multiplayer if everyone kept clearing eachothers debuffs
				{
					foreach (int buffid in playah.imbue.Effects.clearBuffs)
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

		public override void UpdateInventory(Item item, Player player)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
		}

		/// <summary>
		/// used in singleplayer exclusively to display current imbue, might not work in multiplayer idk
		/// </summary>
		public static AOPlayer playerForImbue = null;

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (playerForImbue is not null)
				if (ImbueClassCheck(item))
				{
					string imbuetextthing = Mod.CustomLocalization("ImbueStuff.NoneText").Value;
					if (item.TryGetImbue(playerForImbue.Player, out AOMagic imbue))
						imbuetextthing = imbue.Item.Name;
					tooltips.Add(new TooltipLine(Mod, "ImbueText", Mod.CustomLocalization("ImbueStuff.ImbueTooltip", [imbuetextthing]).Value));
				}

			if (item.ModItem is AOMagic magical)
			{
				tooltips.Add(new TooltipLine(Mod, "MagicTier", Mod.CustomLocalization($"MagicTierLines.{magical.MagicTier}").Value));
			}
		}

		public override bool? UseItem(Item item, Player player)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
			return null;
		}

		public override void ModifyItemScale(Item item, Player player, ref float scale)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
			if (item.TryGetImbue(player, out AOMagic imbue))
			{
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					scale += aoWeapon.AOSize.MultiToPercent() + imbue.AOImbueSize.MultiToPercent() + player.AOPlayer().GetSizeMulti(item).MultiToPercent();
				}
				else if (item.ModItem is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					scale += imbue.AOImbueSize.MultiToPercent() + player.AOPlayer().GetSizeMulti(item).MultiToPercent();
				}
			}
		}

		public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
			if (item.TryGetImbue(player, out AOMagic imbue))
			{
				float extrakbmulti = 1f;
				if (imbue is WindMagic)
				{
					extrakbmulti = 3f;
				}
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					knockback += aoWeapon.AOSize.MultiToPercent() + imbue.AOImbueSize.MultiToPercent() + extrakbmulti + player.AOPlayer().GetSizeMulti(item).MultiToPercent();
				}
				else if (item.ModItem is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					knockback += imbue.AOImbueSize.MultiToPercent() + extrakbmulti.MultiToPercent();
				}

                if (item.ModItem.GetType().IsSubclassOf(typeof(DefaultScroll)))
                {
                    knockback += imbue.AOMagicSize.MultiToPercent() + extrakbmulti;
                }
            }
		}

		public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
            if (item.ModItem is not null && item.ModItem.GetType().IsSubclassOf(typeof(DefaultScroll)))
            {
                damage += ((item.damage+(BonusBossKills() * 2f)) / item.damage)-1; // now it actually shows up on the scrolls damage, although it means nothing to a scroll
            }
            if (item.TryGetImbue(player, out AOMagic imbue))
			{
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					damage += aoWeapon.AODamage.MultiToPercent() + imbue.AOImbueDamage.MultiToPercent();
				}
				else if (item.ModItem is null) // do not touch items from other mods
				{
					damage += imbue.AOImbueDamage.MultiToPercent();
				}

				if (item.ModItem.GetType().IsSubclassOf(typeof(DefaultScroll)))
				{
					damage += imbue.AOMagicDamage.MultiToPercent();
				}
			}
		}
		public override float UseSpeedMultiplier(Item item, Player player)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
			if (item.TryGetImbue(player, out AOMagic imbue) && item.DamageType != DamageClass.MeleeNoSpeed)
			{
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					return aoWeapon.AOSpeed + imbue.AOImbueSpeed.MultiToPercent();
				}
				else if (item.ModItem is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
				{
					return imbue.AOImbueSpeed;
				}
            }

            if (item.ModItem.GetType().IsSubclassOf(typeof(DefaultScroll)))
            {
                return imbue.AOMagicSpeed;
            }
            return 1f;
		}
	}
}
