using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class BlindedEffect : MagicMark
	{
		public override List<int> Counterparts => [BuffID.Confused];
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.GolfPaticle, newColor: dustColour);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}

			var stack = AOUtils.GetAOBuffStack(npc, buffIndex); // stacks disappear over time
			switch (stack)
			{
				case 1:
					return;
				case 2:
					return;
				case 3:
					return;
				case 4:
					return;
				default:
					npc.AddBuff(BuffID.Confused, 60);
					break;
			}

			if (AOUtils.ServerOrSingleplayer)
				if (npc.HasBuff(BuffID.Confused))
			{
				npc.DelBuff(buffIndex);
				buffIndex--;
			}
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			if (!Main.dedServ)
			{
				dustColour = default;
				if (Main.netMode == NetmodeID.SinglePlayer)
				{
					if (Main.LocalPlayer.HasTypeInInventory<LightMagic>(out var imbue))
					{
						dustColour = imbue.Colour;
					}
				}
				else
				{
					var players = Main.player.Where(e => e is not null && e.active).OrderBy(e => e.Distance(npc.Center));
					foreach (var player in players)
					{
						if (player.HasTypeInInventory<LightMagic>(out var imbue))
						{
							dustColour = imbue.Colour;
							break;
						}
					}
				}
			}
			npc.buffTime[buffIndex] += time;
			return true;
		}

		public override bool ReApply(Player player, int time, int buffIndex)
		{
			player.buffTime[buffIndex] += time;
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.GolfPaticle, newColor: dustColour);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}
			if (player.HasBuff(Type))
			{
				var stack = AOUtils.GetAOBuffStack(player, buffIndex); // stacks disappear over time
				switch (stack)
				{
					case 1:
						return;
					case 2:
						return;
					case 3:
						return;
					case 4:
						return;
					default:
						player.AddBuff(BuffID.Confused, 60);
						break;
				}
			}
			if (player.HasBuff(BuffID.Confused))
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}

		public Color dustColour = new(255, 255, 0);

		public static Asset<Texture2D> GlowTexture;

		public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
		{
			if (dustColour != new Color(255, 255, 0) && AOUtils.RequestIfExists(Texture + "_Glow", ref GlowTexture)) 
			{
				if (GlowTexture.IsLoaded)
				{
					drawParams.DrawColor = drawParams.DrawColor.MultiplyRGBA(dustColour);
					drawParams.Texture = GlowTexture.Value;
					spriteBatch.Draw(drawParams.Texture, drawParams.MouseRectangle, drawParams.SourceRectangle, drawParams.DrawColor);
				}
			}
		}
	}
}
