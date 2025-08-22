using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles.Base;
using ArcaneOdyssey.Content.Projectiles.Weapons;
using Humanizer;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey
{
	public class ArcaneOdyssey : Mod 
	{
		public static Dictionary<string, LocalizedText> staticLocalizer = new();
	}

	public class ItemManager : GlobalItem
	{

		public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
			AOPlayer playah = player.AOPlayer();
			if (item.ModItem is AOWeapon weap)
			{
				if (weap.WeaponDebuff is not null && (weap.WeaponDebuff.DebuffPercent is null or 0 || modifiers.GetDamage(item.damage, true) > (target.lifeMax / weap.WeaponDebuff.DebuffPercent)))
				{
					target.AddBuff(weap.WeaponDebuff.debuffID, weap.WeaponDebuff.debuffDuration);
				}
			}

			if (playah.imbue is not null)
			{
				if ((playah.imbue.MagicDebuff is not null) && (playah.imbue.MagicDebuff.DebuffPercent != 0f)) 
				{
					if (playah.imbue.MagicDebuff.DebuffPercent is null || modifiers.GetDamage(item.damage, true) > (target.lifeMax / playah.imbue.MagicDebuff.DebuffPercent))
					{
						target.AddBuff(playah.imbue.MagicDebuff.debuffID, playah.imbue.MagicDebuff.debuffDuration);
					}
				}
				if ((playah.imbue.MagicDebuff2 is not null) && (playah.imbue.MagicDebuff2.DebuffPercent != 0f)) 
				{
					if (playah.imbue.MagicDebuff2.DebuffPercent is null || modifiers.GetDamage(item.damage, true) > (target.lifeMax / playah.imbue.MagicDebuff2.DebuffPercent))
					{
						target.AddBuff(playah.imbue.MagicDebuff2.debuffID, playah.imbue.MagicDebuff2.debuffDuration);
					}
				}

				if (playah.imbue.CombinedDebuffs is not null)
				{
					foreach (CombinedDebuff buffkeys in playah.imbue.CombinedDebuffs)
					{
						if (target.HasBuff(buffkeys.requirement))
						{
							target.AddBuff(buffkeys.result, buffkeys.duration);
						}
					}
				}

				foreach (MagicBuffMultiplier multiplier in playah.imbue.Effects.magicBuffMultipliers)
				{
					if (target.HasBuff(multiplier.buffID))
					{
						modifiers.FinalDamage *= multiplier.multiplier;
					}
				}

				if (Main.netMode == NetmodeID.SinglePlayer) // things would get chaotic in multiplayer if everyone kept clearing eachothers debuffs
				{
					foreach (int buffid in playah.imbue.Effects.clearBuffs)
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
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<ArcaniumScrap>(), 15, 0, 1));
			}
		}

		public override void UpdateInventory(Item item, Player player)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
		}

		/// <summary>
		/// used in singleplayer exclusively to display current imbue, might not work in multiplayer idk
		/// </summary>
		public static AOPlayer playerForImbue = null;

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			bool extraconfs = false;
			if (ModLoader.HasMod("CalamityMod"))
			{
				List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
				if (goodclasses.Contains(item.DamageType.Name))
				{
					extraconfs = true;
				}
			}
			if (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed || item.DamageType == DamageClass.Ranged || extraconfs || item.ModItem is DefaultScroll)
			{
				string imbuetextthing = Mod.CustomLocalization("ImbueStuff.NoneText").Value;
				if (Main.netMode != NetmodeID.SinglePlayer)
					imbuetextthing = Mod.CustomLocalization("ImbueStuff.MultiplayerCannotDisplay").Value;
				if (playerForImbue is not null)
					if (playerForImbue.imbue is not null)
						imbuetextthing = playerForImbue.imbue.Item.Name;
				tooltips.Add(new TooltipLine(Mod, "ImbueText", Mod.CustomLocalization("ImbueStuff.ImbueTooltip", [imbuetextthing]).Value));
			}

			if (item.ModItem is AOMagic magical)
			{
				tooltips.Add(new TooltipLine(Mod, "MagicTier", Mod.CustomLocalization($"MagicTierLines.{magical.MagicTier.ToString()}").Value));
			}
		}

		public override bool? UseItem(Item item, Player player)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
			if (item.ModItem is AOMagic magic)
			{
				if (magic != player.AOPlayer().imbue)
				{
					player.AOPlayer().imbue = magic;
					LocalizedText chatmessage = Mod.CustomLocalization("ImbueStuff.ImbueChatMessage", [item.Name]);
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						Main.NewText(chatmessage.Value, 13, 132, 168);
					}
					else if (Main.netMode == NetmodeID.Server)
					{
						ChatHelper.SendChatMessageToClient(chatmessage.ToNetworkText(), new Color(13, 132, 168), Array.IndexOf(Main.player, player));
					}
				}
				else 
				{
					player.AOPlayer().imbue = null;
					LocalizedText chatmessage = Mod.CustomLocalization("ImbueStuff.UnimbueText");
					if (Main.netMode == NetmodeID.SinglePlayer)
					{
						Main.NewText(chatmessage.Value, 13, 132, 168);
					}
					else if (Main.netMode == NetmodeID.Server)
					{
						ChatHelper.SendChatMessageToClient(chatmessage.ToNetworkText(), new Color(13, 132, 168), Array.IndexOf(Main.player, player));
					}
				}
			}
			return base.UseItem(item, player);
		}

		public override void ModifyItemScale(Item item, Player player, ref float scale)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
			bool extraconfs = false;
			if (ModLoader.HasMod("CalamityMod"))
			{
				List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
				if (goodclasses.Contains(item.DamageType.Name))
				{
					extraconfs = true;
				}
			}
            if (player.AOPlayer().imbue is not null && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed || extraconfs))
			{
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					scale += aoWeapon.AOSize.MultiToPercent() + player.AOPlayer().imbue.AOImbueSize.MultiToPercent() + player.AOPlayer().GetSizeMulti(item).MultiToPercent();
				}
				else if (item.ModItem is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					scale += player.AOPlayer().imbue.AOImbueSize.MultiToPercent() + player.AOPlayer().GetSizeMulti(item).MultiToPercent();
				}
			}
		}

		public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
			bool extraconfs = false;
			if (ModLoader.HasMod("CalamityMod"))
			{
				List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
				if (goodclasses.Contains(item.DamageType.Name))
				{
					extraconfs = true;
				}
			}
			if (player.AOPlayer().imbue is not null && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed || extraconfs))
			{
				float extrakbmulti = 1f;
				if (player.AOPlayer().imbue is WindMagic)
				{
					extrakbmulti = 2f;
				}
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					knockback += aoWeapon.AOSize.MultiToPercent() + player.AOPlayer().imbue.AOImbueSize.MultiToPercent() + extrakbmulti + player.AOPlayer().GetSizeMulti(item).MultiToPercent();
				}
				else if (item.ModItem is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					knockback += player.AOPlayer().imbue.AOImbueSize.MultiToPercent() + extrakbmulti.MultiToPercent();
				}
			}
		}
		public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
			bool extraconfs = false;
			if (ModLoader.HasMod("CalamityMod"))
			{
				List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
				if (goodclasses.Contains(item.DamageType.Name))
				{
					extraconfs = true;
				}
			}
			if (player.AOPlayer().imbue is not null && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed || item.DamageType == DamageClass.Ranged || extraconfs))
			{
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					damage += aoWeapon.AODamage.MultiToPercent() + player.AOPlayer().imbue.AOImbueDamage.MultiToPercent();
				}
				else if (item.ModItem is null) // do not touch items from other mods
				{
					damage += player.AOPlayer().imbue.AOImbueDamage.MultiToPercent();
				}
			}
		}
		public override float UseSpeedMultiplier(Item item, Player player)
		{
			if (player == Main.LocalPlayer)
				playerForImbue = player.AOPlayer();
			bool extraconfs = false;
			if (ModLoader.HasMod("CalamityMod"))
			{
				List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
				if (goodclasses.Contains(item.DamageType.Name))
				{
					extraconfs = true;
				}
			}
			if (player.AOPlayer().imbue is not null && (extraconfs || item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed || item.DamageType == DamageClass.Ranged))
			{
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					return aoWeapon.AOSpeed + player.AOPlayer().imbue.AOImbueSpeed.MultiToPercent();
				}
				else if (item.ModItem is not null && ArcaneOdysseyConfig.Instance.AffectsOtherMods)
				{
					return player.AOPlayer().imbue.AOImbueSpeed;
				}
				else if (item.ModItem is null) // do not touch items from other mods
				{
					return player.AOPlayer().imbue.AOImbueSpeed;
				}
			}
			return 1f;
		}
	}

	public class NPCDrops : GlobalNPC
	{
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			if (npc.type == NPCID.WallofFlesh)
			{
				LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.IsPreHardmode());
				leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HecateOrb>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.CultistBoss)
			{
				LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new FirstCultistKill());
				leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HecateOrb>()));
				npcLoot.Add(leadingConditionRule);
			}
			if (npc.type == NPCID.Plantera)
			{
				LeadingConditionRule leadingConditionRule = new LeadingConditionRule(new Conditions.FirstTimeKillingPlantera());
				leadingConditionRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<HecateShard>()));
				npcLoot.Add(leadingConditionRule);
			}
		}
	}

	public class FirstCultistKill : IItemDropRuleCondition
	{
		public bool CanDrop(DropAttemptInfo info) => !NPC.downedAncientCultist;
		public bool CanShowItemDropInUI() => true;
		public string GetConditionDescription() => Language.GetOrRegister("Mods.ArcaneOdyssey.FirstCultistKillDescription", () => "First Lunatic Cultist Defeated").Value;
	}


	public class AOPlayer : ModPlayer
	{
		public AOMagic imbue = null;

		/// <summary>
		/// Whether the user has a set of sunken armour equipped
		/// </summary>
		public bool sunkenArmour = false;

		public int AOSizeStat = 0;

        public bool RightClicking => Player.altFunctionUse == 2;

		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
		{
			if (!mediumCoreDeath)
			{
				return [new Item(ModContent.ItemType<HecateOrb>())];
			}
			else return [];
		}

        public override void ResetEffects()
        {
			sunkenArmour = false;
			AOSizeStat = 0;
        }

		public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
		{
			if (sunkenArmour)
			{
				npc.AddBuff(BuffID.Wet, 60 * 10);
			}
        }

        public float GetSizeMulti(Item item)
        {
            float stat = AOSizeStat / 5f;
            if (Player.meleeScaleGlove && item.DamageType.Name.Contains("Melee"))
            {
                stat += .1f;
            }
            return stat + 1f;
        }

        public float GetSizeMulti(Projectile projectile)
        {
            float stat = AOSizeStat / 5f;
            if (Player.meleeScaleGlove && projectile.DamageType.Name.Contains("Melee"))
            {
                stat += .1f;
            }
            return stat + 1f;
        }
    }

	public class ProjectileManager : GlobalProjectile
	{
		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (projectile.owner == Main.myPlayer && projectile.owner != 255 && !projectile.hostile && !projectile.npcProj)
			{
				AOPlayer playah = Main.player[projectile.owner].AOPlayer();
				if (ArcaneOdysseyConfig.Instance.IgnoredProjectiles is null || !ArcaneOdysseyConfig.Instance.IgnoredProjectiles.Contains(projectile.Name))
				{
					bool extraconfs = false;
					if (ModLoader.HasMod("CalamityMod"))
					{
						List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
						if (goodclasses.Contains(projectile.DamageType.Name))
						{
							extraconfs = true;
						}
					}
					if ((projectile.ModProjectile is null or AOPlayerProjectile || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && (extraconfs || projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.MeleeNoSpeed || projectile.DamageType == DamageClass.Ranged || projectile.ModProjectile is MagicSpell))
					{
						AOMagic imbue = null;
						bool spell = false;
						if (projectile.ModProjectile is AOPlayerProjectile proj)
						{
							imbue = proj.thisMagic;
							spell = proj.IsSpell;
						}
						else imbue = playah.imbue;
						if (spell)
						{
							modifiers.FinalDamage.Base += BonusBossKills();
						}
						if (imbue is not null)
						{
							modifiers.FinalDamage *= !spell ? imbue.AOImbueDamage : imbue.AOMagicDamage;
							if ((imbue.MagicDebuff is not null) && (imbue.MagicDebuff.DebuffPercent != 0f))
							{
								if (imbue.MagicDebuff.DebuffPercent is null || modifiers.GetDamage(projectile.damage, true) > (target.lifeMax / imbue.MagicDebuff.DebuffPercent))
								{
									target.AddBuff(imbue.MagicDebuff.debuffID, imbue.MagicDebuff.debuffDuration);
								}
							}
							if ((imbue.MagicDebuff2 is not null) && (imbue.MagicDebuff2.DebuffPercent != 0f))
							{
								if (imbue.MagicDebuff2.DebuffPercent is null || modifiers.GetDamage(projectile.damage, true) > (target.lifeMax / imbue.MagicDebuff2.DebuffPercent))
								{
									target.AddBuff(imbue.MagicDebuff2.debuffID, imbue.MagicDebuff2.debuffDuration);
								}
							}

							if (imbue.CombinedDebuffs is not null)
							{
								foreach (CombinedDebuff buffkeys in imbue.CombinedDebuffs)
								{
									if (target.HasBuff(buffkeys.requirement))
									{
										target.AddBuff(buffkeys.result, buffkeys.duration);
									}
								}
							}

							foreach (MagicBuffMultiplier multiplier in imbue.Effects.magicBuffMultipliers)
							{
								if (target.HasBuff(multiplier.buffID))
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
				}
			}
		}

		public static Dictionary<string, Vector2> OriginalScales = [];

		public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
		{
			if (projectile.owner == Main.myPlayer && projectile.owner != 255 && !projectile.hostile && !projectile.npcProj && projectile.Name != "Falling Star")
			{
				Player player = Main.player[projectile.owner];
				bool extraconfs = false;
				if (ModLoader.HasMod("CalamityMod"))
				{
					List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
					if (goodclasses.Contains(projectile.DamageType.Name))
					{
						extraconfs = true;
					}
				}
				Vector2 dim = new(hitbox.Width, hitbox.Height);
				if (projectile.ModProjectile is AOBaseProjectile origin)
				{
					dim = origin.OriginalDimensions.GetValueOrDefault(new(hitbox.Width, hitbox.Height));
				}
				else
				{ 
					dim = OriginalScales.GetValueOrDefault(projectile.Name, new(hitbox.Width, hitbox.Height)); 
				}
				if (extraconfs || projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.MeleeNoSpeed || projectile.DamageType == DamageClass.Ranged || projectile.ModProjectile is MagicSpell)
				{
					AOMagic imbue = null;
					float scale = 1f;
					bool spell = false;
					if (projectile.ModProjectile is AOPlayerProjectile proj)
					{
						imbue = proj.thisMagic;
						scale = proj.BaseScale.GetValueOrDefault(1f) + proj.AOSize.MultiToPercent();
						spell = proj.IsSpell;
					}
					else
						imbue = Main.player[projectile.owner].AOPlayer().imbue;
					float mult = scale;
					if (imbue is not null)
					{
						mult = (spell ? imbue.AOMagicSize : imbue.AOImbueSize).MultiToPercent() + scale + player.AOPlayer().GetSizeMulti(projectile).MultiToPercent();
					}
                    hitbox.Width = (int)(dim.X * mult);
                    hitbox.Height = (int)(dim.Y * mult);
                    projectile.scale = mult;
                }
			}
		}

		public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (projectile.ModProjectile is AOBaseProjectile origin)
            {
                origin.OriginalDimensions ??= projectile.Size;
				origin.BaseScale ??= projectile.scale;
            }
			else
			{
				OriginalScales[projectile.Name] = projectile.Size;
			}
			if (projectile.owner == Main.myPlayer && projectile.owner != 255 && !projectile.hostile && !projectile.npcProj && projectile.Name != "Falling Star")
			{
				bool extraconfs = false;
				if (ModLoader.HasMod("CalamityMod"))
				{
					List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
					if (goodclasses.Contains(projectile.DamageType.Name))
					{
						extraconfs = true;
					}
				}
				if (extraconfs || projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.Ranged || projectile.ModProjectile is MagicSpell)
				{
					AOMagic imbue = Main.player[projectile.owner].AOPlayer().imbue;
					bool spell = false;
					if (projectile.ModProjectile is AOPlayerProjectile proj)
					{
						imbue = proj.thisMagic;
						spell = proj.IsSpell;
					}
					if (imbue is not null)
						projectile.velocity *= spell ? imbue.AOMagicSpeed : imbue.AOImbueSpeed;
				}

				Player player = Main.player[projectile.owner];
				AOPlayer aoPlayerOwner = player.AOPlayer();
				if (projectile.ModProjectile is AOPlayerProjectile proj1 && Main.netMode != NetmodeID.Server)
				{
					aoPlayerOwner?.imbue?.SpawningDust(projectile.position, proj1.BaseScale.Value);
				}
				else
				{
					if ((projectile.ModProjectile is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && Main.netMode != NetmodeID.Server)
					{
						aoPlayerOwner?.imbue?.SpawningDust(projectile.position, projectile.scale);
					}
				}
			}
		}

		public override void AI(Projectile projectile)
		{
			Player player = Main.player[projectile.owner];
			AOPlayer aoPlayerOwner = player.AOPlayer();
			if (projectile.ModProjectile is AOBaseProjectile based)
			{
				based.FramesAlive += 1;
			}
			if (projectile.ModProjectile is AOPlayerProjectile proj)
            {
                proj.BaseScale ??= projectile.scale;
                if (Main.netMode != NetmodeID.Server)
					proj.thisMagic?.LingeringDust(projectile.position, proj.DustVelocity.GetValueOrDefault(projectile.velocity), projectile.scale);
				if (aoPlayerOwner is not null)
				{
					proj.thisMagic ??= aoPlayerOwner.imbue;
				}
			}
			else
			{
				if ((projectile.ModProjectile is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && Main.netMode != NetmodeID.Server)
				{
					aoPlayerOwner?.imbue?.LingeringDust(projectile.position, projectile.velocity, projectile.scale);
				}
			}
		}

		public override void OnKill(Projectile projectile, int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			AOPlayer aoPlayerOwner = player.AOPlayer();
			if (projectile.ModProjectile is AOPlayerProjectile proj && Main.netMode != NetmodeID.Server)
			{
				proj.thisMagic?.KillDust(projectile.position, projectile.scale);
			}
			else
			{
				if ((projectile.ModProjectile is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && Main.netMode != NetmodeID.Server)
				{
					aoPlayerOwner?.imbue?.KillDust(projectile.position, projectile.scale);
				}
			}
		}
	}
}
