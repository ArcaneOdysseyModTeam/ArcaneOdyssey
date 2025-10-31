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
}
