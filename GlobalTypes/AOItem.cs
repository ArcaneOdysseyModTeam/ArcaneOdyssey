using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Accessories.Vanity;
using ArcaneOdyssey.Items.Armour.Vanity.Taz;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Consumable;
using ArcaneOdyssey.Items.EmptyScrolls;
using ArcaneOdyssey.Items.Equipment.Pets;
using ArcaneOdyssey.Items.Materials;
using ArcaneOdyssey.Items.Scrolls.Equipment.Common;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Berserker.Effects;
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
using Terraria.ModLoader.Default;

namespace ArcaneOdyssey.GlobalTypes
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

		public float ApplySize(float value, bool flipfloat = false)
		{
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

		public WeaponType _weaponsType;
		public WeaponType WeaponsType
		{
			get
			{
				if (thisItem is not null && thisItem.ModItem is Weapon weap)
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
				if (thisItem is not null && thisItem.ModItem is Weapon item)
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
				if (thisItem is not null && thisItem.ModItem is Weapon weap)
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
				Vector2 dimensions = new(Math.Max(frame.Width, frame.Height));
				Vector2 location = position + (dimensions * .5f * scale);

				spriteBatch.Draw(texture.Value, location, null, Color.White, 0, texture.Value.Size() / 2f, .3f * (52f / Math.Max(texture.Width(), texture.Height())), SpriteEffects.None, 1f);

				if (Imbue is FightingStyleBarred fs && item.ModItem?.Type != Imbue.Type)
				{
					spriteBatch.DrawString(FontAssets.ItemStack.Value, $"{fs.BarValue.Round()}%", location - (FontAssets.ItemStack.Value.MeasureString($"{fs.BarValue.Round()}%") / 2), Color.Lerp(fs.DisplayColor, fs.ImbueColour, fs.LerpValue));
				}

				if (SecondImbue is not null && ModContent.RequestIfExists<Texture2D>(SecondImbue.ImbueUISprite, out var texture2))
				{
					dimensions.X *= -1f;
					location = position + (dimensions * .5f * scale);

					spriteBatch.Draw(texture2.Value, location, null, Color.White, 0, texture2.Value.Size() / 2f, .3f * (52f / Math.Max(texture2.Width(), texture2.Height())), SpriteEffects.None, 1f);
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
			if (!CanBeAffected)
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
			if (!CanBeAffected)
				return;

			if (item.ModItem is Imbuable imbue)
			{
				knockback *= imbue.ScrollSize * imbue.ScrollSize;
				if (imbue.Imbue is not null)
					knockback *= imbue.Imbue.ImbueDamage * imbue.Imbue.ImbueDamage;
				var extraknockbackmulti = imbue.KBMulti;
				if (imbue.Imbue is not null)
					extraknockbackmulti *= imbue.Imbue.KBMulti;
				knockback *= extraknockbackmulti;
			}
			else if (Imbue is not null)
			{
				if (BenifitsFromScrollStats.GetValueOrDefault())
				{
					knockback *= Imbue.ScrollSize;
					if (SecondImbue is not null)
						knockback *= SecondImbue.ScrollSize;
				}
				else if (item.ModItem is null or BaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					knockback *= Imbue.ImbueSize * Imbue.ImbueSize;
					if (SecondImbue is not null)
						knockback *= SecondImbue.ImbueSize * SecondImbue.ImbueDamage;
				}
				var extraknockbackmulti = Imbue.KBMulti;
				if (SecondImbue is not null)
					extraknockbackmulti *= SecondImbue.KBMulti;
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
			if (ArcaneOdysseyConfig.Instance.AffectsOtherMods && item.ModItem is not null or BaseItem)
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
			if (item.ModItem is null or BaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
			{
				scale *= player.ArcaneOdyssey().SizeMulti;
				if (Imbue is not null)
				{
					if (!BenifitsFromScrollStats.GetValueOrDefault())
					{
						scale *= Imbue.ImbueSize;
						if (SecondImbue is not null)
							scale *= SecondImbue.ImbueSize;
					}
					else
					{
						scale *= Imbue.ScrollSize;
						if (SecondImbue is not null)
							scale *= SecondImbue.ImbueSize;
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
					return imbue.ScrollSpeed * (imbue.Imbue?.ImbueSpeed ?? 1f);
				}
				if (!item.DamageType.Name.Contains("NoSpeed"))
				{
					if (Imbue is not null)
					{
						if (BenifitsFromScrollStats.GetValueOrDefault())
						{
							return Imbue.ScrollSpeed * (SecondImbue?.ImbueSpeed ?? 1f);
						}

						if (item.ModItem is null or BaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods)
						{
							return Imbue.ImbueSpeed * (SecondImbue?.ImbueSpeed ?? 1f);
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

			if (item.type == ModContent.ItemType<SpiritEnergy>())
			{
				item.color = (item.ModItem as SpiritEnergy).SpiritColor;
			}

			if (Main.myPlayer != player.whoAmI)
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
					player.ArcaneOdyssey().StatSize += ArcaneOdysseyMod.Sets.SizeStats[item.type];
				}
				if (ArcaneOdysseyMod.Sets.HasteStats[item.type] > 0)
				{
					player.ArcaneOdyssey().StatHaste += ArcaneOdysseyMod.Sets.HasteStats[item.type];
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

			if (item.type == ItemID.GoldenCrateHard)
			{
				if (!addedScrap)
				{
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunkenScrap>(), 15));
					addedScrap = true;
				}
			}

			if (ItemID.Sets.IsFishingCrateHardmode[item.type])
			{
				if (!addedScrap)
				{
					itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<SunkenScrap>(), 5));
					addedScrap = true;
				}
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<RareScroll>(), 20));
			}

			if (ItemID.Sets.IsFishingCrate[item.type])
			{
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EmptyScroll>(), 10));
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
			if (item.ModItem is UnloadedItem || !item.ArcaneOdyssey().CanBeAffected)
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
					tooltips.AddTooltip(new(Mod, "Size", Mod.CustomLocalization("ArmourAutoTooltip.Size", Math.Round(ArcaneOdysseyMod.Sets.SizeStats[item.type] / Armour.SizeDivision)).Value));
				}
				if (ArcaneOdysseyMod.Sets.HasteStats[item.type] > 0)
				{
					tooltips.AddTooltip(new(Mod, "Haste", Mod.CustomLocalization("ArmourAutoTooltip.Haste", Math.Round(ArcaneOdysseyMod.Sets.HasteStats[item.type] / Armour.HasteDivision)).Value));
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
}
