using ArcaneOdyssey.Content.Buffs.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Stuns
{
	/// <summary>
	/// nobody will tell its a custom debuff thats the point lol
	/// </summary>
	public abstract class Stun : ModBuff
	{
		/// <summary>
		/// literally just for custom magics
		/// </summary>
		public virtual bool AffectsBosses => false;
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.boss || AffectsBosses)
				npc.velocity /= 2;
		}
		public override void SetStaticDefaults()
		{
			Main.pvpBuff[Type] = true;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.AOPlayer().StunCD <= 0 || AffectsBosses)
			{
				player.moveSpeed = 0f;
				player.AOPlayer().StunCD = 1;
				player.canFloatInWater = false;
			}
		}
	}
}
