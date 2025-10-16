using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Items.FightingStyles;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Vanity;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using ArcaneOdyssey.Content.Projectiles;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey
{
	public class ItemManager : GlobalItem
	{
		public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (item.ModItem is AORangedOrMeleeWeapon weap)
			{
				if (weap.WeaponDebuff.HasValue && (weap.WeaponDebuff.Value.debuffPercent == 0 || modifiers.GetDamage(item.damage, true) > (target.lifeMax / weap.WeaponDebuff.Value.debuffPercent)))
				{
					target.AddBuff(weap.WeaponDebuff.Value.debuffID, weap.WeaponDebuff.Value.debuffDuration);
				}
			}

			if (item.TryGetImbue(out Imbuable imbue))
			{
				if (imbue is CrystalMagic && target.HasBuff<Crystallized>() && GetAOBuffStack(target, target.FindBuffIndex(ModContent.BuffType<Crystallized>())) == 4)
				{
					modifiers.FinalDamage += .3f;
				}

				if (imbue is PowderFist)
				{
					Projectile.NewProjectile(item.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<PowderExplosion>(), 0, 3f, player.whoAmI, 0, item.damage/2f);
				}
				foreach (var debuff in imbue.ImbueDebuffs)
				{
					if ((debuff.debuffPercent == 0) || modifiers.GetDamage(item.damage, true) > (target.lifeMax / debuff.debuffPercent))
					{
						target.AddBuff(debuff.debuffID, debuff.debuffDuration);
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

				foreach (var multiplier in imbue.Effects.magicBuffMultipliers)
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
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunkenScrap>(), 15));
			}
			if (ItemID.Sets.BossBag[item.type])
			{
				LeadingConditionRule leadingConditionRule1 = new(new Conditions.TenthAnniversaryIsUp());
				leadingConditionRule1.OnSuccess(ItemDropRule.Common(ModContent.ItemType<KindraBlade>(), 8), true);
				itemLoot.Add(leadingConditionRule1);
				LeadingConditionRule leadingConditionRule2 = new(new Conditions.TenthAnniversaryIsNotUp());
				leadingConditionRule2.OnSuccess(ItemDropRule.Common(ModContent.ItemType<KindraBlade>(), 16), true);
				itemLoot.Add(leadingConditionRule2);
				//LeadingConditionRule leadingConditionRule3 = new(new Conditions.TenthAnniversaryIsUp());
				//leadingConditionRule3.OnSuccess(ItemDropRule.Common(ModContent.ItemType<VesuvianSigil>(), 8), true);
				//itemLoot.Add(leadingConditionRule3);
				//LeadingConditionRule leadingConditionRule4 = new(new Conditions.TenthAnniversaryIsNotUp());
				//leadingConditionRule4.OnSuccess(ItemDropRule.Common(ModContent.ItemType<VesuvianSigil>(), 16), true);
				//itemLoot.Add(leadingConditionRule4);
			}
			LeadingConditionRule AcrimonyCondition = new(new NoShowNoConditon());
			AcrimonyCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Acrimony>(), 6000));
			itemLoot.Add(AcrimonyCondition);
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			var dashline = tooltips.Find(e => e.Text.Contains("{DASHBIND}"));
			if (dashline is not null)
			{
				tooltips[tooltips.IndexOf(dashline)].Text = dashline.Text.Replace("{DASHBIND}", AOKeybinds.DashBind.GetAssignedKeys(InputMode.Keyboard).FirstOrDefault(Mod.CustomLocalization("KeybindStuff.Unbound").Value));
			}
			if (item.ModItem is not null && item.ModItem.Name == "UnloadedItem")
			{
				return;
			}

			if (item.ModItem is null or AOBaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
			{
				if (item.ModItem is Imbuable and not BasicCombat || item.GetItemType() == ItemType.Material)
				{
					tooltips.RemoveAll(e => e.Name == "Material");
				}
				if (item.GetItemType() == ItemType.Vanity)
				{
					tooltips.RemoveAll(e => e.Name == "Vanity");
				}

				if (item.GetItemType() != ItemType.None && item.GetItemType() != ItemType.RESOLVESELF && !item.questItem)
				{
					var line = item.GetItemRare().ToString();
					line += " ";
					line += item.GetItemType().ToString().ToLower();
					tooltips.Insert(1, new TooltipLine(Mod, "ItemTypeLine", line));
				}
			}

			if (item.ModItem is AORangedOrMeleeWeapon weapon)
			{
				if (weapon.Ability.HasValue)
				{
					tooltips.Add(weapon.Ability.Value.GenerateTooltip());
				}

				if (weapon.Arcanium.HasValue)
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
			}


			if (ImbueClassCheck(item))
			{
				bool? coolred = null;
				string imbuetextthing = Mod.CustomLocalization("RandomWords.None").Value;
				if (item.TryGetImbue(out Imbuable imbue))
				{
					coolred = imbue is FightingStyle;
					imbuetextthing = imbue.DisplayName.Value;
				}
				string idkwhattonamethis = coolred.HasValue ? (coolred.Value ? "Strength" : "Magic") : "";
				tooltips.Add(new TooltipLine(Mod, "ImbueText", Mod.CustomLocalization($"ImbueStuff.ImbueTooltip{idkwhattonamethis}", [imbuetextthing]).Value));
			}
		}

		public override void ModifyItemScale(Item item, Player player, ref float scale)
		{
			if (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
			{
				scale += player.ArcaneOdyssey().GetSizeMulti(item).MultiToPercent();
				if (item.TryGetImbue(out Imbuable imbue))
				{
					scale += imbue.AOImbueSize.MultiToPercent();
				}
			} 
		}

		public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
		{
			if (item.TryGetImbue(out Imbuable imbue))
			{
				var extrakbmulti = 1;
				if (imbue is WindMagic or Boxing)
				{
					extrakbmulti = 3;
				}
				if (item.ModItem is not null && item.ModItem.GetType().IsSubclassOf(typeof(MagicScroll)))
				{
					knockback += imbue.AOScrollSize.MultiToPercent() * extrakbmulti;
					return;
				}

				if (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					knockback += imbue.AOImbueSize.MultiToPercent() * extrakbmulti;
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

				if (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					damage += imbue.AOImbueDamage.MultiToPercent();
				}
			}
		}

		public override float UseSpeedMultiplier(Item item, Player player)
		{
			if (item.TryGetImbue(out Imbuable imbue) && !item.DamageType.Name.Contains("NoSpeed"))
			{
				if (item.ModItem is not null && item.ModItem.GetType().IsSubclassOf(typeof(MagicScroll)))
				{
					return imbue.AOScrollSpeed;
				}

				if (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
				{
					return imbue.AOImbueSpeed;
				}
			}
			return 1f;
		}

		//public override bool? UseItem(Item item, Player player)
		//{
		//	if (item.TryGetImbue(out var imbue))
		//	{
		//		imbue.LingeringEffects(item);
		//	}
		//	return base.UseItem(item, player);z
		//}

		//public override void UseAnimation(Item item, Player player)
		//{
		//	if (item.TryGetImbue(out var imbue))
		//	{
		//		imbue.SpawningEffects(item);
		//	}
		//	base.UseAnimation(item, player);
		//}
	}

	public class AOItem : GlobalItem
	{
		public override bool InstancePerEntity => true;

		public Item thisItem;
		public Imbuable imbue = null;
		public int ImbueIndex = 0;
		public bool SpecificImbue = false;

		public bool? Arcanium { get
			{
				if (thisItem.ModItem is AORangedOrMeleeWeapon weap)
				{
					return weap.Arcanium;
				}
				return null;
			} }

		public override GlobalItem Clone(Item from, Item to)
		{
			var clone = (AOItem)base.Clone(from, to);
			clone.imbue = imbue;
			clone.thisItem = thisItem;
			return clone;
		}

		public override void UpdateInventory(Item item, Player player)
		{
			thisItem = item;
			List<Imbuable> options = [null, ..player.GetAllImbues()];
			bool justchangedspecificimbue = false;
			bool settodefault = false;
			if (imbue is null || !imbue.PlayerHasImbue(player))
			{
				if (SpecificImbue)
				{
					settodefault = true;
					justchangedspecificimbue = true;
				}
				SpecificImbue = false;
			}

			if (options.Count > 0 && ImbueClassCheck(item))
			{
				if (!SpecificImbue || item.accessory)
				{
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
						if (imbue is null)
						{
							settodefault = true;
							SpecificImbue = false;
						}
						else if (imbue is AOMagic magic)
						{
							AOMagic.CreateMagicCircle(imbue.Item, player, magic);
						}
					} 
				}

				if (options.Count < 2 && (imbue != player.Imbue()))
				{
					SpecificImbue = true;
					justchangedspecificimbue = true;
					imbue = player.Imbue();
					settodefault = true;
					ImbueIndex = -1;
				}
				

				if (item.ModItem is AORangedOrMeleeWeapon weapon && imbue is not null && weapon.ColdWeapon.HasValue && imbue.Cold.HasValue && (weapon.ColdWeapon.Value != imbue.Cold.Value))
				{
					imbue = SteamImbue.Create(imbue);
				}
			}
			else
			{
				imbue = null;
				SpecificImbue = false;
			}

			if (justchangedspecificimbue && player == Main.LocalPlayer)
			{
				LocalizedText chatmessage = Mod.CustomLocalization("ImbueStuff.SpecificImbue", [item.Name, !settodefault ? imbue.DisplayName : Mod.CustomLocalization("RandomWords.Default").Value]);
				Main.NewText(chatmessage.Value, 13, 132, 168);
			}
		}

		public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
		{
			thisItem = item;
			imbue = null;
			SpecificImbue = false;
		}
	}
}
