using ArcaneOdyssey.Buffs.Base;
using Microsoft.Xna.Framework;
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
			allChosenImbues = tag.GetList<string>("allimbues");
			DarkSealed = tag.GetInt("darksealedchests");
			NimbusSealed = tag.GetInt("nimbussealedchests");
			BronzeSealed = tag.GetInt("bronzesealedchests");
			acumen = tag.GetBool("acumenconsumed");
			hasLoadedWorldBefore = tag.GetBool("wowiveloadedinbefore");
		}

		public override void SaveData(TagCompound tag)
		{
			tag.Add("aodisease", bloodDisease ?? "null");
			tag.Add("aomentality", evil);
			tag.Add("allimbues", allChosenImbues);
			tag.Add("darksealedchests", DarkSealed);
			tag.Add("nimbussealedchests", NimbusSealed);
			tag.Add("bronzesealedchests", BronzeSealed);
			tag.Add("acumenconsumed", acumen);
			tag.Add("wowiveloadedinbefore", true);
		}
	}
}
