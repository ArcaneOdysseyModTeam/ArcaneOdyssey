using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
	public abstract class CooldownSystem
	{
		public static List<CooldownSystem> All = [];
		public abstract int CooldownLength { get; }
		public virtual string ID => GetType().Name;
		public abstract string Name { get; }
		public bool ManualTickdown => true;
		public virtual string Texture => null;

		public Cooldown AOCooldown => new(ID, Language.GetOrRegister(ArcaneOdyssey.Instance.GetLocalizationKey($"Cooldowns.{ID}"), () => Name), ManualTickdown, CooldownLength);

		public CooldownSystem()
		{
			All.Add(this);
		}
	}

	public struct Cooldown
	{
		public string ID;
		public LocalizedText Name;
		public bool TickDown;
		public int cooldownRemaining;

		public Cooldown(string ID, LocalizedText Name, bool TickDown, int CooldownLength)
		{
			this.ID = ID;
			this.Name = Name;
			this.TickDown = TickDown;
			cooldownRemaining = CooldownLength;
		}

		public Cooldown(string ID, Mod mod, string Name, bool TickDown, int CooldownLength)
		{
			this.ID = ID;
			this.Name = Language.GetOrRegister(mod.GetLocalizationKey("Cooldowns." + ID), () => Name);
			this.TickDown = TickDown;
			cooldownRemaining = CooldownLength;
		}

		public Cooldown(string ID, Mod mod, bool TickDown, int CooldownLength)
		{
			this.ID = ID;
			Name = mod.CustomLocalization("Cooldowns." + ID);
			this.TickDown = TickDown;
			cooldownRemaining = CooldownLength;
		}
	}

	public partial class AOPlayer : ModPlayer, IImbuableEntity
	{
		private List<Cooldown> toremove = [];
		private Dictionary<int, Cooldown> tochange = [];
		public override void PreUpdate()
		{
			if (timeTillNextMove > 1)
			{
				for (int i = 0; i < 4; i++)
					Player.doubleTapCardinalTimer[i] = 0;
				timeTillNextMove--;
			}
			else timeTillNextMove = 0;

            foreach (var Cooldown in tochange)
            {
                Cooldowns[Cooldown.Key] = Cooldown.Value;
            }

            foreach (var Cooldown in Cooldowns)
			{
				if (Cooldown.TickDown)
				{
					var cool = Cooldown;
					if (--cool.cooldownRemaining <= 0 || ArcaneOdyssey.devMode)
					{
						if (OnCooldown(Cooldown.ID) && !toremove.Contains(Cooldown))
							toremove.Add(Cooldown);
					}
					else
					{
						if (OnCooldown(Cooldown.ID))
							tochange[Cooldowns.IndexOf(Cooldown)] = cool;
					}
				}
			}

			foreach (var Cooldown in tochange)
			{
				Cooldowns[Cooldown.Key] = Cooldown.Value;
			}
			foreach (var Cooldown in toremove)
			{
				Cooldowns.Remove(Cooldown);
			}
			tochange = [];
			toremove = [];
		}

		public bool OnCooldown(string ID)
		{
			return GetCooldown(ID).ID is not null;
		}

		public Cooldown GetCooldown(string ID)
		{
			return Cooldowns.Find(e => e.ID == ID);
		}

		public void SetCooldown(Cooldown cooldown)
		{
			if (OnCooldown(cooldown.ID))
			{
				tochange[Cooldowns.IndexOf(GetCooldown(cooldown.ID))] = cooldown;
			}
			else
			{
				Cooldowns.Add(cooldown);
			}
		}
	}
}
