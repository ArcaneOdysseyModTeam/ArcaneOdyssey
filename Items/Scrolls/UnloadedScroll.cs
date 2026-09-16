using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Skills.Base;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Items.Scrolls
{
	public sealed class UnloadedScroll : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Junk;

		public string CachedFullName { get; internal set; } = ArcaneOdysseyMod.InternalName + "/StrikeSkill";

		public override string Texture => "ModLoader/UnloadedItem";

		public override void NetSend(BinaryWriter writer)
		{
			base.NetSend(writer);
			writer.Write(CachedFullName);
		}

		public override void NetReceive(BinaryReader reader)
		{
			base.NetReceive(reader);
			CachedFullName = reader.ReadString();
		}

		public override void SaveData(TagCompound tag)
		{
			base.SaveData(tag);
			tag.Add("name", CachedFullName);
		}

		public override void LoadData(TagCompound tag)
		{
			base.LoadData(tag);
			var name = tag.GetString("name");
			if (ModContent.TryFind<ModSkill>(name, out var skill))
			{
				Item.SetDefaults(skill.Scroll);
			}
			else
			{
				CachedFullName = name;
			}
		}

		public override bool CanStack(Item source) => (source.ModItem as UnloadedScroll).CachedFullName == CachedFullName;

		public override bool CanStackInWorld(Item source) => CanStack(source);

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			tooltips.AddTooltip(new(Mod, "InternalSkillName", CachedFullName));
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ArcaneOdysseyMod.Sets.showItemTypeTooltip[Type] = false;
		}
	}
}
