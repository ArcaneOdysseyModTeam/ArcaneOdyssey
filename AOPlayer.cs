using ArcaneOdyssey.Content.Items.Base;
using ArcaneOdyssey.Content.Items.Equipment.MusicBoxes;
using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons;
using ArcaneOdyssey.Content.Projectiles.Weapons.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey
{
	public partial class AOPlayer : ModPlayer
	{
		public Imbuable imbue = null;
		public bool chargingSpell = false;
		public int AOSizeStat = 0;
		public Projectile myCircle = null;
		public int timeTillNextMove = 0;
		public Dictionary<string, int> Cooldowns = [];
		public Dictionary<int, int> BuffCooldowns = [];
		public Dictionary<int, int> ItemCooldowns = [];

		public bool CompletelyFrozen => chargingSpell || timeTillNextMove > 0 || Player.ownedProjectileCounts[ModContent.ProjectileType<Whirlwind>()] > 0;
		public bool Immobile => CompletelyFrozen || Player.CCed;

		public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
		{
			if (!mediumCoreDeath)
			{
				List<Item> items = [
					new Item(ModContent.ItemType<PoseidonChoice>()),
					new Item(ModContent.ItemType<TitleMusicBox>()),
					new Item(ModContent.ItemType<EaglePatrimony>())];
				if (Main.expertMode)
				{
					items.Add(new Item(ModContent.ItemType<Acrimony>()));
				}
				return items;
			}
			else return [];
		}

		public override void PostUpdate()
		{
			if (chargingSpell)
				Player.statDefense *= .75f;
			chargingSpell = false;
			DashStrike();
		}

		public override void ResetEffects()
		{
			AOSizeStat = 0;
			HandleDashing();
		}

		public float GetSizeMulti(Item item = null)
		{
			float stat = AOSizeStat / 300f;
			if (item is not null && Player.meleeScaleGlove && item.DamageType.Name.Contains("Melee"))
			{
				stat += .1f;
			}
			stat++;
			return stat;
		}

		public float GetSizeMulti(Projectile projectile)
		{
			float stat = AOSizeStat / 300f;
			if (Player.meleeScaleGlove && projectile.DamageType.Name.Contains("Melee"))
			{
				stat += .1f;
			}
			stat++;
			return stat;
		}

		public override void PreUpdate()
		{
			if (timeTillNextMove > 1)
			{
				for (int i = 0; i < 4; i++)
					Player.doubleTapCardinalTimer[i] = 0;
				timeTillNextMove--;
			}
			else timeTillNextMove = 0;
			foreach (string i in Cooldowns.Keys)
			{
				Cooldowns[i]--;
				if (Cooldowns[i] <= 0)
				{
					Cooldowns.Remove(i);
				}
			}

			foreach (int i in BuffCooldowns.Keys)
			{
				BuffCooldowns[i]--;
				if (BuffCooldowns[i] <= 0)
				{
					BuffCooldowns.Remove(i);
				}
			}

			foreach (int i in ItemCooldowns.Keys)
			{
				ItemCooldowns[i]--;
				if (ItemCooldowns[i] <= 0)
				{
					ItemCooldowns.Remove(i);
				}
			}
		}
	}
}
