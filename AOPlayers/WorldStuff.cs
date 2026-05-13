using System;
using Terraria.ModLoader;

namespace ArcaneOdyssey.AOPlayers
{
	public partial class AOPlayer : ModPlayer, IImbuable
	{
		private byte _insanity = 0;
		/// <summary>
		/// Player insanity level, use += to increase
		/// </summary>
		public byte Insanity
		{
			get
			{
				return Math.Clamp(_insanity, (byte)0, (byte)5);
			}
			set
			{
				_insanity = Math.Clamp(value, (byte)0, (byte)5);
			}
		}

		public byte BronzeSealed = 0;
		public byte DarkSealed = 0;
		public byte NimbusSealed = 0;

		public override void UpdateDead()
		{
			BronzeSealed = 0;
			DarkSealed = 0;
			NimbusSealed = 0;
		}

		public ushort eliusArenaCounter = 0;
	}
}
