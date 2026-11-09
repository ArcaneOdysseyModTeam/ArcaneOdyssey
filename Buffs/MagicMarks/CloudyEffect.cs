using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues;
using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Imbues.Magic.Lost;
using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;


namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class CloudyEffect : MagicMark
	{
		public override List<int> Counterparts => [BuffID.Confused];
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var spawned = Dust.NewDustDirect(npc.position, npc.width, npc.height, dust, newColor: dustColour, Scale: 2f);
				spawned.velocity *= 0.1f;
				spawned.noGravity = true;
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
			{
				if (npc.HasBuff(BuffID.Confused))
				{
					npc.DelBuff(buffIndex);
					buffIndex--;
				}
			}
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (!Main.dedServ)
			{
				var dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.BubbleBurst_White, Scale: 2f);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
			}

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

			if (player.HasBuff(BuffID.Confused))
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}

		public int dust = DustID.Cloud;
		public Color dustColour = default;
		public Color drawColour = default;

		public void GetVFXInfo(NPC npc)
		{
			foreach (var imbueID in ImbueID.Sets.ImbuesThatCreateBuff[Type])
			{
				if (AOUtils.Singleplayer)
				{
					if (Main.LocalPlayer.HasTypeInInventory<MagicType>(out var imbue, e => e.ID == imbueID && e.magicVariant is not null))
					{
						imbue.magicVariant.BuffVisuals(ref dust, ref drawColour, ref dustColour);
					}
				}
				else
				{
					var players = Main.player.Where(e => e is not null && e.active).OrderBy(e => e.Distance(npc.Center));
					foreach (var player in players)
					{
						if (player.HasTypeInInventory<MagicType>(out var imbue, e => e.ID == imbueID && e.magicVariant is not null))
						{
							imbue.magicVariant.BuffVisuals(ref dust, ref drawColour, ref dustColour);
							break;
						}
					}
				}
			}
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			npc.buffTime[buffIndex] += time;
			if (!Main.dedServ)
			{
				dustColour = default;
				dust = DustID.Cloud;
				GetVFXInfo(npc);
			}
			return true;
		}

		public override bool ReApply(Player player, int time, int buffIndex)
		{
			player.buffTime[buffIndex] += time;
			return true;
		}

		public static Asset<Texture2D> GlowTexture;

		public override void PostDraw(SpriteBatch spriteBatch, int buffIndex, BuffDrawParams drawParams)
		{
			if (drawColour != default && AOUtils.RequestIfExists(Texture + "_Glow", ref GlowTexture))
			{
				if (GlowTexture.IsLoaded)
				{
					drawParams.DrawColor = drawParams.DrawColor.MultiplyRGBA(drawColour);
					drawParams.Texture = GlowTexture.Value;
					spriteBatch.Draw(drawParams.Texture, drawParams.MouseRectangle, drawParams.SourceRectangle, drawParams.DrawColor);
				}
			}
		}
	}
}
