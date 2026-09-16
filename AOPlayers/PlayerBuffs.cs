using ArcaneOdyssey.Buffs;
using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.GodSouls;
using ArcaneOdyssey.Guidebook;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;
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
					if (bloodDisease.Split('/')[0] != "Terraria")
					{
						if (ModContent.TryFind<ModBuff>(bloodDisease, out var buff))
							return buff.Type;
					}
					else
					{
						if (BuffID.Search.TryGetId(bloodDisease.Split('/')[1], out var id))
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
					if (bloodDisease.Split('/')[0] != "Terraria")
					{
						if (ModContent.TryFind<ModBuff>(bloodDisease, out var buff))
							return buff.DisplayName.Value;
					}
					else
					{
						if (BuffID.Search.TryGetId(bloodDisease.Split('/')[1], out var id))
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
			if (tag.TryGet<string>("aodisease", out var Disease))
				bloodDisease = Disease;
			else
				bloodDisease = null;

			DarkSealed = tag.GetByte("darkchests");
			NimbusSealed = tag.GetByte("nimbuschests");
			BronzeSealed = tag.GetByte("bronzechests");

			acumen = tag.GetBool("acumenconsumed");
			hasLoadedWorldBefore = tag.GetBool("wowiveloadedinbefore");

			GodSoul GetSoul(string name)
			{
				if (ModContent.TryFind<GodSoul>(name, out var soul) && soul is not NoneSoul)
				{
					return soul;
				}
				cachedUnloadedSouls.Add(name);
				return null;
			}

			Souls = [GodSoul.None, .. tag.GetList<string>("souls").Select(GetSoul)];
			Souls.RemoveAll(e => e is null);

			foreach (var pagename in tag.GetList<string>("guidebooks"))
			{
				if (ModContent.TryFind<GuidebookPage>(pagename, out var page))
				{
					unlockedPages.Add(page.FullName);
				}
				else
				{
					unlockedPages.Add(pagename);
				}
			}
		}

		public override void SaveData(TagCompound tag)
		{
			tag.Add("wowiveloadedinbefore", true);
			if (bloodDisease is not null)
				tag.Add("aodisease", bloodDisease);
			if (DarkSealed > 0)
				tag.Add("darkchests", DarkSealed);
			if (NimbusSealed > 0)
				tag.Add("nimbuschests", NimbusSealed);
			if (BronzeSealed > 0)
				tag.Add("bronzechests", BronzeSealed);
			if (acumen)
				tag.Add("acumenconsumed", acumen);
			if (Souls.Count > 1)
				tag.Add("souls", Souls.Select(e => e.FullName).ToList());
			if (unlockedPages.Count > 0)
				tag.Add("guidebooks", unlockedPages);
		}

		public bool oiled = false;

		public List<int> debuffs = [];

		public override void UpdateBadLifeRegen()
		{
			void subtract(int num)
			{
				Player.lifeRegen = Math.Min(Player.lifeRegen - num, -num);
			}

			foreach (var debuff in debuffs)
			{
				subtract(debuff);
			}

			// keep at bottom!
			if (oiled && (Player.lifeRegen < 0))
			{
				subtract(10);
			}
		}

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genDust, ref PlayerDeathReason damageSource)
		{
			if ((hitDirection == 0) && (Player.HasBuff<InsanityFour>() || Player.HasBuff<InsanityFive>()))
			{
				damageSource.CustomReason ??= Mod.CustomLocalization("Insanity.Death" + Main.rand.Next(5), Player.name).ToNetworkText();
			}
			return true;
		}

		public void ResetBuffs()
		{
			Gel = null;
			oiled = false;
			debuffs.Clear();
		}
	}
}
