using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using ArcaneOdyssey.Content.Buffs.Base;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace ArcaneOdyssey.Content.Buffs.DOT
{
	public class AOBleed : AODebuff
	{
		private int frameNum = 0;
		public override void Update(NPC npc, ref int buffIndex) 
		{
			frameNum++;
			if (frameNum > 20)
			{
				frameNum = 0;
				for (int dustCountInt = 0; dustCountInt < 10; dustCountInt++)
				{
					Dust.NewDust(npc.position + new Vector2(npc.width / 2f, npc.height / 2f), 1, 1, DustID.Blood, Alpha: 1);
				}
			}
			npc.ArcaneOdyssey().Bleeding = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, int buffIndex, ref BuffDrawParams drawParams)
		{
			drawParams.Texture = TextureAssets.Buff[BuffID.Bleeding].Value;
			return true;
		}
	}
}
