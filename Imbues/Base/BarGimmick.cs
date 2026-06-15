namespace ArcaneOdyssey.Imbues.Base
{
	/// <summary>
	/// Can only be applied to <see cref="IBarrableImbue"/> or <see cref="FightingStyleBarred"/>
	/// </summary>
	public abstract class BarGimmick : ImbueGimmick
	{
		public abstract float MaxScrollSpeed { get; }
		public abstract float MaxScrollDamage { get; }
		public abstract float MaxScrollSize { get; }
		public abstract float MinScrollSpeed { get; }
		public abstract float MinScrollDamage { get; }
		public abstract float MinScrollSize { get; }

		public abstract float BarValueMulti { get; }

		public virtual bool SaveBar => false;

		public const float BarMax = IBarrableImbue.BarMax;
		public const float BarMin = IBarrableImbue.BarMin;
	}
}
