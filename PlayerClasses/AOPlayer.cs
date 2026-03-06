using ArcaneOdyssey.Content.Items.Armour.RavennaNoble;
using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Consumable;
using ArcaneOdyssey.Content.Items.Imbues.FightingStyles.Normal;
using ArcaneOdyssey.Content.Items.Imbues.Magic.Lost;
using ArcaneOdysseyMusic.MusicBoxes;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.PlayerClasses
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		public Imbuable Imbue { get; set; }
		public int AOSizeStat = 0;
		public Projectile myCircle = null;
		public int timeTillNextMove = 0;
		public List<Cooldown> Cooldowns = [];
		public bool HeavySkillActive = false;
		public bool hasLoadedWorldBefore = false;
		public bool Immobile => Player.CCed || timeTillNextMove > 0;
		public bool CanMoveOnGround;
		public int groundedCounter = 0;
		public bool Grounded => groundedCounter >= 15;
		public bool FirstFrozenFrame => timeSinceSoftFrozen < 1;
		public int timeSinceSoftFrozen;

		/// <summary>
		/// Imbues in equipment slots
		/// </summary>
		public List<int> EquippedImbues = [];
		public List<int> EquippedImbuesTimers = [];

		public void AddEquippedImbue(Item imbue)
		{
			var index = EquippedImbues.IndexOf(imbue.type);
			if (index != -1)
			{
				EquippedImbuesTimers[index] = 3;
			}
			else
			{
				EquippedImbues.Add(imbue.type);
				EquippedImbuesTimers.Add(3);
			}
		}

		public bool evil = false;


		public List<ImbueDebuffHelper> DebuffHelpers = [];

		public float MaxRunSpeed => Math.Max(Player.accRunSpeed, Player.maxRunSpeed);

		public float MaxPossibleSpeed => Math.Max(MaxRunSpeed, CurrentDash?.DashSpeed ?? MaxRunSpeed);

		public override void FrameEffects()
		{
			if (Player.body == EquipLoader.GetEquipSlot(Mod, typeof(EliusChest).Name, EquipType.Body) && Player.back == -1)
			{
				Player.back = EquipLoader.GetEquipSlot(Mod, typeof(EliusChest).Name, EquipType.Back);
			}
		}

		public void UpdateDebuffHelpers(int damagedone, NPC npc, Imbuable imbue = null, bool useplayerimbue = true, bool canAddBuffs = true)
		{
			if (useplayerimbue)
				imbue ??= Imbue;
			if (imbue is not null)
			{
				if (imbue is EnergyMagic)
				{
					Player.statMana = Utils.Clamp(Player.statMana + (damagedone / 4), 0, Player.statManaMax2);
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


		internal IList<string> allChosenImbues = [];

		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
		{
			if (!mediumCoreDeath)
			{
				List<Item> items = [
						new Item(ModContent.ItemType<EagleLegacy>()),
						new Item(ModContent.ItemType<TitleMusicBox>())
					];
				return items;
			}
			return [];
		}

		public void TrySpiritLifesteal(int damage, bool cooldown = true)
		{
			if (!(cooldown && OnCooldown("SpiritLifesteal")))
			{
				if (cooldown)
					SetCooldown(new Cooldown("SpiritLifesteal", Mod, 60 * 2));
				Player.Heal(Utils.Clamp(damage / 5, 1, 20));
			}
		}

		public override void PostUpdate()
		{
			if (!hasLoadedWorldBefore)
			{
				hasLoadedWorldBefore = true;
				if (Main.myPlayer == Player.whoAmI && AOUtils.BossesKilled < 1)
				{
					if (!Player.HasTypeInInventory<EagleLegacy>())
					{
						Item.NewItem(Player.GetSource_FromThis(), Player.Hitbox, ModContent.ItemType<EagleLegacy>(), noBroadcast: true, noGrabDelay: true);
					}
					if (!Player.HasTypeInInventory<TitleMusicBox>())
					{
						Item.NewItem(Player.GetSource_FromThis(), Player.Hitbox, ModContent.ItemType<TitleMusicBox>(), noBroadcast: true, noGrabDelay: true);
					}
				} 
			}
			pheonixHealing = 0;
			HeavySkillActive = false;
			DashStrike();
			if (Imbue is not null && !Imbue.PlayerHasImbue(Player))
			{
				Imbue = null;
			}
			Player.statDefense -= _defenseLost;
		}

		public void FreezeMovement()
		{
			if (Math.Abs(Player.velocity.Y) < 1f && Player.wingTime == Player.wingTimeMax && !Player.controlJump)
			{
				if (groundedCounter < 60)
					groundedCounter++;
			}
			else
				groundedCounter = 0;
			if (HeavySkillActive)
			{
				if (FirstFrozenFrame)
				{
					CanMoveOnGround = Grounded;
				}
				if (!CanMoveOnGround)
				{
					Player.gravity = 0f;
					Player.velocity.X *= .001f;
					Player.velocity.Y *= .001f;
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
			AOHasteStat = 0;
			Insanity = 0;
			Gel = null;
			List<int> queue = [];
			foreach (int type in EquippedImbues)
			{
				var index = EquippedImbues.IndexOf(type);
				if (index >= 0)
				{
					if (EquippedImbuesTimers[index] <= 0)
					{
						queue.Add(index);
					}
					else
					{
						EquippedImbuesTimers[index]--;
					}
				}
			}
			foreach (var i in queue)
			{
				EquippedImbues.RemoveAt(i);
				EquippedImbuesTimers.RemoveAt(i);
			}
			HandleDashDetection();
		}

		public float SizeMulti => 1f + (AOSizeStat / 275f);
		public float CooldownDurationMulti => (1f + (AOHasteStat / 200f)).FlipFloat();
	}
}
