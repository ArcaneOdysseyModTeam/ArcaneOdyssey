using Terraria.ModLoader;

namespace ArcaneOdyssey.PlayerClasses
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		/// <summary>
		/// Player insanity level, use += to increase
		/// <para>Defaults to null in normal gameplay, set to 0 in dark sea range 1</para>
		/// </summary>
		public int? insanity = null;

		public int BronzeSealed = 0;
		public int DarkSealed = 0;
		public int NimbusSealed = 0;

		public override void UpdateDead()
		{
			BronzeSealed = 0;
			DarkSealed = 0;
			NimbusSealed = 0;
		}
	}
}
