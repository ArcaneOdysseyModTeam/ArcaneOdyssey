using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Guidebook;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.AOPlayers
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		private int _defenseLost = 0;

		public void LowerDefense(int defense, Rectangle? location = null)
		{
			_defenseLost += defense;
			if (location.HasValue)
				CombatText.NewText(location.Value, Color.Gray, -defense, true);
		}

		public GelBuff Gel { get; set; } = null;
		public int GelDebuff => Gel?.DebuffID ?? 0;

		public string bloodDisease = null;

		public bool acumen = false;

		public int ZapCD = 5 * 50; // ancient lightning chain

		public int BloodDisease
		{
			get
			{
				if (bloodDisease is not null)
				{
					if (bloodDisease.Split('.')[0] != "Terraria")
					{
						if (ModContent.TryFind<ModBuff>(bloodDisease.Split('.')[0], bloodDisease.Split('.')[1], out var buff))
							return buff.Type;
					}
					else
					{
						if (BuffID.Search.TryGetId(bloodDisease.Split(".")[1], out var id))
							return id;
					}
				}
				return 0;
			}
		}

		public string BloodDiseaseName
		{
			get
			{
				if (bloodDisease is not null)
				{
					if (bloodDisease.Split('.')[0] != "Terraria")
					{
						if (ModContent.TryFind<ModBuff>(bloodDisease.Split('.')[0], bloodDisease.Split('.')[1], out var buff))
							return buff.DisplayName.Value;
					}
					else
					{
						if (BuffID.Search.TryGetId(bloodDisease.Split(".")[1], out var id))
							return Lang.GetBuffName(id);
					}
				}
				return Mod.CustomLocalization("RandomWords.None").Value;
			}
		}

		public int pheonixHealing;

		public override void UpdateLifeRegen()
		{
			Player.lifeRegen += pheonixHealing * 7;
		}

		public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
		{
			base.ModifyMaxStats(out health, out mana);
			if (BloodDisease != 0)
				health *= 2f / 3f;
		}

		public override void LoadData(TagCompound tag)
		{
			if (tag.TryGet<string>("aodisease", out var Disease) && Disease != "null")
				bloodDisease = Disease;
			else
				bloodDisease = null;
			evil = tag.GetBool("aomentality");
			DarkSealed = tag.GetByte("darksealedchests");
			NimbusSealed = tag.GetByte("nimbussealedchests");
			BronzeSealed = tag.GetByte("bronzesealedchests");
			acumen = tag.GetBool("acumenconsumed");
			hasLoadedWorldBefore = tag.GetBool("wowiveloadedinbefore");
			if (tag.TryGet<List<int>>("godsouls", out var souls) && souls.Count > 1)
			{
				Souls = [.. souls.Select(e => (GodSoulID)e)];
			}

			unlockedPages = [];
			foreach (string pagename in tag.GetList<string>("guidebooks"))
			{
				var split = pagename.Split(' ');
				if (split.Length > 1)
				{
					if (ModContent.TryFind<GuidebookPage>(split[0], split[1], out var page))
						unlockedPages.Add(page.FullName);
				}
				else
				{
					if (pagename.Contains('/'))
					{
						if (ModContent.TryFind<GuidebookPage>(pagename, out var page))
						{
							unlockedPages.Add(page.FullName);
						}
					}
					else if (Mod.TryFind<GuidebookPage>(pagename, out var page))
					{
						unlockedPages.Add(page.FullName);
					}
				}
			}
		}

		public override void SaveData(TagCompound tag)
		{
			tag.Add("wowiveloadedinbefore", true);
			if (bloodDisease is not null)
				tag.Add("aodisease", bloodDisease);
			if (evil)
				tag.Add("aomentality", true);
			if (DarkSealed > 0)
				tag.Add("darksealedchests", DarkSealed);
			if (NimbusSealed > 0)
				tag.Add("nimbussealedchests", NimbusSealed);
			if (BronzeSealed > 0)
				tag.Add("bronzesealedchests", BronzeSealed);
			if (acumen)
				tag.Add("acumenconsumed", acumen);
			if (Souls.Count > 1)
				tag.Add("godsouls", Souls.Select(e => (int)e).ToList());
			if (unlockedPages.Count > 0)
				tag.Add("guidebooks", unlockedPages);
		}


		public bool bleeding = false;
		public bool elecToxins = false;
		public bool ionized = false;
		public bool scalded = false;
		public int singe = 0;
		public bool seared = false;
		public bool vesburn = false;
		public bool oiled = false;

		public override void UpdateBadLifeRegen()
		{
			void subtract(int num)
			{
				Player.lifeRegen = Math.Min(Player.lifeRegen - num, -num);
			}

			if (bleeding)
			{
				subtract(6);
			}

			if (elecToxins)
			{
				subtract(20);
			}

			if (ionized)
			{
				subtract(50);
			}

			if (scalded)
			{
				subtract(10);
			}

			if (singe > 0)
			{
				subtract(6 * singe);
			}

			if (seared)
			{
				subtract(10);
			}
			
			if (vesburn)
			{
				subtract(60);
			}

			// keep at bottom!
			if (oiled && (Player.lifeRegen < 0))
			{
				subtract(10);
			}
		}

		public void ResetBuffs()
		{
			bleeding = false;
			Gel = null;
			elecToxins = false;
			ionized = false;
			scalded = false;
			singe = 0;
			seared = false;
			vesburn = false;
			oiled = false;
		}
	}
}
