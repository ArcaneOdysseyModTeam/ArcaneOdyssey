using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey
{
    public class AOConversion
    {
        /// <summary>
        /// Arcane Odyssey rarities, converted to RarityID
        /// </summary>
        public static class AORarities
        {
            public const int Common = -1;
            public const int Uncommon = 0;
            public const int Rare = 1;
            public const int Exotic = 4;
            public const int Legendary = 7;
        }

        /// <summary>
        /// Arcane Odyssey weapon tiers, used for scaling. Weapon skill index: 2 is Old; 3 is Normal; 5 is Excellent
        /// </summary>
        public static class AOWeaponTiers 
        {
            public const int Old = 1;
            public const int Normal = 2;
            public const int Excellent = 4;
        }

        /// <summary>
        /// Converts AO Galleons/Drachmae to Terraria Copper
        /// </summary>
        /// <param name="price">Price, in Galleons</param>
        /// <param name="rarity">Rarity of the item, use AORarities</param>
        /// <returns></returns>
        public static int GalleonToCopper(int price, int rarity)
        {
            return price * ((rarity + 2) * (1 + 1 / 9));
        }

        /// <summary>
        /// Converts AO Weapon speed to weapon use time
        /// </summary>
        /// <param name="AOSpeed">AO weapon speed multiplier</param>
        /// <param name="AOWeaponTier">AO weapon tier, use AOWeaponTiers</param>
        /// <returns></returns>
        public static int WeaponSpeed(float AOSpeed, int AOWeaponTier) => (int)(27 / (AOSpeed + ((AOSpeed - 1) * AOWeaponTier)));

        /// <summary>
        /// Converts AO weapon size to weapon scale and knockback
        /// </summary>
        /// <param name="AOSize">AO weapon size multiplier</param>
        /// <param name="AOWeaponTier">AO weapon tier, use AOWeaponTiers</param>
        /// <returns></returns>
        public static float WeaponSize(float AOSize, int AOWeaponTier) => (int)(27 / (AOSize + ((AOSize - 1) * AOWeaponTier)));

        /// <summary>
        /// Converts AO weapon damage to Terraria damage. Scales very heavily with weapon tier
        /// </summary>
        /// <param name="AODamage">AO weapon damage multiplier</param>
        /// <param name="AOWeaponTier">AO weapon tier, use AOWeaponTiers</param>
        /// <returns></returns>
        public static int WeaponDamage(float AODamage, int AOWeaponTier) => (int)(25*AOWeaponTier*AODamage);
    }
}