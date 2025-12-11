using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class AnyScroll : AOBaseItem
	{
		public virtual int AOValue => 100;
		public override AORarities AORarity => AORarities.Uncommon;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = 32;
			Item.height = 32;
			Item.noMelee = true;
			Item.knockBack = 4.5f;
			Item.noUseGraphic = true;
			Item.useStyle = ItemUseStyleID.Rapier;
			Item.value = AOUtils.GalleonToCopper(AOValue);
		}

		public override void UpdateInventory(Player player)
		{
			if (Item.TryGetImbue(out var imbue))
			{
				Item.color = imbue.GetColor() with { A = (byte)(255 * .75f) };
			}
			else Item.color = Color.Transparent;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (player.TryGetImbue(out Imbuable imbue) && Item.CanHaveImbue(imbue))
			{
				Item.ArcaneOdyssey().Imbue = imbue;
				Item.color = imbue.GetColor() with { A = (byte)(255 * .75f) };
			}
			else
			{
				Item.color = Color.Transparent;
				Item.ArcaneOdyssey().Imbue = null;
			}
		}

		public override bool CanUseItem(Player player)
		{
			return Item.ArcaneOdyssey().Imbue is not null;
		}
	}
}
