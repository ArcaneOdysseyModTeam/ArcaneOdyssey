using ArcaneOdyssey.Items.Debug;
using ArcaneOdyssey.Rarities;
using System.Collections.Generic;
using Terraria.GameContent;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class BaseItem : ModItem, ILocalizedModType
	{
		public sealed override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.");

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

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			if (Main.LocalPlayer.HasTypeInInventory<TesterGoggles>())
				tooltips.AddTooltip(new(Mod, "DebugValue", nameof(Value) + " " + Value));
		}

		public virtual int Value => 0;
	}
}
