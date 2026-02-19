using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ArcaneOdyssey.PlayerClasses
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

		public void ResetAlsoDead()
		{
			List<int> queue = [];
			foreach (int type in EquippedImbues)
			{
				var index = EquippedImbues.IndexOf(type);
				if (index >= 0)
				{
					if (EquippedImbuesTimers[index] <= 0)
					{
						queue.Add(index);
					}
					else
					{
						EquippedImbuesTimers[index]--;
					}
				}
			}
			foreach (var i in queue)
			{
				EquippedImbues.RemoveAt(i);
				EquippedImbuesTimers.RemoveAt(i);
			}
		}

		public override void UpdateDead()
		{
			BronzeSealed = 0;
			DarkSealed = 0;
			NimbusSealed = 0;
		}
	}
}
