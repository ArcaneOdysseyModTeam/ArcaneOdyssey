using ArcaneOdyssey.Items.Base;

namespace ArcaneOdyssey.Items.Materials
{
	public class AncientHecateOrb : BaseItem
	{
		public override int Value => 20000;
		public override ItemRarities Rarity => ItemRarities.Mythical;

		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.width = Item.height = 32;
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			ItemID.Sets.ItemIconPulse[Type] = true;
			ItemID.Sets.ItemNoGravity[Type] = true;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			scale *= .8f;
			Lighting.AddLight(Item.Center, new Vector3(2, 0, 2) * scale);
			return true;
		}
	}
}
