using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ArcaneOdyssey.Content.Buffs.Base;
using Terraria.Audio;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Buffs.MagicMarks
{
	public class Crystallized : AODebuff
	{
		private int stack = 1;

		public override void SetStaticDefaults()
		{
			ImbueDebuffHelper.AlternateBuff[Type] = BuffID.Midas;
		}

		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			tip = Mod.CustomLocalization($"Buffs.{Name}.Description", [stack]).Value;
		}

		public override void Update(NPC npc, ref int buffIndex) 
		{
			if (npc.HasBuff(Type))
			{
				stack = GetAOBuffStack(npc, buffIndex); // stacks disappear over time
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
							Dust.NewDust(npc.Center, 1, 1, DustID.GemRuby, (0.5f - Main.rand.NextFloat()) * 5f, (0.1f - Main.rand.NextFloat()) * 5f, 1, default, 2f);
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
				npc.buffTime[buffIndex] = (stack+1) * 5 * 60; // adds a "stack", or 5 second duration... could use "time", but other mods that change debuff duration might mess that up or something
				return true;
			}
			else return false;
		}
	}
}
