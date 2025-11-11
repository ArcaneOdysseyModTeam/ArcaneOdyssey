using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Materials;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Projectiles;
using static ArcaneOdyssey.AOUtils;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Items.Imbues;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Equipment.Vanity;

namespace ArcaneOdyssey
{
	public class ItemManager : GlobalItem
	{
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			if (item.type == ItemID.OceanCrateHard)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunkenScrap>(), 15));
			}
			if (ItemID.Sets.BossBag[item.type] && !ItemID.Sets.PreHardmodeLikeBossBag[item.type])
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
				//LeadingConditionRule leadingConditionRule5 = new(new Conditions.TenthAnniversaryIsUp());
				//leadingConditionRule5.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ElfPetItem>(), 8), true);
				//itemLoot.Add(leadingConditionRule5);
				//LeadingConditionRule leadingConditionRule6 = new(new Conditions.TenthAnniversaryIsNotUp());
				//leadingConditionRule6.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ElfPetItem>(), 16), true);
				//itemLoot.Add(leadingConditionRule6);
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
				tooltips[tooltips.IndexOf(dashline)].Text = dashline.Text.Replace("{DASHBIND}", AOKeybinds.DashBind.GetAssignedKeys(InputMode.Keyboard).FirstOrDefault(Mod.CustomLocalization("RandomWords.Unbound").Value));
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

				if (item.ModItem is not AOBaseItem || (item.ModItem is AOBaseItem based && based.ShowItemTypeTooltip))
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


			if (ImbueClassCheck(item) && item.active)
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

	public class AOItem : GlobalItem, IImbuableEntity
	{
		public override bool InstancePerEntity => true;

		public Item thisItem = null;
		public Imbuable Imbue { get; set; }
		public int ImbueIndex = 0;
		public bool SpecificImbue = false;


		private bool? _cold = null;
		public bool? Cold
		{
			get
			{
				if (thisItem is not null && thisItem.ModItem is AORangedOrMeleeWeapon weap)
				{
					return weap.Cold;
				}
				return _cold;
			}
			set => _cold = value;
		}

		public bool? Arcanium { get
			{
				if (thisItem is not null && thisItem.ModItem is AORangedOrMeleeWeapon weap)
				{
					return weap.Arcanium;
				}
				return null;
			} }

		public override GlobalItem Clone(Item from, Item to)
		{
			var clone = (AOItem)base.Clone(from, to);
			clone.Imbue = Imbue;
			clone._cold = _cold;
			clone.thisItem = thisItem;
			return clone;
		}

        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (Imbue is not null && !item.DamageType.Name.Contains("NoSpeed"))
            {
                if (item.ModItem is EmptyScroll || item.ArcaneOdyssey().Arcanium.GetValueOrDefault(false))
                {
                    velocity *= Imbue.AOScrollSpeed;
                }
                else
                {
                    velocity *= Imbue.AOImbueSpeed;
                }
            }
        }

        public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
        {
            if (Imbue is not null)
            {
                knockback *= Imbue.KBMulti;
                if (item.ModItem is MagicScroll || Arcanium.GetValueOrDefault())
                {
                    knockback += Imbue.AOScrollSize.MultiToPercent();
                    return;
                }

                if (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
                {
                    knockback += Imbue.AOImbueSize.MultiToPercent();
                }
            }
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (item.ModItem is MagicScroll)
            {
                damage += ((item.damage + (BossesKilled * 2f)) / item.damage) - 1; // now it actually shows up on the scrolls damage, although it means nothing to a scroll
            }
            if (Imbue is not null)
            {
                if (item.ModItem is MagicScroll)
                {
                    damage += Imbue.AOScrollDamage.MultiToPercent();
                    return;
                }

                if (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
                {
                    damage += Imbue.AOImbueDamage.MultiToPercent();
                }
            }
        }

        public override void SetDefaults(Item item)
		{
			Cold = ArcaneOdyssey.coldItems.GetValueOrDefault(item.type, null);
			switch (item.type)
			{
				case ItemID.IceSickle:
				case ItemID.IceBlade:
				case ItemID.Frostbrand:
				case ItemID.ChristmasTreeSword:
				case ItemID.NorthPole:
				case ItemID.Snowball:
				case ItemID.SnowballCannon:
				case ItemID.FrostDaggerfish:
				case ItemID.IceBow:
				case ItemID.IceBoomerang:
				case ItemID.Flairon:
				case ItemID.ElfMelter:
				case ItemID.Tsunami:
					Cold = true;
					break;
				case ItemID.DD2SquireBetsySword:
				case ItemID.DD2SquireDemonSword:
				case ItemID.ShadowFlameKnife:
				case ItemID.FieryGreatsword:
				case ItemID.Flamarang:
				case ItemID.Sunfury:
				case ItemID.FlamingMace:
				case ItemID.DayBreak:
				case ItemID.MoltenFury:
				case ItemID.HellwingBow:
				case ItemID.ShadowFlameBow:
				case ItemID.SolarEruption:
				case ItemID.MolotovCocktail:
				case ItemID.PhoenixBlaster:
				case ItemID.Flamethrower:
				case ItemID.BluePhaseblade:
				case ItemID.DD2BetsyBow:
				case ItemID.GreenPhaseblade:
				case ItemID.OrangePhaseblade:
				case ItemID.DD2PhoenixBow:
				case ItemID.PurplePhaseblade:
				case ItemID.RedPhaseblade:
				case ItemID.WhitePhaseblade:
				case ItemID.YellowPhaseblade:
				case ItemID.GreenPhasesaber:
				case ItemID.OrangePhasesaber:
				case ItemID.PurplePhasesaber:
				case ItemID.WhitePhasesaber:
				case ItemID.YellowPhasesaber:
				case ItemID.RedPhasesaber:
				case ItemID.BluePhasesaber:
				case ItemID.HelFire:
				case ItemID.Amarok:
				case ItemID.Cascade:
					Cold = false;
					break;
			}
        }

        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            if (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
            {
                scale += player.ArcaneOdyssey().SizeMulti;
                if (Imbue is not null)
                {
                    if (!Arcanium.GetValueOrDefault(false))
                    {
                        scale += Imbue.AOImbueSize.MultiToPercent();
                    }
                    else 
                    {
                        scale += Imbue.AOScrollSize.MultiToPercent();
                    }
                }
            }
        }

        public override float UseSpeedMultiplier(Item item, Player player)
        {
            if (Imbue is not null && !item.DamageType.Name.Contains("NoSpeed"))
            {
                if (item.ModItem is MagicScroll || Arcanium.GetValueOrDefault())
                {
                    return Imbue.AOScrollSpeed;
                }

                if (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
                {
                    return Imbue.AOImbueSpeed;
                }
            }
            return 1f;
        }

        public override void UpdateInventory(Item item, Player player)
		{
			thisItem = item;
			List<Imbuable> options = [null, ..player.GetAllImbues()];
			bool justchangedspecificimbue = false;
			bool settodefault = false;
			if (Imbue is null || !Imbue.PlayerHasImbue(player))
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
					Imbue = player.ArcaneOdyssey().Imbue;
				}

				if (!item.accessory && player.PlayerItem() == item && AOKeybinds.CycleItemImbue.JustPressed && !player.ArcaneOdyssey().OnCooldown("CycleImbueCooldown"))
				{
					SpecificImbue = true;
					player.ArcaneOdyssey().SetCooldown(new("CycleImbueCooldown", AOKeybinds.CycleItemImbue.DisplayName, true, 60));
					if (options.Count > 1)
					{
						SpecificImbue = true;
						ImbueIndex++;
						if (ImbueIndex >= options.Count)
						{
							ImbueIndex = 0;
						}
						Imbue = options[ImbueIndex];
						justchangedspecificimbue = true;
						if (Imbue is null)
						{
							settodefault = true;
							SpecificImbue = false;
						}
						else if (Imbue is AOMagic magic)
						{
							AOMagic.CreateMagicCircle(Imbue.Item, player, magic);
						}
					} 
				}

				if (options.Count < 2 && (Imbue != player.Imbue()))
				{
					SpecificImbue = true;
					justchangedspecificimbue = true;
					Imbue = player.Imbue();
					settodefault = true;
					ImbueIndex = -1;
				}
				

				if (Imbue is not null && Cold.HasValue && Imbue.Cold.HasValue && (Cold.Value != Imbue.Cold.Value))
				{
					Imbue = SteamImbue.Create(Imbue);
				}
			}
			else
			{
				Imbue = null;
				SpecificImbue = false;
			}

			if (justchangedspecificimbue && player == Main.LocalPlayer)
			{
				LocalizedText chatmessage = Mod.CustomLocalization("ImbueStuff.SpecificImbue", [item.Name, !settodefault ? Imbue.DisplayName : Mod.CustomLocalization("RandomWords.Default").Value]);
				Main.NewText(chatmessage.Value, 13, 132, 168);
			}
		}

		public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
		{
			thisItem = item;
			Imbue = null;
			SpecificImbue = false;
        }

        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (item.ModItem is AORangedOrMeleeWeapon weap)
            {
                if (weap.WeaponDebuff.HasValue && (weap.WeaponDebuff.Value.debuffPercent == 0 || modifiers.GetDamage(item.damage, true) > (target.lifeMax / weap.WeaponDebuff.Value.debuffPercent)))
                {
                    target.AddBuff(weap.WeaponDebuff.Value.debuffID, weap.WeaponDebuff.Value.debuffDuration);
                }
            }

            if (Imbue is not null)
            {
                if (Imbue is CrystalMagic && target.HasBuff<Crystallized>() && GetAOBuffStack(target, target.FindBuffIndex(ModContent.BuffType<Crystallized>())) == 4)
                {
                    modifiers.FinalDamage += .3f;
                }

                if (Imbue is PowderFist)
                {
                    Projectile.NewProjectile(item.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<PowderExplosion>(), 0, 3f, player.whoAmI, 0, item.damage / 2f);
                }
                foreach (var debuff in Imbue.ImbueDebuffs)
                {
                    if ((debuff.debuffPercent == 0) || modifiers.GetDamage(item.damage, true) > (target.lifeMax / debuff.debuffPercent))
                    {
                        target.AddBuff(debuff.debuffID, debuff.debuffDuration);
                    }
                }

                if (Imbue.CombinedDebuffs is not null)
                {
                    foreach (CombinedDebuff buffkeys in Imbue.CombinedDebuffs)
                    {
                        if (target.HasBuff(buffkeys.requirement) || (buffkeys.requirement == BuffID.Wet && target.wet))
                        {
                            target.AddBuff(buffkeys.result, buffkeys.duration);
                        }
                    }
                }

                foreach (var multiplier in Imbue.Effects.magicBuffMultipliers)
                {
                    if (target.HasBuff(multiplier.buffID) || (multiplier.buffID == BuffID.Wet && target.wet))
                    {
                        modifiers.FinalDamage += multiplier.multiplier.MultiToPercent();
                    }
                }

                if (Main.netMode == NetmodeID.SinglePlayer) // things would get chaotic in multiplayer if everyone kept clearing eachothers debuffs
                {
                    foreach (int buffid in Imbue.Effects.clearBuffs)
                    {
                        if (target.HasBuff(buffid))
                        {
                            target.DelBuff(target.FindBuffIndex(buffid));
                        }

                    }
                }
            }
        }
    }
}
