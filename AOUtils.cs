using ArcaneOdyssey.GlobalTypes;
using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.VFX.Rarities;
using ArcaneOdysseyMusic;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.NPCs;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Imbues.Base;

namespace ArcaneOdyssey
{
	public static class AOUtils
	{
		public static float UpdateCount => Main.GameUpdateCount / 100f;

		/// <summary>
		/// Spawns gore, centred to the <paramref name="centre"/>
		/// </summary>
		public static Gore SpawnGore(IEntitySource source, Vector2 centre, Vector2 velocity, int type, float scale = 1f)
		{
			var gore = Gore.NewGorePerfect(source, centre, velocity, type, scale);
			gore.Centre(centre);
			return gore;
		}

		public const string BlankTexture = ArcaneOdysseyMod.InternalName + "/Backgrounds/Blank";
		public const string SlashTexture = ArcaneOdysseyMod.InternalName + "/Assets/BasicSlash";
		public const string GelTexture = ArcaneOdysseyMod.InternalName + "/Assets/GelBuffBackground";
		public const string DebuffTexture = ArcaneOdysseyMod.InternalName + "/Assets/Debuff";

		public static int GetMusic(string name) => MusicLoader.GetMusicSlot(ArcaneOdysseyMusicMod.Instance, "Music/" + name);

		internal static List<string> options = [
			"Terraria FavoriteDesc",
			"Terraria NoTransfer",
			"Terraria SocialDesc",
			"Terraria Damage",
			"Terraria CritChance",
			"Terraria Speed",
			"Terraria NoSpeedScaling",
			"Terraria SpecialSpeedScaling",
			"Terraria Knockback",
			"Terraria FishingPower",
			"Terraria NeedsBait",
			"Terraria BaitPower",
			"Terraria Equipable",
			"Terraria WandConsumes",
			"Terraria Quest",
			"Terraria Vanity",
			"Terraria Defense",
			"Terraria PickPower",
			"Terraria AxePower",
			"Terraria HammerPower",
			"Terraria TileBoost",
			"Terraria HealLife",
			"Terraria HealMana",
			"Terraria UseMana",
			"Terraria Placeable",
			"Terraria Ammo",
			"Terraria Consumable",
			"Terraria Material",
			"Terraria Tooltip",
		];

		public static string GetBuffName(int id)
		{
			if (!(id <= 0 || id >= BuffLoader.BuffCount))
			{
				if (id < BuffID.Count)
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

		public static IItemDropRule Common<T>(int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1) where T : ModItem
		{
			return ItemDropRule.Common(ModContent.ItemType<T>(), chanceDenominator, minimumDropped, maximumDropped);
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

		public static string LocalizationCategoryOf<T>() where T : class, ILocalizedModType
		{
			return ModContent.GetInstance<T>().LocalizationCategory;
		}

		public static Rectangle ToTileRect(this Rectangle rect)
		{
			return new(rect.X / 16, rect.Y / 16, rect.Width / 16, rect.Height / 16);
		}

		public static Rectangle ToWorldRect(this Rectangle rect)
		{
			return new(rect.X * 16, rect.Y * 16, rect.Width * 16, rect.Height * 16);
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

		public static bool? ToNullableBool(this int value)
		{
			if (value == 2)
			{
				return null;
			}
			return value == 1;
		}

		public static int ToInt32(this bool? value)
		{
			if (value.HasValue)
			{
				return value.Value.ToInt();
			}
			return 2;
		}

		public static void AddTooltip(this List<TooltipLine> tooltips, TooltipLine toAdd, Color? colour = null)
		{
			if (colour.HasValue)
			{
				toAdd.Text = $"[c/{colour.Value.Hex3()}:{toAdd.Text}]";
			}

			tooltips.Reverse();
			options.Reverse();

			bool found = false;
			foreach (var option in options)
			{
				var index = tooltips.FindIndex((TooltipLine e) => $"{e.Mod} {e.Name}".StartsWith(option) || $"{e.Mod} {e.Name}" == option);
				if (index != -1)
				{
					tooltips.Insert(index, toAdd);
					options.Reverse();
					if (!options.Contains($"{toAdd.Mod} {toAdd.Name}"))
						options.Add($"{toAdd.Mod} {toAdd.Name}");
					found = true;
					break;
				}
			}

			tooltips.Reverse();

			if (!found)
			{
				tooltips.Add(toAdd);
				options.Reverse();
				if (!options.Contains($"{toAdd.Mod} {toAdd.Name}"))
					options.Add($"{toAdd.Mod} {toAdd.Name}");
			}
		}

		/// <summary>
		/// Scales a rectangle
		/// </summary>
		/// <param name="rect">Rectangle to scale</param>
		/// <param name="scale">Multiplier to scale by</param>
		/// <param name="adjustX">How many times to shift left the hitbox if it grew, or shift right if it shrunk</param>
		/// <param name="adjustY">How many times to shift up the hitbox if it grew, or shift it down if it shrunk</param>
		public static void ScaleRectangle(ref Rectangle rect, float scale, int adjustX = 1, int adjustY = 1)
		{
			var diffX = ((rect.Width - (rect.Width * scale)) / 2f).Round();
			var diffY = ((rect.Height - (rect.Height * scale)) / 2f).Round();
			rect.Width = (rect.Width * scale).Round();
			rect.Height = (rect.Height * scale).Round();
			rect.X += diffX * adjustX;
			rect.Y += diffY * adjustY;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rectangle"></param>
		/// <param name="scale"></param>
		/// <param name="adjustX">How many times to shift left the hitbox if it grew, or shift right if it shrunk</param>
		/// <param name="adjustY">How many times to shift up the hitbox if it grew, or shift it down if it shrunk</param>
		/// <returns></returns>
		public static Rectangle ScaleRectangleNotRef(Rectangle rectangle, float scale, int adjustX = 1, int adjustY = 1)
		{
			var diffX = ((rectangle.Width - (rectangle.Width * scale)) / 2f).Round();
			var diffY = ((rectangle.Height - (rectangle.Height * scale)) / 2f).Round();
			rectangle.Width = (rectangle.Width * scale).Round();
			rectangle.Height = (rectangle.Height * scale).Round();
			rectangle.X += diffX * adjustX;
			rectangle.Y += diffY * adjustY;
			return rectangle;
		}

		public static SynergyEffects CopySynergiesFromImbue<T>() where T : Imbuable
		{
			return ModContent.GetInstance<T>().Effects;
		}

		public static int[] ToIntArray(this Rectangle rect)
		{
			return [rect.X, rect.Y, rect.Width, rect.Height];
		}

		public static Rectangle FromIntArray(this int[] array)
		{
			return new Rectangle(array[0], array[1], array[2], array[3]);
		}

		public static bool NPCAlive<T>() where T : ModNPC
		{
			foreach (var npc in Main.ActiveNPCs)
			{
				if ((npc.type == ModContent.NPCType<T>()) && (npc.life > 0))
				{
					return true;
				}
			}
			return false;
		}

		public static SynergyEffects CopyDamageSynergiesFromImbue<T>() where T : Imbuable
		{
			return ModContent.GetInstance<T>().Effects with { clearBuffs = [] };
		}

		public static float RelativeScale(this Rectangle rect, int scale = 64)
		{
			return MathHelper.Clamp((rect.Width + rect.Height) / 2f / scale, .5f, 2f);
		}

		public static Imbuable Imbue(this Player player) => player?.ArcaneOdyssey()?.Imbue;
		public static Imbuable Imbue(this ModPlayer player) => player?.ArcaneOdyssey()?.Imbue;
		public static Imbuable Imbue(this Projectile projectile) => projectile?.ArcaneOdyssey()?.Imbue;
		public static Imbuable Imbue(this ModProjectile projectile) => projectile?.ArcaneOdyssey()?.Imbue;
		public static Imbuable Imbue(this Item item) => item?.ArcaneOdyssey()?.Imbue;
		public static Imbuable Imbue(this ModItem item) => item?.ArcaneOdyssey()?.Imbue;

		public static Imbuable SecondImbue(this Projectile projectile) => projectile?.ArcaneOdyssey()?.SecondImbue;
		public static Imbuable SecondImbue(this ModProjectile projectile) => projectile?.ArcaneOdyssey()?.SecondImbue;
		public static Imbuable SecondImbue(this Item item) => item?.ArcaneOdyssey()?.SecondImbue;
		public static Imbuable SecondImbue(this ModItem item) => item?.ArcaneOdyssey()?.SecondImbue;

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
				if (Main.netMode != NetmodeID.SinglePlayer)
					NetMessage.SendData(MessageID.KillProjectile, -1, -1, null, projectile.identity, projectile.owner);
			}
			if (entity is Item item)
			{
				item.TurnToAir();
			}
			if (entity is Player player)
			{
				player.KillMe(PlayerDeathReason.ByOther(entity.whoAmI), 99999999, player.direction);
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

		public static bool BossAlive(bool ignoreDummy = true)
		{
			foreach (var npc in Main.ActiveNPCs)
			{
				if (npc.boss && (!ignoreDummy || (npc.ModNPC is not DebuffDummy)))
					return true;
			}
			return false;
		}

		public static bool BothTwinsAlive()
		{
			var alivecount = 0;
			foreach (var npc in Main.ActiveNPCs)
			{
				if (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
					alivecount++;
			}
			return alivecount == 2;
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
			Item[] items = [.. owner.inventory, owner.trashItem];
			foreach (Item item in items)
			{
				if (item.ModItem is Imbuable imbuable)
				{
					imbues.Add(imbuable);
				}
			}
			return imbues;
		}

		/// <summary>
		/// Simulates AoE
		/// </summary>
		/// <param name="range">Range of the attack, will be multiplied by imbue sizes</param>
		/// <param name="damage">Danage of the attack, will be multiplied by imbue damages if <paramref name="updatedamage"/> is true</param>
		/// <param name="origin">Centre of the AoE, in world position</param>
		/// <param name="knockback">Knockback of the AoE, will be multied by imbue sizes</param>
		/// <param name="source">Source of the damage, used to get imbues</param>
		/// <param name="damageClass"><seealso cref="DamageClass"/> of the AoE</param>
		/// <param name="updatedamage">Whether to update damage with imbue stats, defaults to true</param>
		/// <param name="ignoredNPCs">The <seealso cref="Entity.whoAmI"/> of <seealso cref="NPC"/>s you don't want to damage</param>
		public static void SimulateAOE(float range, float damage, Vector2 origin, float knockback, Entity source, DamageClass damageClass, bool updatedamage = true, params int[] ignoredNPCs)
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
							if (projectile.SecondImbue() is not null)
							{
								if (updatedamage)
								{
									damage *= projectile.SecondImbue().AOImbueDamage;
								}
								range *= projectile.SecondImbue().AOImbueSize;
								knockback *= projectile.SecondImbue().AOImbueSize;
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
							if (projectile.SecondImbue() is not null)
							{
								if (updatedamage)
								{
									damage *= projectile.SecondImbue().AOImbueDamage;
								}
								range *= projectile.SecondImbue().AOImbueSize;
								knockback *= projectile.SecondImbue().AOImbueSize;
							}
						}
					}
				}
			}

			foreach (NPC target in Main.ActiveNPCs)
			{
				if (ignoredNPCs.Contains(target.whoAmI))
					continue;
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
					}
					if (modifiers.GetDamage(damage) > 0 && source.TryGetOwner(out Player player) && Main.myPlayer == player.whoAmI)
					{
						target.HitNPC(modifiers.GetDamage(damage), ((target.Center - origin).X > 0).ToDirectionInt(), source.AnyArcaneOdyssey()?.Imbue, player, false, knockback, damageClass, true);
					}
				}
			}
		}

		public static NPC GetMinionTarget(this Vector2 origin, float maxDistanceToCheck, Player owner, bool ignoreTiles = true, bool checksRange = false)
		{
			if (owner is null || !owner.whoAmI.WithinBounds(Main.maxPlayers) || !owner.MinionAttackTargetNPC.WithinBounds(Main.maxNPCs))
				return ClosestNPCAt(origin, maxDistanceToCheck, ignoreTiles);
			NPC npc = Main.npc[owner.MinionAttackTargetNPC];
			bool canHit = true;
			if (!ignoreTiles)
				canHit = Collision.CanHit(origin, 1, 1, npc.Center, 1, 1);
			float extraDistance = (npc.width / 2) + (npc.height / 2);
			bool distCheck = Vector2.Distance(origin, npc.Center) < (maxDistanceToCheck + extraDistance) || !checksRange;
			if (owner.HasMinionAttackTargetNPC && canHit && distCheck)
			{
				return npc;
			}
			return ClosestNPCAt(origin, maxDistanceToCheck, ignoreTiles);
		}

		public static NPC ClosestNPCAt(this Vector2 origin, float maxDistanceToCheck, bool ignoreTiles = true, bool bossPriority = false)
		{
			NPC closestTarget = null;
			if (bossPriority)
			{
				bool bossFound = false;
				for (int index = 0; index < Main.npc.Length; index++)
				{
					if (bossFound && !(Main.npc[index].boss || Main.npc[index].type == NPCID.WallofFleshEye))
						continue;

					if (Main.npc[index].CanBeChasedBy(null, false))
					{
						float extraDistance = (Main.npc[index].width / 2) + (Main.npc[index].height / 2);

						bool canHit = true;
						if (extraDistance < maxDistanceToCheck && !ignoreTiles)
							canHit = Collision.CanHit(origin, 1, 1, Main.npc[index].Center, 1, 1);

						if (Vector2.Distance(origin, Main.npc[index].Center) < maxDistanceToCheck && canHit)
						{
							if (Main.npc[index].boss || Main.npc[index].type == NPCID.WallofFleshEye)
								bossFound = true;

							maxDistanceToCheck = Vector2.Distance(origin, Main.npc[index].Center);
							closestTarget = Main.npc[index];
						}
					}
				}
			}
			else
			{
				for (int index = 0; index < Main.npc.Length; index++)
				{
					if (Main.npc[index].CanBeChasedBy(null, false))
					{
						float extraDistance = (Main.npc[index].width / 2) + (Main.npc[index].height / 2);

						bool canHit = true;
						if (extraDistance < maxDistanceToCheck && !ignoreTiles)
							canHit = Collision.CanHit(origin, 1, 1, Main.npc[index].Center, 1, 1);

						if (Vector2.Distance(origin, Main.npc[index].Center) < maxDistanceToCheck && canHit)
						{
							maxDistanceToCheck = Vector2.Distance(origin, Main.npc[index].Center);
							closestTarget = Main.npc[index];
						}
					}
				}
			}
			return closestTarget;
		}


		/// <summary>
		/// Simulates AoE
		/// </summary>
		/// <param name="hitbox">hitbox of the attack, will be multiplied by imbue sizes</param>
		/// <param name="damage">Danage of the attack, will be multiplied by imbue damages if <paramref name="updatedamage"/> is true</param>
		/// <param name="knockback">Knockback of the AoE, will be multied by imbue sizes</param>
		/// <param name="source">Source of the damage, used to get imbues</param>
		/// <param name="damageClass"><seealso cref="DamageClass"/> of the AoE</param>
		/// <param name="updatedamage">Whether to update damage with imbue stats, defaults to true</param>
		/// <param name="adjustX">How many times to shift left the hitbox if it grew, or shift right if it shrunk</param>
		/// <param name="adjustY">How many times to shift up the hitbox if it grew, or shift it down if it shrunk</param>
		/// <param name="ignoredNPCs">The <seealso cref="Entity.whoAmI"/> of <seealso cref="NPC"/>s you don't want to damage</param>
		public static Rectangle SimulateAOE(Rectangle hitbox, float damage, float knockback, Entity source, DamageClass damageClass, bool updatedamage = true, int adjustX = 1, int adjustY = 1, params int[] ignoredNPCs)
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
									damage *= projectile.ArcaneOdyssey().SecondImbue.AOImbueDamage;
								}
								mult *= projectile.ArcaneOdyssey().SecondImbue.AOImbueSize;
								knockback *= projectile.ArcaneOdyssey().SecondImbue.AOImbueSize;
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
				mult *= player1.SizeMulti;
			}

			ScaleRectangle(ref hitbox, mult, adjustX, adjustY);

			foreach (NPC target in Main.ActiveNPCs)
			{
				if (ignoredNPCs.Contains(target.whoAmI))
					continue;
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
					}
					if (modifiers.GetDamage(damage) > 0 && source.TryGetOwner(out Player player) && Main.myPlayer == player.whoAmI)
					{
						if (source.TryGetOwner(out AOPlayer player2))
						{
							if (source is Item item && item.ModItem is SpiritEnergy)
							{
								if (!target.immortal)
									player2.TrySpiritLifesteal(Math.Min(item.OriginalDamage, item.damage), false);
								if (Main.netMode == NetmodeID.SinglePlayer && (item.Imbue() is DeathMagic || item.SecondImbue() is DeathMagic) && (target.lifeMax < (player.statLifeMax2 * 2)))
								{
									target.StrikeInstantKill();
								}
							}
							else if (source is Projectile projectile)
							{
								if (Main.netMode == NetmodeID.SinglePlayer && (projectile.Imbue() is DeathMagic || projectile.SecondImbue() is DeathMagic) && (target.lifeMax < (player.statLifeMax2 * 2)))
								{
									target.StrikeInstantKill();
								}
								if (projectile.ModProjectile is SpiritProjectile)
								{
									if (!target.immortal)
										player2.TrySpiritLifesteal(Math.Min(projectile.originalDamage, projectile.damage), false);
								}
								else
								{
									var proj = projectile.ArcaneOdyssey();
									if (proj.Imbue is SpiritEnergy || proj.SecondImbue is SpiritEnergy)
									{
										if (!target.immortal)
											player2.TrySpiritLifesteal(Math.Min(projectile.originalDamage, projectile.damage));
									}
								}
							}
						}
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
				if (item.ModItem is Imbuable imbue)
					second = imbue.Imbue;
				else
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

		public static Tile GetTile(int x, int y)
		{
			if (!WorldGen.InWorld(x, y))
				return new Tile();

			return Main.tile[x, y];
		}

		public static string Replace(this string text, string toRemove) => text.Replace(toRemove, null);

		public static bool ImbueClassCheck(Projectile projectile)
		{
			if (projectile is not null && projectile.active)
			{
				if ((projectile.ModProjectile is null or AOBaseProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && projectile.ArcaneOdyssey().CanBeAffected)
				{
					return (
							projectile.DamageType.CountsAsClass(DamageClass.Melee)
							|| projectile.DamageType.CountsAsClass(DamageClass.Ranged)
							|| projectile.ModProjectile is MagicSpell or SpiritProjectile or StrengthTechnique or BaseMagicCircle
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
			if (item is not null && item.active && (!item.accessory || item.ModItem is Scroll or Imbuable) && (item.ModItem is null or AOBaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && item.ArcaneOdyssey().CanBeAffected && item.ammo == AmmoID.None)
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
				if (imbue is null)
				{
					return true;
				}
				if (item.ModItem is Scroll scroll)
				{
					if (scroll.CanHaveMagic && imbue is AOMagic && scroll.ExtraConditionsForImbue(imbue))
					{
						return true;
					}
					if (scroll.CanHaveFS && imbue is FightingStyle && scroll.ExtraConditionsForImbue(imbue))
					{
						return true;
					}
					if (scroll.CanHaveRelic && imbue is SpiritEnergy && scroll.ExtraConditionsForImbue(imbue))
					{
						return true;
					}
					return false;
				}
				if (item.ModItem is SpiritEnergy)
				{
					return imbue is AOMagic && Main.hardMode;
				}
				if (imbue is FightingStyle)
				{
					return (item.ArcaneOdyssey()?.WeaponsType == WeaponType.Normal || item.ArcaneOdyssey()?.WeaponsType == WeaponType.Strength) && item.ModItem is not Imbuable;
				}
				if (imbue is AOMagic)
				{
					return (item.ArcaneOdyssey()?.WeaponsType == WeaponType.Normal || item.ArcaneOdyssey()?.WeaponsType == WeaponType.Arcanium) && (item.ModItem is not Imbuable || (item.ModItem is SpiritEnergy or FightingStyle && Main.hardMode));
				}
				if (imbue is SpiritEnergy)
				{
					return item.ArcaneOdyssey()?.WeaponsType == WeaponType.Normal && (item.ModItem is not Imbuable || (item.ModItem is AOMagic or FightingStyle && Main.hardMode));
				}
			}
			return false;
		}
		public static bool IsTileSolidGround(this Tile tile) => tile != null && tile.HasUnactuatedTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]);
		public static bool IsTileReallySolidGround(this Tile tile) => tile != null && tile.HasUnactuatedTile && (Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType]);

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

		public static bool WithinBounds(this int index, int cap) => index >= 0 && index < cap;

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
					foreach (Combo buffkeys in imbue.CombinedDebuffs)
					{
						if (target.HasBuff(buffkeys.requirement) || (buffkeys.requirement == BuffID.Wet && target.wet))
						{
							target.AddBuff(buffkeys.result, buffkeys.duration);
						}

						foreach (var alt in buffkeys.alternatives)
						{
							if (target.HasBuff(alt) || (alt == BuffID.Wet && target.wet))
							{
								target.AddBuff(buffkeys.result, buffkeys.duration);
							}
						}
					}
				}

				foreach (Synergy multiplier in imbue.Effects.magicBuffMultipliers)
				{
					if (target.HasBuff(multiplier.buffID) || (multiplier.buffID == BuffID.Wet && target.wet))
					{
						modifiers.FinalDamage += multiplier.multiplier.MultiToPercent();
					}

					foreach (var alt in multiplier.alternatives)
					{
						if (target.HasBuff(alt) || (alt == BuffID.Wet && target.wet))
						{
							modifiers.FinalDamage += multiplier.multiplier.MultiToPercent();
						}
					}
				}

				if (Main.netMode == NetmodeID.SinglePlayer) // things would get chaotic in multiplayer if everyone kept clearing eachothers debuffs
				{
					foreach (var buff in imbue.Effects.clearBuffs)
					{
						if (target.HasBuff(buff.id))
						{
							target.DelBuff(target.FindBuffIndex(buff.id));
						}

						foreach (var alt in buff.alternatives)
						{
							if (target.HasBuff(alt))
							{
								target.DelBuff(target.FindBuffIndex(alt));
							}
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

			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player, ai0, ai1, ai2);
			projectile.netUpdate = true;
			projectile.netSpam = 0;
			return projectile;
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
				if (imbue is SpiritEnergy)
					if (!npc.immortal)
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
						return AOUtils.BossesKilled;
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
			if (item.consumable && item.createTile == -1 && item.createWall == -1)
			{
				return ItemType.Consumable;
			}
			if (item.material)
			{
				return ItemType.Material;
			}
			if (item.createTile != -1 || item.createWall != -1)
			{
				return ItemType.Block;
			}
			return ItemType.Item;
		}

		public static AORarities GetItemRare(this Item item)
		{
			if (ExternalModSupport.HasCalamity)
			{
				if (item.rare == ExternalModSupport.Calamity.Find<ModRarity>("DarkOrange").Type)
				{
					return AORarities.Unknown;
				}
			}

			if (item.rare == ModContent.RarityType<HotPinkRare>())
			{
				return AORarities.Special;
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
				return AORarities.Mystic;
			}

			if (item.master || item.rare == ItemRarityID.Master)
			{
				return AORarities.Legendary;
			}

			return item.rare switch
			{
				ItemRarityID.Gray => AORarities.Junk,
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
				ItemRarityID.Red => AORarities.Mythical,
				ItemRarityID.Purple => AORarities.Mythical,
				_ => AORarities.Lost,
			};
		}
		#endregion
		#region Enum Methods
		public static T ParseEnum<T>(string value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}

		/// <summary>
		/// Gets a <see cref="List{T}"/> containing all posible values from an <see cref="Enum"/>
		/// </summary>
		/// <typeparam name="T">The <see cref="Enum"/> to get a list from</typeparam>
		/// <returns></returns>
		public static List<T> GetEnumValues<T>() where T : Enum
		{
			List<T> list = [];
			foreach (object o in Enum.GetValues(typeof(T))) list.Add(ParseEnum<T>(o.ToString()));
			return list;
		}
		/// <summary> 
		/// <inheritdoc cref="GetEnumValues{T}()"/> minus all <typeparamref name="T"/> in <paramref name="exceptions"/>
		/// </summary>
		public static List<T> GetEnumValues<T>(List<T> exceptions) where T : Enum
		{
			List<T> list = [];
			foreach (object o in Enum.GetValues(typeof(T))) list.Add(ParseEnum<T>(o.ToString()));
			foreach (var e in exceptions) list.Remove(e);
			return list;
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
		public static bool HasTypeInInventory<T>(this Player player) where T : ModItem
		{
			List<Item> no = [.. player.inventory, player.trashItem];
			no.RemoveAll(e => e.ModItem is null);
			foreach (var item in no)
			{
				if (item.ModItem.GetType().Name == typeof(T).Name || item.ModItem.GetType().IsSubclassOf(typeof(T)))
				{
					return true;
				}
			}
			return false;
		}

		public static bool HasTypeInInventory<T>(this Player player, out T item) where T : ModItem
		{
			item = null;
			if (player.ArcaneOdyssey().EquippedImbues.Contains(ModContent.ItemType<T>()) || player.ArcaneOdyssey().EquippedSecondImbues.Contains(ModContent.ItemType<T>()))
			{
				item = ModContent.GetInstance<T>();
				return true;
			}
			List<Item> no = [.. player.inventory, player.trashItem];
			no.RemoveAll(e => e.ModItem is null);
			foreach (var items in no)
			{
				if (items.ModItem.GetType().Name == typeof(T).Name || items.ModItem.GetType().IsSubclassOf(typeof(T)))
				{
					item = (T)items.ModItem;
					return true;
				}
			}
			return false;
		}

		public static List<T> Sorted<T>(this List<T> self, Comparison<T> comparer)
		{
			self.Sort(comparer);
			return self;
		}

		public static bool HasTypeInInventory(this Player player, Type type, Mod mod = null)
		{
			mod ??= ArcaneOdysseyMod.Instance;
			if (mod.TryFind<ModItem>(type.Name, out var moditem) && player.ArcaneOdyssey().EquippedImbues.Contains(moditem.Type)) 
			{
				return true; 
			}
			List<Item> no = [.. player.inventory, player.trashItem];
			no.RemoveAll(e => e.ModItem is null);
			foreach (var items in no)
			{
				if (items.ModItem.GetType().Name == type.Name || items.ModItem.GetType().IsSubclassOf(type))
				{
					return true;
				}
			}
			return false;
		}

		public static bool HasTypeInInventory(this Player player, Type type, out ModItem item)
		{
			List<Item> no = [.. player.inventory, player.trashItem];
			item = null;
			no.RemoveAll(e => e.ModItem is null);
			foreach (var items in no)
			{
				if (items.ModItem.GetType().Name == type.Name || items.ModItem.GetType().IsSubclassOf(type))
				{
					item = items.ModItem;
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

		public static Item PlayerItem(this ModPlayer player)
		{
			if (Main.myPlayer == player.Player.whoAmI && (!Main.mouseItem.IsAir) && Main.mouseItem.active)
			{
				return Main.mouseItem;
			}
			else return player.Player.HeldItem;
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

		public static AONPC ArcaneOdyssey(this NPC npc)
		{
			if (npc is not null && npc.active && npc.TryGetGlobalNPC<AONPC>(out var npcc))
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
				if (item.ModItem is AOWeapon weap)
					return weap;
				return item.ArcaneOdyssey();
			}
			return null;
		}

		#endregion
	}

	public struct WeaponAbility
	{
		public string Name;
		public string Description;
		public Color Colour;
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
	/// <param name="moditem">This moditem</param>
	/// <param name="otherItems">The internal names of the other two items in this set, head then boots</param>
	/// <param name="colour">The colour of this set</param>
	public struct SetBonusHelper(ModItem moditem, Color colour, params string[] otherItems)
	{
		private Color Colour = colour;
		public string[] OtherItems = otherItems;

		public static string Key(ModItem item, string suffix)
		{
			return $"Mods.{item.Mod.Name}.{item.LocalizationCategory}.{item.Name}.Set.{suffix}";
		}

		public LocalizedText LocalizedName = Language.GetOrRegister(Key(moditem, "DisplayName"), () => Key(moditem, "DisplayName"));
		public LocalizedText LocalizedDescription = Language.GetOrRegister(Key(moditem, "Description"), () => Key(moditem, "Description"));

		public readonly string Tooptip => $"[c/{Colour.Hex3()}:{LocalizedName.Value}]: {LocalizedDescription.Value}";
	}

	public struct ImbueArmourStats(int size = 0, int attkspeed = 0, int power = 0, int defence = 0, int agility = 0, int pierce = 0, int haste = 0)
	{
		public int Size = size;
		public int Attkspeed = attkspeed;
		public int Power = power;
		public int Pierce = pierce;
		public int Defence = defence;
		public int Agility = agility;
		public int Haste = haste;

		public readonly ImbueArmourStats Corrected(Imbuable imbue)
		{
			if (imbue is FightingStyleBarred barred)
			{
				return new ImbueArmourStats(
					MathHelper.Lerp(Size / 4f, Size, barred.LerpValue).Round(),
					MathHelper.Lerp(Attkspeed / 4f, Attkspeed, barred.LerpValue).Round(),
					MathHelper.Lerp(Power / 4f, Power, barred.LerpValue).Round(),
					MathHelper.Lerp(Defence / 4f, Defence, barred.LerpValue).Round(),
					MathHelper.Lerp(Agility / 4f, Agility, barred.LerpValue).Round(),
					MathHelper.Lerp(Pierce / 4f, Pierce, barred.LerpValue).Round(),
					MathHelper.Lerp(Haste / 4f, Haste, barred.LerpValue).Round()
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
		Vanity,
		Consumable
	}

	/// <summary>
	/// Arcane Odyssey rarities, converted to RarityID
	/// </summary>
	public enum AORarities
	{
		Junk = ItemRarityID.Gray,
		Common = ItemRarityID.White,
		Uncommon = ItemRarityID.Green,
		Rare = ItemRarityID.LightRed,
		Mystic = ItemRarityID.LightPurple,
		Legendary = ItemRarityID.Yellow,
		Mythical = ItemRarityID.Red,
		Lost,
		Unknown,
		Special
	}

	public enum WeaponType
	{
		Normal,
		Arcanium,
		Strength,
		Artisinal
	}

	public enum AOImbuableTier
	{
		Normal,
		Lost,
		Ancient,
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
	/// <param name="debuffRequirement">Damage% requirement to activate debuff</param>
	public struct Debuff(int debuffid, int duration = 600, int debuffRequirement = 0)
	{
		public float debuffPercent = debuffRequirement / 100f;
		public int debuffID = debuffid;
		public int debuffDuration = duration;

		public static Debuff Create<T>(int duration = 600, int debuffRequirement = 0) where T : ModBuff
		{
			return new(ModContent.BuffType<T>(), duration, debuffRequirement);
		}
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
	public struct SynergyEffects(ClearBuff[] buffsToClear, List<Synergy> buffMultipliers)
	{
		public ClearBuff[] clearBuffs = buffsToClear;
		public List<Synergy> magicBuffMultipliers = buffMultipliers;
	}

	public struct ClearBuff(int id, params int[] alternatives)
	{
		public int id = id;
		public int[] alternatives = alternatives;

		public static ClearBuff Create<T>() where T : AOBaseBuff
		{
			return new(ModContent.BuffType<T>(), [..ModContent.GetInstance<T>().Counterparts]);
		}
	}

	/// <summary>
	/// sahhhhhduiahyfoahgoaig
	/// </summary>
	/// <param name="requirement"></param>
	/// <param name="result"></param>
	/// <param name="duration"></param>
	public struct Combo(int requirement, int result, int duration = 60, params int[] alternatives)
	{
		public int requirement = requirement;
		public int result = result;
		public int duration = duration;
		public int[] alternatives = alternatives;

		public static Combo Create<T>(int result, int duration = 60) where T : AOBaseBuff
		{
			return new(ModContent.BuffType<T>(), result, duration, [..ModContent.GetInstance<T>().Counterparts]);
		}

		public static Combo Create<T, R>(int duration = 60) where T : AOBaseBuff where R : AOBaseBuff
		{
			return new(ModContent.BuffType<T>(), ModContent.BuffType<R>(), duration, [..ModContent.GetInstance<T>().Counterparts]);
		}
	}

	/// <summary>
	/// Damage multipliers from having debuffs interact
	/// </summary>
	/// <param name="buffid">Terraria.ID.BuffID</param>
	/// <param name="multi">Damage multipier (ex. 1.25f)</param>
	public struct Synergy(int buffid, float multi, params int[] alternatives)
	{
		public int buffID = buffid;
		public float multiplier = multi;
		public int[] alternatives = alternatives;

		public static Synergy Create<T>(float multi) where T : AOBaseBuff
		{
			return new(ModContent.BuffType<T>(), multi, [..ModContent.GetInstance<T>().Counterparts]);
		}
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

	public enum ScrollTier
	{
		Common,
		Rare,
		Lost
	}
}
