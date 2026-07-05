using ArcaneOdyssey.Imbues.Base;
using ArcaneOdyssey.Items.Base;
using ArcaneOdyssey.Skills.Base;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ArcaneOdyssey.Items.Scrolls.Attacks.Rare
{
	public class EnchantmentSpell : RareScroll
	{
		public override bool MetConditions() => Main.netMode != NetmodeID.SinglePlayer;
		public override bool CanHaveMagic => true;
		public override ModSkill Skill => ModContent.GetInstance<EnchantmentSkill>();
	}

	public class EnchantmentSkill : PassiveSkill
	{
		public override int Length => 60 * 60 * 5;

		public override int Scroll => ModContent.ItemType<EnchantmentSpell>();

		public override bool PreActivate(Player player, Imbuable imbue)
		{
			if (Main.netMode != NetmodeID.SinglePlayer)
			{
				if (player.CheckMana(200, true))
				{
					Imbuable.CreateMagicCircle(imbue.Item, player, Projectiles.MagicCircleMode.Rotating, true);
					ActivateAbility(player, imbue);
					var packet = Mod.GetPacket();
					packet.Write(ArcaneOdysseyMod.PacketID.Enchantment);
					packet.Send();
					return true;
				}
			}
			else
			{
				imbue.RemoveSkill(Imbuable.SlotIndexID.Passive);
			}
			return false;
		}

		public override void Activate(Player player, Imbuable imbue)
		{
		}
	}
}
