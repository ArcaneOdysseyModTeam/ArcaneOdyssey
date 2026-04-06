using System;
using Terraria.ModLoader;

namespace ArcaneOdyssey.AOPlayers
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		private int _insanity = 0;
		/// <summary>
		/// Player insanity level, use += to increase
		/// </summary>
		public int Insanity
		{
			get
			{
				return Math.Clamp(_insanity, 0, 5);
			}
			set
			{
				_insanity = Math.Clamp(value, 0, 5);
			}
		}

		public int BronzeSealed = 0;
		public int DarkSealed = 0;
		public int NimbusSealed = 0;

		public override void UpdateDead()
		{
			BronzeSealed = 0;
			DarkSealed = 0;
			NimbusSealed = 0;
		}

		public uint eliusArenaCounter = 0;
	}
}
