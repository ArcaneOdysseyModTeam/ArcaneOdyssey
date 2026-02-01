using ArcaneOdyssey.Content.Buffs.MagicMarks;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Normal;
using ArcaneOdyssey.Content.Projectiles;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.PlayerClasses;
using ArcaneOdysseyMusic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public static class AOUtils
	{
		public static int GetMusic(string name) => MusicLoader.GetMusicSlot(ArcaneOdysseyMusicMod.Instance, "Music/" + name);

		internal static List<string> options = [
			"FavoriteDesc", 
			"NoTransfer",
			"SocialDesc",
			"Damage",
			"CritChance",
			"Speed",
			"NoSpeedScaling",
			"SpecialSpeedScaling",
			"Knockback",
			"FishingPower",
			"NeedsBait",
			"BaitPower",
			"Equipable",
			"WandConsumes",
			"Quest",
			"Vanity",
			"Defense",
			"PickPower",
			"AxePower",
			"HammerPower",
			"TileBoost",
			"HealLife",
			"HealMana",
			"UseMana",
			"Placeable",
			"Ammo",
			"Consumable",
			"Material",
			"Tooltip",
			//"EtherianManaWarning",
			//"WellFedExpert",
			//"BuffTime",
			//"OneDropLogo",
			//"PrefixDamage",
			//"PrefixSpeed",
			//"PrefixCritChance",
			//"PrefixUseMana",
			//"PrefixSize",
			//"PrefixShootSpeed",
			//"PrefixKnockback",
			//"PrefixAccDefense",
			//"PrefixAccMaxMana",
			//"PrefixAccCritChance",
			//"PrefixAccDamage",
			//"PrefixAccMoveSpeed",
			//"PrefixAccMeleeSpeed",
		];

		public static string GetBuffName(int id)
		{
			if (!(id <= 0 || id >= BuffLoader.BuffCount))
			{
				if (BuffID.Search.ContainsId(id))
				{
					return Lang.GetBuffName(id);
				}
				else
				{
					var modbuff = ModContent.GetModBuff(id);
					if (modbuff is not null)
					{
						return modbuff.DisplayName.Value;
					} 
				}
			}
			return ArcaneOdysseyMod.Instance.CustomLocalization("RandomWords.None").Value;
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n-- > 1)
			{
				int k = Main.rand.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static IList<T> ShuffledList<T>(IList<T> list)
		{
			int n = list.Count;
			while (n-- > 1)
			{
				int k = Main.rand.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
			return list;
		}

		public static void AddTooltip(this List<TooltipLine> tooltips, TooltipLine toAdd)
		{
			tooltips.Reverse();
			options.Reverse();

			bool found = false;
			foreach (var option in options)
			{
				var index = tooltips.FindIndex((TooltipLine e) => e.Name.StartsWith(option) || e.Name == option);
				if (index != -1)
				{
					tooltips.Insert(index, toAdd);
					options.Reverse();
					if (!options.Contains(toAdd.Name))
						options.Add(toAdd.Name);
					found = true;
					break;
				}
			}

			tooltips.Reverse();

			if (!found)
			{
				tooltips.Add(toAdd);
				options.Reverse();
				if (!options.Contains(toAdd.Name))
					options.Add(toAdd.Name);
			}
		}


		public static void ScaleRectangle(ref Rectangle rect, float scale, bool adjustX = true, bool adjustY = true)
		{
			var diffX = ((rect.Width - (rect.Width * scale)) / 2f).Round();
			var diffY = ((rect.Height - (rect.Height * scale)) / 2f).Round();
			rect.Width = (rect.Width * scale).Round();
			rect.Height = (rect.Height * scale).Round();
			if (!adjustX)
				rect.X += diffX;
			rect.X += diffX;
			if (!adjustY)
				rect.Y += diffY;
			rect.Y += diffY;
		}

		public static SynergyEffects CopySynergiesFromImbue<T>() where T : Imbuable
		{
			return ModContent.GetInstance<T>().Effects;
		}

		public static SynergyEffects CopyDamageSynergiesFromImbue<T>() where T : Imbuable
		{
			return ModContent.GetInstance<T>().Effects with { clearBuffs = [] };
		}

		public static float RelativeScale(this Rectangle rect, int scale = 64)
		{
			return MathHelper.Clamp(((rect.Width + rect.Height) / 2f / scale), .5f, 2.5f);
		}

		public static DamageClass Imbued(this DamageClass damageClass, Imbuable imbue, Item item = null)
		{
			if (imbue is not SteamImbue steam)
			{
				if (item is not null)
				{
					if (item.ModItem is RelicImbue)
					{
						if (imbue is AOMagic)
						{
							return PaladinDamage.Instance;
						}
					}

					if (damageClass == DamageClass.Magic && imbue is AOMagic && item.TryGetSecondImbue(imbue, out var second))
					{
						if (second is RelicImbue)
						{
							return PaladinDamage.Instance;
						}
					}

					if (damageClass == DamageClass.Melee && imbue is FightingStyle && item.TryGetSecondImbue(imbue, out var second1))
					{
						if (item.ModItem is Scroll)
						{
							if (second1 is AOMagic)
							{
								return WarlockDamage.Instance;
							}

							if (second1 is RelicImbue)
							{
								return JuggernautDamage.Instance;
							}
						}
						else
						{
							return SavantDamage.Instance;
						}
					}
				}

				if (damageClass == DamageClass.Melee && imbue is AOMagic)
				{
					return ConjurerDamage.Instance;
				}
				if (damageClass == DamageClass.MeleeNoSpeed && imbue is AOMagic)
				{
					return ConjurerNoSpeedDamage.Instance;
				}
				if (damageClass == DamageClass.Melee && imbue is RelicImbue)
				{
					return KnightDamage.Instance;
				}
				if (damageClass == DamageClass.MeleeNoSpeed && imbue is RelicImbue)
				{
					return KnightNoSpeedDamage.Instance;
				}
				if (damageClass == DamageClass.Melee && imbue is FightingStyle)
				{
					return WarlordDamage.Instance;
				}
				if (damageClass == DamageClass.MeleeNoSpeed && imbue is FightingStyle)
				{
					return WarlordNoSpeedDamage.Instance;
				}

				if (damageClass == DamageClass.Ranged && imbue is AOMagic)
				{
					return RangedConjurerDamage.Instance;
				}
				if (damageClass == DamageClass.Ranged && imbue is RelicImbue)
				{
					return RangedKnightDamage.Instance;
				}
				if (damageClass == DamageClass.Ranged && imbue is FightingStyle)
				{
					return RangedWarlordDamage.Instance;
				}
			}
			else
			{
				return damageClass.Imbued(steam.Imbue);
			}

			return damageClass;
		}

		public static DamageClass UnImbued(this DamageClass damageClass, Item item = null)
		{
			if (damageClass.Name == WarlockDamage.InternalName || damageClass.Name == SavantDamage.InternalName || damageClass.Name == JuggernautDamage.InternalName || damageClass.Name == ConjurerDamage.InternalName || damageClass.Name == WarlordDamage.InternalName || damageClass.Name == KnightDamage.InternalName)
			{
				return DamageClass.Melee;
			}
			if (damageClass.Name == ConjurerNoSpeedDamage.InternalName || damageClass.Name == WarlordNoSpeedDamage.InternalName || damageClass.Name == KnightNoSpeedDamage.InternalName)
			{
				return DamageClass.MeleeNoSpeed;
			}

			if (damageClass.Name == RangedWarlordDamage.InternalName || damageClass.Name == RangedConjurerDamage.InternalName || damageClass.Name == RangedKnightDamage.InternalName)
			{
				return DamageClass.Ranged;
			}

			if (damageClass.Name == PaladinDamage.InternalName)
			{
				if (item is not null)
				{
					if (item.ModItem is RelicImbue)
					{
						return OracleDamage.Instance;
					}
					if (item.ModItem is Scroll)
					{
						return DamageClass.Magic;
					}
				}
			}

			return damageClass;
		}

		public static Vector2 GetDrawOriginCentre(this Entity entity) => new(entity.width / 2, entity.height / 2);

		public static Imbuable Imbue(this Player player) => player?.ArcaneOdyssey()?.Imbue;
		public static Imbuable Imbue(this ModPlayer player) => player?.ArcaneOdyssey()?.Imbue;
		public static Imbuable Imbue(this Projectile projectile) => projectile?.ArcaneOdyssey()?.Imbue;
		public static Imbuable Imbue(this ModProjectile projectile) => projectile?.ArcaneOdyssey()?.Imbue;
		public static Imbuable Imbue(this Item item) => item?.ArcaneOdyssey()?.Imbue;
		public static Imbuable Imbue(this ModItem item) => item?.ArcaneOdyssey()?.Imbue;

		public static Dust NewDustImperfect(Vector2 position, int type, Vector2? velocity = null, int Alpha = 0, Color newColor = default, float Scale = 1f)
		{
			velocity ??= Vector2.Zero;
			return Dust.NewDustDirect(position, 0, 0, type, velocity.Value.X, velocity.Value.Y, Alpha, newColor, Scale);
		}

		public static EntitySource_ItemUse GetSource_ItemUse(this Entity item, Player player, string context = null) => new(player, item as Item, context);

		public static int Round(this float num) => (int)Math.Round(num);

		public static string GetTexture<T>() where T : ModType
		{
			return typeof(T).FullName.Replace('.', '/');
		}

		public static void Kill(this Entity entity)
		{
			if (entity is Projectile projectile)
			{
				projectile.Kill();
			}
			if (entity is Item item)
			{
				item.active = false;
			}
			if (entity is Player player)
			{
				player.statLife = 0;
			}
			if (entity is NPC npc)
			{
				npc.StrikeInstantKill();
			}
		}

		public static StatInheritanceData WarlordInheritance => QuickInheritance(1.1f);
		public static StatInheritanceData MostInheritance => QuickInheritance(.9f);
		public static StatInheritanceData ThreeQuartersInheritance => QuickInheritance(.75f);
		public static StatInheritanceData QuarterInheritance => QuickInheritance(.25f);
		public static StatInheritanceData HalfInheritance => QuickInheritance(.5f);
		public static StatInheritanceData QuickInheritance(float num) => new(num, num, num, num, num); // makes me hungry
		public static StatInheritanceData QuickInheritance(double num) => new((float)num, (float)num, (float)num, (float)num, (float)num); // makes me less hungry

		public static Vector2 Centre(this Dust dust, Vector2? newPos = null)
		{
			Vector2 dimensions = new(dust.frame.Width, dust.frame.Height);
			if (newPos.HasValue)
			{
				dust.position = newPos.Value - (dimensions * dust.scale / 2f);
				return dust.position;
			}
			else
			{
				return dust.position + (dimensions * dust.scale / 2f);
			}
		}

		public static bool BossAlive()
		{
			foreach (var npc in Main.ActiveNPCs)
			{
				if (npc.boss)
					return true;
			}
			return false;
		}

		public static DamageClass TrueMelee()
		{
			if (ExternalModSupport.HasCalamity)
			{
				return ExternalModSupport.Calamity.Find<DamageClass>("TrueMeleeDamageClass");
			}
			return DamageClass.Melee;
		}

		public static DamageClass TrueMeleeNoSpeed()
		{
			if (ExternalModSupport.HasCalamity)
			{
				return ExternalModSupport.Calamity.Find<DamageClass>("TrueMeleeNoSpeedDamageClass");
			}
			return DamageClass.MeleeNoSpeed;
		}

		public static float Clamp(this float num, float min, float max) => MathHelper.Clamp(num, min, max);

		public static bool IsLocked(this Chest chest) => Chest.IsLocked(chest.x, chest.y);

		/// <summary>
		/// Call after setting the width + height of the projectile
		/// </summary>
		/// <param name="projectile"></param>
		public static void AverageDimensions(this Entity projectile)
		{
			projectile.width = projectile.height = (projectile.width + projectile.height) / 2;
		}

		public static List<Imbuable> GetAllImbues(this Player owner)
		{
			List<Imbuable> imbues = [];
			Item[] items = [..owner.inventory, owner.trashItem];
			foreach (Item item in items)
			{
				if (item.ModItem is Imbuable imbuable)
				{
					imbues.Add(imbuable);
				}
			}
			return imbues;
		}

		public static void SimulateAOE(float range, float damage, Vector2 origin, float knockback, Entity source, DamageClass damageClass, bool updatedamage = true)
		{
			if (source is null) return;
			if (!source.active) return;
			Imbuable imbue = source.AnyArcaneOdyssey()?.Imbue;
			if (imbue is not null)
			{
				if (source.AnyArcaneOdyssey()?.BenifitsFromScrollStats.HasValue == true) 
				{
					if (source.AnyArcaneOdyssey().BenifitsFromScrollStats.Value)
					{
						if (updatedamage)
						{
							damage *= imbue.AOScrollDamage;
						}
						range *= imbue.AOScrollSize;
						knockback *= imbue.AOScrollSize;
						if (source is Projectile projectile)
						{
							if (projectile.ArcaneOdyssey().SecondImbue is not null)
							{
								if (updatedamage)
								{
									damage *= projectile.ArcaneOdyssey().SecondImbue.AOScrollDamage;
								}
								range *= projectile.ArcaneOdyssey().SecondImbue.AOScrollSize;
								knockback *= projectile.ArcaneOdyssey().SecondImbue.AOScrollSize;
							}
						}
					}
					else
					{
						if (updatedamage)
						{
							damage *= imbue.AOImbueDamage;
						}
						range *= imbue.AOImbueSize;
						knockback *= imbue.AOImbueSize;
						if (source is Projectile projectile)
						{
							if (projectile.ArcaneOdyssey().SecondImbue is not null)
							{
								if (updatedamage)
								{
									damage *= projectile.ArcaneOdyssey().SecondImbue.AOImbueDamage;
								}
								range *= projectile.ArcaneOdyssey().SecondImbue.AOImbueSize;
								knockback *= projectile.ArcaneOdyssey().SecondImbue.AOImbueSize;
							}
						}
					}
				}
			}

			foreach (NPC target in Main.ActiveNPCs)
			{
				if (target.Hitbox.Distance(origin) <= range)
				{
					ModDamageHelper modifiers = new(null);
					if (imbue is not null)
					{
						modifiers = CalculateImbueDamage(imbue, target, modifiers);
						if (source.HasSecondImbue(out var second))
						{
							modifiers = CalculateImbueDamage(second, target, modifiers);
						}
						else if (source is Item item && item.ModItem is Imbuable imbue2)
						{
							modifiers = CalculateImbueDamage(imbue2.Imbue, target, modifiers);
						}
					}
					if (modifiers.GetDamage(damage) > 0 && source.TryGetOwner(out Player player) && Main.myPlayer == player.whoAmI)
					{
						target.HitNPC(modifiers.GetDamage(damage), ((target.Center - origin).X > 0).ToDirectionInt(), source.AnyArcaneOdyssey()?.Imbue, player, false, knockback, damageClass, true);
					}
				}
			}
		}

		public static Rectangle SimulateAOE(Rectangle hitbox, float damage, float knockback, Entity source, DamageClass damageClass, bool updatedamage = true, bool adjustY = true, bool adjustX = true)
		{
			if (source is null) return hitbox;
			if (!source.active) return hitbox;
			Imbuable imbue = source.AnyArcaneOdyssey()?.Imbue;
			float mult = 1f;
			if (imbue is not null)
			{
				if (source.AnyArcaneOdyssey()?.BenifitsFromScrollStats.HasValue == true)
				{
					if (source.AnyArcaneOdyssey().BenifitsFromScrollStats.Value)
					{
						if (updatedamage)
						{
							damage *= imbue.AOScrollDamage;
						}
						mult *= imbue.AOScrollSize;
						knockback *= imbue.AOScrollSize;
						if (source is Projectile projectile)
						{
							if (projectile.ArcaneOdyssey().SecondImbue is not null)
							{
								if (updatedamage)
								{
									damage *= projectile.ArcaneOdyssey().SecondImbue.AOScrollDamage;
								}
								mult *= projectile.ArcaneOdyssey().SecondImbue.AOScrollSize;
								knockback *= projectile.ArcaneOdyssey().SecondImbue.AOScrollSize;
							}
						}
					}
					else
					{
						if (updatedamage)
						{
							damage *= imbue.AOImbueDamage;
						}
						mult *= imbue.AOImbueSize;
						knockback *= imbue.AOImbueSize;
						if (source is Projectile projectile)
						{
							if (projectile.ArcaneOdyssey().SecondImbue is not null)
							{
								if (updatedamage)
								{
									damage *= projectile.ArcaneOdyssey().SecondImbue.AOImbueDamage;
								}
								mult *= projectile.ArcaneOdyssey().SecondImbue.AOImbueSize;
								knockback *= projectile.ArcaneOdyssey().SecondImbue.AOImbueSize;
							}
						}
					}
				}
			}

			if (source.TryGetOwner(out AOPlayer player1))
			{
				mult *= 1f + player1.SizeMulti;
			}
			ScaleRectangle(ref hitbox, mult, adjustX, adjustY);

			foreach (NPC target in Main.ActiveNPCs)
			{
				if (target.Hitbox.Intersects(hitbox))
				{
					ModDamageHelper modifiers = new(null);
					if (imbue is not null)
					{
						modifiers = CalculateImbueDamage(imbue, target, modifiers);
						if (source.HasSecondImbue(out var second))
						{
							modifiers = CalculateImbueDamage(second, target, modifiers);
						}
						else if (source is Item item && item.ModItem is Imbuable imbue2)
						{
							modifiers = CalculateImbueDamage(imbue2.Imbue, target, modifiers);
						}
					}
					if (modifiers.GetDamage(damage) > 0 && source.TryGetOwner(out Player player) && Main.myPlayer == player.whoAmI)
					{
						target.HitNPC(modifiers.GetDamage(damage), ((target.Center - hitbox.Center()).X > 0).ToDirectionInt(), source.AnyArcaneOdyssey()?.Imbue, player, false, knockback, damageClass, true);
					}
				}
			}
			return hitbox;
		}

		public static bool HasSecondImbue(this Entity entity, out Imbuable second)
		{
			second = null;
			if (entity is Item item)
			{
				second = item.ArcaneOdyssey()?.SecondImbue;
			}
			if (entity is Projectile projectile)
			{
				second = projectile.ArcaneOdyssey()?.SecondImbue;
			}
			if (entity is Player player)
			{
				second = player.ArcaneOdyssey()?.CurrentDash?.SecondImbue;
			}	
			return second is not null;
		}

		public static string Replace(this string text, string toRemove) => text.Replace(toRemove, null);

		public static bool ImbueClassCheck(Projectile projectile)
		{
			if (projectile is not null && projectile.active)
			{
				if (projectile.ModProjectile is MagicCircle1 or MagicCircle2)
				{
					return true;
				}
				if ((projectile.ModProjectile is null or AOPlayerProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && projectile.ArcaneOdyssey().CanBeAffected)
				{
					return (
							projectile.DamageType.CountsAsClass(DamageClass.Melee)
							|| projectile.DamageType.CountsAsClass(DamageClass.Ranged)
							|| projectile.ModProjectile is MagicSpell or SpiritProjectile or StrengthTechnique
						)
						&& projectile.owner != 255
						&& !projectile.hostile
						&& !projectile.npcProj
						&& !projectile.trap;
				}
			}
			return false;
		}

		public static bool ImbueClassCheck(Item item)
		{
			if (item is not null && item.active && (!item.accessory || item.ModItem is Scroll) && (item.ModItem is null or AORangedOrMeleeWeapon || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && item.ArcaneOdyssey().CanBeAffected && item.ammo == AmmoID.None)
			{
				if (item.ArcaneOdyssey().WeaponsType != WeaponType.Artisinal)
				{
					return item.DamageType.CountsAsClass(DamageClass.Melee)
						|| item.DamageType.CountsAsClass(DamageClass.Ranged)
						||
						(
							item.ModItem is Scroll or Imbuable
						);
				}
			}
			return false;
		}

		public static bool CanHaveImbue(this Item item, Imbuable imbue)
		{
			if (ImbueClassCheck(item))
			{
				if (imbue is SteamImbue steam)
				{
					return CanHaveImbue(item, steam.Imbue);
				}
				if (item.ModItem is Scroll scroll)
				{
					if (scroll.CanHaveMagic && imbue is AOMagic)
					{
						return true;
					}
					if (scroll.CanHaveFS && imbue is FightingStyle)
					{
						return true;
					}
					if (scroll.CanHaveRelic && imbue is RelicImbue)
					{
						return true;
					}
					return false;
				}
				if (item.ModItem is RelicImbue)
				{
					return imbue is AOMagic;
				}
				if (imbue is FightingStyle)
				{
					return (item.ArcaneOdyssey()?.WeaponsType == WeaponType.Normal || item.ArcaneOdyssey()?.WeaponsType == WeaponType.Strength) && item.ModItem is not Imbuable;
				}
				if (imbue is AOMagic)
				{
					return (item.ArcaneOdyssey()?.WeaponsType == WeaponType.Normal || item.ArcaneOdyssey()?.WeaponsType == WeaponType.Arcanium) && (item.ModItem is not Imbuable || item.ModItem is RelicImbue or FightingStyle);
				}
				if (imbue is RelicImbue)
				{
					return item.ArcaneOdyssey()?.WeaponsType == WeaponType.Normal && (item.ModItem is not Imbuable || item.ModItem is AOMagic or FightingStyle);
				}
				if (imbue is null)
				{
					return true;
				}
			}
			return false;
		}

		public static bool TryGetSecondImbue(this Entity entity, Imbuable imbue, out Imbuable secondimbue)
		{
			secondimbue = null;
			if (imbue is not SteamImbue)
			{
				if (entity is Projectile projectile)
				{
					if (projectile.ArcaneOdyssey()?.SecondImbue is not null)
					{
						secondimbue = projectile.ArcaneOdyssey()?.SecondImbue;
						return true;
					}
					if (imbue?.Imbue is not null)
					{
						secondimbue = imbue.Imbue;
						return projectile.ArcaneOdyssey()?.BenifitsFromScrollStats.GetValueOrDefault() == true;
					}
				}
				if (entity is Item item && item.ModItem is not Imbuable)
				{
					if (item.ArcaneOdyssey()?.SecondImbue is not null)
					{
						secondimbue = item.ArcaneOdyssey()?.SecondImbue;
						return true;
					}
					if (imbue?.Imbue is not null)
					{
						secondimbue = imbue.Imbue;
						return item.ArcaneOdyssey()?.BenifitsFromScrollStats.GetValueOrDefault() == true;
					}
				}
				if (entity is Player)
				{
					secondimbue = imbue.Imbue;
					return true;
				}
			}
			return false;
		}

		public static ModDamageHelper CalculateImbueDamage(Imbuable imbue, NPC target, ModDamageHelper modifiers)
		{
			if (imbue is not null)
			{
				if (imbue is CrystalMagic && target.HasBuff<Crystallized>() && GetAOBuffStack(target, target.FindBuffIndex(ModContent.BuffType<Crystallized>())) == 4)
				{
					modifiers.FinalDamage += .3f;
				}

				if (imbue.CombinedDebuffs is not null)
				{
					foreach (CombinedDebuff buffkeys in imbue.CombinedDebuffs)
					{
						if (target.HasBuff(ArcaneOdysseyMod.alternateBuffs[buffkeys.requirement]) || (ArcaneOdysseyMod.alternateBuffs[buffkeys.requirement] == BuffID.Wet && target.wet))
						{
							target.AddBuff(buffkeys.result, buffkeys.duration);
						}
						if (target.HasBuff(buffkeys.requirement) || (buffkeys.requirement == BuffID.Wet && target.wet))
						{
							target.AddBuff(buffkeys.result, buffkeys.duration);
						}
					}
				}

				foreach (MagicBuffMultiplier multiplier in imbue.Effects.magicBuffMultipliers)
				{
					if (target.HasBuff(ArcaneOdysseyMod.alternateBuffs[multiplier.buffID]) || (ArcaneOdysseyMod.alternateBuffs[multiplier.buffID] == BuffID.Wet && target.wet))
					{
						modifiers.FinalDamage += multiplier.multiplier.MultiToPercent();
					}
					if (target.HasBuff(multiplier.buffID) || (multiplier.buffID == BuffID.Wet && target.wet))
					{
						modifiers.FinalDamage += multiplier.multiplier.MultiToPercent();
					}
				}

				if (Main.netMode == NetmodeID.SinglePlayer) // things would get chaotic in multiplayer if everyone kept clearing eachothers debuffs
				{
					foreach (int buffid in imbue.Effects.clearBuffs)
					{
						if (target.HasBuff(ArcaneOdysseyMod.alternateBuffs[buffid]))
						{
							target.DelBuff(target.FindBuffIndex(ArcaneOdysseyMod.alternateBuffs[buffid]));
						}
						if (target.HasBuff(buffid))
						{
							target.DelBuff(target.FindBuffIndex(buffid));
						}
					}
				}
			}
			return modifiers;
		}
		
		public static NPC.HitModifiers CalculateImbueDamage(Imbuable imbue, NPC target, NPC.HitModifiers modifiers)
		{
			return modifiers with { FinalDamage = CalculateImbueDamage(imbue, target, new ModDamageHelper(modifiers.FinalDamage)).FinalDamage };
		}

		/// <summary>
		/// <inheritdoc cref="Projectile.NewProjectile(IEntitySource, float, float, float, float, int, int, float, int, float, float, float)"/>
		/// </summary>
		/// <param name="imbue">The first imbue to add to the projectile</param>
		/// <param name="secondimbue">The second imbue to add to the projectile</param>
		/// <param name="usescrollstats">Whether to multiply projectile speed by scroll or imbue stats</param>
		/// <returns></returns>
		public static Projectile ShootProjectile(IEntitySource source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int player, Imbuable imbue = null, Imbuable secondimbue = null, bool usescrollstats = false, float ai0 = 0, float ai1 = 0, float ai2 = 0)
		{
			if (imbue is not null)
			{
				if (usescrollstats)
				{
					velocity *= imbue.AOScrollSpeed;
					if (secondimbue is not null)
					{
						velocity *= secondimbue.AOScrollSpeed;
					}
				}
				else
				{
					velocity *= imbue.AOImbueSpeed;
					if (secondimbue is not null)
					{
						velocity *= secondimbue.AOImbueSpeed;
					}
				}
			}

			return Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player, ai0, ai1, ai2);
		}

		public static int FromAODefense(this int val) => (int)Math.Round(val / 15f);

		public static int IndexOf<T>(this Array array, T item) => Array.IndexOf(array, item);

		public static bool TryGetImbue(this Item item, out Imbuable imbue)
		{
			imbue = item.ArcaneOdyssey()?.Imbue;
			return imbue is not null;
		}

		public static bool TryGetImbue(this Projectile projectile, out Imbuable imbue)
		{
			imbue = projectile.ArcaneOdyssey()?.Imbue;
			return imbue is not null;
		}

		public static bool TryGetImbue(this Player player, out Imbuable imbue)
		{
			imbue = player.ArcaneOdyssey()?.Imbue;
			return imbue is not null;
		}
		public static bool TryGetImbue(this ModPlayer player, out Imbuable imbue)
		{
			imbue = player.Player.ArcaneOdyssey()?.Imbue;
			return imbue is not null;
		}

		public static bool ServerOrSingleplayer => Main.netMode != NetmodeID.MultiplayerClient;

		public static bool AltUse(this Player player) => player.altFunctionUse == 2;

		public static Rectangle ScreenRect => new(Main.screenPosition.X.Round(), Main.screenPosition.Y.Round(), Main.screenWidth, Main.screenHeight);

		public static bool OnScreen(this Entity entity)
		{
			return entity.Hitbox.Intersects(ScreenRect);
		}

		public static void HitNPC(this NPC npc, int damage, int hitDirection, Imbuable imbue = null, Player player = null, bool crit = false, float knockBack = 0f, DamageClass damageType = null, bool damageVariation = false)
		{
			if (npc.dontTakeDamage || npc.friendly)
				return;
			if (player is not null)
			{
				if (imbue is RelicImbue)
					player.ArcaneOdyssey()?.TrySpiritLifesteal(damage);
				if (player.dontHurtCritters && NPCID.Sets.CountsAsCritter[npc.type])
					return;
				if (npc.immune[player.whoAmI] > 0 || player.whoAmI != Main.myPlayer)
					return;
				if (npc.noTileCollide || player.CanHit(npc))
				{
					player.ApplyDamageToNPC(npc, damage, knockBack, hitDirection, crit, damageType, damageVariation);
					player.ArcaneOdyssey()?.UpdateDebuffHelpers(damage, npc, imbue, false);
				}
			}
			else
			{
				npc.SimpleStrikeNPC(damage, hitDirection, crit, knockBack, damageType);
			}
		}

		public static bool PlayerHasImbue(this Imbuable imbue, Player player)
		{
			var type = imbue.GetType();
			if (imbue is SteamImbue steam)
			{
				type = steam.Imbue.GetType();
			}
			return player.HasTypeInInventory(type);
		}

		/// <summary>
		/// Automatically generates localization, and formats statically
		/// </summary>
		/// <param name="mod">literally the mod</param>
		/// <param name="key">The localization key</param>
		/// <param name="formatting">Formatting args, not required</param>
		/// <returns></returns>
		public static LocalizedText CustomLocalization(this Mod mod, string key, params object[] formatting)
		{
			if (mod is not ArcaneOdysseyMod)
			{
				mod = ModInstance;
			}
			LocalizedText text = LocalizedText.Empty;
			string fulllocalstuff = "";
			foreach (object format in formatting)
			{
				fulllocalstuff += " " + format;
			}
			if (ArcaneOdysseyMod.staticLocalizer.TryGetValue(mod.GetLocalizationKey(key) + fulllocalstuff, out LocalizedText value))
			{
				text = value;
			}
			else
			{
				text = Language.GetOrRegister(mod.GetLocalizationKey(key), () => key.Split('.').LastOrDefault(key)).WithFormatArgs(formatting);
				ArcaneOdysseyMod.staticLocalizer[mod.GetLocalizationKey(key) + fulllocalstuff] = text;
			}
			return text;
		}

		public static LocalizedText CoolCustomLocalization(this Mod mod, string key, string fallback = null) => Language.GetOrRegister(mod.GetLocalizationKey(key), () => fallback ?? mod.CustomLocalization(key).Value);


		public static ArcaneOdysseyMod ModInstance => ArcaneOdysseyMod.Instance;


		private static bool checklistfailed = true; // do not spam console if set to true

		/// <summary>
		/// Includes minibosses
		/// </summary>
		public static int BossesKilled
		{
			get
			{
				int count = 0;
				List<bool> conditions = [];
				if (checklistfailed || !ModLoader.TryGetMod("BossChecklist", out var checklist))
				{
					conditions.AddRange([DownedBosses.downedEvander, DownedBosses.downedLaelus, DownedBosses.downedCrone, DownedBosses.downedDelamere, DownedBosses.downedDusk, NPC.downedBoss1, DownedBosses.downedWorldEater, DownedBosses.downedBrain, NPC.downedBoss3, NPC.downedQueenBee, NPC.downedSlimeKing, NPC.downedDeerclops, NPC.downedAncientCultist, NPC.downedChristmasIceQueen, NPC.downedChristmasSantank, NPC.downedClown, NPC.downedChristmasTree, NPC.downedEmpressOfLight, NPC.downedFishron, NPC.downedFrost, NPC.downedGoblins, NPC.downedGolemBoss, NPC.downedHalloweenKing, NPC.downedHalloweenTree, NPC.downedMartians, NPC.downedMechBoss1, NPC.downedMechBoss2, NPC.downedMechBoss3, NPC.downedMechBossAny, NPC.downedMoonlord, NPC.downedPlantBoss, NPC.downedPirates]);
					if (ModLoader.TryGetMod("CalamityMod", out var cal))
					{
						string[] extrBosses = "desertscourge giantclam crabulon hivemind perforator slimegod cryogen aquaticscourge cragmawmire brimstoneelemental calamitasclone greatsandshark anahitaleviathan astrumaureus plaguebringergoliath ravager astrumdeus guardians dragonfolly providence polterghast mauler nuclearterror oldduke ceaselessvoid stormweaver signus devourerofgods yharon exomechs calamitas primordialwyrm".Split(" ");
						foreach (var boss in extrBosses)
						{
							conditions.Add((bool)cal.Call("GetBossDowned", boss));
						}
					}
				}
				else
				{
					var raw = checklist.Call("GetBossInfoDictionary", ModInstance);
					if (raw is Dictionary<string, Dictionary<string, object>> data)
					{
						foreach (var boss in data)
						{
							bool isbossormini = (bool)boss.Value["isBoss"] || (bool)boss.Value["isMiniboss"];
							if (isbossormini)
							{
								var func = (Func<bool>)boss.Value["downed"];
								conditions.Add(func.Invoke());
							}
						}
					}
					else
					{
						checklistfailed = true;
						return BossesKilled;
					}
				}
				foreach (bool killed in conditions)
				{
					if (killed)
						count++;
				}
				//checklistfailed = false;
				return count;
			}
		}

		public static bool TryGetOwner(this Entity entity, out AOPlayer player)
		{
			var e = entity.TryGetOwner(out Player playr);
			player = playr?.ArcaneOdyssey();
			if (e && playr.ArcaneOdyssey() is not null)
			{
				return e;
			}
			return false;
		}

		public static bool TryGetOwner(this Entity entity, out Player player)
		{
			player = null;
			if (entity is Projectile projectile)
			{
				player = Main.player[projectile.owner];
			}
			if (entity is NPC npc)
			{
				player = Main.player[npc.releaseOwner];
			}
			if (entity is Player player1)
			{
				player = player1;
			}
			if (entity is Item item)
			{
				if (item.ArcaneOdyssey()?.owner is not null)
				{
					player = item.ArcaneOdyssey().owner;
				}
			}
			return player is not null && player.active;
		}

		public static Player GetOwner(this Entity entity)
		{
			entity.TryGetOwner(out Player player);
			return player;
		}

		#region Enum Getters

		public static ItemType GetItemType(this Item item)
		{
			if (item.ModItem is AOBaseItem based && based.ItemCategory.HasValue)
			{
				return based.ItemCategory.Value;
			}
			if (item.vanity)
			{
				return ItemType.Vanity;
			}
			if (item.accessory)
			{
				return ItemType.Accessory;
			}
			if (item.bodySlot != -1 || item.legSlot != -1 || item.headSlot != -1 || item.wornArmor)
			{
				return ItemType.Armour;
			}
			if (item.axe > 0 || item.pick > 0)
			{
				return ItemType.Tool;
			}
			if (item.ammo != AmmoID.None)
			{
				return ItemType.Ammo;
			}
			if (item.damage > 0 && item.useStyle != ItemUseStyleID.None)
			{
				return ItemType.Weapon;
			}
			if (item.material)
			{
				return ItemType.Material;
			}
			if (item.createTile != -1)
			{
				return ItemType.Block;
			}
			return ItemType.Item;
		}

		public static AORarities GetItemRare(this Item item)
		{
			if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
			{
				if (item.rare == calamity.Find<ModRarity>("DarkOrange").Type)
				{
					return AORarities.Unknown;
				}
			}

			if (ModLoader.TryGetMod("NoxusBoss", out var wotg))
			{
				if (item.rare == wotg.Find<ModRarity>("SolynRewardRarity").Type)
				{
					return AORarities.Mystic;
				}
				if (item.rare == wotg.Find<ModRarity>("GenesisComponentRarity").Type)
				{
					return AORarities.Unknown;
				}
				if (item.rare == wotg.Find<ModRarity>("NamelessDeityRarity").Type)
				{
					return AORarities.Unknown;
				}
				if (item.rare == wotg.Find<ModRarity>("AvatarRarity").Type)
				{
					return AORarities.Unknown;
				}
			}
			if (item.questItem || item.rare == ItemRarityID.Quest)
			{
				return AORarities.Rare;
			}

			if (item.expert || item.rare == ItemRarityID.Expert)
			{
				return AORarities.Legendary;
			}
			if (item.master || item.rare == ItemRarityID.Master)
			{
				return AORarities.Mythical;
			}
			return item.rare switch
			{
				ItemRarityID.Gray => AORarities.Common,
				ItemRarityID.White => AORarities.Common,
				ItemRarityID.Blue => AORarities.Common,
				ItemRarityID.Green => AORarities.Uncommon,
				ItemRarityID.Orange => AORarities.Uncommon,
				ItemRarityID.LightRed => AORarities.Rare,
				ItemRarityID.Pink => AORarities.Rare,
				ItemRarityID.LightPurple => AORarities.Mystic,
				ItemRarityID.Lime => AORarities.Mystic,
				ItemRarityID.Yellow => AORarities.Legendary,
				ItemRarityID.Cyan => AORarities.Legendary,
				_ => AORarities.Mythical,
			};
		}
		#endregion

		#region Random Math Functions
		public static int GetAOBuffStack(NPC npc, int index)
		{
			return (npc.buffTime[index] / 60 / 5) + 1;
		}

		/// <summary>
		/// Converts AO Galleons/Drachmae to Terraria Copper
		/// </summary>
		/// <param name="price">Price, in Galleons</param>
		/// <returns></returns>
		public static int GalleonToCopper(int price) => Item.buyPrice(silver: price);


		/// <summary>
		/// Converts AO weapon damage to Terraria damage. Scales very heavily with weapon tier
		/// </summary>
		/// <param name="AODamage">AO weapon damage multiplier</param>
		/// <param name="AOWeaponTier">AO weapon tier, use <see cref="AOItemTiers"/></param>
		/// <returns></returns>
		public static float WeaponDamage(AOItemTiers AOWeaponTier) => 25 * (int)AOWeaponTier;

		public static Vector2 Centre(this Gore gore, Vector2? newCentre)
		{
			if (newCentre.HasValue)
			{
				gore.position.X = newCentre.Value.X - (gore.Width / 2);
				gore.position.Y = newCentre.Value.Y - (gore.Height / 2);
				return gore.position;
			}
			else
				return new Vector2(gore.position.X - (gore.Width / 2), gore.position.Y - (gore.Height / 2));
		}

		/// <summary>
		/// Turns 1.4 into .6
		/// </summary>
		/// <param name="input">Input</param>
		/// <returns></returns>
		public static float FlipFloat(this float input) => MathHelper.Clamp(2f - input, .1f, 2f);

		public static float MultiToPercent(this float multiplier) => multiplier - 1f; // wow simplest function on the earth

		public static Vector2 SafeDirectionTo(this Entity entity, Vector2 destination, Vector2? defaultValue = null)
		{
			defaultValue ??= Vector2.Zero;
			return (destination - entity.Center).SafeNormalize(defaultValue.Value);
		}
		#endregion

		#region Player Inventory Helpers
		public static bool HasTypeInInventory(this Player player, Type type)
		{
			List<Item> no = [..player.inventory, player.trashItem];
			no.RemoveAll(e => e.ModItem is null);
			foreach (var item in no)
			{
				if (item.ModItem.GetType().Name == type.Name || item.ModItem.GetType().IsSubclassOf(type))
				{
					return true;
				}
			}
			return false;
		}
		public static bool HasTypeInInventory(this Player player, Type type, out Item item)
		{
			List<Item> no = [.. player.inventory, player.trashItem];
			item = null;
			no.RemoveAll(e => e.ModItem is null);
			foreach (var items in no)
			{
				if (items.ModItem.GetType().Name == type.Name || items.ModItem.GetType().IsSubclassOf(type))
				{
					item = items;
					return true;
				}
			}
			return false;
		}

		public static Item PlayerItem(this Player player)
		{
			if (Main.myPlayer == player.whoAmI && (!Main.mouseItem.IsAir) && Main.mouseItem.active)
			{
				return Main.mouseItem;
			}
			else return player.HeldItem;
		}

		public static bool GetThisImbue(this Imbuable imbue, Player player)
		{
			if (imbue is not null)
			{
				foreach (var item in player.inventory)
				{
					if (item.active)
					{
						if (item.Name == imbue.DisplayName.Value)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
		#endregion

		#region ArcaneOdyssey()

		public static AOPlayer ArcaneOdyssey(this Player player)
		{
			if (player is not null && player.active && player.TryGetModPlayer<AOPlayer>(out var playah))
				return playah;
			return null;
		}

		public static AOPlayer ArcaneOdyssey(this ModPlayer player) => player?.Player?.ArcaneOdyssey();

		public static ArcaneNPC ArcaneOdyssey(this NPC npc)
		{
			if (npc is not null && npc.active && npc.TryGetGlobalNPC<ArcaneNPC>(out var npcc))
				return npcc;
			return null;
		}

		public static AOProjectile ArcaneOdyssey(this Projectile projectile)
		{
			if (projectile is not null && projectile.active && projectile.TryGetGlobalProjectile<AOProjectile>(out var proj))
				return proj;
			return null;
		}

		public static AOProjectile ArcaneOdyssey(this ModProjectile projectile) => projectile?.Projectile?.ArcaneOdyssey();

		public static AOItem ArcaneOdyssey(this Item item)
		{
			if (item is not null && !item.IsAir && item.active && item.TryGetGlobalItem<AOItem>(out var item1))
				return item1;
			return null;
		}

		public static AOItem ArcaneOdyssey(this ModItem item) => item?.Item?.ArcaneOdyssey();

		public static IImbuable AnyArcaneOdyssey(this Entity entity)
		{
			if (entity is Projectile projectile)
			{
				if (projectile.ModProjectile is AOPlayerProjectile proj)
				{
					return proj;
				}
				return projectile.ArcaneOdyssey();
			}
			if (entity is Player player)
				return player.ArcaneOdyssey();
			if (entity is Item item)
			{
				if (item.ModItem is AORangedOrMeleeWeapon weap)
					return weap;
				return item.ArcaneOdyssey();
			}
			return null;
		}

		#endregion
	}

	public struct WeaponAbility(Mod mod, string name = null, string description = null, Color? color = null)
	{
		public static string Key(Mod mod, string name)
		{
			return $"Mods.{mod.Name}.WeaponAbilities." + name.Replace(" ", null);
		}

		public string Name = name;
		public string Description = description;
		public Color? Colour = color;
		public Mod mod = mod;
		public LocalizedText LocalizedName = Language.GetOrRegister(Key(mod, name) + ".DisplayName", () => name);
		public LocalizedText LocalizedDescription = Language.GetOrRegister(Key(mod, name) + ".Description", () => description);


		public readonly TooltipLine GenerateTooltip()
		{
			string text = "";
			if (Name is not null)
			{
				if (Colour.HasValue)
				{
					text += $"[c/{Colour.Value.Hex3()}:{LocalizedName.Value}]";
				}
				else
				{
					text += LocalizedName.Value;
				}
			}
			if (Description is not null)
			{
				if (Name is not null)
					text += $": {LocalizedDescription.Value}";
				else if (Colour.HasValue)
					text += $"[c/{Colour.Value.Hex3()}:{LocalizedDescription.Value}";
				else
					text += LocalizedDescription.Value;
			}
			return new TooltipLine(mod, "AOAbility", text);
		}
	}

	public enum DashType
	{
		Standard,
		Burst,
		Instant
	}

	/// <summary>
	/// Helper struct for set bonuses
	/// </summary>
	/// <param name="mod">This mod</param>
	/// <param name="name">The name of the set bonus</param>
	/// <param name="description">The description of this set bonus</param>
	/// <param name="otherItems">The internal names of the other two items in this set, head then body</param>
	/// <param name="colour">The colour of this set</param>
	public struct SetBonusHelper(Mod mod, string name, string description, string[] otherItems, Color? colour = null)
	{
		public Mod Mod = mod;
		public string Name = name;
		public string Description = description;
		public Color? Colour = colour;
		public string[] OtherItems = otherItems;

		public static string Key(Mod mod, string name)
		{
			return $"Mods.{mod.Name}.ArmourSetTooltips." + name.Replace(" ", null);
		}

		public LocalizedText LocalizedName = Language.GetOrRegister(Key(mod, name) + ".DisplayName", () => name);
		public LocalizedText LocalizedDescription = Language.GetOrRegister(Key(mod, name) + ".Description", () => description);

		public readonly string GenerateTooltip()
		{
			string text = "";
			if (Colour.HasValue)
			{
				text += $"[c/{Colour.Value.Hex3()}:{LocalizedName.Value}]";
			}
			else
			{
				text += LocalizedName.Value;
			}
			text += $" - {LocalizedDescription.Value}";
			return text;
		}
	}

	public struct ImbueArmourStats(int size, int attkspeed, int power, int defence, int agility, int pierce)
	{
		public int Size = size;
		public int Attkspeed = attkspeed;
		public int Power = power;
		public int Pierce = pierce;
		public int Defence = defence;
		public int Agility = agility;

		public readonly ImbueArmourStats Corrected(Imbuable imbue)
		{
			if (imbue is FightingStyleBarred barred)
			{
				return new ImbueArmourStats(
					MathHelper.Lerp(0, Size, barred.LerpValue).Round(),
					MathHelper.Lerp(0, Attkspeed, barred.LerpValue).Round(),
					MathHelper.Lerp(0, Power, barred.LerpValue).Round(),
					MathHelper.Lerp(0, Defence, barred.LerpValue).Round(),
					MathHelper.Lerp(0, Agility, barred.LerpValue).Round(),
					MathHelper.Lerp(0, Pierce, barred.LerpValue).Round()
					);
			}
			return this;
		}
	}
	public enum ItemType
	{
		Block,
		Ammo,
		Item,
		Material,
		Accessory,
		Armour,
		Weapon,
		Tool,
		Vanity
	}

	/// <summary>
	/// Arcane Odyssey rarities, converted to RarityID
	/// </summary>

	public enum AORarities
	{
		Unknown = ItemRarityID.Gray,
		Common = ItemRarityID.White,
		Uncommon = ItemRarityID.Green,
		Rare = ItemRarityID.LightRed,
		Mystic = ItemRarityID.LightPurple,
		Legendary = ItemRarityID.Yellow,
		Mythical = ItemRarityID.Red,
		Special
	}

	public enum WeaponType
	{
		Normal = -1,
		Arcanium,
		Strength,
		Artisinal
	}

	public enum AOImbuableTier
	{
		Normal,
		Lost,
		Ancient,
		Primordial, // unused
		Developer,
	}

	/// <summary>
	/// Arcane Odyssey weapon tiers, used for scaling
	/// </summary>
	public enum AOItemTiers
	{
		/// <summary>
		/// Literally doesn't exist, don't bother
		/// </summary>
		None,
		/// <summary>
		/// Old weapons
		/// </summary>
		Poor,
		/// <summary>
		/// Bronze weapons
		/// </summary>
		Average,
		/// <summary>
		/// All the cool weapons
		/// </summary>
		Good,
		/// <summary>
		/// Atleantean weapons+ use these, not in ao
		/// </summary>
		Great,
	}

	/// <summary>
	/// Represents an AO debuff
	/// </summary>
	/// <param name="debuffid">Terraria.ID.BuffID</param>
	/// <param name="duration">Duration, in ticks (60/second)</param>
	/// <param name="debuffRequiement">Damage% requirement to activate debuff</param>
	public struct AODebuffRequirement(int debuffid, int duration, int debuffRequiement = 0)
	{
		public float debuffPercent = debuffRequiement / 100f;
		public int debuffID = debuffid;
		public int debuffDuration = duration;
	}

	public struct ImbueDebuffHelper(Imbuable imbue, int damagedone, NPC npc, int buffID)
	{
		public Imbuable imbue = imbue;
		public int damagedone = damagedone;
		public NPC npc = npc;
		public int buffID = buffID;
	}

	/// <summary>
	/// Imbue status effects
	/// </summary>
	public struct SynergyEffects(int[] buffsToClear, MagicBuffMultiplier[] buffMultipliers)
	{
		public List<int> clearBuffs = [.. buffsToClear];
		public MagicBuffMultiplier[] magicBuffMultipliers = buffMultipliers;
		public readonly float MultiFromID(int id)
		{
			foreach (MagicBuffMultiplier multiplier in magicBuffMultipliers)
			{
				if (multiplier.buffID == id)
				{
					return multiplier.multiplier;
				}
			}
			return 1f;
		}
	}

	/// <summary>
	/// sahhhhhduiahyfoahgoaig
	/// </summary>
	/// <param name="requirement"></param>
	/// <param name="result"></param>
	/// <param name="duration"></param>
	public struct CombinedDebuff(int requirement, int result, int duration = 60)
	{
		public int requirement = requirement;
		public int result = result;
		public int duration = duration;
	}

	/// <summary>
	/// Damage multipliers from having debuffs interact
	/// </summary>
	/// <param name="buffid">Terraria.ID.BuffID</param>
	/// <param name="multi">Damage multipier (ex. 1.25f)</param>
	public struct MagicBuffMultiplier(int buffid, float multi)
	{
		public int buffID = buffid;
		public float multiplier = multi;
	}

	public interface IImbuable
	{
		public bool? BenifitsFromScrollStats => null;
		public Imbuable Imbue { get; set; }
	}

	/// <summary>
	/// used so i can copy paste code
	/// </summary>
	public struct ModDamageHelper(StatModifier? statModifier)
	{
		public StatModifier FinalDamage = statModifier.GetValueOrDefault(new(1, 1));
		public int GetDamage(int damage)
		{
			return FinalDamage.ApplyTo(damage).Round();
		}

		public int GetDamage(float damage)
		{
			return FinalDamage.ApplyTo(damage).Round();
		}

		public static ModDamageHelper FromHitModifiers(NPC.HitModifiers hitModifiers)
		{
			return new ModDamageHelper(hitModifiers.FinalDamage);
		}
	}
}
