using ArcaneOdyssey.Content.Buffs.Base;
using ArcaneOdyssey.Content.Items.Accessories.Vanity;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Consumable;
using ArcaneOdyssey.Content.Items.Equipment.Pets;
using ArcaneOdyssey.Content.Items.Imbues;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Ancient;
using ArcaneOdyssey.Content.Items.Imbues.Relics;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Scrolls.Equipment.Common;
using ArcaneOdyssey.Content.Projectiles.Berserker.Effects;
using ArcaneOdyssey.PlayerClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public class AOItem : GlobalItem, IImbuable
	{
		public float ApplySpeed(float value, bool flipfloat = false)
		{
			if (BenifitsFromScrollStats.HasValue)
			{
				if (BenifitsFromScrollStats.Value)
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.AOScrollSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSpeed;
						}
						else
						{
							value *= Imbue.AOScrollSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSpeed.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.AOImbueSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSpeed;
						}
						else
						{
							value *= Imbue.AOImbueSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.AOImbueSpeed.FlipFloat();
						}
					}
				}
			}
			return value;
		}

		public override bool InstancePerEntity => true;

		public Item thisItem = null;
		public Player owner = null;

		public Imbuable Imbue { get; set; }
		public Imbuable SecondImbue { get; set; }

		private int imbueIndex = 0;
		public bool specificImbue = false;

		public WeaponType _weaponsType;
		public WeaponType WeaponsType
		{
			get
			{
				if (thisItem is not null && thisItem.ModItem is AOWeapon weap)
				{
					return weap.WeaponsType;
				}
				return _weaponsType;
			}
			set => _weaponsType = value;
		}

		public bool? BenifitsFromScrollStats
		{
			get
			{
				if (thisItem.CanHaveImbue(Imbue))
				{
					if (thisItem is not null)
					{
						if (WeaponsType == WeaponType.Artisinal)
							return null;
						return thisItem.ModItem is Scroll and not AuraScroll || WeaponsType != WeaponType.Normal;
					}
				}
				return null;
			}
		}

		private bool _canImbue = true;
		public bool CanBeAffected
		{
			get
			{
				if (thisItem is not null && thisItem.ModItem is AOWeapon item)
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
				if (thisItem is not null && thisItem.ModItem is AOWeapon weap)
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
			clone.SecondImbue = SecondImbue;
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
			if (ModContent.RequestIfExists<Texture2D>(Imbue.ImbueUISprite, out var texture) && Imbue.Type != item.type)
			{
				Vector2 dimensions = new(frame.Width, frame.Height);
				Vector2 location = position + (dimensions * (.25f * (52f / texture.Width())));

				spriteBatch.Draw(texture.Value, location, null, Color.White, 0, dimensions / 2, .3f * (52f / texture.Width()), SpriteEffects.None, 1f);

				if (Imbue is FightingStyleBarred fs && item.ModItem?.Type != Imbue.Type)
				{
					spriteBatch.DrawString(FontAssets.ItemStack.Value, $"{fs.BarValue.Round()}%", position - (FontAssets.ItemStack.Value.MeasureString($"{fs.BarValue.Round()}%") / 2), fs.GetColour(fs.DisplayColor));
				}

				if (SecondImbue is not null && SecondImbue.Type != Imbue.Type && SecondImbue.Type != item.type && ModContent.RequestIfExists<Texture2D>(SecondImbue.ImbueUISprite, out var texture2))
				{
					location.X -= texture2.Width() * (.4f * (52f / texture2.Width()));

					spriteBatch.Draw(texture2.Value, location, null, Color.White, 0, dimensions / 2, .3f * (52f / texture2.Width()), SpriteEffects.None, 1f);
				}
			}
		}

		public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			thisItem = item;
			owner = player;
			if (!CanBeAffected)
				return;

			if (item.ModItem is Imbuable imbue)
			{
				if (!item.DamageType.Name.Contains("NoSpeed"))
				{
					velocity *= imbue.AOScrollSpeed;
					if (imbue.Imbue is not null)
						velocity *= imbue.Imbue.AOImbueSpeed;
				}
			}
			else if (Imbue is not null)
			{
				if (!item.DamageType.Name.Contains("NoSpeed"))
				{
					if (BenifitsFromScrollStats.GetValueOrDefault())
					{
						velocity *= Imbue.AOScrollSpeed;
						if (SecondImbue is not null)
							velocity *= SecondImbue.AOImbueSpeed;
					}
					else if (item.ModItem is null or AOBaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
					{
						velocity *= Imbue.AOImbueSpeed;
						if (SecondImbue is not null)
							velocity *= SecondImbue.AOImbueSpeed;
					}
				}
			}
		}

		public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
		{
			thisItem = item;
			owner = player;
			if (!CanBeAffected)
				return;

			if (item.ModItem is Imbuable imbue)
			{
				crit *= imbue.AOScrollDamage;
				if (imbue.Imbue is not null)
					crit *= imbue.Imbue.AOImbueDamage;
				if (imbue is VanishingStyle vanish && vanish.BarValue > FightingStyleBarred.BarMin)
					if (!player.ArcaneOdyssey().OnCooldown(vanish.Name))
						crit = 100;
			}
			else if (Imbue is not null)
			{
				if (BenifitsFromScrollStats.GetValueOrDefault())
				{
					crit *= Imbue.AOScrollDamage;
					if (SecondImbue is not null)
						crit *= SecondImbue.AOImbueDamage;
				}
				else
				{
					crit *= Imbue.AOImbueDamage;
					if (SecondImbue is not null)
						crit *= SecondImbue.AOImbueDamage;
				}

				if (Imbue is VanishingStyle vanish && vanish.BarValue > FightingStyleBarred.BarMin)
					if (!player.ArcaneOdyssey().OnCooldown(vanish.Name))
						crit = 100;
			}
		}

		public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
		{
			thisItem = item;
			owner = player;
			if (!CanBeAffected)
				return;

			if (item.ModItem is Imbuable imbue)
			{
				knockback *= imbue.AOScrollSize * imbue.AOScrollSize;
				if (imbue.Imbue is not null)
					knockback *= imbue.Imbue.AOImbueDamage * imbue.Imbue.AOImbueDamage;
				var extraknockbackmulti = imbue.KBMulti;
				if (imbue.Imbue is not null)
					extraknockbackmulti += imbue.Imbue.KBMulti.MultiToPercent();
				knockback *= extraknockbackmulti;
			}
			else if (Imbue is not null)
			{
				if (BenifitsFromScrollStats.GetValueOrDefault())
				{
					knockback *= Imbue.AOScrollSize;
					if (SecondImbue is not null)
						knockback *= SecondImbue.AOScrollSize;
				}
				else if (item.ModItem is null or AOBaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					knockback *= Imbue.AOImbueSize * Imbue.AOImbueSize;
					if (SecondImbue is not null)
						knockback *= SecondImbue.AOImbueSize * SecondImbue.AOImbueDamage;
				}
				var extraknockbackmulti = Imbue.KBMulti;
				if (SecondImbue is not null)
					extraknockbackmulti += SecondImbue.KBMulti.MultiToPercent();
				knockback *= extraknockbackmulti;
			}
		}

		public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
		{
			thisItem = item;
			owner = player;
			if (!CanBeAffected)
				return;
			if (item.ModItem is Scroll)
			{
				damage += ((item.damage + (AOUtils.BossesKilled * 2f)) / item.damage) - 1;
			}
			
			if (item.ModItem is Imbuable imbue)
			{
				damage += imbue.AOScrollDamage.MultiToPercent();
				if (imbue.Imbue is not null)
					damage += imbue.Imbue.AOScrollDamage.MultiToPercent();
			}
			else if (Imbue is not null)
			{
				if (BenifitsFromScrollStats.GetValueOrDefault())
				{
					damage += Imbue.AOScrollDamage.MultiToPercent();
					if (SecondImbue is not null)
						damage += SecondImbue.AOImbueDamage.MultiToPercent();
				}
				else if (item.ModItem is null or AOBaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					damage += Imbue.AOImbueDamage.MultiToPercent();
					if (SecondImbue is not null)
						damage += SecondImbue.AOImbueDamage.MultiToPercent();
				}
			}
		}

		public override void SetDefaults(Item item)
		{
			if (!item.active || item.IsAir || string.IsNullOrWhiteSpace(item.Name))
				return;
			thisItem = item;
			owner = null;
			if (ArcaneOdysseyMod.excludedItems.Contains(item.type))
			{
				CanBeAffected = false;
				return;
			}
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
					case ItemID.MoltenPickaxe:
					case ItemID.SolarFlareDrill:
					case ItemID.SolarFlarePickaxe:
					case ItemID.MeteorHamaxe:
					case ItemID.MoltenHamaxe:
					case ItemID.LunarHamaxeSolar:
						Cold = false;
						break;
				}
				switch (item.type)
				{
					case ItemID.Anchor:
					case ItemID.BreakerBlade:
						WeaponsType = WeaponType.Strength;
						break;
					case ItemID.Zenith:
						WeaponsType = WeaponType.Artisinal;
						break;
				}
			}
			if (ArcaneOdysseyConfig.Instance.AffectsOtherMods && item.ModItem is not null or AOBaseItem)
			{
				Cold = ExternalModSupport.CheckItemTemperature(item.ModItem);
				WeaponsType = ExternalModSupport.CheckWeaponsType(item.ModItem);
			}
		}

		public override void ModifyItemScale(Item item, Player player, ref float scale)
		{
			thisItem = item;
			owner = player;
			if (item.noMelee || !CanBeAffected)
				return;
			if (item.ModItem is null or AOBaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
			{
				scale *= player.ArcaneOdyssey().SizeMulti;
				if (Imbue is not null)
				{
					if (!BenifitsFromScrollStats.GetValueOrDefault())
					{
						scale += Imbue.AOImbueSize.MultiToPercent();
						if (SecondImbue is not null)
							scale += SecondImbue.AOImbueSize.MultiToPercent();
					}
					else
					{
						scale += Imbue.AOScrollSize.MultiToPercent();
						if (SecondImbue is not null)
							scale += SecondImbue.AOImbueSize.MultiToPercent();
					}
				}
			}
		}

		public override float UseSpeedMultiplier(Item item, Player player)
		{
			thisItem = item;
			owner = player;
			if (CanBeAffected)
			{
				if (item.ModItem is Imbuable imbue)
				{
					return imbue.AOScrollSpeed * (imbue.Imbue?.AOScrollSpeed ?? 1f);
				}
				if (!item.DamageType.Name.Contains("NoSpeed"))
				{
					if (Imbue is not null)
					{
						if (BenifitsFromScrollStats.GetValueOrDefault())
						{
							return Imbue.AOScrollSpeed * (SecondImbue?.AOImbueSpeed ?? 1f);
						}

						if (item.ModItem is null or AOBaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
						{
							return Imbue.AOImbueSpeed * (SecondImbue?.AOImbueSpeed ?? 1f);
						}
					}
				}
			}
			return base.UseSpeedMultiplier(item, player);
		}

		public override void UpdateInventory(Item item, Player player)
		{
			thisItem = item;
			owner = player;

			if (item.ModItem is null && !ArcaneOdysseyConfig.Instance.VanillaItemTemperatures)
			{
				Cold = null;
				WeaponsType = WeaponType.Normal;
			}

			if (!CanBeAffected)
				return;

			if (Main.myPlayer != player.whoAmI)
				return;

			List<Imbuable> options = [null, .. player.GetAllImbues(), .. player.ArcaneOdyssey().EquippedImbues.Select(e => (Imbuable)ModContent.GetModItem(e))];
			options.RemoveAll(e => !item.CanHaveImbue(e));
			bool justchangedspecificimbue = false;
			bool settodefault = false;

			if (SecondImbue is not null)
			{
				if (Imbue?.Imbue != SecondImbue)
					SecondImbue = Imbue?.Imbue;
			}

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

			if (options.Count > 0 && AOUtils.ImbueClassCheck(item))
			{
				if (!specificImbue || (item.accessory && item.ModItem is not Imbuable))
				{
					if (item.CanHaveImbue(player.Imbue()))
					{
						Imbue = player.Imbue();
						if (item.TryGetSecondImbue(Imbue, out var second))
							SecondImbue = second;
						else
							SecondImbue = null;
					}
					else
					{
						Imbue = null;
						SecondImbue = null;
					}
				}

				if ((!item.accessory || item.ModItem is Imbuable) && player.PlayerItem() == item && AOKeybinds.CycleItemImbue.JustPressed && !player.ArcaneOdyssey().OnCooldown("CycleImbueCooldown"))
				{
					if (options.Count > 1)
					{
						specificImbue = true;
						player.ArcaneOdyssey()?.SetCooldown(new Cooldown("CycleImbueCooldown", AOKeybinds.CycleItemImbue.DisplayName, 60));
						specificImbue = true;
						if (++imbueIndex >= options.Count)
						{
							imbueIndex = 0;
						}
						Imbue = options[imbueIndex];
						SoundEngine.PlaySound(Imbue?.ImbueSound, player.MountedCenter);
						if (item.TryGetSecondImbue(Imbue, out var second))
							SecondImbue = second;
						else
							SecondImbue = null;
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
			}
			else
			{
				Imbue = null;
				SecondImbue = null;
				specificImbue = false;
			}

			if (!specificImbue || (item.accessory && item.ModItem is not Imbuable))
			{
				if (item.CanHaveImbue(player.Imbue()))
				{
					Imbue = player.Imbue();
					if (item.TryGetSecondImbue(Imbue, out var second))
						SecondImbue = second;
					else
						SecondImbue = null;
				}
				else
				{
					Imbue = null;
					SecondImbue = null;
				}
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
		}

		public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
		{
			owner = null;
			thisItem = item;
			Imbue = null;
			SecondImbue = null;
			specificImbue = false;
		}

		public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			owner = player;
			thisItem = item;
			if (player.ArcaneOdyssey().BloodDisease != 0)
			{
				target.AddBuff(player.ArcaneOdyssey().BloodDisease, 60 * Main.rand.Next(4, 10));
			}
			if (player.meleeEnchant == GelBuff.meleeEnchantID && (item.DamageType.CountsAsClass(DamageClass.Melee) || item.DamageType == DamageClass.SummonMeleeSpeed))
			{
				if (player.ArcaneOdyssey().GelDebuff != 0)
					target.AddBuff(player.ArcaneOdyssey().GelDebuff, 60 * Main.rand.Next(5, 10));
			}
			if (!CanBeAffected)
				return;
			if (Imbue is SpiritEnergy)
			{
				if (!target.immortal)
					player.ArcaneOdyssey()?.TrySpiritLifesteal(Math.Min(item.OriginalDamage, item.damage));
			}
			if (Main.netMode == NetmodeID.SinglePlayer && (Imbue is DeathMagic || SecondImbue is DeathMagic) && (target.lifeMax < (player.statLifeMax2 * 2)))
			{
				target.StrikeInstantKill();
			}
			if (Imbue is PowderFist)
			{
				Projectile.NewProjectile(item.GetSource_ItemUse(player), target.Center, Vector2.Zero, ModContent.ProjectileType<PowderExplosion>(), damageDone / 2, 3f, player.whoAmI);
			}
		}

		public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
		{
			if (!Main.dedServ && player.meleeEnchant == GelBuff.meleeEnchantID && (item.DamageType.CountsAsClass(DamageClass.Melee) || item.DamageType == DamageClass.SummonMeleeSpeed))
			{
				player.ArcaneOdyssey()?.Gel?.Effects(hitbox);
			}
			thisItem = item;
			owner = player;
			if (!CanBeAffected)
				return;
			if (Imbue is not null && Imbue.PreEffects(item))
			{
				Imbue.LingeringEffects(hitbox, Vector2.Zero, item);
			}
			if (SecondImbue is not null && SecondImbue.PreEffects(item))
				SecondImbue.LingeringEffects(hitbox, Vector2.Zero, item);
		}

		public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			thisItem = item;
			owner = player;
			if (!CanBeAffected)
				return;

			if (item.ModItem is AOWeapon weap)
			{
				if (weap.WeaponDebuff.HasValue)
				{
					target.AddBuff(weap.WeaponDebuff.Value.debuffID, weap.WeaponDebuff.Value.debuffDuration);
				}
			}

			if (Imbue is not null)
			{
				modifiers = AOUtils.CalculateImbueDamage(Imbue, target, modifiers);
				modifiers = AOUtils.CalculateImbueDamage(SecondImbue, target, modifiers);
			}
		}

		public static int[] SizeStats = ItemID.Sets.Factory.CreateIntSet([
			ItemID.MoltenBreastplate, 7, 
			ItemID.MoltenGreaves, 5, 
			ItemID.MoltenHelmet, 3,
			]);

		public static int[] HasteStats = ItemID.Sets.Factory.CreateIntSet();

		public override void UpdateEquip(Item item, Player player)
		{
			if (ArcaneOdysseyConfig.Instance.VanillaItemTemperatures)
			{
				if (item.type < ItemID.Count) // only vanilla items have entries
				{
					if (SizeStats[item.type] > 0)
					{
						player.ArcaneOdyssey().AOSizeStat += SizeStats[item.type];
					}
					if (HasteStats[item.type] > 0)
					{
						player.ArcaneOdyssey().AOHasteStat += HasteStats[item.type];
					}
				}
			}
		}
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			if (item.type == ItemID.WoodenCrate || item.type == ItemID.WoodenCrateHard)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunkenScrap>(), 25));
			}
			else if (item.type == ItemID.IronCrate || item.type == ItemID.IronCrateHard)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunkenScrap>(), 20));
			}
			else if (item.type == ItemID.GoldenCrate || item.type == ItemID.GoldenCrateHard)
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunkenScrap>(), 15));
			}
			else if (ItemID.Sets.IsFishingCrateHardmode[item.type])
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunkenScrap>(), 5));
			}
			else if (ItemID.Sets.IsFishingCrate[item.type])
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunkenScrap>(), 10));
			}

			if (ItemID.Sets.BossBag[item.type] && !ItemID.Sets.PreHardmodeLikeBossBag[item.type])
			{
				LeadingConditionRule leadingConditionRule1 = new(new Conditions.TenthAnniversaryIsUp());
				leadingConditionRule1.OnSuccess(ItemDropRule.Common(ModContent.ItemType<KindraBlade>(), 16), true);
				itemLoot.Add(leadingConditionRule1);
				LeadingConditionRule leadingConditionRule2 = new(new Conditions.TenthAnniversaryIsNotUp());
				leadingConditionRule2.OnSuccess(ItemDropRule.Common(ModContent.ItemType<KindraBlade>(), 32), true);
				itemLoot.Add(leadingConditionRule2);
				//LeadingConditionRule leadingConditionRule3 = new(new Conditions.TenthAnniversaryIsUp());
				//leadingConditionRule3.OnSuccess(ItemDropRule.Common(ModContent.ItemType<VesuvianSigil>(), 8), true);
				//itemLoot.Add(leadingConditionRule3);
				//LeadingConditionRule leadingConditionRule4 = new(new Conditions.TenthAnniversaryIsNotUp());
				//leadingConditionRule4.OnSuccess(ItemDropRule.Common(ModContent.ItemType<VesuvianSigil>(), 16), true);
				//itemLoot.Add(leadingConditionRule4);
				LeadingConditionRule leadingConditionRule5 = new(new Conditions.TenthAnniversaryIsUp());
				leadingConditionRule5.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ElfPetItem>(), 16), true);
				itemLoot.Add(leadingConditionRule5);
				LeadingConditionRule leadingConditionRule6 = new(new Conditions.TenthAnniversaryIsNotUp());
				leadingConditionRule6.OnSuccess(ItemDropRule.Common(ModContent.ItemType<ElfPetItem>(), 32), true);
				itemLoot.Add(leadingConditionRule6);
			}

			if (ItemID.Sets.OpenableBag[item.type])
			{
				LeadingConditionRule AcrimonyCondition = new(new NoShowNoConditon());
				AcrimonyCondition.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Acrimony>(), 500));
				itemLoot.Add(AcrimonyCondition);
			}
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			var dashline = tooltips.Find(e => e.Text.Contains("{AODASHBIND}"));
			if (dashline is not null)
			{
				tooltips[tooltips.IndexOf(dashline)].Text = dashline.Text.Replace("{AODASHBIND}", AOKeybinds.DashBind.GetAssignedKeys(InputMode.Keyboard).FirstOrDefault(Mod.CustomLocalization("RandomWords.Unbound").Value));
			}
			if ((item.ModItem is not null && item.ModItem.Name == "UnloadedItem") || !item.ArcaneOdyssey().CanBeAffected)
			{
				return;
			}

			if (ArcaneOdysseyClientConfig.Instance.ItemTypeTooltips)
			{
				if (item.GetItemType() == ItemType.Material)
				{
					tooltips.RemoveAll(e => e.Name == "Material" && e.Mod == "Terraria");
				}
				else if (item.GetItemType() == ItemType.Vanity)
				{
					tooltips.RemoveAll(e => e.Name == "Vanity" && e.Mod == "Terraria");
				}
				else if (item.GetItemType() == ItemType.Ammo)
				{
					tooltips.RemoveAll(e => e.Name == "Ammo" && e.Mod == "Terraria");
				}

				if (item.ModItem is not AOBaseItem || (item.ModItem is AOBaseItem based && based.ShowItemTypeTooltip))
				{
					var line = item.GetItemRare().ToString();
					line += " ";
					line += item.GetItemType().ToString().ToLower();
					tooltips.Insert(1, new TooltipLine(Mod, "ItemTypeLine", line));
				}
			}

			if (ArcaneOdysseyConfig.Instance.VanillaItemTemperatures)
			{
				if (item.type < ItemID.Count) // only vanilla items have entries
				{
					if (SizeStats[item.type] > 0)
					{
						tooltips.AddTooltip(new(Mod, "AOSize", Mod.CustomLocalization("ArmourAutoTooltip.Size", Math.Round(SizeStats[item.type] / 2.75f)).Value));
					}
					if (HasteStats[item.type] > 0)
					{
						tooltips.AddTooltip(new(Mod, "AOHaste", Mod.CustomLocalization("ArmourAutoTooltip.Haste", Math.Round(HasteStats[item.type] / 2f)).Value));
					}
				}
			}

			if (item.ModItem is AOWeapon weapon)
			{
				if (weapon.Ability.HasValue)
				{
					string text = $"[c/{weapon.Ability.Value.Colour.Hex3()}:{weapon.Ability.Value.Name}]";
					if (weapon.Ability.Value.Description is not null)
					{
						text += $": {weapon.Ability.Value.Description}";
					}
					tooltips.AddTooltip(new(weapon.Mod, "AOAbility", text));
				}
			}

			switch (item.ArcaneOdyssey().WeaponsType)
			{
				case WeaponType.Artisinal:
					tooltips.AddTooltip(new TooltipLine(Mod, "ArtisinalIndicator", Mod.CustomLocalization("ImbueStuff.ArtisinalIndicator").Value));
					return;
				case WeaponType.Arcanium:
					tooltips.AddTooltip(new TooltipLine(Mod, "ArcaniumIndicator", Mod.CustomLocalization("ImbueStuff.ArcaniumIndicator").Value));
					return;
				case WeaponType.Strength:
					tooltips.AddTooltip(new TooltipLine(Mod, "StrengthIndicator", Mod.CustomLocalization("ImbueStuff.StrengthIndicator").Value));
					return;
			}
		}
	}
}
