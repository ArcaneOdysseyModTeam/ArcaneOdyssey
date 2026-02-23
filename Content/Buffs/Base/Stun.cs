using ArcaneOdyssey.PlayerClasses;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Buffs.Base
{
	/// <summary>
	/// nobody will tell its a custom debuff thats the point lol
	/// </summary>
	public abstract class Stun : AOBaseBuff
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
			Main.pvpBuff[Type] = true;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
			ExternalModSupport.RegisterDebuff(this);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ArcaneOdyssey().OnCooldown(Name + "Buff") || LiterallyCheating)
			{
				player.moveSpeed = 0f;
				player.ArcaneOdyssey().SetCooldown(new Cooldown(Name + "Buff", DisplayName, 60));
				player.canFloatInWater = false;
			}
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			return !LiterallyCheating;
		}

		public override bool ReApply(Player player, int time, int buffIndex)
		{
			return !LiterallyCheating;
		}
	}
}
