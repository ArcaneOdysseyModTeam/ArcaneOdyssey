using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Base
{
	public abstract class Scroll : AOBaseItem, IImbuable, ILocalizedModType
	{
		public override string LocalizationCategory => "Scrolls";
		public Imbuable Imbue
		{
			get
			{
				return Item?.ArcaneOdyssey()?.Imbue;
			}
			set
			{
				if (Item?.ArcaneOdyssey() is not null)
				{
					Item.ArcaneOdyssey().Imbue = value;
				}
			}
		}

		public Imbuable SecondImbue
		{
			get
			{
				return Item?.ArcaneOdyssey()?.SecondImbue;
			}
			set
			{
				if (Item?.ArcaneOdyssey() is not null)
				{
					Item.ArcaneOdyssey().SecondImbue = value;
				}
			}
		}

		public virtual bool CanHaveMagic => false;
		public virtual bool CanHaveRelic => false;
		public virtual bool CanHaveFS => false;

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
			if (Imbue is not null)
			{
				Item.color = Imbue.GetColor(Color.Transparent) with { A = (byte)(Imbue.GetColor(Color.Transparent).A * .75f) };
			}
			else Item.color = Color.Transparent;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Imbue = player.Imbue();
			SecondImbue = Imbue?.Imbue;
			if (Imbue is not null && Item.CanHaveImbue(Imbue))
			{
				Item.color = Imbue.GetColor(Color.Transparent) with { A = (byte)(Imbue.GetColor(Color.Transparent).A * .75f) };
			}
			else Item.color = Color.Transparent;
		}

		public override bool CanUseItem(Player player) => Imbue is not null;
	}
}
