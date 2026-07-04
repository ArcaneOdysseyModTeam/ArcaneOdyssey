using ArcaneOdyssey.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Imbues.Relics;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.Consumable;
using ArcaneOdyssey.Items.Scrolls;
using ArcaneOdyssey.Items.Scrolls.Equipment.Common;
using ArcaneOdyssey.Projectiles;
using ArcaneOdyssey.Projectiles.Base;
using ArcaneOdyssey.Skills.Base;
using ArcaneOdyssey.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Imbues.Base
{
	/// <summary>
	/// Imbue values are applied as multipliers to imbued projectiles,
	/// <para>Scroll values are applied as multipliers to projectiles created using spell scrolls</para>
	/// </summary>
	public abstract class Imbuable : BaseItem, IImbuable
	{
		public override void Load()
		{
			ModTypeLookup<Imbuable>.Register(this);
		}

		/// <summary>
		/// Aura spell coefficient
		/// </summary>
		public virtual float Aura => .7f;

		public virtual int Drawback => 0;

		public ModSkill[] Skills
		{
			get
			{
				return [Attacks.Item1, Attacks.Item2, Attacks.Item3, Passive, Mobility, Dash];
			}
			set
			{
				Attacks.Item1 = value[0] as AttackSkill;
				Attacks.Item2 = value[1] as AttackSkill;
				Attacks.Item3 = value[2] as AttackSkill;
				Passive =  value[3] as PassiveSkill;
				Mobility = value[4];
				Dash = value[5] as DashSkill;
			}
		}

		public ModSkill[] DefaultSkills => [DefaultAttacks.Item1, DefaultAttacks.Item2, DefaultAttacks.Item3, DefaultPassive, DefaultMobility, DefaultDash];

		public virtual AttackSkill DefaultAttack => null;
		public (AttackSkill, AttackSkill, AttackSkill) DefaultAttacks => (DefaultAttack, DefaultAttack, DefaultAttack);
		public virtual PassiveSkill DefaultPassive => null;
		public virtual ModSkill DefaultMobility => null;
		public virtual DashSkill DefaultDash => null;


		public (AttackSkill, AttackSkill, AttackSkill) Attacks;

		public PassiveSkill Passive;
		public bool PassiveActive;
		public int PassiveTime;

		public ModSkill Mobility;
		public DashSkill Dash;

		public AttackSkill selectedAttack;
		public byte selectedIndex;

		internal string[] cachedSpells = new string[6];

		public void CycleAttack()
		{
			switch (selectedIndex)
			{
				case 0:
					selectedAttack = Attacks.Item2;
					selectedIndex++;
					break;
				case 1:
					selectedAttack = Attacks.Item3;
					selectedIndex++;
					break;
				default:
					selectedAttack = Attacks.Item1;
					selectedIndex = 0;
					break;
			}

			if (selectedAttack is not null)
			{
				if (this is MagicType)
				{
					Item.mana = selectedAttack.ManaCost;
				}
				Item.damage = selectedAttack.Damage;
				Item.knockBack = selectedAttack.Knockback;
				Item.channel = selectedAttack.Channel;
				Item.useAnimation = Item.useTime = selectedAttack.Time;
				Item.shoot = selectedAttack.Shoot;
				Item.shootSpeed = selectedAttack.Speed;
				Item.useStyle = selectedAttack.UseStyleID;
			}
		}

		public override bool CanUseItem(Player player)
		{
			if (player.AltUse())
			{
				if (Item.useStyle != ArcaneOdysseyMod.Sets.imbuableDefaultUseID[Type])
				{
					Item.useStyle = ArcaneOdysseyMod.Sets.imbuableDefaultUseID[Type];
					return false;
				}
				else return true;
			}
			else
			{
				if (selectedAttack is null)
				{
					return false;
				}
				else if (Item.useStyle != selectedAttack.UseStyleID)
				{
					Item.useStyle = selectedAttack.UseStyleID;
					return false;
				}
				else return true;
			}
		}

		public static class SlotIndexID
		{
			public const byte Attack1 = 0;
			public const byte Attack2 = 1;
			public const byte Attack3 = 2;
			public const byte Passive = 3;
			public const byte Mobility = 4;
			public const byte Dash = 5;
		}

		/// <summary>
		/// Removes a skill from the imbue
		/// </summary>
		/// <param name="slotIndex">0 - 5</param>
		public void RemoveSkill(byte slotIndex)
		{
			if (slotIndex < Skills.Length)
			{
				ModSkill skill = Skills[slotIndex];
				if (skill is null)
				{
					if (!string.IsNullOrWhiteSpace(cachedSpells[slotIndex]))
					{
						var item = Main.LocalPlayer.QuickSpawnItemDirect(Item.GetSource_FromThis(), ModContent.ItemType<UnloadedScroll>()).ModItem as UnloadedScroll;
						item.CachedFullName = cachedSpells[slotIndex];
						cachedSpells[slotIndex] = null;
					}
				}
				else
				{
					Main.LocalPlayer.QuickSpawnItem(Item.GetSource_FromThis(), skill.Scroll);
				}
				var skills = Skills;
				skills[slotIndex] = DefaultSkills[slotIndex];
				Skills = skills;
			}
		}

		public void SetSkill(byte slotIndex, ModSkill skill, bool refund = true)
		{
			if (refund)
				RemoveSkill(slotIndex);

			if (slotIndex == 3)
			{
				PassiveActive = false;
				PassiveTime = 0;
			}

			var skills = Skills;
			skills[slotIndex] = skill;
			Skills = skills;

			if (slotIndex == selectedIndex)
			{
				selectedIndex--;
				CycleAttack();
			}
		}

		public override void NetSend(BinaryWriter writer)
		{
			base.NetSend(writer);
			writer.Write(Attacks.Item1?.Type);
			writer.Write(Attacks.Item2?.Type);
			writer.Write(Attacks.Item3?.Type);
			writer.Write(Passive?.Type);
			writer.Write(Mobility?.Type);
			writer.Write(Dash?.Type);
			writer.Write(selectedAttack?.Type);
			writer.Write(selectedIndex);
		}

		public override void NetReceive(BinaryReader reader)
		{
			base.NetReceive(reader);
			Attacks = (ModSkill.Sets.All[reader.ReadInt32()] as AttackSkill, ModSkill.Sets.All[reader.ReadInt32()] as AttackSkill, ModSkill.Sets.All[reader.ReadInt32()] as AttackSkill);
			Passive = ModSkill.Sets.All[reader.ReadInt32()] as PassiveSkill;
			Mobility = ModSkill.Sets.All[reader.ReadInt32()];
			Dash = ModSkill.Sets.All[reader.ReadInt32()] as DashSkill;
			selectedAttack = ModSkill.Sets.All[reader.ReadInt32()] as AttackSkill;
			selectedIndex = reader.ReadByte();
		}

		public override void SaveData(TagCompound tag)
		{
			base.SaveData(tag);

			string[] attackSpells = new string[3];

			if (Attacks.Item1 is not null)
			{
				attackSpells[0] = Attacks.Item1.FullName;
			}
			else if (cachedSpells[0] != null) 
			{
				attackSpells[0] = cachedSpells[0];
			}

			if (Attacks.Item2 is not null)
			{
				attackSpells[1] = Attacks.Item2.FullName;
			}
			else if (cachedSpells[1] != null)
			{
				attackSpells[1] = cachedSpells[1];
			}

			if (Attacks.Item3 is not null)
			{
				attackSpells[2] = Attacks.Item3.FullName;
			}
			else if (cachedSpells[2] != null)
			{
				attackSpells[2] = cachedSpells[2];
			}

			if (attackSpells.Any(e => e is not null))
			{
				tag.Add("attackspells", attackSpells.Select(e => string.IsNullOrWhiteSpace(e) ? "" : e).ToList());
			}

			if (Passive is not null)
			{
				tag.Add("passivespell", Passive.FullName);
			}
			else if (cachedSpells[3] != null)
			{
				tag.Add("passivespell", cachedSpells[3]);
			}

			if (Mobility is not null)
			{
				tag.Add("mobilityspell", Mobility.FullName);
			}
			else if (cachedSpells[4] != null)
			{
				tag.Add("mobilityspell", cachedSpells[4]);
			}

			if (Dash is not null)
			{
				tag.Add("dashspell", Dash.FullName);
			}
			else if (cachedSpells[5] != null)
			{
				tag.Add("dashspell", cachedSpells[5]);
			}

			selectedIndex = 80;
			CycleAttack();
		}

		public override void LoadData(TagCompound tag)
		{
			base.LoadData(tag);

			Attacks = DefaultAttacks;
			Passive = DefaultPassive;
			Mobility = DefaultMobility;
			Dash = DefaultDash;

			var attacks = tag.GetList<string>("attackspells").ToArray();

			if (attacks.Length == 0)
			{
				attacks = new string[3];
			}

			if (!string.IsNullOrWhiteSpace(attacks[0]))
			{
				if (!ModContent.TryFind(attacks[0], out Attacks.Item1))
				{
					cachedSpells[0] = attacks[0];
				}
			}

			if (!string.IsNullOrWhiteSpace(attacks[1]))
			{
				if (!ModContent.TryFind(attacks[1], out Attacks.Item2))
				{
					cachedSpells[1] = attacks[1];
				}
			}

			if (!string.IsNullOrWhiteSpace(attacks[2]))
			{
				if (!ModContent.TryFind(attacks[2], out Attacks.Item3))
				{
					cachedSpells[2] = attacks[2];
				}
			}

			var passive = tag.GetString("passivespell");

			if (!string.IsNullOrWhiteSpace(passive))
			{
				if (!ModContent.TryFind(passive, out Passive))
				{
					cachedSpells[3] = passive;
				}
			}

			var mobility = tag.GetString("mobilityspell");

			if (!string.IsNullOrWhiteSpace(mobility))
			{
				if (!ModContent.TryFind(mobility, out Mobility))
				{
					cachedSpells[4] = mobility;
				}
			}

			var dash = tag.GetString("dashspell");

			if (!string.IsNullOrWhiteSpace(dash))
			{
				if (!ModContent.TryFind(dash, out Dash))
				{
					cachedSpells[5] = dash;
				}
			}

			selectedIndex = 80;
			CycleAttack();
		}

		public override bool CanShoot(Player player)
		{
			bool condition = true;
			condition &= !player.AltUse();
			if (selectedAttack != null)
			{
				condition &= selectedAttack.PreActivate(player, this);
			}
			return condition;
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			selectedAttack?.AttackStats(player, this, ref position, ref velocity, ref damage, ref knockback);
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			if (selectedAttack != null)
				return selectedAttack.Attack(player, this, source, position, velocity, damage, knockback);
			return false;
		}

		public int AuraHP(Player player)
		{
			float aura;
			if (this is not FightingStyle || player.ArcaneOdyssey().acumen)
			{
				aura = player.statLifeMax * (.225f * Aura);
			}
			else
			{
				aura = player.statLifeMax * (.18f * Aura);
			}
			return (int)Math.Round(aura / 5f, MidpointRounding.AwayFromZero) * 5;
		}

		public override void UpdateInventory(Player player)
		{
			for (int i = 0; i < Skills.Length; i++)
			{
				if (Skills[i] == null && string.IsNullOrWhiteSpace(cachedSpells[i]))
				{
					SetSkill((byte)i, DefaultSkills[i], false);
				}
			}
			if (selectedAttack == null)
				CycleAttack();
			if (Main.myPlayer == player.whoAmI && player.PlayerItem().type == Type)
			{
				if (player.ItemAnimationEndingOrEnded)
				{
					if (AOKeybinds.CycleImbueAttack.JustPressed)
					{
						CycleAttack();
					}
				}

				if (AOKeybinds.ActivateImbuePassive.JustPressed && Passive is not null)
				{
					if (Passive.PreActivate(player, this))
					{
						PassiveActive = true;
						PassiveTime = Passive.Length;
					}
				}

				if (PassiveActive)
				{
					if (PassiveTime-- <= 0)
					{
						PassiveActive = false;
					}
					else
					{
						if (Passive is not null)
						{
							Passive.Activate(player, this);
						}
						else
						{
							PassiveActive = false;
							PassiveTime = 0;
						}
					}
				}
			}

			Gimmick?.UpdateInventory(player);

			if (player.Imbue()?.Type == Type)
			{
				if (Mobility is not null && Mobility.PreActivate(player, this))
				{
					Mobility.Activate(player, this);
				}
				if (Dash is not null && Dash.PreActivate(player, this))
				{
					Dash.Activate(player, this);
				}
			}
		}

		public bool PlayerHasImbue(Player player)
		{
			var type = Type;
			if (this is SteamImbue steam)
			{
				type = steam.Imbue.Type;
			}
			return player.HasTypeInInventory<Imbuable>(e => e.Type == type);
		}

		public virtual void UpdateProjectile(Projectile Projectile)
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		public void ActivateAbility(Player player, bool passive)
		{
			if (Property.HasValue)
			{
				if (ArcaneOdysseyClientConfig.Instance.AbilityText && player is not null && player.active && !player.DeadOrGhost && Main.myPlayer == player.whoAmI)
				{
					CombatText.NewText(player.Hitbox, Property.Value.Colour, (Property.Value.Name + "!").Trim(), !passive);
				}
			}
		}

		public static bool IsValidEnemyImbue(Imbuable imbue)
		{
			if (imbue is MagicType or FightingStyle)
			{
				if (NPC.downedMoonlord || DownedBosses.downedEnragedEmpress)
				{
					if ((imbue.ImbuableTier == ImbuableTiers.Normal) || ((imbue.ImbuableTier == ImbuableTiers.Lost) && (!imbue.Special)) || ((imbue.ImbuableTier == ImbuableTiers.Ancient) && (imbue is MagicType) && (!imbue.Special)))
					{
						return true;
					}
				}

				else if (NPC.downedMechBossAny)
				{
					if ((imbue.ImbuableTier == ImbuableTiers.Normal) || ((imbue.ImbuableTier == ImbuableTiers.Lost) && (imbue is MagicType) && (!imbue.Special)))
					{
						return true;
					}
				}

				else if (Main.hardMode)
				{
					if (imbue.ImbuableTier == ImbuableTiers.Normal)
					{
						return true;
					}
				}

				else
				{
					if (imbue.ImbuableTier == ImbuableTiers.Normal)
					{
						if (imbue is FightingStyle)
						{
							return imbue.Type == ModContent.ItemType<BasicCombat>();
						}
						return true;
					}
				}
			}

			return false;
		}

		public static IEnumerable<Imbuable> AllValidEnemyImbues => ModContent.GetContent<Imbuable>().Where(IsValidEnemyImbue);

		public WeaponAbility? Property
		{
			get
			{
				var ab = new WeaponAbility { Colour = Colour };
				if (Imbue is not null)
				{
					ab.Colour = Imbue.ImbueColour;
				}
				if (Language.Exists($"Mods.{Mod.Name}.{LocalizationCategory}.{Name}.Ability"))
				{
					ab.Name = Mod.CustomLocalization($"{LocalizationCategory}.{Name}.Ability").Value;
					ab.Description = null;
					return ab;
				}
				return null;
			}
		}

		/// <summary>
		/// The second imbue
		/// </summary>
		public virtual Imbuable Imbue { get => Item.ArcaneOdyssey()?.Imbue; set => Item.ArcaneOdyssey().Imbue = value; }

		public virtual string ImbueUISprite => ModContent.HasAsset(Texture + "_Imbue") ? (Texture + "_Imbue") : Texture;

		public override void SetStaticDefaults()
		{
			ItemID.Sets.CanGetPrefixes[Type] = false;
			ArcaneOdysseyMod.Sets.showItemTypeTooltip[Type] = false;
			ItemID.Sets.IgnoresEncumberingStone[Type] = true;
			_ = PrettyAttackPrefix;
			_ = PrettySpellPrefix;
		}

		public virtual ImbueGimmick Gimmick => null;

		/// <summary>
		/// Sets the armour stats of this magic, will be multiplied by the armour tier
		/// </summary>
		public virtual ImbueArmourStats? ArmourStats => null;

		public sealed override ItemRarities Rarity => ImbuableTier switch
		{
			ImbuableTiers.Normal => ItemRarities.Rare,
			ImbuableTiers.Lost => ItemRarities.Mystic,
			ImbuableTiers.Ancient => ItemRarities.Legendary,
			ImbuableTiers.Mythical => ItemRarities.Mythical,
			_ => ItemRarities.Special,
		};

		public virtual float? DashResist => null;
		public virtual float DashSpeed => 1f;

		public virtual bool ImmuneDash => false;

		public virtual float ImbueSpeed => ((ScrollSpeed * .5f) + .5f).CleanRound();
		public virtual float ImbueSize => ScrollSize < 1f ? ScrollSize : ScrollSize - .1f;
		public virtual float ImbueDamage => ScrollDamage + 0.075f;
		public abstract float ScrollSpeed { get; }
		public abstract float ScrollSize { get; }
		public abstract float ScrollDamage { get; }

		/// <summary>
		/// For magics or fighing styles, you may return any value
		/// </summary>
		public virtual ImbuableTiers ImbuableTier => ImbuableTiers.Normal;
		public virtual Debuff[] ImbueDebuffs => [];
		public virtual SynergyEffects Effects => new();
		public abstract Color ImbueColour { get; }
		public virtual Color ImbueColour2 => Color.White;
		public virtual bool AnimatedColours => false;
		public virtual Combo[] CombinedDebuffs => [];
		public virtual SoundStyle? ImbueSound => null;

		/// <summary>
		/// Leave null for neutral, true for cold, false for hot
		/// </summary>
		public ref bool? Cold => ref ArcaneOdysseyMod.Sets.cold[Type];

		/// <summary>
		/// magic/fs works underwater
		/// </summary>
		public virtual bool CanBeWet => true;

		public virtual float KBMulti => 1f;

		public virtual string AttackPrefix => Name.Replace("Magic");

		public LocalizedText PrettyAttackPrefix => Language.GetOrRegister(this.GetLocalizationKey("AttackPrefix"), () => Regex.Replace(AttackPrefix, "([A-Z])", " $1").Trim());

		public LocalizedText PrettySpellPrefix => Language.GetOrRegister(this.GetLocalizationKey("SpellPrefix"), () => Regex.Replace(AttackPrefix, "([A-Z])", " $1").Trim());

		public virtual void SpawningEffects(Rectangle area, Vector2 direction) { }

		public virtual void LingeringEffects(Rectangle area, Vector2? direction = null, Entity source = null) { }

		public virtual int[] Dusts => [DustID.ShimmerSpark];

		public sealed override void ModifyManaCost(Player player, ref float reduce, ref float mult)
		{
			base.ModifyManaCost(player, ref reduce, ref mult);
			if (player.AltUse())
			{
				mult *= 0;
				return;
			}
			Gimmick?.ModifyManaCost(player, ref reduce, ref mult);
		}

		/// <summary>
		/// Called after a projectile is killed usually
		/// </summary>
		/// <param name="area">The area the dust spawns in</param>
		public virtual void KillEffects(Rectangle area, Entity source = null) { }

		/// <summary>
		/// Used for explosions and pulsar stuff ect
		/// </summary>
		/// <param name="position">The centre of the explosion</param>
		/// <param name="intensity">The multiplier on the explosion size</param>
		public virtual void ExplosionEffects(Vector2 position, float intensity = 1f) { }

		/// <summary>
		/// Draws a solid box out of dust for walls ect
		/// </summary>
		/// <param name="area">The box</param>
		public virtual void BoxEffects(Rectangle area)
		{
			for (int i = 0; i <= area.Length(); i++)
			{
				Dust.NewDustPerfect(area.RandomBorder(), Main.rand.Next(Dusts), Vector2.Zero, newColor: Colour);
				Dust.NewDustPerfect(area.RandomBorder(), Main.rand.Next(Dusts), Vector2.Zero, newColor: Colour);
			}

			LingeringEffects(area);
		}


		/// <summary>
		/// For surge, ect
		/// </summary>
		public virtual void ConeEffects(Vector2 coneCenter, float coneLength, float coneRotation, float maximumAngle = 0f)
		{
			for (int i = 0; i < 2; i++)
			{
				AOUtils.NewDustImperfect(coneCenter, Main.rand.Next(Dusts), (coneRotation + Main.rand.NextFloat(-maximumAngle, maximumAngle)).ToRotationVector2() * (coneLength / 45f), newColor: Colour, Scale: .2f * (coneLength / 25f));
			}
		}

		public override void UseAnimation(Player player)
		{
			if (player.AltUse() && Main.myPlayer == player.whoAmI)
			{
				var name = "";
				if (player.Imbue() is SteamImbue steam)
				{
					name = steam.Imbue.Name;
				}
				else if (player.Imbue() is not null)
					name = player.Imbue().Name;
				if (Name != name)
				{
					CreateMagicCircle(Item, player, MagicCircleMode.Rotating, true);
				}
				if (Name != name)
				{
					player.ArcaneOdyssey().Imbue = this;
					LocalizedText chatmessage = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.ImbueChatMessage", DisplayName.Value);
					Main.NewText(chatmessage.Value, 13, 132, 168);

				}
				else
				{
					player.ArcaneOdyssey().Imbue = null;
					LocalizedText chatmessage = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.UnimbueText");
					Main.NewText(chatmessage.Value, 13, 132, 168);

				}
			}
			else 
			{
				if (!Main.dedServ)
				{
					SoundEngine.PlaySound(selectedAttack?.ExtraSound, player.MountedCenter);
				}
				if (Drawback > 0)
				{
					if ((!player.AltUse()) && Main.myPlayer == player.whoAmI)
					{
						player.Hurt(PlayerDeathReason.ByCustomReason(ArcaneOdysseyMod.Instance.CustomLocalization($"Drawback.Death{Main.rand.Next(4)}", player.name).ToNetworkText()), player.statLifeMax / 100 * Drawback, Main.rand.NextBool().ToDirectionInt(), dodgeable: false, knockback: 0f, scalingArmorPenetration: 1f);
					}
				}
			}
		}

		public Color Colour
		{
			get
			{
				var colour = ImbueColour2;
				if (this is IBarrableImbue bar)
				{
					if (Imbue is not null)
					{
						colour = Imbue.ImbueColour;
					}
					return Color.Lerp(colour, ImbueColour, bar.LerpValue);
				}
				if (AnimatedColours)
				{
					return Color.Lerp(ImbueColour, colour, MathF.Sin(AOUtils.UpdateCount) / 2f + .5f);
				}
				return ImbueColour;
			}
		}

		/// <summary>
		/// Return false to cancel VFX
		/// </summary>
		/// <param name="entity">The entity to check</param>
		/// <returns></returns>
		public static bool PreEffects(Entity entity)
		{
			if (Main.dedServ || entity.velocity.Length() == 1)
			{
				return false;
			}
			if (entity is Projectile projectile)
			{
				if (projectile.ModProjectile is PlayerProjectile proj && (!proj.CanHaveImbueVFX))
				{
					return false;
				}
				if (entity.TryGetOwner(out Player player) && player.heldProj == entity.whoAmI)
				{
					return false;
				}
			}
			return true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.width = Item.height = 52;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.UseSound = ImbueSound;
			Item.autoReuse = true;
		}

		public sealed override bool AltFunctionUse(Player player) => true;

		/// <summary>
		/// Whether this imbue is a:
		/// <list>Lesser Lost Magic</list>
		/// <list>Lost Spirit Mutation</list>
		/// <list>Ancient Spirit Mutation</list>
		/// <list>Dev Spirit Mutation</list>
		/// </summary>
		public virtual bool Special => false;

		public string TooltipsPrefix
		{
			get
			{
				if (this is MagicType)
				{
					if (Special && ArcaneOdysseyMod.DevMode) // eventually i want to add some way to get lore, like athenas wisdom. if we have the knowledge it will be added here...
					{
						return "Special";
					}
					return "Magic";
				}

				if (this is FightingStyle)
				{
					return "FS";
				}

				if (this is SpiritEnergy)
				{
					return "Relic";
				}

				return null;
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			if (!tooltips.Contains(tooltips.Find(e => e.Name == "Social" && e.Mod == "Terraria")))
			{
				TooltipLine drawback = new(Mod, "Drawback", ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Drawback", Drawback).Value);
				if (Drawback < 1)
				{
					drawback.Hide();
				}
				tooltips.AddTooltip(drawback, Color.Red);

				if (!Main.keyState.IsKeyDown(Keys.LeftShift))
				{
					tooltips.AddTooltip(new(Mod, "DisplayedAODamage", ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.ScrollDamage", MathF.Round(ScrollDamage, 3)).Value));
					tooltips.AddTooltip(new(Mod, "DisplayedAOSpeed", ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.ScrollSpeed", MathF.Round(ScrollSpeed, 3)).Value));
					tooltips.AddTooltip(new(Mod, "DisplayedAOSize", ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.ScrollSize", MathF.Round(ScrollSize, 3)).Value));

					if (ImbueDebuffs.Length > 0)
					{
						string req = "";
						if (ImbueDebuffs[0].debuffPercent > 0)
						{
							req = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Requirement", (ImbueDebuffs[0].debuffPercent * 100f).Round()).Value;
						}
						var debufftext = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Debuffs", Lang.GetBuffName(ImbueDebuffs[0].debuffID) + req).Value;
						foreach (var debuff in ImbueDebuffs)
						{
							req = "";
							if (debuff.debuffID != ImbueDebuffs[0].debuffID)
							{
								if (debuff.debuffPercent > 0)
								{
									req = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Requirement", (debuff.debuffPercent * 100f).Round()).Value;
								}
								debufftext = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Conjoined", debufftext, Lang.GetBuffName(debuff.debuffID) + req).Value;
							}
						}
						tooltips.AddTooltip(new(Mod, "DebuffInfo", debufftext));
					}
					else
					{
						tooltips.AddTooltip(new(Mod, "DebuffInfo", ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.NoDebuffs").Value));
					}
				}
				else
				{
					tooltips.AddTooltip(new(Mod, "DisplayedAODamage", ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.ImbueDamage", MathF.Round(ImbueDamage, 3)).Value));
					tooltips.AddTooltip(new(Mod, "DisplayedAOSpeed", ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.ImbueSpeed", MathF.Round(ImbueSpeed, 3)).Value));
					tooltips.AddTooltip(new(Mod, "DisplayedAOSize", ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.ImbueSize", MathF.Round(ImbueSize, 3)).Value));

					if (CombinedDebuffs.Length > 0)
					{
						var aaaaa = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Result", Lang.GetBuffName(CombinedDebuffs[0].requirement), Lang.GetBuffName(CombinedDebuffs[0].result));
						var debufftext = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Combined", aaaaa).Value;
						foreach (var debuff in CombinedDebuffs)
						{
							if (debuff.requirement != CombinedDebuffs[0].requirement)
							{
								aaaaa = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Result", Lang.GetBuffName(debuff.requirement), Lang.GetBuffName(debuff.result));
								debufftext = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Conjoined", debufftext, aaaaa).Value;
							}
						}
						tooltips.AddTooltip(new(Mod, "DebuffInfo", debufftext));
					}
					else
					{
						tooltips.AddTooltip(new(Mod, "DebuffInfo", ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.NoCombinedDebuffs").Value));
					}
				}
				tooltips.AddTooltip(new(Mod, "ShiftNotice", ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.ShiftNotice").Value));

				if (Property.HasValue)
				{
					tooltips.AddTooltip(new TooltipLine(Mod, "Property", Property?.Name), Property?.Colour);
				}

				if (Gimmick is not null)
				{
					string text = $"{Gimmick.DisplayName.Value}: {Gimmick.Description}";
					tooltips.AddTooltip(new TooltipLine(Mod, "Gimmick", text), Colour);
				}

				if (Passive is AuraSkill aura)
				{
					tooltips.AddTooltip(new(Mod, "CycleKeybind", ArcaneOdysseyMod.Instance.CustomLocalization("RandomWords.AuraMode", aura.Mode, AOKeybinds.CycleAuraMode.GetAssignedKeys().FirstOrDefault(ArcaneOdysseyMod.Instance.CustomLocalization("RandomWords.Unbound").Value)).Value));
				}

				if (Dash is ReflexSkill reflex)
				{
					var scroll = ModContent.GetModItem(reflex.Scroll);

					var tool = new TooltipLine(Mod, "ReflexInfo", scroll.GetLocalizedValue("Special.Generic")); // second line of tooltip

					if (DashSpeed > 1f)
					{
						tool.Text = scroll.GetLocalizedValue("Special.Fast");
					}

					if (DashResist.HasValue)
					{
						tool.Text = scroll.GetLocalizedValue("Special.Resist");
					}

					if (ImmuneDash)
					{
						tool.Text = scroll.GetLocalizedValue("Special.Instant");
					}

					if (this is VanishingStyle)
					{
						tool.Text = scroll.GetLocalizedValue("Special.Vanish");
					}

					if (this is ThermoFist)
					{
						tool.Text = scroll.GetLocalizedValue("Special.Thermo");
					}

					if (this is SailorStyle)
					{
						tool.Text = scroll.GetLocalizedValue("Special.Sailor");
					}
					tooltips.AddTooltip(tool, Colour);
				}

				foreach (var action in ExtraTooltips)
				{
					var tip = action(this);
					if (tip is not null)
						tooltips.AddTooltip(tip);
				}
			}

			if (TooltipsPrefix is not null)
				tooltips.AddTooltip(new TooltipLine(Mod, "ImbuableTier", ArcaneOdysseyMod.Instance.CustomLocalization($"{TooltipsPrefix}TierLines.{ImbuableTier}").Value));
		}

		public static List<Func<Imbuable, TooltipLine>> ExtraTooltips = [];

		public override void Unload()
		{
			ExtraTooltips.Clear();
		}

		private static int SortMultipliers(Synergy x, Synergy y)
		{
			if (x.multiplier > y.multiplier)
			{
				return 1;
			}
			if (x.multiplier < y.multiplier)
			{
				return -1;
			}
			return 0;
		}

		public string SynergiesText()
		{
			var text = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.NoSynergies").Value;

			var syns = Effects.magicBuffMultipliers.Sorted(new Comparison<Synergy>(SortMultipliers));
			if (syns.Count > 0)
			{
				text = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.SynergiesInfo", DisplayName.Value, Lang.GetBuffName(syns[0].buffID) + Mod.CustomLocalization("ImbueStuff.SynergyMulti", syns[0].multiplier).Value).Value;

				foreach (var effect in syns)
				{
					if (effect.buffID != syns[0].buffID)
					{
						text = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Conjoined", text, Lang.GetBuffName(effect.buffID) + Mod.CustomLocalization("ImbueStuff.SynergyMulti", effect.multiplier).Value).Value;
					}
				}
			}

			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				var clear = Effects.clearBuffs.OrderBy(e => Lang.GetBuffName(e.id)).ToArray();
				if (clear.Length > 0)
				{
					text = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.ClearsInfo", text, Lang.GetBuffName(clear[0].id)).Value;

					foreach (var buff in clear)
					{
						if (buff.id != clear[0].id)
						{
							text = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.Conjoined", text, Lang.GetBuffName(buff.id)).Value;
						}
					}
				}
			}

			text = ArcaneOdysseyMod.Instance.CustomLocalization("ImbueStuff.SentenceEnd", text).Value;

			return text;
		}

		// spread 
		public static Circle CreateMagicCircle(Item item, Player player, MagicCircleMode mode, bool markedfordeath, int chargingProjectile = 0, bool altfire = false, float spread = 0f, Vector2? position = null, float? rotation = null)
		{
			if (mode != MagicCircleMode.Rotating)
			{
				position ??= player.RotatedRelativePoint(player.MountedCenter) + (player.SafeDirectionTo(Main.MouseWorld) * 30f);
				rotation ??= player.AngleTo(Main.MouseWorld);
			}
			else
			{
				position ??= player.RotatedRelativePoint(player.MountedCenter);
				rotation ??= 0;
			}
			Circle circle = Projectile.NewProjectileDirect(item.GetSource_ItemUse(player), position.Value, Vector2.Zero, ModContent.ProjectileType<Circle>(), player.GetWeaponDamage(item), player.GetWeaponKnockback(item), player.whoAmI, ai2: (int)mode).ModProjectile as Circle;
			circle.ProjectileSpread = spread;
			circle.MarkedForDeath = markedfordeath;
			circle.originallyAltFire = altfire;
			circle.ChargingProjectile = chargingProjectile;
			circle.Projectile.rotation = rotation.Value;
			if ((chargingProjectile != 0) || (!markedfordeath))
			{
				player.ArcaneOdyssey().myCircle = circle;
			}
			return circle;
		}

		public Circle CreateMagicCircle(Player player, MagicCircleMode mode, bool markedfordeath, int chargingProjectile = 0, bool altfire = false, float spread = 0f, Vector2? position = null, float? rotation = null)
		{
			if (mode != MagicCircleMode.Rotating)
			{
				position ??= player.RotatedRelativePoint(player.MountedCenter) + (player.SafeDirectionTo(Main.MouseWorld) * 30f);
				rotation ??= player.AngleTo(Main.MouseWorld);
			}
			else
			{
				position ??= player.RotatedRelativePoint(player.MountedCenter);
				rotation ??= 0;
			}
			Circle circle = Projectile.NewProjectileDirect(Item.GetSource_ItemUse(player), position.Value, Vector2.Zero, ModContent.ProjectileType<Circle>(), player.GetWeaponDamage(Item), player.GetWeaponKnockback(Item), player.whoAmI, ai2: (int)mode).ModProjectile as Circle;
			circle.ProjectileSpread = spread;
			circle.MarkedForDeath = markedfordeath;
			circle.originallyAltFire = altfire;
			circle.ChargingProjectile = chargingProjectile;
			circle.Projectile.rotation = rotation.Value;
			if ((chargingProjectile != 0) || (!markedfordeath))
			{
				player.ArcaneOdyssey().myCircle = circle;
			}
			return circle;
		}

		public static bool AltUsing => AOKeybinds.AltSkillUse.Current;

		public float ApplySpeed(float value, bool flipfloat = false)
		{
			if (!flipfloat)
			{
				value *= ScrollSpeed;
				if (Imbue is not null)
					value *= Imbue.ImbueSpeed;
			}
			else
			{
				value *= ScrollSpeed.FlipFloat();
				if (Imbue is not null)
					value *= Imbue.ImbueSpeed.FlipFloat();
			}
			return value;
		}

		public int ApplyDamage(float value, bool flipfloat = false)
		{
			if (!flipfloat)
			{
				value *= Item.ArcaneOdyssey()?.owner?.GetTotalDamage(Item.DamageType).ApplyTo(1f) ?? 1f;
				value *= ScrollDamage;
				if (Imbue is not null)
					value *= Imbue.ImbueDamage;
			}
			else
			{
				value *= (Item.ArcaneOdyssey()?.owner?.GetTotalDamage(Item.DamageType).ApplyTo(1f) ?? 1f).FlipFloat();
				value *= ScrollDamage.FlipFloat();
				if (Imbue is not null)
					value *= Imbue.ImbueDamage.FlipFloat();
			}
			return (int)value;
		}

		public float ApplySize(float value, bool flipfloat = false)
		{
			if (!flipfloat)
			{
				value *= Item.ArcaneOdyssey()?.owner?.ArcaneOdyssey()?.SizeMulti ?? 1f;
				value *= ScrollSize;
				if (Imbue is not null)
					value *= Imbue.ImbueSize;
			}
			else
			{
				value *= (Item.ArcaneOdyssey()?.owner?.ArcaneOdyssey()?.SizeMulti ?? 1f).FlipFloat();
				value *= ScrollSize.FlipFloat();
				if (Imbue is not null)
					value *= Imbue.ImbueSize.FlipFloat();
			}
			return value;
		}

		public float ApplyKnockback(float value, bool flipfloat = false)
		{
			if (!flipfloat)
			{
				value *= ScrollSize.Pow();
				if (Imbue is not null)
					value *= Imbue.ImbueSize.Pow();
				value *= KBMulti;
				if (Imbue is not null)
					value *= Imbue.KBMulti;
			}
			else
			{
				value *= ScrollSize.FlipFloat().Pow();
				if (Imbue is not null)
					value *= Imbue.ImbueSize.FlipFloat().Pow();
				value *= KBMulti.FlipFloat();
				if (Imbue is not null)
					value *= Imbue.KBMulti.FlipFloat();
			}
			return value;
		}

		#region Acrimony Handling, here are the methods for right clicking in inventory (in case they are needed for something else)
		public override void RightClick(Player player)
		{
			Main.playerInventory = false;
			var instance = ModContent.GetInstance<ModUISystem>();
			if (!instance.CanShowImbueChange())
				instance.ShowSwapUI(this);
		}

		public override bool CanRightClick()
		{
			try
			{
				Player player = Main.LocalPlayer;

				if (!(Type == ModContent.ItemType<SpiritEnergy>() || this is EaglePatrimony or MagicType or FightingStyle && ImbuableTier == ImbuableTiers.Normal))
				{
					//Main.NewText($"Item is not swappable");
					return false;
				}

				if (!player.HasTypeInInventory<Acrimony>())
				{
					//Main.NewText($"Doesn't have acrimony");
					return false;
				}

				//Main.NewText($"Can use item {!ModContent.GetInstance<MagicChoiceUISystem>().CanShowUI()}");

				return true;
			}
			catch (Exception ex)
			{
				Main.NewText($"Error in {nameof(CanRightClick)}: \n{ex}", new Color(255, 0, 255));
				return false;
			}
		}
		public override bool ConsumeItem(Player player) => false;
		#endregion
	}
}
