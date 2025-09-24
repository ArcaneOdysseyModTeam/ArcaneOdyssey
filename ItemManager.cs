using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
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

			if (item.TryGetImbue(out Imbuable imbue))
			{
				if (imbue is CrystalMagic && target.HasBuff<Crystallized>() && Crystallized.GetCrystalStack(target, target.FindBuffIndex(ModContent.BuffType<Crystallized>())) == 4)
				{
					modifiers.FinalDamage += .3f;
				}

				if ((imbue.ImbueDebuff is not null) && (imbue.ImbueDebuff.DebuffPercent != 0f))
				{
					if (imbue.ImbueDebuff.DebuffPercent is null || modifiers.GetDamage(item.damage, true) > (target.lifeMax / imbue.ImbueDebuff.DebuffPercent))
					{
						target.AddBuff(imbue.ImbueDebuff.debuffID, imbue.ImbueDebuff.debuffDuration);
					}
				}
				if ((imbue.ImbueDebuff2 is not null) && (imbue.ImbueDebuff2.DebuffPercent != 0f))
				{
					if (imbue.ImbueDebuff2.DebuffPercent is null || modifiers.GetDamage(item.damage, true) > (target.lifeMax / imbue.ImbueDebuff2.DebuffPercent))
					{
						target.AddBuff(imbue.ImbueDebuff2.debuffID, imbue.ImbueDebuff2.debuffDuration);
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
			if (item.ModItem is AOWeapon weapon && weapon.Arcanium.HasValue)
			{
				if (weapon.Arcanium.Value)
				{
					tooltips.Add(new TooltipLine(Mod, "ArcaniumIndicator", Mod.CustomLocalization("ImbueStuff.ArcaniumIndicator").Value));
				}
				else
				{
					tooltips.Add(new TooltipLine(Mod, "StrengthIndicator", Mod.CustomLocalization("ImbueStuff.StrengthIndicator").Value));
				}
			}


			if (ImbueClassCheck(item))
			{
				bool? coolred = null;
				string imbuetextthing = Mod.CustomLocalization("ImbueStuff.NoneText").Value;
				if (item.TryGetImbue(out Imbuable imbue) && imbue is not SteamImbue)
				{
					coolred = imbue is FightingStyle;
					imbuetextthing = imbue.DisplayName.Value;
				}
				else if (item.ArcaneOdyssey().imbue is SteamImbue)
				{
					imbuetextthing = Language.GetTextValue("RandomWorldName_Adjective.Steaming");
				}
				string idkwhattonamethis = coolred.HasValue ? (coolred.Value ? "Strength" : "Magic") : "";
				tooltips.Add(new TooltipLine(Mod, "ImbueText", Mod.CustomLocalization($"ImbueStuff.ImbueTooltip{idkwhattonamethis}", [imbuetextthing]).Value));
			}
		}

		public override void ModifyItemScale(Item item, Player player, ref float scale)
		{
			if (item.TryGetImbue(out Imbuable imbue))
			{
				if (item.ModItem is null or AOWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					scale += imbue.AOImbueSize.MultiToPercent() + player.ArcaneOdyssey().GetSizeMulti(item).MultiToPercent();
				}
			}
		}

		public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
		{
			if (item.TryGetImbue(out Imbuable imbue))
			{
				float extrakbmulti = 1f;
				if (imbue is WindMagic)
				{
					extrakbmulti = 3f;
				}

				if (item.ModItem is not null && item.ModItem.GetType().IsSubclassOf(typeof(MagicScroll)))
				{
					knockback += imbue.AOScrollSize.MultiToPercent() + extrakbmulti;
					return;
				}

				if (item.ModItem is null or AOWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					knockback += imbue.AOImbueSize.MultiToPercent() + extrakbmulti.MultiToPercent() + player.ArcaneOdyssey().GetSizeMulti(item).MultiToPercent();
				}
			}
		}

		public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
		{
			if (item.ModItem is not null && item.ModItem.GetType().IsSubclassOf(typeof(MagicScroll)))
			{
				damage += ((item.damage+(BossesKilled * 2f)) / item.damage)-1; // now it actually shows up on the scrolls damage, although it means nothing to a scroll
			}
			if (item.TryGetImbue(out Imbuable imbue))
			{
				if (item.ModItem is not null && item.ModItem.GetType().IsSubclassOf(typeof(MagicScroll)))
				{
					damage += imbue.AOScrollDamage.MultiToPercent();
					return;
				}

				if (item.ModItem is null or AOWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					damage += imbue.AOImbueDamage.MultiToPercent();
				}
			}
		}
		public override float UseSpeedMultiplier(Item item, Player player)
		{
			if (item.TryGetImbue(out Imbuable imbue) && item.DamageType != DamageClass.MeleeNoSpeed)
			{
				if (item.ModItem is not null && item.ModItem.GetType().IsSubclassOf(typeof(MagicScroll)))
				{
					return imbue.AOScrollSpeed;
				}

				if (item.ModItem is null or AOWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
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

		public Player owner = null;
		public Imbuable imbue = null;
		private int ImbueIndex;
		public bool SpecificImbue = false;

		public override GlobalItem Clone(Item from, Item to)
		{
			var clone = (AOItem)base.Clone(from, to);
			clone.imbue = imbue;
			clone.owner = owner;
			return clone;
		}

		public override void UpdateInventory(Item item, Player player)
		{
			owner = player;
			var options = player.GetAllImbues();
			if (options.Count > 0 && ImbueClassCheck(item))
			{
				bool justchangedspecificimbue = false;
				bool settodefault = false;
				if (!SpecificImbue)
				{
					if (imbue is not null)
					{
						if (!imbue.PlayerHasImbue(player, options))
						{
							imbue = null;
						}
					}
					imbue = player.ArcaneOdyssey().imbue;
				}

				if (!item.accessory && player.HeldItem == item && AOKeybinds.CycleItemImbue.JustPressed && !player.ArcaneOdyssey().Cooldowns.ContainsKey("CycleItemImbue"))
				{
					SpecificImbue = true;
					player.ArcaneOdyssey().Cooldowns["CycleItemImbue"] = 60;
					if (options.Count > 1)
					{
						SpecificImbue = true;
						ImbueIndex++;
						if (ImbueIndex >= options.Count)
						{
							ImbueIndex = 0;
						}
						imbue = options[ImbueIndex];
						justchangedspecificimbue = true;
						if (imbue is AOMagic magic)
						{
							AOMagic.CreateMagicCircle(imbue.Item, player, magic);
						}
					}
					else
					{
						SpecificImbue = false;
						justchangedspecificimbue = true;
						imbue = options[0];
						settodefault = true;
					}
				}

				if (item.ModItem is AOWeapon weapon && imbue is not null && weapon.ColdWeapon.HasValue && imbue.Cold.HasValue && (weapon.ColdWeapon.Value != imbue.Cold.Value))
				{
					imbue = new SteamImbue() { originalImbue = imbue };
				}

				if (justchangedspecificimbue && player == Main.LocalPlayer)
				{
					LocalizedText chatmessage = Mod.CustomLocalization("ImbueStuff.SpecificImbue", [item.Name, !settodefault ? (imbue is not SteamImbue ? imbue.DisplayName : Language.GetTextValue("RandomWorldName_Adjective.Steaming")) : Mod.CustomLocalization("ImbueStuff.DefaultText").Value]);
					Main.NewText(chatmessage.Value, 13, 132, 168);
				}
			}
			else
			{
				imbue = null;
			}
		}

		public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
		{
			owner = null;
			imbue = null;
			SpecificImbue = false;
		}
	}
}
