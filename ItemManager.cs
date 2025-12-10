using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Equipment.Vanity;
using ArcaneOdyssey.Content.Items.Imbues;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

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
			if ((item.ModItem is not null && item.ModItem.Name == "UnloadedItem") || !item.ArcaneOdyssey().CanBeAffected)
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
			}

			if (item.ArcaneOdyssey().WeaponsType == WeaponType.Arcanium)
			{
				tooltips.Add(new TooltipLine(Mod, "ArcaniumIndicator", Mod.CustomLocalization("ImbueStuff.ArcaniumIndicator").Value));
			}
			if (item.ArcaneOdyssey().WeaponsType == WeaponType.Strength)
			{
				tooltips.Add(new TooltipLine(Mod, "StrengthIndicator", Mod.CustomLocalization("ImbueStuff.StrengthIndicator").Value));
			}
			if (item.ArcaneOdyssey().WeaponsType == WeaponType.Artisinal)
			{
				tooltips.Add(new TooltipLine(Mod, "ArtisinalIndicator", Mod.CustomLocalization("ImbueStuff.ArtisinalIndicator").Value));
			}
		}
	}

	public class AOItem : GlobalItem, IImbuable
	{
		public override bool InstancePerEntity => true;

		public Item thisItem = null;
		public Imbuable Imbue { get; set; }
		private int imbueIndex = 0;
		public bool specificImbue = false;

		public WeaponType _weaponsType;
		public WeaponType WeaponsType
		{
			get
			{
				if (thisItem is not null && thisItem.ModItem is AORangedOrMeleeWeapon weap)
				{
					return weap.WeaponsType;
				}
				return _weaponsType;
			}
			set => _weaponsType = value;
		}

		public bool BenifitsFromScrollStats => thisItem.ModItem is AnyScroll || WeaponsType == WeaponType.Arcanium || WeaponsType == WeaponType.Strength;

		private bool _canImbue = true;
		public bool CanBeAffected
		{
			get
			{
				if (thisItem is not null && thisItem.ModItem is AORangedOrMeleeWeapon item)
				{
					return item.CanBeAffected;
				}
				return _canImbue;
			}
			set => _canImbue = value;
		}


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

		public override GlobalItem Clone(Item from, Item to)
		{
			var clone = (AOItem)base.Clone(from, to);
			clone.Imbue = Imbue;
			clone._cold = _cold;
			clone._weaponsType = _weaponsType;
			clone.thisItem = to;
			clone._canImbue = _canImbue;
			return clone;
		}

		public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			thisItem = item;
			if (Imbue is null || !CanBeAffected)
				return;
			if (ModContent.RequestIfExists<Texture2D>(Imbue.Texture, out var texture))
			{
				Vector2 dimensions = new(frame.Width, frame.Height);
				Vector2 location = position + (dimensions * .25f);

				spriteBatch.Draw(texture.Value, location, null, Color.White, 0, dimensions / 2, .35f, SpriteEffects.None, 1f);
				
				if (Imbue is FightingStyleBarred fs && item.ModItem.Type != Imbue.Type)
					spriteBatch.DrawString(FontAssets.ItemStack.Value, $"{fs.BarValue.Round()}%", location - (FontAssets.ItemStack.Value.MeasureString($"{fs.BarValue.Round()}%") / 2), fs.GetColor(Color.White));
			}
			if (item.CanHaveSecondImbue(Imbue, out var second) && ModContent.RequestIfExists<Texture2D>(second.Texture, out var texture2))
			{
				Vector2 dimensions = new(frame.Width, frame.Height);
				Vector2 location = position + (dimensions * .25f);
				location.X -= texture2.Width() * .35f;

				spriteBatch.Draw(texture2.Value, location, null, Color.White, 0, dimensions / 2, .35f, SpriteEffects.None, 1f);

				if (second is FightingStyleBarred fs && item.ModItem.Type != second.Type)
					spriteBatch.DrawString(FontAssets.ItemStack.Value, $"{fs.BarValue.Round()}%", location - (FontAssets.ItemStack.Value.MeasureString($"{fs.BarValue.Round()}%") / 2), fs.GetColor(Color.White));
			}
		}

		public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			thisItem = item;
			if (!CanBeAffected)
				return;
			if (Imbue is not null) 
			{
				if (!item.DamageType.Name.Contains("NoSpeed"))
				{
					if (BenifitsFromScrollStats)
					{
						velocity *= Imbue.AOScrollSpeed;
					}
					else if (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
					{
						velocity *= Imbue.AOImbueSpeed;
					}
					if (item.CanHaveSecondImbue(Imbue, out var secondimbue))
						velocity *= secondimbue.AOImbueSpeed;
				}
			}
		}

		public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
		{
			thisItem = item;
			if (!CanBeAffected)
				return;
			if (Imbue is not null)
			{
				if (BenifitsFromScrollStats)
				{
					crit *= Imbue.AOScrollDamage;
				}
				else
				{
					crit *= Imbue.AOImbueDamage;
					if (item.CanHaveSecondImbue(Imbue, out var second))
						crit *= second.AOImbueDamage;
				}
			}
			if (Imbue is VanishingStyle && player.HasBuff(BuffID.Invisibility))
				crit = 100;
		}

		public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
		{
			thisItem = item;
			if (!CanBeAffected)
				return;
			if (Imbue is not null)
			{
				if (BenifitsFromScrollStats)
				{
					knockback += Imbue.AOScrollSize.MultiToPercent();
				}
				else if (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					knockback += Imbue.AOImbueSize.MultiToPercent();
					if (item.CanHaveSecondImbue(Imbue, out var second))
						knockback *= second.AOImbueSize.MultiToPercent();
				}
				var extraknockbackmulti = Imbue.KBMulti;
				if (item.CanHaveSecondImbue(Imbue, out var second1))
					extraknockbackmulti += second1.KBMulti.MultiToPercent();
				knockback *= extraknockbackmulti;
			}
		}

		public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
		{
			thisItem = item;
			if (!CanBeAffected)
				return;
			if (item.ModItem is MagicScroll)
			{
				damage += ((item.damage + (BossesKilled * 2f)) / item.damage) - 1; // now it actually shows up on the scrolls damage, although it means nothing to a scroll
			}
			if (Imbue is not null)
			{
				if (BenifitsFromScrollStats)
				{
					damage += Imbue.AOScrollDamage.MultiToPercent();
				}
				else if (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					damage += Imbue.AOImbueDamage.MultiToPercent();
				}
				if (item.CanHaveSecondImbue(Imbue, out var second))
					damage += second.AOImbueDamage.MultiToPercent();
			}
		}

		public override void SetDefaults(Item item)
		{
			thisItem = item;
			if (ArcaneOdysseyMod.excludedItems.Contains(item.type))
			{
				CanBeAffected = false;
				return;
			}
			WeaponsType = (WeaponType)ArcaneOdysseyMod.weaponTypes[item.type];
			Cold = ArcaneOdysseyMod.itemTemperatures[item.type];
			if (ArcaneOdysseyConfig.Instance.VanillaItemTemperatures)
			{
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
				switch (item.type)
				{
					case ItemID.BreakerBlade:
						WeaponsType = WeaponType.Strength;
						break;
					case ItemID.Zenith:
						WeaponsType = WeaponType.Artisinal;
						break;
				}
			}
		}

		public override void ModifyItemScale(Item item, Player player, ref float scale)
		{
			thisItem = item;
			if (item.noMelee || !CanBeAffected)
				return;
			if (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
			{
				scale += player.ArcaneOdyssey().SizeMulti;
				if (Imbue is not null)
				{
					if (!BenifitsFromScrollStats)
					{
						scale += Imbue.AOImbueSize.MultiToPercent();
					}
					else
					{
						scale += Imbue.AOScrollSize.MultiToPercent();
					}
					if (item.CanHaveSecondImbue(Imbue, out var second))
						scale += second.AOImbueSize.MultiToPercent();
				}
			}
		}

		public override float UseSpeedMultiplier(Item item, Player player)
		{
			thisItem = item;
			if (CanBeAffected)
			{
				if (Imbue is not null && !item.DamageType.Name.Contains("NoSpeed") && CanBeAffected)
				{
					if (BenifitsFromScrollStats)
					{
						return Imbue.AOScrollSpeed;
					}

					if (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
					{
						return Imbue.AOImbueSpeed + (item.CanHaveSecondImbue(Imbue, out var second) ? second.AOImbueSpeed.MultiToPercent() : 0f);
					}
				}
			}
			return base.UseSpeedMultiplier(item, player);
		}

		public override void UpdateInventory(Item item, Player player)
		{
			thisItem = item;
			if (item.ModItem is null && !ArcaneOdysseyConfig.Instance.VanillaItemTemperatures)
			{
				Cold = null;
				WeaponsType = WeaponType.Normal;
			}
			if (!CanBeAffected)
				return;
			if (Main.myPlayer != player.whoAmI)
				return;
			List<Imbuable> options = [null, .. player.GetAllImbues()];
			options.RemoveAll(e => !item.CanHaveImbue(e));
			bool justchangedspecificimbue = false;
			bool settodefault = false;

			if (Imbue is not null && !Imbue.PlayerHasImbue(player))
			{
				if (specificImbue)
				{
					settodefault = true;
					specificImbue = false;
				}
			}

			if (Imbue?.Type == player.Imbue()?.Type)
			{
				specificImbue = false;
			}

			if (options.Count > 0 && ImbueClassCheck(item))
			{
				if (!specificImbue || item.accessory)
				{
					if (item.CanHaveImbue(player.Imbue()))
						Imbue = player.Imbue();
					else
						Imbue = null;
				}

				if (!item.accessory && player.PlayerItem() == item && AOKeybinds.CycleItemImbue.JustPressed && !player.ArcaneOdyssey().OnCooldown("CycleImbueCooldown"))
				{
					specificImbue = true;
					player.ArcaneOdyssey().SetCooldown(new Cooldown("CycleImbueCooldown", AOKeybinds.CycleItemImbue.DisplayName, 60));
					if (options.Count > 1)
					{
						specificImbue = true;
						if (++imbueIndex >= options.Count)
						{
							imbueIndex = 0;
						}
						Imbue = options[imbueIndex];
						justchangedspecificimbue = true;
						if (Imbue?.Type == player.Imbue()?.Type)
						{
							settodefault = true;
							specificImbue = false;
						}

						if (Imbue is AOMagic magic)
						{
							AOMagic.CreateMagicCircle(Imbue.Item, player, magic);
						}
					}
				}

				if (options.Count < 2 && (Imbue != player.Imbue()))
				{
					specificImbue = true;
					//justchangedspecificimbue = true;
					if (item.CanHaveImbue(player.Imbue()))
						Imbue = player.Imbue();
					else
						Imbue = null;
					settodefault = true;
					imbueIndex = -1;
				}
			}
			else
			{
				Imbue = null;
				specificImbue = false;
			}

			if (!specificImbue || item.accessory)
			{
				if (item.CanHaveImbue(player.Imbue()))
					Imbue = player.Imbue();
				else
					Imbue = null;
			}

			if (Imbue is not null && Cold.HasValue && Imbue.Cold.HasValue && (Cold.Value != Imbue.Cold.Value))
			{
				Imbue = SteamImbue.Create(Imbue);
			}

			if (justchangedspecificimbue && player == Main.LocalPlayer)
			{
				LocalizedText chatmessage = Mod.CustomLocalization("ImbueStuff.SpecificImbue", [item.Name, Imbue is null ? Mod.CustomLocalization("RandomWords.None") : (!settodefault ? Imbue.DisplayName : Mod.CustomLocalization("RandomWords.Default").Value)]);
				Main.NewText(chatmessage.Value, 13, 132, 168);
			}
			item.DamageType = item.DamageType.UnImbued();
			if (item.ModItem is not TechniqueScroll)
				item.DamageType = item.DamageType.Imbued(Imbue);
		}

		public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
		{
			thisItem = item;
			Imbue = null;
			specificImbue = false;
		}

		public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			thisItem = item;
			if (!CanBeAffected)
				return;

			if (player.meleeEnchant != 0 && (item.DamageType.CountsAsClass(DamageClass.Melee) || item.DamageType == DamageClass.SummonMeleeSpeed))
			{
				// apply early for synergies and stuff, no way to do it for modded imbues
				foreach (var buff in player.buffType)
				{
					if (Main.meleeBuff[buff])
					{
						switch (player.meleeEnchant)
						{
							case 1:
								target.AddBuff(BuffID.Venom, 60 * Main.rand.Next(5, 10));
								break;
							case 2:
								target.AddBuff(BuffID.CursedInferno, 60 * Main.rand.Next(3, 7));
								break;
							case 3:
								target.AddBuff(BuffID.OnFire, 60 * Main.rand.Next(3, 7));
								break;
							case 4:
								target.AddBuff(BuffID.Midas, 120);
								break;
							case 5:
								target.AddBuff(BuffID.Ichor, 60 * Main.rand.Next(10, 20));
								break;
							case 6:
								target.AddBuff(BuffID.Confused, 60 * Main.rand.Next(1, 4));
								break;
							case 8:
								target.AddBuff(BuffID.Poisoned, 60 * Main.rand.Next(5, 10));
								break;
							default:
								if (player.ArcaneOdyssey().gel.HasValue)
									target.AddBuff(player.ArcaneOdyssey().gel.Value, 60 * Main.rand.Next(5, 10));
								break;
						}
					}
				}
			}

			if (item.ModItem is AORangedOrMeleeWeapon weap)
			{
				if (weap.WeaponDebuff.HasValue && (weap.WeaponDebuff.Value.debuffPercent == 0 || modifiers.GetDamage(item.damage, true) > (target.lifeMax / weap.WeaponDebuff.Value.debuffPercent)))
				{
					target.AddBuff(weap.WeaponDebuff.Value.debuffID, weap.WeaponDebuff.Value.debuffDuration);
				}
			}

			if (Imbue is not null)
			{
				if (Imbue is PowderFist)
				{
					Projectile.NewProjectile(item.GetSource_FromThis(), target.Center, Vector2.Zero, ModContent.ProjectileType<PowderExplosion>(), 0, 3f, player.whoAmI, 0, item.damage / 2f);
				}
				CalculateImbueDamage(Imbue, target, ref modifiers);
				if (item.CanHaveSecondImbue(Imbue, out var second))
					CalculateImbueDamage(second, target, ref modifiers);
			}
		}
	}
}
