using ArcaneOdyssey.AOPlayers;
using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Buffs.MagicMarks;
using ArcaneOdyssey.GlobalTypes;
using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Ancient;
using ArcaneOdyssey.Imbues.Magic.Normal;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace ArcaneOdyssey
{
	public static class AOUtils
	{
		public static float UpdateCount => Main.GameUpdateCount / 100f;

		public const string BlankTexture = ArcaneOdysseyMod.InternalName + "/Assets/Blank";
		public const string SlashTexture = ArcaneOdysseyMod.InternalName + "/Assets/BasicSlash";
		public const string GelTexture = ArcaneOdysseyMod.InternalName + "/Assets/GelBuffBackground";
		public const string DebuffTexture = ArcaneOdysseyMod.InternalName + "/Assets/Debuff";

		public static T Safe<T>(ModItem item) where T : ModItem
		{
			if (item is T)
			{
				return item as T;
			}
			return null;
		}

		internal static List<string> options = [
			"Terraria/FavoriteDesc",
			"Terraria/NoTransfer",
			"Terraria/SocialDesc",
			"Terraria/Damage",
			"Terraria/CritChance",
			"Terraria/Speed",
			"Terraria/NoSpeedScaling",
			"Terraria/SpecialSpeedScaling",
			"Terraria/Knockback",
			"Terraria/FishingPower",
			"Terraria/NeedsBait",
			"Terraria/BaitPower",
			"Terraria/Equipable",
			"Terraria/WandConsumes",
			"Terraria/Quest",
			"Terraria/Vanity",
			"Terraria/Defense",
			"Terraria/PickPower",
			"Terraria/AxePower",
			"Terraria/HammerPower",
			"Terraria/TileBoost",
			"Terraria/HealLife",
			"Terraria/HealMana",
			"Terraria/UseMana",
			"Terraria/Placeable",
			"Terraria/Ammo",
			"Terraria/Consumable",
			"Terraria/Material",
			"Terraria/Tooltip",
		];

		public static Vector2 Add(this Vector2 vec, float add) => vec.SafeNormalize() * (vec.Length() + add);

		public static Vector2 SafeNormalize(this Vector2 vector) => vector.SafeNormalize(Vector2.Zero);

		public static int BiomeType<T>() where T : ModBiome => ModContent.GetInstance<T>()?.Type ?? 0;

		public static IItemDropRule Common<T>(int chanceDenominator = 1, int minimumDropped = 1, int maximumDropped = 1) where T : ModItem => ItemDropRule.Common(ModContent.ItemType<T>(), chanceDenominator, minimumDropped, maximumDropped);

		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n-- > 1)
			{
				int k = Main.rand.Next(n + 1);
				(list[n], list[k]) = (list[k], list[n]);
			}
		}

		public static string LocalizationCategoryOf<T>() where T : class, ILocalizedModType => ModContent.GetInstance<T>().LocalizationCategory;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rect"></param>
		/// <param name="offset">set to 8 to use middle of tiles</param>
		/// <returns></returns>
		public static Rectangle ToTileRect(this Rectangle rect, int offset = 0)
		{
			return new((rect.X / 16) - (offset / 2), (rect.Y / 16) - (offset / 2), (rect.Width / 16) + offset, (rect.Height / 16) + offset);
		}

		public static Rectangle ToWorldRect(this Rectangle rect, int offset = 0)
		{
			return new((rect.X * 16) - (offset / 2), (rect.Y * 16) - (offset / 2), (rect.Width * 16) + offset, (rect.Height * 16) + offset);
		}

		public static bool RequestIfExists<T>(string name, ref Asset<T> texture, AssetRequestMode mode = AssetRequestMode.AsyncLoad) where T : class
		{
			if (ModContent.HasAsset(name))
			{
				texture ??= ModContent.Request<T>(name, mode);
				return true;
			}
			return false;
		}

		public static bool TryGetConfig(this Mod mod, string name, out ModConfig config)
		{
			config = mod.GetConfig(name);
			return config is not null;
		}

		public static void AddRange<T>(this List<T> list, params T[] items) => list.AddRange(items.ToList());

		public static Asset<T> Request<T>(string name, ref Asset<T> texture, AssetRequestMode mode = AssetRequestMode.AsyncLoad) where T : class
		{
			texture ??= ModContent.Request<T>(name, mode);
			return texture;
		}

		public static IList<T> ShuffledList<T>(IList<T> list)
		{
			int n = list.Count;
			while (n-- > 1)
			{
				int k = Main.rand.Next(n + 1);
				(list[n], list[k]) = (list[k], list[n]);
			}
			return list;
		}

		public static bool? ToNullableBool(this int value)
		{
			if (value == 0)
			{
				return null;
			}
			return value == 1;
		}

		public static int ToInt32(this bool? value)
		{
			if (value.HasValue)
			{
				return value.Value.ToDirectionInt();
			}
			return 0;
		}

		public static int AddTooltip(this List<TooltipLine> tooltips, TooltipLine toAdd, Color? colour = null)
		{
			toAdd.OverrideColor = colour;
			tooltips.Reverse();
			options.Reverse();

			bool found = false;
			foreach (var option in options)
			{
				var index = tooltips.FindIndex((TooltipLine e) => e.FullName.StartsWith(option) || e.FullName == option);
				if (index != -1)
				{
					tooltips.Insert(index, toAdd);
					options.Reverse();
					if (!options.Contains(toAdd.FullName))
						options.Add(toAdd.FullName);
					found = true;
					break;
				}
			}

			tooltips.Reverse();

			if (!found)
			{
				tooltips.Add(toAdd);
				options.Reverse();
				if (!options.Contains(toAdd.FullName))
					options.Add(toAdd.FullName);
			}
			return tooltips.IndexOf(toAdd);
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

		public static void Write(this BinaryWriter writer, Rectangle rect)
		{
			writer.Write(rect.X);
			writer.Write(rect.Y);
			writer.Write(rect.Width);
			writer.Write(rect.Height);
		}

		public static Rectangle ReadRectangle(this BinaryReader reader) => new(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());

		/// <inheritdoc cref="BinaryWriter.Write(float)"/>
		public static void Write(this BinaryWriter writer, Vector2? vec) => writer.WriteVector2(vec.GetValueOrDefault(Vector2.Zero));

		/// <inheritdoc cref="BinaryReader.ReadSingle"/>
		public static Vector2? ReadNullableVector2(this BinaryReader reader)
		{
			var vec = reader.ReadVector2();
			if (vec == Vector2.Zero)
			{
				return null;
			}
			else
			{
				return vec;
			}
		}

		public static Color GetAlpha(this Projectile projectile) => projectile.GetAlpha(Color.White);


		/// <param name="adjustX">How many times to shift left the hitbox if it grew, or shift right if it shrunk</param>
		/// <param name="adjustY">How many times to shift up the hitbox if it grew, or shift it down if it shrunk</param>
		public static Rectangle Scaled(this Rectangle rectangle, float scale, int adjustX = 1, int adjustY = 1)
		{
			Rectangle rect = new(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
			var diffX = ((rect.Width - (rect.Width * scale)) / 2f).Round();
			var diffY = ((rect.Height - (rect.Height * scale)) / 2f).Round();
			rect.Width = (rect.Width * scale).Round();
			rect.Height = (rect.Height * scale).Round();
			rect.X += diffX * adjustX;
			rect.Y += diffY * adjustY;
			return rect;
		}

		public static Rectangle Inflated(this Rectangle rect, Vector2 increase) => Utils.CenteredRectangle(rect.Center(), rect.Size() + increase);

		public static SynergyEffects CopySynergiesFromImbue<T>() where T : Imbuable
		{
			return ModContent.GetInstance<T>().Effects;
		}

		public static Combo[] CopyCombosFromImbue<T>() where T : Imbuable
		{
			return ModContent.GetInstance<T>().CombinedDebuffs;
		}

		public static int[] ToIntArray(this Rectangle rect) => [rect.X, rect.Y, rect.Width, rect.Height];

		public static Rectangle FromIntArray(this int[] array) => new(array[0], array[1], array[2], array[3]);

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

		public static Vector2 RandomBorder(this Rectangle rect)
		{
			var pos = rect.TopLeft();
			switch (Main.rand.Next(4))
			{
				case 0: // left
					pos.Y += Main.rand.NextFloat(rect.Height);
					break;
				case 1: // right
					pos.X += rect.Width;
					pos.Y += Main.rand.NextFloat(rect.Height);
					break;
				case 2: // bottom
					pos.X += Main.rand.NextFloat(rect.Width);
					pos.Y += rect.Height;
					break;
				case 3: // top
					pos.X += Main.rand.NextFloat(rect.Width);
					break;
			}
			return pos;
		}

		public static Vector2 RandomArea(this Rectangle rect) => new(rect.X + Main.rand.NextFloat(rect.Width), rect.Y + Main.rand.NextFloat(rect.Height));

		public static bool NPCAlive<T>(out NPC found) where T : ModNPC
		{
			found = null;
			foreach (var npc in Main.ActiveNPCs)
			{
				if ((npc.type == ModContent.NPCType<T>()) && (npc.life > 0))
				{
					found = npc;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"><see cref="NPCID"/></param>
		/// <param name="found"></param>
		/// <returns></returns>
		public static bool NPCAlive(int type, out NPC found)
		{
			found = null;
			foreach (var npc in Main.ActiveNPCs)
			{
				if ((npc.type == type) && (npc.life > 0))
				{
					found = npc;
					return true;
				}
			}
			return false;
		}

		public static SynergyEffects CopyDamageSynergiesFromImbue<T>() where T : Imbuable
		{
			return ModContent.GetInstance<T>().Effects with { clearBuffs = [] };
		}

		public static float RelativeScale(this Rectangle rect, int scale = 64, float min = .5f, float max = 2.5f)
		{
			return MathHelper.Clamp((rect.Width + rect.Height) / 2f / scale, min, max);
		}

		public static Imbuable Imbue(this Player player) => player?.ArcaneOdyssey()?.Imbue;
		public static Imbuable Imbue(this Projectile projectile) => projectile?.ArcaneOdyssey()?.Imbue;
		public static Imbuable Imbue(this Item item) => item?.ArcaneOdyssey()?.Imbue;

		public static Imbuable SecondImbue(this Projectile projectile) => projectile?.ArcaneOdyssey()?.SecondImbue;
		public static Imbuable SecondImbue(this Item item) => item?.ArcaneOdyssey()?.SecondImbue;

		public static Dust NewDustImperfect(Vector2 position, int type, Vector2? velocity = null, int Alpha = 0, Color newColor = default, float Scale = 1f)
		{
			Scale = Math.Clamp(Scale, 0f, 10f);
			velocity ??= Vector2.Zero;
			return Dust.NewDustDirect(position, 0, 0, type, velocity.Value.X, velocity.Value.Y, Alpha, newColor, Scale);
		}

		public static EntitySource_ItemUse GetSource_ItemUse(this Entity item, Player player, string context = null) => new(player, item as Item, context);
		public static EntitySource_ItemUse GetSource_ItemUse(this Item item, Player player, string context = null) => new(player, item, context);

		public static int Round(this float num) => (int)Math.Round(num);

		public static string GetTexture<T>(bool usemodtype = true) where T : class
		{
			if (usemodtype && ArcaneOdysseyMod.finishedLoading)
			{
				if (typeof(T).IsSubclassOf(typeof(ModItem)))
				{
					var inst = ModContent.GetInstance<T>() as ModItem;
					var tex = inst.Mod.Name + "/" + inst.Texture.Replace(inst.Mod.Name + "/");
					if (ModContent.HasAsset(tex))
						return tex;
				}
				if (typeof(T).IsSubclassOf(typeof(ModProjectile)))
				{
					var inst = ModContent.GetInstance<T>() as ModProjectile;
					var tex = inst.Mod.Name + "/" + inst.Texture.Replace(inst.Mod.Name + "/");
					if (ModContent.HasAsset(tex))
						return tex;
				}
				if (typeof(T).IsSubclassOf(typeof(ModGore)))
				{
					var inst = ModContent.GetInstance<T>() as ModGore;
					var tex = inst.Mod.Name + "/" + inst.Texture.Replace(inst.Mod.Name + "/");
					if (ModContent.HasAsset(tex))
						return tex;
				}
				if (typeof(T).IsSubclassOf(typeof(ModBuff)))
				{
					var inst = ModContent.GetInstance<T>() as ModBuff;
					var tex = inst.Mod.Name + "/" + inst.Texture.Replace(inst.Mod.Name + "/");
					if (ModContent.HasAsset(tex))
						return tex;
				}
			}
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

		public static Vector2 Centre(this Dust dust, Vector2? newPos = null)
		{
			if (newPos.HasValue)
			{
				dust.position = newPos.Value - (dust.frame.Size() * dust.scale / 2f);
				return dust.position;
			}
			else
			{
				return dust.position + (dust.frame.Size() * dust.scale / 2f);
			}
		}

		public static bool BossAlive
		{
			get
			{
				foreach (var npc in Main.ActiveNPCs)
				{
					if (npc.boss)
						return true;
				}
				return false;
			}
		}

		public static bool BothTwinsAlive
		{
			get
			{
				var alivecount = 0;
				foreach (var npc in Main.ActiveNPCs)
				{
					if (npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
						alivecount++;
				}
				return alivecount > 1;
			}
		}

		public static bool EoWStillAlive
		{
			get
			{
				var alivecount = 0;
				foreach (var npc in Main.ActiveNPCs)
				{
					if (npc.type == NPCID.EaterofWorldsBody || npc.type == NPCID.EaterofWorldsHead || npc.type == NPCID.EaterofWorldsTail)
						alivecount++;
				}
				return alivecount > 1;
			}
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
		public static int Clamp(this int num, int min, int max) => Math.Clamp(num, min, max);

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
							damage *= imbue.ScrollDamage;
						}
						range *= imbue.ScrollSize;
						knockback *= imbue.ScrollSize;
						if (source is Projectile projectile)
						{
							if (projectile.SecondImbue() is not null)
							{
								if (updatedamage)
								{
									damage *= projectile.SecondImbue().ImbueDamage;
								}
								range *= projectile.SecondImbue().ImbueSize;
								knockback *= projectile.SecondImbue().ImbueSize;
							}
						}
					}
					else
					{
						if (updatedamage)
						{
							damage *= imbue.ImbueDamage;
						}
						range *= imbue.ImbueSize;
						knockback *= imbue.ImbueSize;
						if (source is Projectile projectile)
						{
							if (projectile.SecondImbue() is not null)
							{
								if (updatedamage)
								{
									damage *= projectile.SecondImbue().ImbueDamage;
								}
								range *= projectile.SecondImbue().ImbueSize;
								knockback *= projectile.SecondImbue().ImbueSize;
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
					ModDamageHelper modifiers = new();
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

		/// <summary>
		/// Draws a line of a sprite
		/// </summary>
		/// <param name="start"></param>
		/// <param name="end"></param>
		/// <param name="sprite"></param>
		/// <param name="scale"></param>
		/// <param name="maxframes"></param>
		/// <param name="frame"></param>
		/// <param name="colour"></param>
		/// <param name="effects"></param>
		/// <returns></returns>
		public static ChainEndInfo DrawChain(Vector2 start, Vector2 end, Texture2D sprite, float scale = 1f, int maxframes = 1, int frame = 0, Color? colour = null, SpriteEffects effects = SpriteEffects.None, SpriteBatch batch = null)
		{
			batch ??= Main.spriteBatch;

			var size = new Vector2(sprite.Width, sprite.Height / maxframes) / 2f;

			bool colourisntnull = colour.HasValue;

			float rotation = start.AngleTo(end);
			var width = sprite.Width * scale;

			bool hasNotEnded = true;
			var length = 0;
			while (hasNotEnded)
			{
				var source = sprite.Frame(1, maxframes, 0, frame);
				float distance = start.Distance(end);
				if (distance < width)
				{
					hasNotEnded = false;
				}
				else if (float.IsNaN(distance))
				{
					hasNotEnded = false;
				}
				else
				{
					length++;
					start += start.DirectionTo(end) * width;
					if (!colourisntnull)
						colour = Lighting.GetColor(start.ToTileCoordinates());
					batch.Draw(sprite, start - Main.screenPosition, source, colour.Value, rotation, size, scale, effects, 0); distance = start.Distance(end);

					if (distance < width)
					{
						hasNotEnded = false;
					}
					else if (float.IsNaN(distance))
					{
						hasNotEnded = false;
					}
					else if (++frame >= maxframes)
					{
						frame = 0;
					}
				}
			}
			return new ChainEndInfo(frame, start, length, rotation);
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


		public static void NPCDialogue(this NPC npc, string message, Color? colour = null)
		{
			Main.NewText(message, colour);
			CombatText.NewText(npc.Hitbox, colour.GetValueOrDefault(Color.White), message, true);
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
							damage *= imbue.ScrollDamage;
						}
						mult *= imbue.ScrollSize;
						knockback *= imbue.ScrollSize;
						if (source is Projectile projectile)
						{
							if (projectile.ArcaneOdyssey().SecondImbue is not null)
							{
								if (updatedamage)
								{
									damage *= projectile.ArcaneOdyssey().SecondImbue.ImbueDamage;
								}
								mult *= projectile.ArcaneOdyssey().SecondImbue.ImbueSize;
								knockback *= projectile.ArcaneOdyssey().SecondImbue.ImbueSize;
							}
						}
					}
					else
					{
						if (updatedamage)
						{
							damage *= imbue.ImbueDamage;
						}
						mult *= imbue.ImbueSize;
						knockback *= imbue.ImbueSize;
						if (source is Projectile projectile)
						{
							if (projectile.ArcaneOdyssey().SecondImbue is not null)
							{
								if (updatedamage)
								{
									damage *= projectile.ArcaneOdyssey().SecondImbue.ImbueDamage;
								}
								mult *= projectile.ArcaneOdyssey().SecondImbue.ImbueSize;
								knockback *= projectile.ArcaneOdyssey().SecondImbue.ImbueSize;
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
					ModDamageHelper modifiers = new();
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

		public static float Length(this Rectangle rect)
		{
			return MathF.Sqrt(rect.Width.Pow() + rect.Height.Pow());
		}

		public static float Pow(this int num, float raise = 2) => MathF.Pow(num, raise);

		public static float Pow(this float num, float raise = 2f) => MathF.Pow(num, raise);

		public static Tile GetTile(Point point) => GetTile(point.X, point.Y);
		public static Tile GetTile(Point16 point) => GetTile(point.X, point.Y);

		public static string Replace(this string text, string toRemove) => text.Replace(toRemove, null);

		public static bool ImbueClassCheck(Projectile projectile)
		{
			if (projectile is not null && projectile.active)
			{
				if ((projectile.ModProjectile is null or BaseProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && (projectile.ArcaneOdyssey()?.CanBeAffected == true))
				{
					return (
							projectile.DamageType.CountsAsClass(DamageClass.Melee)
							|| projectile.DamageType.CountsAsClass(DamageClass.Ranged)
							|| projectile.DamageType.CountsAsClass(DamageClass.Throwing)
							|| projectile.DamageType.CountsAsClass(DamageClass.Magic)
							|| projectile.ModProjectile is MagicSpell or SpiritProjectile or StrengthTechnique or Circle
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
			if ((item is not null) && item.active && ((!item.accessory) || item.ModItem is Scroll or Imbuable) && (item.ModItem is null or BaseItem || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && (item.ArcaneOdyssey()?.CannotBeAffected == false) && (item.ammo == AmmoID.None))
			{
				if (item.ArcaneOdyssey()?.WeaponsType != WeaponType.Artisinal)
				{
					return item.DamageType.CountsAsClass(DamageClass.Melee)
						|| item.DamageType.CountsAsClass(DamageClass.Ranged)
						|| item.DamageType.CountsAsClass(DamageClass.Throwing)
						|| item.DamageType.CountsAsClass(DamageClass.Magic)
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
					if (scroll.CanHaveMagic && imbue is MagicType && scroll.ExtraConditionsForImbue(imbue))
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
					return imbue is MagicType && Main.hardMode;
				}
				if (imbue is FightingStyle)
				{
					if (item.DamageType.CountsAsClass(DamageClass.Magic))
					{
						return false;
					}
					return (item.ArcaneOdyssey()?.WeaponsType == WeaponType.Normal || item.ArcaneOdyssey()?.WeaponsType == WeaponType.Strength) && item.ModItem is not Imbuable;
				}
				if (imbue is MagicType)
				{
					if (item.DamageType.CountsAsClass(DamageClass.Magic))
					{
						return false;
					}
					return (item.ArcaneOdyssey()?.WeaponsType == WeaponType.Normal || item.ArcaneOdyssey()?.WeaponsType == WeaponType.Arcanium) && (item.ModItem is not Imbuable || (item.ModItem is SpiritEnergy or FightingStyle && Main.hardMode));
				}
				if (imbue is SpiritEnergy)
				{
					return item.ArcaneOdyssey()?.WeaponsType == WeaponType.Normal && (item.ModItem is not Imbuable || (item.ModItem is MagicType or FightingStyle && Main.hardMode));
				}
			}
			return false;
		}
		public static bool IsTileSolidGround(this Tile tile) => tile != null && tile.HasUnactuatedTile && (Main.tileSolid[tile.TileType] || Main.tileSolidTop[tile.TileType]);
		public static bool IsTileReallySolidGround(this Tile tile) => tile != null && tile.HasUnactuatedTile && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType];

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
			if (imbue is not null && target is not null && target.active)
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

		public static ModDamageHelper CalculateImbueDamage(Imbuable imbue, Player target, ModDamageHelper modifiers)
		{
			if (imbue is not null && target is not null && target.active)
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
			return modifiers;
		}

		public static NPC.HitModifiers CalculateImbueDamage(Imbuable imbue, NPC target, NPC.HitModifiers modifiers)
		{
			return modifiers with { FinalDamage = CalculateImbueDamage(imbue, target, new ModDamageHelper(modifiers.FinalDamage)).FinalDamage };
		}

		public static Player.HurtModifiers CalculateImbueDamage(Imbuable imbue, Player target, Player.HurtModifiers modifiers)
		{
			return modifiers with { FinalDamage = CalculateImbueDamage(imbue, target, new ModDamageHelper(modifiers.FinalDamage)).FinalDamage };
		}

		/// <summary>
		/// <inheritdoc cref="Array.Find{T}(T[], Predicate{T})"/>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="array"></param>
		/// <param name="predicate"></param>
		/// <returns><inheritdoc cref="Array.Find{T}(T[], Predicate{T})"/></returns>
		public static T Find<T>(this T[] array, Predicate<T> predicate)
		{
			return Array.Find(array, predicate);
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
					velocity *= imbue.ScrollSpeed;
					if (secondimbue is not null)
					{
						velocity *= secondimbue.ScrollSpeed;
					}
				}
				else
				{
					velocity *= imbue.ImbueSpeed;
					if (secondimbue is not null)
					{
						velocity *= secondimbue.ImbueSpeed;
					}
				}
			}

			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player, ai0, ai1, ai2);
			projectile.netUpdate = true;
			projectile.netSpam = 0;
			return projectile;
		}

		public static int FromAODefense(this int val) => (int)Math.Round(val / 15f);
		public static int FromAODefense(this short val) => (int)Math.Round(val / 15f);
		public static int FromAODefense(this ushort val) => (int)Math.Round(val / 15f);

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

		public static bool ServerOrSingleplayer => Main.netMode != NetmodeID.MultiplayerClient;

		public static bool AltUse(this Player player) => player.altFunctionUse == 2;

		public static Rectangle ScreenRect => Main.screenPosition.ToRectangle(Main.ScreenSize);

		public static Rectangle ToRectangle(this Vector2 vec, Vector2 size) => new(vec.X.Round(), vec.Y.Round(), size.X.Round(), size.Y.Round());
		public static Rectangle ToRectangle(this Vector2 vec, Point size) => new(vec.X.Round(), vec.Y.Round(), size.X, size.Y);
		public static Rectangle ToRectangle(this Vector2 vec, Point16 size) => new(vec.X.Round(), vec.Y.Round(), size.X, size.Y);
		public static Rectangle ToRectangle(this Point vec, Vector2 size) => new(vec.X, vec.Y, size.X.Round(), size.Y.Round());
		public static Rectangle ToRectangle(this Point vec, Point size) => new(vec.X, vec.Y, size.X, size.Y);
		public static Rectangle ToRectangle(this Point vec, Point16 size) => new(vec.X, vec.Y, size.X, size.Y);
		public static Rectangle ToRectangle(this Point16 vec, Vector2 size) => new(vec.X, vec.Y, size.X.Round(), size.Y.Round());
		public static Rectangle ToRectangle(this Point16 vec, Point size) => new(vec.X, vec.Y, size.X, size.Y);
		public static Rectangle ToRectangle(this Point16 vec, Point16 size) => new(vec.X, vec.Y, size.X, size.Y);

		public static bool OnScreen(this Rectangle Hitbox) => Hitbox.Intersects(ScreenRect);

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

		public static LocalizedText CoolCustomLocalization(this Mod mod, string key, string fallback) => Language.GetOrRegister(mod.GetLocalizationKey(key), () => fallback ?? mod.CustomLocalization(key).Value);
		public static LocalizedText CoolCustomLocalization(this Mod mod, string key, Func<string> fallback = null) => Language.GetOrRegister(mod.GetLocalizationKey(key), fallback ?? (() => mod.CustomLocalization(key).Value));

		public static ArcaneOdysseyMod ModInstance => ArcaneOdysseyMod.Instance;

		/// <summary>
		/// Includes minibosses
		/// </summary>
		public static int BossesKilled
		{
			get
			{
				int count = 0;
				List<bool> conditions = [];
				conditions.AddRange([DownedBosses.DownedEvander, DownedBosses.DownedElius, DownedBosses.DownedCalvus, DownedBosses.DownedAllanon, DownedBosses.DownedArgos, DownedBosses.DownedLaelus, DownedBosses.DownedCrone, DownedBosses.DownedDelamere, DownedBosses.DownedDusk, NPC.downedBoss1, DownedBosses.downedWorldEater, DownedBosses.downedBrain, NPC.downedBoss3, NPC.downedQueenBee, NPC.downedSlimeKing, NPC.downedDeerclops, NPC.downedAncientCultist, NPC.downedChristmasIceQueen, NPC.downedChristmasSantank, NPC.downedClown, NPC.downedChristmasTree, NPC.downedEmpressOfLight, NPC.downedFishron, NPC.downedFrost, NPC.downedGoblins, NPC.downedGolemBoss, NPC.downedHalloweenKing, NPC.downedHalloweenTree, NPC.downedMartians, NPC.downedMechBoss1, NPC.downedMechBoss2, NPC.downedMechBoss3, NPC.downedMechBossAny, NPC.downedMoonlord, NPC.downedPlantBoss, NPC.downedPirates]);
				if (ExternalModSupport.HasCalamity)
				{
					string[] extrBosses = "desertscourge giantclam crabulon hivemind perforator slimegod cryogen aquaticscourge cragmawmire brimstoneelemental calamitasclone greatsandshark anahitaleviathan astrumaureus plaguebringergoliath ravager astrumdeus guardians dragonfolly providence polterghast mauler nuclearterror oldduke ceaselessvoid stormweaver signus devourerofgods yharon exomechs calamitas primordialwyrm".Split(' ');
					foreach (var boss in extrBosses)
					{
						conditions.Add((bool)ExternalModSupport.Calamity.Call("GetBossDowned", boss));
					}
				}
				if (ExternalModSupport.HasThorium)
				{
					string[] extrBosses = "Lich Viscount PatchWerk StarScouter Illusionist CorpseBloom ForgottenOne BoreanStrider FallenBeholder BuriedChampion ThePrimordials QueenJellyfish GraniteEnergyStorm TheGrandThunderBird".Split(' ');
					foreach (var boss in extrBosses)
					{
						conditions.Add((bool)ExternalModSupport.Thorium.Call("GetDownedBoss", boss));
					}
				}
				foreach (var killed in conditions)
				{
					if (killed)
						count++;
				}
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
				return projectile.TryGetOwner(out player);
			}
			if (entity is NPC npc)
			{
				player = Main.player[npc.releaseOwner];
			}
			if (entity is Player)
			{
				player = entity as Player;
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
			if (item.ModItem is BaseItem based && based.ItemCategory.HasValue)
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

		public static ItemRarities GetItemRare(this Item item)
		{
			if (ExternalModSupport.HasCalamity)
			{
				if (item.rare == ExternalModSupport.Calamity.Find<ModRarity>("DarkOrange").Type)
				{
					return ItemRarities.Unknown;
				}
			}

			if (item.rare == ModContent.RarityType<HotPinkRare>())
			{
				return ItemRarities.Special;
			}

			if (ModLoader.TryGetMod("NoxusBoss", out var wotg))
			{
				if (item.rare == wotg.Find<ModRarity>("SolynRewardRarity").Type)
				{
					return ItemRarities.Mystic;
				}
				if (item.rare == wotg.Find<ModRarity>("GenesisComponentRarity").Type)
				{
					return ItemRarities.Unknown;
				}
				if (item.rare == wotg.Find<ModRarity>("NamelessDeityRarity").Type)
				{
					return ItemRarities.Unknown;
				}
				if (item.rare == wotg.Find<ModRarity>("AvatarRarity").Type)
				{
					return ItemRarities.Unknown;
				}
			}

			return item.rare switch
			{
				ItemRarityID.Gray => ItemRarities.Junk,
				ItemRarityID.White => ItemRarities.Common,
				ItemRarityID.Blue => ItemRarities.Common,
				ItemRarityID.Green => ItemRarities.Uncommon,
				ItemRarityID.Orange => ItemRarities.Uncommon,
				ItemRarityID.Quest => ItemRarities.Rare,
				ItemRarityID.LightRed => ItemRarities.Rare,
				ItemRarityID.Pink => ItemRarities.Rare,
				ItemRarityID.Expert => ItemRarities.Mystic,
				ItemRarityID.LightPurple => ItemRarities.Mystic,
				ItemRarityID.Lime => ItemRarities.Mystic,
				ItemRarityID.Master => ItemRarities.Mystic,
				ItemRarityID.Yellow => ItemRarities.Legendary,
				ItemRarityID.Cyan => ItemRarities.Legendary,
				ItemRarityID.Red => ItemRarities.Mythical,
				ItemRarityID.Purple => ItemRarities.Mythical,
				_ => ItemRarities.Lost,
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

		public static int GetAOBuffStack(Player player, int index)
		{
			return (player.buffTime[index] / 60 / 5) + 1;
		}

		/// <summary>
		/// Converts AO Galleons/Drachmae to Terraria Copper
		/// </summary>
		/// <param name="price">Price, in Galleons</param>
		/// <returns></returns>
		public static int GalleonToCopper(int price) => Item.sellPrice(silver: price);

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
		public static float FlipFloat(this float input) => MathHelper.Clamp(2f - input, .01f, 2f);

		public static float MultiToPercent(this float multiplier) => multiplier - 1f; // wow simplest function on the earth

		public static Vector2 SafeDirectionTo(this Entity entity, Vector2 destination, Vector2? defaultValue = null)
		{
			defaultValue ??= Vector2.Zero;
			return (destination - entity.Center).SafeNormalize(defaultValue.Value);
		}
		public static Vector2 SafeDirectionFrom(this Entity entity, Vector2 destination, Vector2? defaultValue = null)
		{
			defaultValue ??= Vector2.Zero;
			return (entity.Center - destination).SafeNormalize(defaultValue.Value);
		}

		public static float Average(params float[] inputs)
		{
			if (inputs.Length <= 0)
				return 0;

			float val = 0;

			foreach (var num in inputs)
			{
				val += num;
			}

			return val / inputs.Length;
		}

		#endregion

		#region Player Inventory Helpers
		public static bool HasTypeInInventory<T>(this Player player, Predicate<T> check = null) where T : ModItem
		{
			List<Item> no = [.. player.inventory, player.trashItem];
			if (player.ArcaneOdyssey()?.EquippedImbues is not null)
			{
				no.AddRange(player.ArcaneOdyssey().EquippedImbues.Select(e => new Item(e)));
			}
			if (player.useVoidBag())
			{
				no.AddRange(player.bank4.item);
			}
			no.RemoveAll(e => e.ModItem is null);
			foreach (var item in no)
			{
				if (item.ModItem is T)
				{
					if (check is not null)
					{
						if (check(item.ModItem as T))
						{
							return true;
						}
					}
					else
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool HasItemInInventory(this Player player, Predicate<Item> check)
		{
			List<Item> no = [.. player.inventory, player.trashItem];

			if (player.ArcaneOdyssey()?.EquippedImbues is not null)
			{
				no.AddRange(player.ArcaneOdyssey().EquippedImbues.Select(e => new Item(e)));
			}

			if (player.useVoidBag())
			{
				no.AddRange(player.bank4.item);
			}

			no.RemoveAll(e => e.IsAir || !e.active);

			foreach (var item in no)
			{
				if (check(item))
				{
					return true;
				}
			}

			return false;
		}

		public static bool HasItemInInventory(this Player player, Predicate<Item> check, out Item i)
		{
			i = null;

			List<Item> no = [.. player.inventory, player.trashItem];

			if (player.ArcaneOdyssey()?.EquippedImbues is not null)
			{
				no.AddRange(player.ArcaneOdyssey().EquippedImbues.Select(e => new Item(e)));
			}

			if (player.useVoidBag())
			{
				no.AddRange(player.bank4.item);
			}

			no.RemoveAll(e => e.IsAir || !e.active);

			foreach (var item in no)
			{
				if (check(item))
				{
					i = item;
					return true;
				}
			}

			return false;
		}

		public static bool HasTypeInInventory<T>(this Player player, out T item, Predicate<T> check = null) where T : ModItem
		{
			item = null;
			if (player?.ArcaneOdyssey() is not null)
			{
				if (player.ArcaneOdyssey().EquippedImbues.Contains(ModContent.ItemType<T>()) || player.ArcaneOdyssey().EquippedSecondImbues.Contains(ModContent.ItemType<T>()))
				{
					item ??= ModContent.GetInstance<T>();
				}
			}
			List<Item> no = [.. player.inventory, player.trashItem];
			no.RemoveAll(e => e.ModItem is null);
			foreach (var items in no)
			{
				if (items.ModItem is T)
				{
					if (check is not null)
					{
						if (check(items.ModItem as T))
						{
							item ??= items.ModItem as T;
							break;
						}
					}
					else
					{
						item ??= items.ModItem as T;
						break;
					}
				}
			}
			return item is not null;
		}

		public static List<T> Sorted<T>(this List<T> self, Comparison<T> comparer)
		{
			self.Sort(comparer);
			return self;
		}

		public static Item PlayerItem(this Player player)
		{
			if (Main.myPlayer == player.whoAmI && (!Main.mouseItem.IsAir) && Main.mouseItem.active)
			{
				return Main.mouseItem;
			}
			else return player.HeldItem;
		}
		#endregion

		#region ArcaneOdyssey()

		public static AOPlayer ArcaneOdyssey(this Player player)
		{
			if (player is not null && player.active && player.TryGetModPlayer<AOPlayer>(out var playah))
				return playah;
			return null;
		}

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

		public static AOItem ArcaneOdyssey(this Item item)
		{
			if (item is not null && !item.IsAir && item.active && item.TryGetGlobalItem<AOItem>(out var item1))
				return item1;
			return null;
		}

		public static IImbuable AnyArcaneOdyssey(this Entity entity)
		{
			if (entity is Projectile projectile)
			{
				if (projectile.ModProjectile is PlayerProjectile proj)
					return proj;
				return projectile.ArcaneOdyssey();
			}
			if (entity is Player player)
				return player.ArcaneOdyssey();
			if (entity is Item item)
			{
				if (item.ModItem is Weapon weap)
					return weap;
				if (item.ModItem is BaseArmour armour)
					return armour;
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

	/// <summary>
	/// Helper struct for set bonuses
	/// </summary>
	/// <param name="moditem">This moditem</param>
	/// <param name="otherItems">The internal names of the other two items in this set, head then boots</param>
	public struct SetBonusHelper(ModItem moditem, params string[] otherItems)
	{
		public string[] OtherItems = otherItems;
		public static string Key(ModItem item) => $"Mods.{item.Mod.Name}.{item.LocalizationCategory}.{item.Name}.Set";

		public LocalizedText LocalizedDescription = Language.GetOrRegister(Key(moditem), () => Key(moditem));

		public readonly string Tooptip => LocalizedDescription.Value;
	}

	public struct ImbueArmourStats(short size = 0, short attkspeed = 0, short power = 0, ushort defence = 0, short agility = 0, short pierce = 0, short haste = 0)
	{
		public short Size = size;
		public short Attkspeed = attkspeed;
		public short Power = power;
		public short Pierce = pierce;
		public ushort Defence = defence;
		public short Agility = agility;
		public short Haste = haste;

		public readonly ImbueArmourStats Corrected(Imbuable imbue)
		{
			if (imbue is FightingStyleBarred barred)
			{
				return new ImbueArmourStats(
					(short)MathHelper.Lerp(Size / 4f, Size, barred.LerpValue).Round(),
					(short)MathHelper.Lerp(Attkspeed / 4f, Attkspeed, barred.LerpValue).Round(),
					(short)MathHelper.Lerp(Power / 4f, Power, barred.LerpValue).Round(),
					(ushort)MathHelper.Lerp(Defence / 4f, Defence, barred.LerpValue).Round(),
					(short)MathHelper.Lerp(Agility / 4f, Agility, barred.LerpValue).Round(),
					(short)MathHelper.Lerp(Pierce / 4f, Pierce, barred.LerpValue).Round(),
					(short)MathHelper.Lerp(Haste / 4f, Haste, barred.LerpValue).Round()
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
	public enum ItemRarities
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

	public enum ImbuableTiers
	{
		Normal,
		Lost,
		Ancient,
		Mythical,
	}

	/// <summary>
	/// Arcane Odyssey weapon tiers, used for scaling
	/// </summary>
	public enum ItemTiers
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
	public struct SynergyEffects
	{
		public ClearBuff[] clearBuffs;
		public List<Synergy> magicBuffMultipliers;

		public SynergyEffects(ClearBuff[] buffsToClear, List<Synergy> buffMultipliers)
		{
			clearBuffs = buffsToClear;
			magicBuffMultipliers = buffMultipliers;
		}

		public SynergyEffects()
		{
			clearBuffs = [];
			magicBuffMultipliers = [];
		}

		public static SynergyEffects operator +(SynergyEffects one, SynergyEffects two)
		{
			return new([.. one.clearBuffs, .. two.clearBuffs], [.. one.magicBuffMultipliers, .. two.magicBuffMultipliers]);
		}
	}

	public struct ClearBuff(int id, params int[] alternatives)
	{
		public int id = id;
		public int[] alternatives = alternatives;

		public static ClearBuff Create<T>() where T : BaseBuff
		{
			return new(ModContent.BuffType<T>(), [.. ModContent.GetInstance<T>().Counterparts]);
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

		public static Combo Create<T>(int result, int duration = 60) where T : BaseBuff
		{
			return new(ModContent.BuffType<T>(), result, duration, [.. ModContent.GetInstance<T>().Counterparts]);
		}

		public static Combo Create<T, R>(int duration = 60) where T : BaseBuff where R : BaseBuff
		{
			return new(ModContent.BuffType<T>(), ModContent.BuffType<R>(), duration, [.. ModContent.GetInstance<T>().Counterparts]);
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

		public static Synergy Create<T>(float multi) where T : BaseBuff
		{
			return new(ModContent.BuffType<T>(), multi, [.. ModContent.GetInstance<T>().Counterparts]);
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
	public struct ModDamageHelper
	{
		public StatModifier FinalDamage;

		public ModDamageHelper(StatModifier statModifier)
		{
			FinalDamage = statModifier;
		}

		public ModDamageHelper()
		{
			FinalDamage = StatModifier.Default;
		}

		public int GetDamage(int damage)
		{
			return FinalDamage.ApplyTo(damage).Round();
		}

		public int GetDamage(float damage)
		{
			return FinalDamage.ApplyTo(damage).Round();
		}
	}

	public enum ScrollTier
	{
		Common,
		Rare,
		Lost
	}

	public readonly struct ChainEndInfo(int finalFrame, Vector2 ending, int length, float rotation)
	{
		public readonly int FinalFrame = finalFrame;

		public readonly Vector2 Ending = ending;

		public readonly int Length = length;

		public readonly float Rotation = rotation;
	}

	public enum MagicCircleTypes
	{
		Ancient,
		Collision,
		Ornamental,
		Penta,
		Reminiscent,
		Segmented,
		Singularity,
		Solar,
		Tidal,
		Tesla,
		Imperial,
		Malignant,
		Monolith,
		Draconic,
		Demonic
	}
}
