using Terraria;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs.Base
{
	/// <summary>
	/// nobody will tell its a custom debuff thats the point lol
	/// </summary>
	public abstract class Stun : BaseBuff
	{
		/// <summary>
		/// literally just for custom magics
		/// </summary>
		public virtual bool LiterallyCheating => false;
		public override void Update(NPC npc, ref int buffIndex)
		{
			if (!npc.boss && npc.ArcaneOdyssey().StunCD <= 0 || LiterallyCheating)
			{
				npc.ArcaneOdyssey().AOStunned = true;
			}
		}

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.pvpBuff[Type] = true;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
			ExternalModSupport.RegisterDebuff(this);
			ExternalModSupport.RegisterStatusBuff(Type);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.SetCCed();
		}

		public override bool ReApply(Player player, int time, int buffIndex) => !LiterallyCheating;
	}
}
