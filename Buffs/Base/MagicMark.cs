using Terraria;

namespace ArcaneOdyssey.Buffs.Base
{
	/// <summary>
	/// basic debuff for things like Snowy
	/// </summary>
	public abstract class MagicMark : BaseBuff
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			ExternalModSupport.RegisterDebuff(this);
		}
	}
}
