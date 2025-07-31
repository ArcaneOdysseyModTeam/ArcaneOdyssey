using ArcaneOdyssey.Content.Items;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Magic;
using ArcaneOdyssey.Content.Projectiles.Base;
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
	}
	public class VanillaSynergy : GlobalItem
	{
		public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				playerForImbue = player.GetModPlayer<AOPlayer>();
			AOPlayer playah = player.GetModPlayer<AOPlayer>();
			if (item.ModItem is AOWeapon weap)
			{
				if (weap.WeaponDebuff is not null && (weap.WeaponDebuff.DebuffPercent is null or 0 || modifiers.GetDamage(item.damage, true) > (target.lifeMax / weap.WeaponDebuff.DebuffPercent)))
				{
					target.AddBuff(weap.WeaponDebuff.debuffID, weap.WeaponDebuff.debuffDuration);
				}
			}

			if (playah.imbue is not null)
			{
				if ((playah.imbue.MagicDebuff is not null)&&(!(playah.imbue.MagicDebuff.DebuffPercent == 0f))) {
					if ((playah.imbue.MagicDebuff.DebuffPercent is null || modifiers.GetDamage(item.damage, true) > (target.lifeMax / playah.imbue.MagicDebuff.DebuffPercent)))
					{
					target.AddBuff(playah.imbue.MagicDebuff.debuffID, playah.imbue.MagicDebuff.debuffDuration);
				}
					}
				if ((playah.imbue.MagicDebuff2 is not null)&&(!(playah.imbue.MagicDebuff2.DebuffPercent == 0f))) {
					if ((playah.imbue.MagicDebuff2.DebuffPercent is null || modifiers.GetDamage(item.damage, true) > (target.lifeMax / playah.imbue.MagicDebuff2.DebuffPercent)))
					{
					target.AddBuff(playah.imbue.MagicDebuff2.debuffID, playah.imbue.MagicDebuff2.debuffDuration);
				}
					}

				if (playah.imbue.combinedDebuffs is not null)
				{
					foreach (CombinedDebuff buffkeys in playah.imbue.combinedDebuffs)
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
			if (Main.netMode == NetmodeID.SinglePlayer)
				playerForImbue = player.GetModPlayer<AOPlayer>();
		}

		/// <summary>
		/// used in singleplayer exclusively to display current imbue
		/// </summary>
		public static AOPlayer? playerForImbue = null;

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
				string imbuetextthing = Mod.GetLocalization("ImbueStuff.NoneText").Value;
				if (Main.netMode != NetmodeID.SinglePlayer)
					imbuetextthing = Mod.GetLocalization("ImbueStuff.MultiplayerCannotDisplay").Value;
				if (playerForImbue is not null)
					if (playerForImbue.imbue is not null)
						imbuetextthing = playerForImbue.imbue.Item.Name;
				tooltips.Add(new TooltipLine(Mod, "ImbueText", Mod.GetLocalization("ImbueStuff.ImbueTooltip").Format([imbuetextthing])));
			}

			if (item.ModItem is AOMagic magical)
			{
				tooltips.Add(new TooltipLine(Mod, "MagicTier", Mod.GetLocalization($"MagicTierLines.{magical.MagicTier.ToString()}").Value));
			}
		}

		public override bool? UseItem(Item item, Player player)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				playerForImbue = player.GetModPlayer<AOPlayer>();
			if (item.ModItem is AOMagic magic)
			{
				if (magic != player.GetModPlayer<AOPlayer>().imbue)
				{
					player.GetModPlayer<AOPlayer>().imbue = magic;
					LocalizedText chatmessage = Mod.GetLocalization("ImbueStuff.ImbueChatMessage").WithFormatArgs([item.Name]);
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
					player.GetModPlayer<AOPlayer>().imbue = null;
					LocalizedText chatmessage = Mod.GetLocalization("ImbueStuff.UnimbueText");
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
			if (Main.netMode == NetmodeID.SinglePlayer)
				playerForImbue = player.GetModPlayer<AOPlayer>();
			bool extraconfs = false;
			if (ModLoader.HasMod("CalamityMod"))
			{
				List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
				if (goodclasses.Contains(item.DamageType.Name))
				{
					extraconfs = true;
				}
			}
			if (player.GetModPlayer<AOPlayer>().imbue is not null && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed || extraconfs))
			{
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					scale *= aoWeapon.AOSize * player.GetModPlayer<AOPlayer>().imbue.AOImbueSize;
				}
				else if (item.ModItem is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					scale *= player.GetModPlayer<AOPlayer>().imbue.AOImbueSize;
				}
			}
		}

		public override void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				playerForImbue = player.GetModPlayer<AOPlayer>();
			bool extraconfs = false;
			if (ModLoader.HasMod("CalamityMod"))
			{
				List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
				if (goodclasses.Contains(item.DamageType.Name))
				{
					extraconfs = true;
				}
			}
			if (player.GetModPlayer<AOPlayer>().imbue is not null && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed || extraconfs))
			{
				float extrakbmulti = 1f;
				if (item.ModItem is WindMagic)
				{
					extrakbmulti = 2f;
				}
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					knockback *= aoWeapon.AOSize * player.GetModPlayer<AOPlayer>().imbue.AOImbueSize * extrakbmulti;
				}
				else if (item.ModItem is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) // do not touch items from other mods
				{
					knockback *= player.GetModPlayer<AOPlayer>().imbue.AOImbueSize * extrakbmulti;
				}
			}
		}
		public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				playerForImbue = player.GetModPlayer<AOPlayer>();
			bool extraconfs = false;
			if (ModLoader.HasMod("CalamityMod"))
			{
				List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
				if (goodclasses.Contains(item.DamageType.Name))
				{
					extraconfs = true;
				}
			}
			if (player.GetModPlayer<AOPlayer>().imbue is not null && (item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed || item.DamageType == DamageClass.Ranged || extraconfs))
			{
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					damage *= aoWeapon.AODamage * player.GetModPlayer<AOPlayer>().imbue.AOImbueDamage;
				}
				else if (item.ModItem is null) // do not touch items from other mods
				{
					damage *= player.GetModPlayer<AOPlayer>().imbue.AOImbueDamage;
				}
			}
		}

		public override float UseTimeMultiplier(Item item, Player player)
		{
			return UseAnimationMultiplier(item, player);
		}

		public override float UseAnimationMultiplier(Item item, Player player)
		{
			if (Main.netMode == NetmodeID.SinglePlayer)
				playerForImbue = player.GetModPlayer<AOPlayer>();
			bool extraconfs = false;
			if (ModLoader.HasMod("CalamityMod"))
			{
				List<string> goodclasses = new(["TrueMeleeDamageClass", "TrueMeleeNoSpeedDamageClass", "MeleeRangedHybridDamageClass"]);
				if (goodclasses.Contains(item.DamageType.Name))
				{
					extraconfs = true;
				}
			}
			if (player.GetModPlayer<AOPlayer>().imbue is not null && (extraconfs || item.DamageType == DamageClass.Melee || item.DamageType == DamageClass.MeleeNoSpeed || item.DamageType == DamageClass.Ranged))
			{
				if (item.ModItem is not null && item.ModItem is AOWeapon aoWeapon)
				{
					return FlipFloat(aoWeapon.AOSpeed * player.GetModPlayer<AOPlayer>().imbue.AOImbueSpeed);
				}
				else if (item.ModItem is not null && ArcaneOdysseyConfig.Instance.AffectsOtherMods)
				{
					return FlipFloat(player.GetModPlayer<AOPlayer>().imbue.AOImbueSpeed);
				}
				else if (item.ModItem is null) // do not touch items from other mods
				{
					return FlipFloat(player.GetModPlayer<AOPlayer>().imbue.AOImbueSpeed);
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
		public AOMagic? imbue = null;
		public bool RightClicking => Player.altFunctionUse == 2;

		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
		{
			if (!mediumCoreDeath)
			{
				return [new Item(ModContent.ItemType<HecateOrb>())];
			}
			else return [];
		}
	}

	public class ProjectileImbuer : GlobalProjectile
	{
		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
		{
			if (projectile.owner == Main.myPlayer && projectile.owner != 255)
			{
				AOPlayer playah = Main.player[projectile.owner].GetModPlayer<AOPlayer>();
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
					if ((projectile.ModProjectile is null || ArcaneOdysseyConfig.Instance.AffectsOtherMods) && (extraconfs || projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.MeleeNoSpeed || projectile.DamageType == DamageClass.Ranged))
					{
						if (playah.imbue is not null)
						{
							modifiers.FinalDamage *= playah.imbue.AOImbueDamage;
							if ((playah.imbue.MagicDebuff is not null) && (!(playah.imbue.MagicDebuff.DebuffPercent == 0f)))
							{
								if ((playah.imbue.MagicDebuff.DebuffPercent is null || modifiers.GetDamage(projectile.damage, true) > (target.lifeMax / playah.imbue.MagicDebuff.DebuffPercent)))
								{
									target.AddBuff(playah.imbue.MagicDebuff.debuffID, playah.imbue.MagicDebuff.debuffDuration);
								}
							}
							if ((playah.imbue.MagicDebuff2 is not null) && (!(playah.imbue.MagicDebuff2.DebuffPercent == 0f)))
							{
								if ((playah.imbue.MagicDebuff2.DebuffPercent is null || modifiers.GetDamage(projectile.damage, true) > (target.lifeMax / playah.imbue.MagicDebuff2.DebuffPercent)))
								{
									target.AddBuff(playah.imbue.MagicDebuff2.debuffID, playah.imbue.MagicDebuff2.debuffDuration);
								}
							}
							if (playah.imbue.combinedDebuffs is not null)
							{
								foreach (CombinedDebuff buffkeys in playah.imbue.combinedDebuffs)
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
				}
			}
		}

		public override void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
		{
			if (projectile.owner == Main.myPlayer && projectile.owner != 255)
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
				if (extraconfs || projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.MeleeNoSpeed || projectile.DamageType == DamageClass.Ranged)
				{
					AOMagic? imbue = Main.player[projectile.owner].GetModPlayer<AOPlayer>().imbue;
					if (imbue is not null)
					{
						hitbox.Width = (int)(hitbox.Width * imbue.AOImbueSize);
						hitbox.Height = (int)(hitbox.Height * imbue.AOImbueSize);
					}
				}
			}
		}

        public override void PostDraw(Projectile projectile, Color lightColor)
		{

            if (projectile.owner == Main.myPlayer && projectile.owner != 255)
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
                if (extraconfs || projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.MeleeNoSpeed || projectile.DamageType == DamageClass.Ranged)
                {
                    AOMagic? imbue = Main.player[projectile.owner].GetModPlayer<AOPlayer>().imbue;
                    if (imbue is not null)
                    {
                        projectile.scale = imbue.AOImbueSize;
                    }
                }
            }
        }

		public override void OnSpawn(Projectile projectile, IEntitySource source)
		{
			if (projectile.owner == Main.myPlayer && projectile.owner != 255)
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
				if (extraconfs || projectile.DamageType == DamageClass.Melee || projectile.DamageType == DamageClass.Ranged)
				{
					AOMagic? imbue = Main.player[projectile.owner].GetModPlayer<AOPlayer>().imbue;
					if (imbue is not null)
						projectile.velocity *= imbue.AOImbueSpeed;
				}
			}
		}
	}
}
