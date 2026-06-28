using ArcaneOdyssey.Items.Base;
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
			if (ModContent.TryFind<Scroll>(name, out var scroll))
			{
				Item.SetDefaults(scroll.Type);
			}
			else
			{
				CachedFullName = name;
			}
		}

		public override bool CanStack(Item source) => (source.ModItem as UnloadedScroll).CachedFullName == CachedFullName;

		public override bool CanStackInWorld(Item source) => CanStack(source);
	}
}
