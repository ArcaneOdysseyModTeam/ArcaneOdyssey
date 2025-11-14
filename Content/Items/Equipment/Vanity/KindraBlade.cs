using ArcaneOdyssey.Content.Items.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Equipment.Vanity
{
    public class KindraBlade : AOBaseItem
    {
        public override AORarities AORarity => AORarities.Special;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                EquipLoader.AddEquipTexture(Mod, Texture.Replace("Blade", "_Head"), EquipType.Head, this);
                EquipLoader.AddEquipTexture(Mod, Texture.Replace("Blade", "_Body"), EquipType.Body, this);
                EquipLoader.AddEquipTexture(Mod, Texture.Replace("Blade", "_Legs"), EquipType.Legs, this);
                EquipLoader.AddEquipTexture(Mod, Texture.Replace("Blade", "_Back"), EquipType.Back, this);
            }
        }

        public override void SetStaticDefaults()
        {
            if (Main.dedServ)
                return;

            int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;

            int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
            ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;

            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
            ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 38;
            Item.accessory = true;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<GuardPlayer>().vanityEquipped = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
            {
                player.GetModPlayer<GuardPlayer>().vanityEquipped = true;
            }
        }
    }

    public class GuardPlayer : ModPlayer
    {
        public bool vanityEquipped = false;

        public override void ResetEffects()
        {
            vanityEquipped = false;
        }

        public override void FrameEffects()
        {
            if (vanityEquipped)
            {
                Player.back = EquipLoader.GetEquipSlot(Mod, nameof(KindraBlade), EquipType.Back);
                Player.legs = EquipLoader.GetEquipSlot(Mod, nameof(KindraBlade), EquipType.Legs);
                Player.head = EquipLoader.GetEquipSlot(Mod, nameof(KindraBlade), EquipType.Head);
                Player.body = EquipLoader.GetEquipSlot(Mod, nameof(KindraBlade), EquipType.Body);
            }
        }
    }
}
