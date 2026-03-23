using ArcaneOdyssey.VFX.Rarities;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Base
{
	public abstract class BaseItem : ModItem, ILocalizedModType
	{
		public override string LocalizationCategory => GetType().Namespace.Replace($"{Mod.Name}.");

		public abstract AORarities AORarity { get; }

		public virtual ItemType? ItemCategory => null;

		public virtual bool ShowItemTypeTooltip => true;

		public Texture2D Sprite => ModContent.Request<Texture2D>(Texture).Value;

		public override void SetDefaults()
		{
			if (AORarity != AORarities.Special)
				Item.rare = (int)AORarity;
			if (AORarity == AORarities.Special)
			{
				Item.rare = ModContent.RarityType<HotPinkRare>();
			}
		}
	}
}
