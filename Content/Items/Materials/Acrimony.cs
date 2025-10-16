using ArcaneOdyssey.Content.Items.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Materials
{
	[LegacyName("DarkSeaMusicBox")] // removed items are added here
	public class Acrimony : AOBaseItem
	{
		public int AOValue = 10000;

		public override ItemType ItemType => ItemType.Material;
		public override AORarities AORarity => AORarities.Arcane;
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.value = GalleonToCopper(AOValue);
			Item.width = Item.height = 64;
		}

		public override void SetStaticDefaults()
		{
			ItemID.Sets.CanGetPrefixes[Type] = false;
		}

		public override void AddRecipes()
		{
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Lighting.AddLight(Item.Center,3,3,3);
			Texture2D texture = TextureAssets.Item[Item.type].Value;
			Main.EntitySpriteDraw(texture,Item.Center - Main.screenPosition,new Rectangle(0,0,Item.width,Item.height),Color.White,0f,Vector2.Zero,0.5f,SpriteEffects.None);
			return false;
		}
	}
}
