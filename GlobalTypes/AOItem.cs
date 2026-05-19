using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Biomes;
using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Imbues.Magic.Lost;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Accessories.Vanity;
using ArcaneOdyssey.Items.Armour.Vanity.Taz;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Consumable;
using ArcaneOdyssey.Items.EmptyScrolls;
using ArcaneOdyssey.Items.Equipment.Pets;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Items.Scrolls.Equipment.Common;
using ArcaneOdyssey.Items.Weapons.Atlantean;
using ArcaneOdyssey.Prefixes;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Berserker.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.GlobalTypes
{
	public class AOItem : GlobalItem, IImbuable
	{
		public ItemType ItemType => thisItem?.GetItemType() ?? ItemType.Item;

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
							value *= Imbue.ScrollSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed;
						}
						else
						{
							value *= Imbue.ScrollSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ImbueSpeed;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed;
						}
						else
						{
							value *= Imbue.ImbueSpeed.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSpeed.FlipFloat();
						}
					}
				}
			}
			return value;
		}

		/// <summary>
		/// Adds atlantean essence to an item
		/// </summary>
		/// <returns>Whether it was applied successfully</returns>
		public bool AddAtlanteanEssense()
		{
			if (CanHaveAtlanteanEssence())
			{
				if (thisItem.accessory)
				{
					return thisItem.Prefix(ModContent.PrefixType<AtlanteanPrefix>());
				}
				if (!ArcaneOdysseyMod.Sets.atlanteanItem[thisItem.type])
				{
					if (ArcaneOdysseyMod.Sets.greatsword[thisItem.type])
					{
						thisItem.SetDefaults(ModContent.ItemType<AtlanteanGreatsword>());
						return true;
					}
				}
			}
			return false;
		}

		public bool CanHaveAtlanteanEssence()
		{
			if (thisItem is not null)
			{
				if (thisItem.accessory && thisItem.CanHavePrefixes() && thisItem.CanApplyPrefix(ModContent.PrefixType<AtlanteanPrefix>()) && thisItem.prefix != ModContent.PrefixType<AtlanteanPrefix>())
				{
					return true;
				}
				if (!ArcaneOdysseyMod.Sets.atlanteanItem[thisItem.type])
				{
					if (ArcaneOdysseyMod.Sets.greatsword[thisItem.type])
					{
						return true;
					}
				}
			}
			return false;
		}

		public enum RandomBoostType
		{
			Power,
			Defense,
			Speed,
			Agility,
			Size,
			Haste,
			Pierce,
			Mana,
			Minions
		}

		public override void UpdateAccessory(Item item, Player player, bool hideVisual)
		{
			if (Boost is not null)
			{
				switch (Boost.Value)
				{
					case RandomBoostType.Power:
						player.GetDamage(DamageClass.Generic) += .06f;
						player.GetCritChance(DamageClass.Generic) += .05f;
						break;
					case RandomBoostType.Defense:
						player.statDefense += 8;
						break;
					case RandomBoostType.Agility:
						player.moveSpeed += .075f;
						break;
					case RandomBoostType.Size:
						player.ArcaneOdyssey().StatSize += 25;
						break;
					case RandomBoostType.Haste:
						player.ArcaneOdyssey().StatHaste += 25;
						break;
					case RandomBoostType.Pierce:
						player.GetArmorPenetration(DamageClass.Generic) += 3;
						break;
					case RandomBoostType.Mana:
						player.statManaMax2 += 40;
						break;
					case RandomBoostType.Minions:
						player.maxMinions += 2;
						break;
					case RandomBoostType.Speed:
						player.GetAttackSpeed(DamageClass.Generic) += .075f;
						break;
				}
			}
		}

		public override void SaveData(Item item, TagCompound tag)
		{
			thisItem = item;
			if (AtlanteanApplied)
			{
				tag.Add("atlantean", (int)Boost);
			}
		}

		public override void NetSend(Item item, BinaryWriter writer)
		{
			thisItem = item;
			writer.Write(Imbue?.Type ?? ItemID.None);
			writer.Write(SecondImbue?.Type ?? ItemID.None);
			if (Boost.HasValue)
			{
				writer.Write((sbyte)Boost);
			}
			else
			{
				writer.Write((sbyte)-1);
			}
		}

		public override void NetReceive(Item item, BinaryReader reader)
		{
			thisItem = item;
			Imbue = AOUtils.SafeImbuable(ModContent.GetModItem(reader.ReadInt32()));
			SecondImbue = AOUtils.SafeImbuable(ModContent.GetModItem(reader.ReadInt32()));
			var boost = reader.ReadSByte();
			Boost = boost == -1 ? null : (RandomBoostType)boost;
		}

		public override void LoadData(Item item, TagCompound tag)
		{
			thisItem = item;
			if (tag.ContainsKey("atlantean"))
			{
				Boost = (RandomBoostType)tag.GetInt("atlantean");
			}
		}

		public bool AtlanteanApplied => thisItem.prefix == ModContent.PrefixType<AtlanteanPrefix>();
		public RandomBoostType? Boost = null;

		public float ApplySize(float value, bool flipfloat = false)
		{
			value *= owner?.ArcaneOdyssey()?.SizeMulti ?? 1f;
			if (BenifitsFromScrollStats.HasValue)
			{
				if (BenifitsFromScrollStats.Value)
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ScrollSize;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize;
						}
						else
						{
							value *= Imbue.ScrollSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ImbueSize;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize;
						}
						else
						{
							value *= Imbue.ImbueSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize.FlipFloat();
						}
					}
				}
			}
			return value;
		}

		public float ApplyKnockback(float value, bool flipfloat = false)
		{
			if (BenifitsFromScrollStats.HasValue)
			{
				if (Imbue is not null)
				{
					value *= Imbue.KBMulti;
					if (SecondImbue is not null)
						value *= SecondImbue.KBMulti;
				}
				if (BenifitsFromScrollStats.Value)
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ScrollSize * Imbue.ScrollSize;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize * SecondImbue.ImbueSize;
						}
						else
						{
							value *= Imbue.ScrollSize.FlipFloat() * Imbue.ScrollSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize.FlipFloat() * SecondImbue.ImbueSize.FlipFloat();
						}
					}
				}
				else
				{
					if (Imbue is not null)
					{
						if (!flipfloat)
						{
							value *= Imbue.ImbueSize * Imbue.ImbueSize;
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize * SecondImbue.ImbueSize;
						}
						else
						{
							value *= Imbue.ImbueSize.FlipFloat();
							if (SecondImbue is not null)
								value *= SecondImbue.ImbueSize.FlipFloat() * SecondImbue.ImbueSize.FlipFloat();
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

		public override bool CanUseItem(Item item, Player player)
		{
			if (WeaponsType == WeaponType.Arcanium)
			{
				return Imbue is MagicType;
			}

			if (WeaponsType == WeaponType.Strength)
			{
				return Imbue is FightingStyle;
			}

			return true;
		}

		public ref WeaponType WeaponsType => ref ArcaneOdysseyMod.Sets.weaponType[thisItem?.type ?? 0];

		public bool? BenifitsFromScrollStats
		{
			get
			{
				if (thisItem is not null)
				{
					if (thisItem.CanHaveImbue(Imbue))
					{
						if (WeaponsType == WeaponType.Artisinal)
							return null;
						return thisItem.ModItem is Scroll and not AuraScroll || WeaponsType != WeaponType.Normal;
					}
				}
				return null;
			}
		}

		public bool canBeAffected = true;

		public override void ApplyPrefix(Item item, int pre)
		{
			if (pre == ModContent.PrefixType<AtlanteanPrefix>())
			{
				Boost ??= Main.rand.Next(Enum.GetValues<RandomBoostType>());
			}
			else
			{
				Boost = null;
			}
		}

		public ref bool? Cold => ref ArcaneOdysseyMod.Sets.cold[thisItem?.type ?? 0];

		public override GlobalItem Clone(Item from, Item to)
		{
			var clone = (AOItem)base.Clone(from, to);
			clone.Imbue = Imbue;
			clone.SecondImbue = SecondImbue;
			clone.thisItem = to;
			clone.canBeAffected = canBeAffected;
			clone.Boost = Boost;
			return clone;
		}

		public static Asset<Texture2D> AtlanteanIndicator;

		public override void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
		{
			if (Imbue is EnergyMagic)
			{
				mult *= 0;
			}
		}

		public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (AOUtils.RequestIfExists(Mod.Name + "/Assets/AtlanteanIndicator", ref AtlanteanIndicator))
			{
				if (Main.LocalPlayer.HasTypeInInventory<AtlanteanEssence>() && CanHaveAtlanteanEssence())
				{
					spriteBatch.Draw(AtlanteanIndicator.Value, position, null, Color.White * .5f, 0, AtlanteanIndicator.Size() / 2f, Main.inventoryScale * 1.1f, SpriteEffects.None, 1f);
				}

				if (AtlanteanApplied)
				{
					spriteBatch.Draw(AtlanteanIndicator.Value, position, null, Color.White * .75f, 0, AtlanteanIndicator.Size() / 2f, Main.inventoryScale * 1.1f, SpriteEffects.None, 1f);
				}
			}

			return base.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}

		public override void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			thisItem = item;

			if (Imbue is null || !canBeAffected)
				return;

			if (ModContent.RequestIfExists<Texture2D>(Imbue.ImbueUISprite, out var texture) && Imbue.Type != item.type)
			{
				var imbueScale = 52f / Math.Max(texture.Width(), texture.Height());
				Vector2 dimensions = new(Math.Max(frame.Width, frame.Height));
				Vector2 location = position + (dimensions * .5f * scale);

				spriteBatch.Draw(texture.Value, location, null, Color.White, 0, texture.Value.Size() / 2f, Main.inventoryScale * .5f * imbueScale, SpriteEffects.None, 1f);

				if (Imbue is FightingStyleBarred fs)
				{
					var textScale = Main.inventoryScale * .75f;
					spriteBatch.DrawString(FontAssets.ItemStack.Value, $"{fs.BarValue.Round()}%", location, Color.Lerp(fs.DisplayColor, fs.ImbueColour, fs.LerpValue), 0f, FontAssets.ItemStack.Value.MeasureString($"{fs.BarValue.Round()}%") / 2f, textScale, SpriteEffects.None, 0f);
				}

				if (SecondImbue is not null && ModContent.RequestIfExists<Texture2D>(SecondImbue.ImbueUISprite, out var texture2))
				{
					dimensions.X *= -1f;
					location = position + (dimensions * .5f * scale);

					spriteBatch.Draw(texture2.Value, location, null, Color.White, 0, texture2.Value.Size() / 2f, Main.inventoryScale * .5f * imbueScale, SpriteEffects.None, 1f);
				}
			}
		}

		public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			thisItem = item;
			owner = player;
			if (!canBeAffected)
				return;

			if (item.ModItem is Imbuable imbue)
			{
				if (!item.DamageType.Name.Contains("NoSpeed"))
				{
					velocity *= imbue.ScrollSpeed;
					if (imbue.Imbue is not null)
						velocity *= imbue.Imbue.ImbueSpeed;
				}
			}
			else if (Imbue is not null)
			{
				if (!item.DamageType.Name.Contains("NoSpeed"))
				{
					if (BenifitsFromScrollStats.GetValueOrDefault())
					{
						velocity *= Imbue.ScrollSpeed;
						if (SecondImbue is not null)
							velocity *= SecondImbue.ImbueSpeed;
					}
					else if (item.ModItem is null or BaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
					{
						velocity *= Imbue.ImbueSpeed;
						if (SecondImbue is not null)
							velocity *= SecondImbue.ImbueSpeed;
					}
				}
			}
		}

		public override void ModifyWeaponCrit(Item item, Player player, ref float crit)
		{
			thisItem = item;
			owner = player;
			if (!canBeAffected)
				return;

			if (item.ModItem is Imbuable imbue)
			{
				crit *= imbue.ScrollDamage;
				if (imbue.Imbue is not null)
					crit *= imbue.Imbue.ImbueDamage;
				if (imbue is VanishingStyle vanish && vanish.BarValue > FightingStyleBarred.BarMin)
					if (!player.ArcaneOdyssey().OnCooldown(vanish.Name))
						crit = 100;
			}
			else if (Imbue is not null)
			{
				if (BenifitsFromScrollStats.GetValueOrDefault())
				{
					crit *= Imbue.ScrollDamage;
					if (SecondImbue is not null)
						crit *= SecondImbue.ImbueDamage;
				}
				else
				{
					crit *= Imbue.ImbueDamage;
					if (SecondImbue is not null)
						crit *= SecondImbue.ImbueDamage;
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
			if (!canBeAffected)
				return;

			if (item.ModItem is Imbuable imbue)
			{
				knockback *= imbue.ScrollSize * imbue.ScrollSize;
				if (imbue.Imbue is not null)
					knockback *= imbue.Imbue.ScrollSize * imbue.Imbue.ScrollSize;
				knockback *= imbue.KBMulti;
				if (imbue.Imbue is not null)
					knockback *= imbue.Imbue.KBMulti;
			}
			else if (Imbue is not null)
			{
				if (BenifitsFromScrollStats.GetValueOrDefault())
				{
					knockback *= Imbue.ScrollSize * Imbue.ScrollSize;
					if (SecondImbue is not null)
						knockback *= SecondImbue.ImbueSize * SecondImbue.ImbueSize;
				}
				else if (item.ModItem is null or BaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					knockback *= Imbue.ImbueSize * Imbue.ImbueSize;
					if (SecondImbue is not null)
						knockback *= SecondImbue.ImbueSize * SecondImbue.ImbueSize;
				}
				knockback *= Imbue.KBMulti;
				if (SecondImbue is not null)
					knockback *= SecondImbue.KBMulti;
			}
		}

		public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
		{
			thisItem = item;
			owner = player;
			if (!canBeAffected)
				return;
			if (item.ModItem is Scroll)
			{
				damage += ((item.damage + (AOUtils.BossesKilled * 2f)) / item.damage) - 1;
			}

			if (item.ModItem is Imbuable imbue)
			{
				damage *= imbue.ScrollDamage;
				if (imbue.Imbue is not null)
					damage *= imbue.Imbue.ImbueDamage;
			}
			else if (Imbue is not null)
			{
				if (BenifitsFromScrollStats.GetValueOrDefault())
				{
					damage *= Imbue.ScrollDamage;
					if (SecondImbue is not null)
						damage *= SecondImbue.ImbueDamage;
				}
				else if (item.ModItem is null or BaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					damage *= Imbue.ImbueDamage;
					if (SecondImbue is not null)
						damage *= SecondImbue.ImbueDamage;
				}
			}
		}

		public override void SetDefaults(Item item)
		{
			if (!item.active || item.IsAir || string.IsNullOrWhiteSpace(item.Name))
				return;
			thisItem = item;
			owner = null;
			if (ArcaneOdysseyMod.Sets.excludedItem[item.type])
			{
				canBeAffected = false;
			}
		}

		public override void ModifyItemScale(Item item, Player player, ref float scale)
		{
			thisItem = item;
			owner = player;
			if (item.noMelee || !canBeAffected)
				return;
			if (item.ModItem is null or BaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
			{
				scale = ApplySize(scale);
			}
		}

		public override float UseSpeedMultiplier(Item item, Player player)
		{
			thisItem = item;
			owner = player;
			float mult = 1f;
			if (canBeAffected)
			{
				if (item.ModItem is Imbuable imbue)
				{
					mult *= imbue.ScrollSpeed;
					if (imbue.Imbue is not null)
					{
						mult *= imbue.Imbue.ImbueSpeed;
					}
				}
				else if (ItemID.Sets.Spears[item.type] || (!item.DamageType.Name.Contains("NoSpeed")))
				{
					if (Imbue is not null)
					{
						if (BenifitsFromScrollStats.HasValue)
						{
							if (!BenifitsFromScrollStats.Value)
							{
								mult *= Imbue.ImbueSpeed;
								if (SecondImbue is not null)
									mult *= SecondImbue.ImbueSpeed;
							}
							else
							{
								mult *= Imbue.ScrollSpeed;
								if (SecondImbue is not null)
									mult *= SecondImbue.ImbueSpeed;
							}
						}
					}
				}
			}
			return mult;
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

			if (!canBeAffected)
				return;

			if (item.type == ModContent.ItemType<SpiritEnergy>())
			{
				item.color = (item.ModItem as SpiritEnergy).SpiritColor;
			}

			if (Main.myPlayer != player.whoAmI)
				return;

			if (player.ItemAnimationActive && player.PlayerItem()?.ModItem is not Imbuable)
				return;

			List<Imbuable> options = [null, .. player.GetAllImbues(), .. player.ArcaneOdyssey().AllEquippedImbues()];
			options.RemoveAll(e => !item.CanHaveImbue(e));
			bool justchangedspecificimbue = false;
			bool settodefault = false;

			if (item.TryGetSecondImbue(Imbue, out var second5))
				SecondImbue = second5;

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
				if ((!specificImbue) || (item.accessory && item.ModItem is not Imbuable))
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

				if (((!item.accessory) || item.ModItem is Imbuable) && player.PlayerItem() == item && AOKeybinds.CycleItemImbue.JustPressed && !player.ArcaneOdyssey().OnCooldown("CycleImbueCooldown"))
				{
					if (options.Count > 1)
					{
						specificImbue = true;
						player.ArcaneOdyssey()?.SetCooldown(new Cooldown("CycleImbueCooldown", AOKeybinds.CycleItemImbue.DisplayName, 60));
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

						if (Imbue is MagicType magic)
						{
							Imbuable.CreateMagicCircle(Imbue.Item, player, MagicCircleMode.Rotating, true);
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

			if (justchangedspecificimbue)
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
			if (!canBeAffected)
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
			if (player.meleeEnchant == GelBuff.meleeEnchantID && (item.DamageType.CountsAsClass(DamageClass.Melee) || item.DamageType == DamageClass.SummonMeleeSpeed))
			{
				player.ArcaneOdyssey()?.Gel?.Effects(hitbox);
			}

			thisItem = item;
			owner = player;

			if (!canBeAffected)
				return;

			int imbue1 = 0;
			int imbue2 = 0;

			if (Imbue is not null && Imbue.PreEffects(item))
			{
				imbue1 = Imbue.Type;
				Imbue.LingeringEffects(hitbox, Vector2.Zero, item);
			}
			if (SecondImbue is not null && SecondImbue.PreEffects(item))
			{
				imbue2 = SecondImbue.Type;
				SecondImbue.LingeringEffects(hitbox, Vector2.Zero, item);
			}

			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				var packet = Mod.GetPacket();
				packet.Write(ArcaneOdysseyMod.PacketID.LingeringVisuals);
				packet.Write(imbue1);
				packet.Write(imbue2);
				packet.Write(hitbox);
				packet.Send();
			}
		}

		public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			thisItem = item;
			owner = player;
			if (!canBeAffected)
				return;

			if (item.ModItem is Weapon weap)
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

		public override void UpdateEquip(Item item, Player player)
		{
			if (ArcaneOdysseyConfig.Instance.VanillaItemTemperatures)
			{
				if (ArcaneOdysseyMod.Sets.SizeStats[item.type] > 0)
				{
					player.ArcaneOdyssey().StatSize += (short)ArcaneOdysseyMod.Sets.SizeStats[item.type];
				}
				if (ArcaneOdysseyMod.Sets.HasteStats[item.type] > 0)
				{
					player.ArcaneOdyssey().StatHaste += (short)ArcaneOdysseyMod.Sets.HasteStats[item.type];
				}
			}
		}

		public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			bool addedScrap = false;

			if (item.type == ItemID.WoodenCrateHard)
			{
				if (!addedScrap)
				{
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunkenScrap>(), 25));
					addedScrap = true;
				}
			}

			if (item.type == ItemID.IronCrateHard)
			{
				if (!addedScrap)
				{
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunkenScrap>(), 20));
					addedScrap = true;
				}
			}

			if (item.type == ItemID.GoldenCrate)
			{
				itemLoot.Add(new AnyDropHelper(ArcaneOdysseyMod.Sets.OldWeapons, 5));
			}

			if (item.type == ItemID.GoldenCrateHard)
			{
				if (!addedScrap)
				{
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunkenScrap>(), 15));
					addedScrap = true;
				}
				itemLoot.Add(new AnyDropHelper(ArcaneOdysseyMod.Sets.OldWeapons, 5));
			}

			if (ItemID.Sets.IsFishingCrateHardmode[item.type])
			{
				if (!addedScrap)
				{
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunkenScrap>(), 5));
					addedScrap = true;
				}
			}

			if (ItemID.Sets.IsFishingCrate[item.type])
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<LostEmptyScroll>(), 10));
			}

			if (ItemID.Sets.BossBag[item.type])
			{
				if (!ItemID.Sets.PreHardmodeLikeBossBag[item.type])
				{
					LeadingConditionRule devItems1 = new(new Conditions.TenthAnniversaryIsNotUp());
					devItems1.OnSuccess(new AnyDropHelper([ModContent.ItemType<KindraBlade>(), ModContent.ItemType<ElfPetItem>()], 16), true);
					itemLoot.Add(devItems1);
					LeadingConditionRule devItems2 = new(new Conditions.TenthAnniversaryIsUp());
					devItems2.OnSuccess(new AnyDropHelper([ModContent.ItemType<KindraBlade>(), ModContent.ItemType<ElfPetItem>()], 8), true);
					itemLoot.Add(devItems2);
				}
				else
				{
					LeadingConditionRule devItems1 = new(new Conditions.TenthAnniversaryIsNotUp());
					devItems1.OnSuccess(new MultiAnyDropHelper([[ModContent.ItemType<TazBoots>(), ModContent.ItemType<TazChest>(), ModContent.ItemType<TazHat>()]], 16), true);
					itemLoot.Add(devItems1);
					LeadingConditionRule devItems2 = new(new Conditions.TenthAnniversaryIsUp());
					devItems2.OnSuccess(new MultiAnyDropHelper([[ModContent.ItemType<TazBoots>(), ModContent.ItemType<TazChest>(), ModContent.ItemType<TazHat>()]], 8), true);
					itemLoot.Add(devItems2);
				}
			}

			if (itemLoot.Get().Count > 0)
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


			if (AtlanteanApplied && Boost.HasValue)
			{
				var tip = tooltips.Find(e => e.Mod == Mod.Name && e.Name == "RandomStat" && e.IsModifier == true);
				if (tip is not null)
				{
					switch (Boost.Value)
					{
						case RandomBoostType.Power:
							tip.Text = Mod.CustomLocalization($"ArmourAutoTooltip.{Boost}", 6, 5).Value;
							break;
						case RandomBoostType.Defense:
							tip.Text = Mod.CustomLocalization($"ArmourAutoTooltip.{Boost}", 8).Value;
							break;
						case RandomBoostType.Agility:
							tip.Text = Mod.CustomLocalization($"ArmourAutoTooltip.{Boost}", 7.5).Value;
							break;
						case RandomBoostType.Size:
							tip.Text = Mod.CustomLocalization($"ArmourAutoTooltip.{Boost}", Math.Round(25 / BaseArmour.SizeDivision, 1)).Value;
							break;
						case RandomBoostType.Haste:
							tip.Text = Mod.CustomLocalization($"ArmourAutoTooltip.{Boost}", Math.Round(25 / BaseArmour.HasteDivision, 1)).Value;
							break;
						case RandomBoostType.Pierce:
							tip.Text = Mod.CustomLocalization($"ArmourAutoTooltip.{Boost}", 3).Value;
							break;
						case RandomBoostType.Mana:
							tip.Text = Mod.CustomLocalization($"ArmourAutoTooltip.{Boost}", 40).Value;
							break;
						case RandomBoostType.Minions:
							tip.Text = Mod.CustomLocalization($"ArmourAutoTooltip.{Boost}", 2).Value;
							break;
						case RandomBoostType.Speed:
							tip.Text = Mod.CustomLocalization($"ArmourAutoTooltip.{Boost}", 7.5).Value;
							break;
					}
				}
			}

			if (item.ModItem is UnloadedItem || !item.ArcaneOdyssey().canBeAffected)
			{
				return;
			}

			if (ArcaneOdysseyClientConfig.Instance.ItemTypeTooltips)
			{
				if (item.GetItemType() == ItemType.Material)
				{
					tooltips.Find(e => e.Name == "Material" && e.Mod == "Terraria")?.Hide();
				}
				else if (item.GetItemType() == ItemType.Vanity)
				{
					tooltips.Find(e => e.Name == "Vanity" && e.Mod == "Terraria")?.Hide();
				}
				else if (item.GetItemType() == ItemType.Ammo)
				{
					tooltips.Find(e => e.Name == "Ammo" && e.Mod == "Terraria")?.Hide();
				}
				else if (item.GetItemType() == ItemType.Consumable)
				{
					tooltips.Find(e => e.Name == "Consumable" && e.Mod == "Terraria")?.Hide();
				}

				if (item.ModItem is not BaseItem || (item.ModItem is BaseItem based && based.ShowItemTypeTooltip))
				{
					var line = item.GetItemRare().ToString();
					line += " ";
					line += item.GetItemType().ToString().ToLower();
					tooltips.Insert(1, new TooltipLine(Mod, "ItemTypeLine", line));
				}
			}

			if (ArcaneOdysseyConfig.Instance.VanillaItemTemperatures || item.ModItem is not null)
			{
				if (ArcaneOdysseyMod.Sets.SizeStats[item.type] > 0)
				{
					tooltips.AddTooltip(new(Mod, "Size", Mod.CustomLocalization("ArmourAutoTooltip.Size", Math.Round(ArcaneOdysseyMod.Sets.SizeStats[item.type] / BaseArmour.SizeDivision)).Value));
				}
				if (ArcaneOdysseyMod.Sets.HasteStats[item.type] > 0)
				{
					tooltips.AddTooltip(new(Mod, "Haste", Mod.CustomLocalization("ArmourAutoTooltip.Haste", Math.Round(ArcaneOdysseyMod.Sets.HasteStats[item.type] / BaseArmour.HasteDivision)).Value));
				}
			}

			if (item.ModItem is Weapon weapon)
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

	public class CheeseRestrictions : GlobalItem
	{
		public override bool CanUseItem(Item item, Player player)
		{
			bool inArena = player.InModBiome<EliusArena>(); // add subworlds here later
			bool illegalItemForArena = item.type is ItemID.Sandgun or ItemID.DirtBomb or ItemID.DirtStickyBomb or ItemID.DryBomb or ItemID.BottomlessShimmerBucket or ItemID.WaterBucket or ItemID.BottomlessBucket or ItemID.BottomlessHoneyBucket or ItemID.BottomlessLavaBucket;
			if (illegalItemForArena && inArena)
				return false;

			return base.CanUseItem(item, player);
		}
	}
}
