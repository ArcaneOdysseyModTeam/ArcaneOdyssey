using ArcaneOdyssey.Buffs.Base;
using Terraria;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Buffs
{
	public class Enchanted : BaseBuff
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.persistentBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetDamage(DamageClass.Generic) += .05f;
			player.ArcaneOdyssey().StatSize += 15;
			player.statLifeMax2 += 50;
		}
	}
}
