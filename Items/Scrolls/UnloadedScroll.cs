using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Items.EmptyScrolls;
using ArcaneOdyssey.Spells.Base;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ArcaneOdyssey.Items.Scrolls
{
	public class UnloadedScroll : BaseItem
	{
		public override ItemRarities Rarity => ItemRarities.Unknown;

		public string CachedFullName = ArcaneOdysseyMod.InternalName + "/BlastScroll";

		public override string Texture => AOUtils.GetTexture<EmptyScroll>();

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
