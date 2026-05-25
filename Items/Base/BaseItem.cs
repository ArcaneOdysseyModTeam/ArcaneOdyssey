using ArcaneOdyssey.Rarities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class BaseItem : ModItem, ILocalizedModType
	{
		public override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.");

		public abstract ItemRarities Rarity { get; }

		public virtual ItemType? ItemCategory => null;

		public virtual Texture2D Sprite => (Texture != $"{Mod.Name}/{TextureAssets.Item[Type]?.Name.Replace("\\", "/") ?? Texture}" ? ModContent.Request<Texture2D>(Texture) : TextureAssets.Item[Type])?.Value;

		public override void SetDefaults()
		{
			if (Rarity != ItemRarities.Special)
				Item.rare = (int)Rarity;
			if (Rarity == ItemRarities.Special)
			{
				Item.rare = ModContent.RarityType<HotPinkRare>();
			}
			Item.value = AOUtils.GalleonToCopper(Value);
		}

		public virtual int Value => 0;
	}
}
