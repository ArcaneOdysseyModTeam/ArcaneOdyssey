using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		public Imbuable Imbue { get; set; }
		public bool chargingSpell = false;
		public int AOSizeStat = 0;
		public Projectile myCircle = null;
		public int timeTillNextMove = 0;
		public List<Cooldown> Cooldowns = [];

		public int? gel = null;
		public bool SoftFrozen => chargingSpell || Player.ownedProjectileCounts[ModContent.ProjectileType<Whirlwind>()] > 0;
		public bool Immobile => Player.CCed || timeTillNextMove > 0;
		public bool CanMoveOnGround;
		public int groundedCounter = 0;
		public bool FirstFrozenFrame => timeSinceSoftFrozen < 1;
		public int timeSinceSoftFrozen;

		public int pheonixHealing;

		public List<ImbueDebuffHelper> DebuffHelpers = [];

		public void UpdateDebuffHelpers(int damagedone, NPC npc, Imbuable imbue = null, bool useplayerimbue = true, bool canAddBuffs = true)
		{
			if (useplayerimbue)
				imbue ??= Imbue;
			if (imbue is not null)
			{
				if (imbue is EnergyMagic)
				{
					Player.statMana = Math.Clamp(Player.statMana + (damagedone / 4), 0, Player.statManaMax2);
				}
				if (imbue is VanishingStyle vanish)
				{
					if (!(npc.CountsAsACritter || npc.friendly || Main.npcCatchable[npc.type]))
					{
						Player.ArcaneOdyssey()?.SetCooldown(new Cooldown(vanish.Name, vanish.DisplayName, 60));
						if (npc.boss)
							vanish.BarValue += damagedone / (npc.lifeMax / 5f) * FightingStyleBarred.BarMax;
						else
							vanish.BarValue += damagedone / (npc.lifeMax * 1f) * FightingStyleBarred.BarMax;
					}
				}
				foreach (var buff in imbue.ImbueDebuffs)
				{
					var instance = DebuffHelpers.Find(e => e.buffID == buff.debuffID && e.imbue.Type == imbue.Type && e.npc.type == npc.type);
					if (DebuffHelpers.Contains(instance))
					{
						int damage = instance.damagedone + damagedone;
						if (canAddBuffs && (float)damage / npc.lifeMax > buff.debuffPercent)
						{
							npc.AddBuff(buff.debuffID, buff.debuffDuration);
							damage = 0;
						}
						DebuffHelpers[DebuffHelpers.IndexOf(instance)] = instance with { damagedone = damage };
					}
					else
					{
						DebuffHelpers.Add(new(imbue, damagedone, npc, buff.debuffID));
					}
				}
			}
		}

		public override void NaturalLifeRegen(ref float regen)
		{
			regen *= 1f + (pheonixHealing / 5f);
		}

		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
		{
			if (!mediumCoreDeath)
			{
				List<Item> items = [
						new Item(ModContent.ItemType<PoseidonChoice>()),
						new Item(ModContent.ItemType<StarterAcrimony>())
					];
				return items;
			}
			return [];
		}

		public override void PostUpdate()
		{
			if (chargingSpell)
				Player.statDefense *= .75f;
			chargingSpell = false;
			DashStrike();
			if (Imbue is not null && !Imbue.PlayerHasImbue(Player))
			{
				Imbue = null;
			}
		}

		public void FreezeMovement()
		{
			if (Player.velocity.Y < 1 && Player.velocity.Y > -1)
			{
				groundedCounter++;
			}
			else
				groundedCounter = 0;
			if (SoftFrozen)
			{
				if (FirstFrozenFrame)
				{
					CanMoveOnGround = groundedCounter > 10;
				}
				if (!CanMoveOnGround)
				{
					Player.gravity = 0f;
					Player.velocity.X *= 0;
					Player.velocity.Y *= 0;
				}
				timeSinceSoftFrozen++;
			}
			else
			{
				timeSinceSoftFrozen = 0;
				CanMoveOnGround = false;
			}
		}

		public override void ResetEffects()
		{
			AOSizeStat = 0;
			pheonixHealing = 0;
			HandleDashDetection();
		}

		public float SizeMulti => AOSizeStat / 275f;
	}
}
