using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using ArcaneOdyssey.Content.Items.Materials;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.PlayerClasses
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		public Imbuable Imbue { get; set; }
		public bool chargingSpell = false;
		public int AOSizeStat = 0;
		public Projectile myCircle = null;
		public int timeTillNextMove = 0;
		public List<Cooldown> Cooldowns = [];

		public bool WhirlwindActive = false;
		public bool SoftFrozen => chargingSpell || WhirlwindActive;
		public bool Immobile => Player.CCed || timeTillNextMove > 0;
		public bool CanMoveOnGround;
		public int groundedCounter = 0;
		public bool FirstFrozenFrame => timeSinceSoftFrozen < 1;
		public int timeSinceSoftFrozen;


		public List<ImbueDebuffHelper> DebuffHelpers = [];

		public float MaxRunSpeed => Math.Max(Player.accRunSpeed, Player.maxRunSpeed);

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
						if (npc.boss || !AOUtils.BossAlive())
						{
							Player.ArcaneOdyssey()?.SetCooldown(new Cooldown(vanish.Name, vanish.DisplayName, 60));
							if (npc.boss)
								vanish.BarValue += damagedone / (npc.lifeMax / 10f) * FightingStyleBarred.BarMax;
							else
								vanish.BarValue += damagedone / (npc.lifeMax * 2f) * FightingStyleBarred.BarMax;
						}
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

		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
		{
			if (!mediumCoreDeath)
			{
				List<Item> items = [
						new Item(ModContent.ItemType<StarterPoseidon>()),
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
