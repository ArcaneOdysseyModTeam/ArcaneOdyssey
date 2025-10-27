using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Content.Items.Vanity
{
    public class VesuvianSigil : ModItem
    {
        public override void Load()
        {
            if (!Main.dedServ)
            {
                EquipLoader.AddEquipTexture(Mod, $"{nameof(ArcaneOdyssey)}/Content/Items/Vanity/Red_Head", EquipType.Head, this);
                EquipLoader.AddEquipTexture(Mod, $"{nameof(ArcaneOdyssey)}/Content/Items/Vanity/Red_Body", EquipType.Body, this);
                EquipLoader.AddEquipTexture(Mod, $"{nameof(ArcaneOdyssey)}/Content/Items/Vanity/Red_Legs", EquipType.Legs, this);
                EquipLoader.AddEquipTexture(Mod, $"{nameof(ArcaneOdyssey)}/Content/Items/Vanity/Red_Back", EquipType.Back, this);
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
            Item.width = 40;
            Item.height = 40;
            Item.accessory = true;
            Item.value = 0;
            Item.rare = ItemRarityID.Red;
            Item.vanity = true;
        }

        public override void UpdateVanity(Player player)
        {
            player.GetModPlayer<RedPlayer>().vanityEquipped = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!hideVisual)
            {
                player.GetModPlayer<RedPlayer>().vanityEquipped = true;
            }
        }
    }

    public class RedPlayer : ModPlayer
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
                Player.back = EquipLoader.GetEquipSlot(Mod, "VesuvianSigil", EquipType.Back);
                Player.legs = EquipLoader.GetEquipSlot(Mod, "VesuvianSigil", EquipType.Legs);
                Player.head = EquipLoader.GetEquipSlot(Mod, "VesuvianSigil", EquipType.Head);
                Player.body = EquipLoader.GetEquipSlot(Mod, "VesuvianSigil", EquipType.Body);
            }
        }
    }
}
