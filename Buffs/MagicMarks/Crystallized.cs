using ArcaneOdyssey.Buffs.Base;
using ArcaneOdyssey.Imbues.Magic.Normal;
using System.Collections.Generic;
using System.Linq;
using Terraria.Audio;
using Terraria.DataStructures;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class Crystallized : MagicMark
	{
		private int stack = 1;

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			tip = Mod.CustomLocalization(LocalizationCategory.Replace($"Mods.{Mod.Name}.") + ".Description", [stack]).Value;
		}

		public override List<int> Counterparts => [BuffID.Midas];

		public override void Update(NPC npc, ref int buffIndex)
		{
			stack = AOUtils.GetAOBuffStack(npc, buffIndex); // stacks disappear over time
			switch (stack)
			{
				case 1:
					return;
				case 2:
					return;
				case 3:
					return;
				case 4:
					if (!Main.dedServ)
					{
						Dust.NewDust(npc.Center, 0, 0, dust, (0.5f - Main.rand.NextFloat()) * 5f, (0.1f - Main.rand.NextFloat()) * 5f, 1, dustColour, 2f);
					}
					break;
				default: // if the stack number isnt valid or over 4, just delete the buff
					npc.DelBuff(buffIndex);
					SoundEngine.PlaySound(SoundID.DeerclopsIceAttack, npc.Center);
					buffIndex--;
					break;
			}
		}

		public override void Update(Player player, ref int buffIndex)
		{
			stack = AOUtils.GetAOBuffStack(player, buffIndex); // stacks disappear over time
			switch (stack)
			{
				case 1:
					return;
				case 2:
					return;
				case 3:
					return;
				case 4:
					if (!Main.dedServ)
					{
						Dust.NewDust(player.Center, 0, 0, DustID.GemRuby, (0.5f - Main.rand.NextFloat()) * 5f, (0.1f - Main.rand.NextFloat()) * 5f, 1, default, 2f);
					}
					break;
				default: // if the stack number isnt valid or over 4, just delete the buff
					player.DelBuff(buffIndex);
					SoundEngine.PlaySound(SoundID.DeerclopsIceAttack, player.Center);
					buffIndex--;
					break;
			}
		}

		public int dust = DustID.GemRuby;
		public Color dustColour = default;
		public Color drawColour = Color.Red;

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			npc.buffTime[buffIndex] += time;
			dustColour = default;
			dust = DustID.GemRuby;
			if (Main.netMode == NetmodeID.SinglePlayer)
			{
				if (Main.LocalPlayer.HasTypeInInventory<CrystalMagic>(out var imbue))
				{
					CrystalMagic.GetVFX(imbue, out dustColour, out drawColour, out dust);
				}
			}
			else
			{
				var players = Main.player.Where(e => e.active).OrderBy(e => e.Distance(npc.Center));
				foreach (var player in players)
				{
					if (player is not null && player.HasTypeInInventory<CrystalMagic>(out var imbue))
					{
						CrystalMagic.GetVFX(imbue, out dustColour, out drawColour, out dust);
						break;
					}
				}
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
			if (AOUtils.RequestIfExists(Texture + "_Glow", ref GlowTexture))
			{
				if (GlowTexture.IsLoaded)
				{
					drawParams.DrawColor = drawParams.DrawColor.MultiplyRGBA(drawColour);
					drawParams.Texture = GlowTexture.Value;
				}
			}
			spriteBatch.Draw(drawParams.Texture, drawParams.MouseRectangle, drawParams.SourceRectangle, drawParams.DrawColor);
		}
	}
}
