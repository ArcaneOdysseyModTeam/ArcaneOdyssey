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
			DarkSealed = tag.GetInt("darksealedchests");
			NimbusSealed = tag.GetInt("nimbussealedchests");
			BronzeSealed = tag.GetInt("bronzesealedchests");
			acumen = tag.GetBool("acumenconsumed");
			hasLoadedWorldBefore = tag.GetBool("wowiveloadedinbefore");
			if (tag.TryGet<List<int>>("godsouls", out var souls) && souls.Count > 1)
			{
				Souls = [.. souls.Select(e => (GodSoulID)e)];
			}
			unlockedPages = tag.Get<List<string>>("guidebooks");
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
			if (AvailablePages().Count > 0)
				tag.Add("guidebooks", unlockedPages);
		}

		public override void PreUpdateBuffs()
		{
			foreach (var page in AvailablePages())
			{
				if (!unlockedPages.Contains(page.Name))
				{
					Main.NewText(Mod.CustomLocalization("NewGuide", page.DisplayName.Value).Value);
				}
			}

			foreach (string str in AvailablePages().Select(e => e.Name))
			{
				if (!unlockedPages.Contains(str))
					unlockedPages.Add(str);
			}
		}

		private static int SortPages(GuidebookPage x, GuidebookPage y)
		{
			if (x.PageNum > y.PageNum)
			{
				return 1;
			}
			if (x.PageNum < y.PageNum)
			{
				return -1;
			}
			return 0;
		}

		internal List<string> unlockedPages = [];

		public List<GuidebookPage> AvailablePages()
		{
			List<GuidebookPage> pages = [.. GuidebookSystem.AllPages];
			pages.Sort(new Comparison<GuidebookPage>(SortPages));
			pages.RemoveAll(e => !e.MetConditions(Player));
			return pages;
		}
	}
}
