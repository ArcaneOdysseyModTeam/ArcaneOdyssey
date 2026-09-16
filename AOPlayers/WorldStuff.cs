using ArcaneOdyssey.Buffs;
using System;

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

		private byte _banishment = 0;
		public byte Banishment
		{
			get
			{
				return Math.Clamp(_banishment, (byte)0, (byte)5);
			}
			set
			{
				_banishment = Math.Clamp(value, (byte)0, (byte)5);
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

		public void AddInsanityDebuff()
		{
			switch (Insanity)
			{
				case 1:
					Player.AddBuff(ModContent.BuffType<InsanityOne>(), 2);
					break;
				case 2:
					Player.AddBuff(ModContent.BuffType<InsanityTwo>(), 2);
					break;
				case 3:
					Player.AddBuff(ModContent.BuffType<InsanityThree>(), 2);
					break;
				case 4:
					Player.AddBuff(ModContent.BuffType<InsanityFour>(), 2);
					break;
				case 5:
					Player.AddBuff(ModContent.BuffType<InsanityFive>(), 2);
					break;
			}
		}
	}
}
