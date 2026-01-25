using ArcaneOdyssey.Content.Items.Materials;
using ArcaneOdyssey.Content.Items.Weapons.Scrolls;
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
			if (HasCorrectImbue)
			{
				Item.color = Color.Lerp(Color.Transparent, Imbue.GetColour(Color.Transparent), .75f);
			}
			else Item.color = Color.Transparent;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			Item.DamageType = Item.DamageType.UnImbued(Item);
			if (Item.CanHaveImbue(player.Imbue()))
			{
				Imbue = player.Imbue();
			}
			else
			{
				Imbue = null;
			}
			SecondImbue = Imbue?.Imbue;
			if (HasCorrectImbue)
			{
				Item.color = Color.Lerp(Color.Transparent, Imbue.GetColour(Color.Transparent), .75f);
			}
			else Item.color = Color.Transparent;
			Item.DamageType = Item.DamageType.Imbued(Imbue, Item);
		}

		public override bool CanUseItem(Player player) => Imbue is not null;

		public void AddRecipe(params int[] ingredients)
		{
			var rec = CreateRecipe().AddIngredient<EmptyScroll>();
			foreach (var i in ingredients)
			{
				rec.AddIngredient(i);
			}
			rec.Register();
		}

		public bool HasCorrectImbue => Item.CanHaveImbue(Imbue);
	}
}
