using ArcaneOdyssey.Content.Items.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using static ArcaneOdyssey.AOUtils;

namespace ArcaneOdyssey.Content.Items.Equipment.Scrolls
{
	public class HoverScroll : MagicScroll
    {
        public override int AOValue => 1000;
        public override void SetDefaults()
		{
			base.SetDefaults();
			Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            AOPlayer playah = player.ArcaneOdyssey();
            Item.ArcaneOdyssey().imbue = playah.imbue;
            if (playah.imbue is AOMagic)
            {
                Item.color = playah.imbue.ImbueColour;
                player.GetJumpState<LeapAirStep>().Enable();
            }
            else Item.color = Color.Transparent;

        }

        public override void UpdateEquip(Player player)
        {
            if (player.TryGetImbue(out var imbue) && imbue is AOMagic)
            {
                player.carpet = true;
                player.GetModPlayer<HoverPlayer>().hasHoverEquipped = true;
                if (player.carpetTime > 0 && player.controlJump)
                {
                    player.moveSpeed += imbue.AOScrollSpeed.MultiToPercent();
                    imbue.LingeringEffects(player);
                    if (player.carpetTime <= 3 && player.CheckMana(1, true))
                        player.carpetTime = 15;
                }
            }
        }
	}

    public class HoverPlayer : ModPlayer
    {
        public bool hasHoverEquipped = false;
    }
}
