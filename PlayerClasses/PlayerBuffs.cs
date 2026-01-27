using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.PlayerClasses
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		public int gel = 0;

		public string bloodDisease = null;

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

		public override void NaturalLifeRegen(ref float regen)
		{
			regen *= 1f + (pheonixHealing / 5f);
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
		}

		public override void SaveData(TagCompound tag)
		{
			tag.Add("aodisease", bloodDisease ?? "null");
			tag.Add("aomentality", evil);
		}
	}
}
