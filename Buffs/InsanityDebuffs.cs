using ArcaneOdyssey.Buffs.Base;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Chat;
using Terraria.ID;

namespace ArcaneOdyssey.Buffs
{
	public class InsanityOne : BaseBuff
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			Main.debuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.pvpBuff[Type] = true;
			BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
			Main.buffNoSave[Type] = true;
			ExternalModSupport.RegisterDebuff(this);
		}

		public override string Texture => AOUtils.GetTexture<InsanityOne>();

		public virtual byte BanishmentReq => 2;

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ArcaneOdyssey().Banishment < BanishmentReq)
			{
				if (Main.dedServ && AOUtils.PlayerCount > 1)
				{
					if (Main.rand.NextBool(60 * 60 * 2))
					{
						var randplayer = Main.rand.Next(Array.FindAll(Main.player, e => e.active && e.whoAmI != player.whoAmI && !string.IsNullOrEmpty(e.name)));
						ChatHelper.SendChatMessageToClientAs((byte)randplayer.whoAmI, Mod.CustomLocalization("Insanity.FakeMessage" + Main.rand.Next(6)).ToNetworkText(), Color.Purple, player.whoAmI);
					}
				}
			}
		}
	}

	public class InsanityTwo : InsanityOne
	{
		public override byte BanishmentReq => (byte)(base.BanishmentReq + 1);
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ArcaneOdyssey().Banishment < BanishmentReq)
			{
				base.Update(player, ref buffIndex);
				if (Main.myPlayer == player.whoAmI)
				{
					if (Main.rand.NextBool(60 * 60))
					{
						Main.NewText(Mod.CustomLocalization("Insanity.Message" + Main.rand.Next(5)), Color.Purple);
					}
				}
			}
		}
	}

	public class InsanityThree : InsanityTwo
	{
		public override byte BanishmentReq => (byte)(base.BanishmentReq + 1);
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ArcaneOdyssey().Banishment < BanishmentReq)
			{
				base.Update(player, ref buffIndex);
			}
		}
	}

	public class InsanityFour : InsanityThree
	{
		public override byte BanishmentReq => (byte)(base.BanishmentReq + 1);
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ArcaneOdyssey().Banishment < BanishmentReq)
			{
				base.Update(player, ref buffIndex);
				player.ArcaneOdyssey().debuffs.Add(100);
			}
		}
	}

	public class InsanityFive : InsanityFour
	{
		public override byte BanishmentReq => (byte)(base.BanishmentReq + 1);
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ArcaneOdyssey().Banishment < BanishmentReq)
			{
				base.Update(player, ref buffIndex);
				player.ArcaneOdyssey().debuffs.Add(100);
			}
		}
	}
}
