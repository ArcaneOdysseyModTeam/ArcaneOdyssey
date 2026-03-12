using ArcaneOdyssey.Buffs.Base;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Buffs.MagicMarks
{
	public class Crystallized : AODebuff
	{
		private int stack = 1;

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			tip = Mod.CustomLocalization(LocalizationCategory.Replace($"Mods.{Mod.Name}.") + ".Description", [stack]).Value;
		}

		public override List<int> Counterparts => [BuffID.Midas];

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.HasBuff(Type))
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
					case 4: // ArcaneOdyssey.cs damage calculation uses this stack to increase damage
						if (!Main.dedServ)
						{
							Dust.NewDust(npc.Center, 0, 0, DustID.GemRuby, (0.5f - Main.rand.NextFloat()) * 5f, (0.1f - Main.rand.NextFloat()) * 5f, 1, default, 2f);
						}
						break;
					default: // if the stack number isnt valid or over 4, just delete the buff
						npc.DelBuff(buffIndex);
						SoundEngine.PlaySound(SoundID.DeerclopsIceAttack, npc.Center);
						buffIndex--;
						break;
				}
			}
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			if (npc.HasBuff(Type))
			{
				npc.buffTime[buffIndex] += time;
				return true;
			}
			else return false;
		}
	}
}
